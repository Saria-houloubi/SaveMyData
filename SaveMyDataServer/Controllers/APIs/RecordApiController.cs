using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using SaveMyDataServer.Attributes;
using SaveMyDataServer.Controllers.APIs.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Models;
using SaveMyDataServer.Models.API;
using SaveMyDataServer.SharedKernal.Static;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    [TokenAuthorize]
    public class RecordApiController : BaseAPIController
    {
        #region Services
        /// <summary>
        /// The service to work with mongo collections
        /// </summary>
        public IMongoCollectionService MongoCollectionService { get; private set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public RecordApiController(IMongoCollectionService mongoCollectionService)
        {
            MongoCollectionService = mongoCollectionService;
        }
        #endregion

        #region GET requests
        /// <summary>
        /// Get the records from the sent table name
        /// </summary>
        /// <param name="database">The database to get the values from</param>
        /// <param name="table">the table to get the values for</param>
        /// <param name="key">The key to the database</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Records([FromBody] GetRecordModel model)
        {
            //if the user did not provide any table name
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Table) || string.IsNullOrEmpty(model.Database))
            {
                //Send and error response
                return BadRequest(ErrorMessages.InvalidData);
            }

            try
            {
                //Try to get the records from the database
                var records = await MongoCollectionService.GetCollectionFilterd<BsonDocument>(model.Table, UniqueDatabaseName(model.Database), model.Filters);

                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region POST requests
        /// <summary>
        /// Adds a record into the user selected database
        /// </summary>
        /// <param name="model">The data for addind and holds a data json property for the values to save</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecordDetailModel<JObject> model)
        {
            //If the user forgot to provide the database name or table that he/she is working on
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Database) || string.IsNullOrEmpty(model.Table))
            {
                //Return a bad request
                return BadRequest(ErrorMessages.MissingData);
            }

            try
            {
                //trim any spaces in the properties names nad values
                var data = Regex.Replace(model.Data.ToString(), RegexPatterns.SpaceJsonEnd, "");
                data = Regex.Replace(data, RegexPatterns.SpaceJsonStart, "");
                //Try to add the record
                var record = await MongoCollectionService.AddRecord(BsonDocument.Parse(data), model.Table, $"{UniqueDatabaseName(model.Database)}");

                return Ok(record);
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
        public async Task<IActionResult> Update([FromBody] UpdateRecordModel model)
        {
            //If the user forgot to provide the database name or table that he/she is working on
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Database) || string.IsNullOrEmpty(model.Table))
            {
                //Return a bad request
                return BadRequest(ErrorMessages.MissingData);
            }

            try
            {
                //Try to add the record
                var result = await MongoCollectionService.UpdateReocrds(model.Filters, model.Update, model.Table, UniqueDatabaseName(model.Database));

                return Ok(result);
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
        public async Task<IActionResult> Delete([FromBody] GetRecordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessages.InvalidData);
            }

            try
            {
                var result = await MongoCollectionService.DeleteRecord<BsonDocument>(model.Filters, model.Table, UniqueDatabaseName(model.Database));
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
        #endregion
    }
}
