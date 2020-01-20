using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using SaveMyDataServer.Controllers.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Helpers;
using SaveMyDataServer.Models;
using SaveMyDataServer.SharedKernal.Models;
using SaveMyDataServer.SharedKernal.Static;
using System;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    [Authorize]
    public class CollectionController : BaseController
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
        public CollectionController(IMongoCollectionService mongoCollectionService)
        {
            MongoCollectionService = mongoCollectionService;
        }
        #endregion

        #region GET requests
        /// <summary>
        /// Gets an html table for the sent table
        /// </summary>
        /// <param name="table">the table to get the data for</param>
        /// <param name="database">the database were the user is working in</param>
        /// <param name="count">the amount of records to get for pagination</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string table, string database, string pagination)
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
        #endregion

        #region POST requests
        /// <summary>
        /// Adds a new empty collection
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]UpdateCollectionModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.NewName))
            {
                //Return a bad request
                return BadRequest(ErrorMessages.MissingData);
            }

            try
            {
                //Try to add the new collection
                await MongoCollectionService.AddRecord<BsonDocument>(new BsonDocument(), model.NewName, UniqueDatabaseName(model.Database));

                return Ok(SuccessMessages.OperationSuccess);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        #endregion

        #region PUT requests
        /// <summary>
        /// Update a collection
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]UpdateCollectionModel model)
        {
            if (!ModelState.IsValid)
            {
                //Return a bad request
                return BadRequest(ErrorMessages.MissingData);
            }

            try
            {
                //Try to update the name
                await MongoCollectionService.RenameCollection(model.Name, model.NewName, UniqueDatabaseName(model.Database));

                return Ok(SuccessMessages.OperationSuccess);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        #endregion

        #region DELETE requests
        /// <summary>
        /// Deletes a collection from the server
        /// </summary>
        /// <param name="db">The database name to delete</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm]DeteleRecordModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Database) || string.IsNullOrWhiteSpace(model.Table))
            {
                return BadRequest(ErrorMessages.InvalidData);
            }

            try
            {
                //Delete the record from the users table
                await MongoCollectionService.DropCollection(model.Table, UniqueDatabaseName(model.Database));
                return Ok(SuccessMessages.OperationSuccess);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
