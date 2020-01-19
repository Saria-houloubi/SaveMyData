using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SaveMyDataServer.Controllers.Base;
using SaveMyDataServer.Core.Extensions;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.SharedKernal;
using SaveMyDataServer.SharedKernal.Enums;
using SaveMyDataServer.SharedKernal.Static;
using SaveMyDataServer.ViewModels.DataCenter;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    /// <summary>
    /// The controller to access the user data center
    /// </summary>
    [Authorize]
    public class DataCenterController : BaseController
    {

        #region Properties
        /// <summary>
        /// The service to work with mongo collections
        /// </summary>
        public IMongoCollectionService MongoCollectionService { get; private set; }
        /// <summary>
        /// The excel service layer to create and update files
        /// </summary>
        public IExcelService ExcelService { get; private set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public DataCenterController(IMongoCollectionService mongoCollectionService, IExcelService excelService)
        {
            MongoCollectionService = mongoCollectionService;
            ExcelService = excelService;
        }
        #endregion

        #region GET requests
        /// <summary>
        /// The home page to show the user database details
        /// </summary>
        /// <param name="databaseId">The id of the database the user wants to explore in</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Home([FromQuery]string db)
        {
            //Create the viewmodel
            var viewModel = new DataCenterHomeViewModel()
            {
                TablesAndCount = await MongoCollectionService.GetDatabaseTables(UniqueDatabaseName(db)),
                Database = db,
            };

            //Assign the first table as the selected value
            if (viewModel.TablesAndCount.Any())
            {
                viewModel.SelectedTable = viewModel.TablesAndCount.First().Key;
            }

            return View(viewModel);
        }

        /// <summary>
        /// Export the data into the sent wanted type
        /// </summary>
        /// <param name="table">The name of the table to export</param>
        /// <param name="database">The database the user is connected to</param>
        /// <param name="type">the <see cref="SupportedExportFileTypes>"/> the server can export the data into</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExportData(string table, string database, SupportedExportFileTypes type)
        {
            try
            {
                //Get the colleciton from teh database
                var data = await MongoCollectionService.GetCollection<BsonDocument>(table, UniqueDatabaseName(database));
                //Check what format the user requestd
                switch (type)
                {
                    case SupportedExportFileTypes.Excel:
                        return File(ExcelService.CreateExcelFile($"{table}-{database}", data), "application/vnd.ms-excel", $"{table}-{database}-{DateTime.UtcNow.ToString("dd_MM_yyyy")}.xls");
                    case SupportedExportFileTypes.CSV:
                        return BadRequest(ErrorMessages.NotSupported);
                    default:
                        return BadRequest(ErrorMessages.InvalidData);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Exports a record into the wanted type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExportRecord(string id, string table, string database, SupportedExportFileTypes type)
        {
            try
            {
                if (ObjectId.TryParse(id, out ObjectId objectId))
                {
                    //Get the data from the database
                    var data = await MongoCollectionService.GetCollectionFilterd(table, UniqueDatabaseName(database), Builders<BsonDocument>.Filter.Eq(MongoTableBaseFieldNames.Id, objectId));

                    if (data.Count > 0)
                    {
                        //Check what format the user requestd
                        switch (type)
                        {
                            case SupportedExportFileTypes.Excel:
                                return BadRequest(ErrorMessages.NotSupported);
                            case SupportedExportFileTypes.CSV:
                                //Create the csv file string
                                var csv = new StringBuilder();
                                //Get the properties and values
                                var columnNames = data[0].GetKeyNameValue("");
                                //Start the header and the value row
                                var headerLine = $"{columnNames.First().Key}";
                                var valueLine = $"{columnNames.First().Value}";
                                //Loop throw the properties
                                for (int i = 1; i < columnNames.Count; i++)
                                {
                                    //Get the element in the i(th) postion
                                    var element = columnNames.ElementAt(i);
                                    //Build the rows
                                    headerLine = $"{headerLine},{element.Key}";
                                    valueLine = $"{valueLine},{element.Value}";
                                }
                                //Add the the main csv file
                                csv.AppendLine(headerLine);
                                csv.AppendLine(valueLine);
                                //Send the file back to the user
                                return File(ASCIIEncoding.ASCII.GetBytes(csv.ToString()), "text/csv", $"{id}-{table}-{database}-{DateTime.UtcNow.ToString("dd_MM_yyyy")}.csv");
                            default:
                                return BadRequest(ErrorMessages.InvalidData);
                        }
                    }
                }
                return BadRequest(ErrorMessages.InvalidData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
