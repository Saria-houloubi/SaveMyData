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
using SaveMyDataServer.Static;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    public class HomeController : BaseController
    {
        #region Properties
        /// <summary>
        /// The service layer to work with the database
        /// </summary>
        public IMongoCollectionService MongoCollectionService { get; private set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public HomeController(IMongoCollectionService mongoCollectionService)
        {
            MongoCollectionService = mongoCollectionService;
        }
        #endregion

        #region GET requests
        [HttpGet(ServerRedirectsURLs.Index)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Guid()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// The index page to show the user 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(ServerRedirectsURLs.Home)]
        public async Task<IActionResult> Home()
        {
            //Get the database count for the logged in user
            ViewData["database-count"] = await MongoCollectionService.GetCollectionCount<UserDatabaseModel>(DatabaseTableNames.UserDatabases, DatabaseNames.Main, item => item.UserId == new Guid(User.GetClaim(ClaimTypes.PrimarySid).Value));

            return View();
        }

        /// <summary>
        /// Get the database the user has created 
        /// </summary>
        /// <param name="pagination">The pagination options for the table</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDatabaseData(string pagination)
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
                }).ToList(), new string[] { "Name", "Creation data", "" }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
