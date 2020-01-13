using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using SaveMyDataServer.Attributes;
using SaveMyDataServer.Controllers.APIs.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Models.API;
using SaveMyDataServer.SharedKernal.Static;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers.APIs
{
    /// <summary>
    /// The controller to control how to work with the collections
    /// </summary>
    [TokenAuthorize]
    public class CollectionAPIController : BaseAPIController
    {
        #region Services

        /// <summary>
        /// The layer to work with Mongo collections from adding to getting records
        /// </summary>
        public IMongoCollectionService CollectionService { get; private set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public CollectionAPIController(IMongoCollectionService collectionService)
        {
            CollectionService = collectionService;
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
        [HttpGet("{database}/{table}")]
        public async Task<IActionResult> Records(string database, string table)
        {
            //if the user did not provide any table name
            if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(database))
            {
                //Send and error response
                return BadRequest(ErrorMessages.InvalidData);
            }

            try
            {
                //Try to get the records from the database
                var records = await CollectionService.GetCollection<BsonDocument>(table, database);

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
        /// <typeparam name="T">The type of the record to add</typeparam>
        /// <param name="model">The data for addind and holds a data json property for the values to save</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Record([FromBody] RecordDetailModel<JObject> model)
        {
            //If the user forgot to provide the database name or table that he/she is working on
            if (string.IsNullOrEmpty(model.Database) || string.IsNullOrEmpty(model.Table))
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
                var record = await CollectionService.AddRecord(BsonDocument.Parse(data), model.Table, $"{UniqueDatabaseName(model.Database)}");

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
