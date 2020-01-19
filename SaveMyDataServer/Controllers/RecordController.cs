using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SaveMyDataServer.Controllers.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Models;
using SaveMyDataServer.Models.API;
using SaveMyDataServer.SharedKernal.Static;
using System;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    [Authorize]
    public class RecordController : BaseController
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
        public RecordController(IMongoCollectionService mongoCollectionService)
        {
            MongoCollectionService = mongoCollectionService;
        }
        #endregion

        #region GET requests

        #endregion

        #region POST requests

        #endregion

        #region PUT requests
        /// <summary>
        /// Update a record in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]RecordDetailModel<string> model)
        {
            //If the user forgot to provide the database name or table that he/she is working on
            if (string.IsNullOrEmpty(model.Database) || string.IsNullOrEmpty(model.Table))
            {
                //Return a bad request
                return BadRequest(ErrorMessages.MissingData);
            }

            try
            {
                //Convert the data into a bson object
                var bsonData = BsonDocument.Parse(model.Data);
                //Try to add the record
                var record = await MongoCollectionService.EditHoleRecordById(bsonData, model.Id, model.Table, $"{UniqueDatabaseName(model.Database)}");

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
        public async Task<IActionResult> Delete([FromForm] DeteleRecordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessages.InvalidData);
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
