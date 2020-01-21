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
        public IActionResult ApiGuid()
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
        #endregion
    }
}
