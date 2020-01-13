using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveMyDataServer.Controllers.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Database.Models.Users;
using SaveMyDataServer.Database.Static;
using SaveMyDataServer.Static;
using SaveMyDataServer.ViewModels.Home;
using System;
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
            //Create the viewmodel
            var indexViewModel = new IndexViewModel();
            try
            {
                //Get the databases the user created
                var databases = await MongoCollectionService.GetCollection<UserDatabaseModel>(DatabaseTableNames.UserDatabases, DatabaseNames.Main);

                //Assign the valiues
                indexViewModel.Databases = databases;
            }
            catch (Exception ex)
            {
                //Set the error message
                TempData[TempDataDictionaryKeys.ErrorMessage] = ex.Message;
                //Set the message to the viewbag
                // centrilize the were we record any errors for further error logging
                SetMessages();
            }
            return View(indexViewModel);
        }
        #endregion


    }
}
