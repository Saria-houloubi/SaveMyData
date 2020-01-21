using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using SaveMyDataServer.Attributes;
using SaveMyDataServer.Controllers.APIs.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Database.Models.Users;
using SaveMyDataServer.Database.Static;
using SaveMyDataServer.ExteintionMethods;
using SaveMyDataServer.SharedKernal.Models;
using SaveMyDataServer.SharedKernal.Static;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    [TokenAuthorize]
    public class DatabaseApiController : BaseAPIController
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
        public DatabaseApiController(IMongoCollectionService mongoCollectionService)
        {
            MongoCollectionService = mongoCollectionService;
        }
        #endregion

        #region GET requests
        /// <summary>
        /// Get the database the user has created 
        /// </summary>
        /// <param name="pagination">The pagination options for the table</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Databases([FromHeader]string pagination)
        {
            try
            {
                //Get the databases the user created
                var databases = await MongoCollectionService.GetCollectionFilterd<UserDatabaseModel>(DatabaseTableNames.UserDatabases,
                                                        DatabaseNames.Main,
                                                        Builders<UserDatabaseModel>.Filter.Eq(item => item.UserId, new Guid(User.GetClaim(ClaimTypes.PrimarySid).Value))
                                                        , string.IsNullOrEmpty(pagination) ? null : JsonConvert.DeserializeObject<PaginationModel>(pagination));

                return Ok(databases);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region POST requests

        #endregion

        #region PUT requests


        #endregion

        #region DELETE requests
        /// <summary>
        /// Deletes a database from the server
        /// </summary>
        /// <param name="db">The database name to delete</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromHeader]Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest(ErrorMessages.InvalidData);
            }

            try
            {
                //Delete the record from the users table
                var record = await MongoCollectionService.DeleteRecord<UserDatabaseModel>(item => item.Id == id, DatabaseTableNames.UserDatabases, DatabaseNames.Main);
                //Drop the database
                await MongoCollectionService.DropDatabase(UniqueDatabaseName(record.Name));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
