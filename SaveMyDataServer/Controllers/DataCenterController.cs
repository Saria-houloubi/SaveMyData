using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using SaveMyDataServer.Controllers.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Helpers;
using SaveMyDataServer.Models;
using SaveMyDataServer.Models.API;
using SaveMyDataServer.SharedKernal.Enums;
using SaveMyDataServer.SharedKernal.Models;
using SaveMyDataServer.SharedKernal.Static;
using SaveMyDataServer.ViewModels.DataCenter;
using System;
using System.Linq;
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
        public IExcelService ExcelService { get; set; }
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
        /// Gets an html table for the sent table
        /// </summary>
        /// <param name="table">the table to get the data for</param>
        /// <param name="database">the database were the user is working in</param>
        /// <param name="count">the amount of records to get for pagination</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTableData(string table, string database, string pagination)
        {
            try
            {
                //Get the pagination object
                var paginationObject = JsonConvert.DeserializeObject<PaginationModel>(pagination);

                return Json(HTMLCreatorHelper.GetMongoTableBson(await MongoCollectionService.GetCollection<BsonDocument>(table, UniqueDatabaseName(database), paginationObject), table));

            }
            catch (System.Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
                        return File(ExcelService.CreateExcelFile("Test", data), "application/vnd.ms-excel");
                    default:
                        return BadRequest(ErrorMessages.InvalidData);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region PUT requests
        /// <summary>
        /// Update a record in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Record([FromForm]RecordDetailModel<string> model)
        {
            //If the user forgot to provide the database name or table that he/she is working on
            if (string.IsNullOrEmpty(model.Database) || string.IsNullOrEmpty(model.Table))
            {
                //Return a bad request
                return BadRequest(ErrorMessages.MissingData);
            }

            try
            {
                //Try to add the record
                var record = await MongoCollectionService.EditHoleRecordById(BsonDocument.Parse(model.Data), model.Id, model.Table, $"{UniqueDatabaseName(model.Database)}");

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        #endregion

        #region DELETE requests
        /// <summary>
        /// Deletes a record from the database
        /// </summary>
        /// <param name="model">The model that hold the record detalise to delete it</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Record([FromForm] DeteleRecordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(nameof(Home));
            }

            try
            {
                return Ok(await MongoCollectionService.DeleteRecordById<BsonDocument>(model.Id, model.Table, UniqueDatabaseName(model.Database)));
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }
        #endregion
    }
}
