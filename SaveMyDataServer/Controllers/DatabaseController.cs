using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using SaveMyDataServer.Controllers.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Database.Models.Users;
using SaveMyDataServer.Database.Static;
using SaveMyDataServer.ExteintionMethods;
using SaveMyDataServer.Helpers;
using SaveMyDataServer.SharedKernal.Models;
using SaveMyDataServer.SharedKernal.Static;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    [Authorize]
    public class DatabaseController : BaseController
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
        public DatabaseController(IMongoCollectionService mongoCollectionService)
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
        public async Task<IActionResult> Get(string pagination)
        {
            try
            {
                //Get the pagination object model
                var paginationModel = JsonConvert.DeserializeObject<PaginationModel>(pagination);

                //Get the databases the user created
                var databases = await MongoCollectionService.GetCollectionFilterd<UserDatabaseModel>(DatabaseTableNames.UserDatabases,
                                                        DatabaseNames.Main,
                                                        Builders<UserDatabaseModel>.Filter.Eq(item => item.UserId, new Guid(User.GetClaim(ClaimTypes.PrimarySid).Value))
                                                        , paginationModel);

                return Json(HTMLCreatorHelper.CreateTable(databases.Select(item => new
                {
                    name = item.Name,
                    creationData = item.CreationDateUTC.ToString("dd / MM / yyyy"),
                    link = $"<a class='btn btn-outline-info' href='/datacenter/home?db={item.Name}'>Show</a>",
                    deleteLink = $"<button class='btn btn-outline-danger' onClick='askConfirmation(this,\"{item.Id}\")'>Delete</button>"
                }).ToList(), new string[] { "Name", "Creation data", "", "" }));
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
        public async Task<IActionResult> Delete([FromForm]Guid id)
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
