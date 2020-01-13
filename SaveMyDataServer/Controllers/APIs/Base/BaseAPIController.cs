using Microsoft.AspNetCore.Mvc;
using SaveMyDataServer.ExteintionMethods;
using SaveMyDataServer.SharedKernal.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers.APIs.Base
{
    /// <summary>
    /// The base class for API controllers
    ///     holds cross functions and data
    ///     and sets the api route
    /// </summary>
    [Route("api/{controller}/{action}")]
    public class BaseAPIController : Controller
    {

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public BaseAPIController()
        {

        }
        #endregion


        #region Methods
        /// <summary>
        /// Set an error message to the view bag so if any error it will show back
        ///  to the view
        /// </summary>
        /// <param name="message"></param>
        public void SetViewBagErrorMessage(string message)
        {
            //Set the message
            ViewBag.ErrorMessage = message;
        }
        /// <summary>
        /// Gets the unique database name for userId_databaseName
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public string UniqueDatabaseName(string database)
        {
            return $"{HttpContext.User.GetClaim(ClaimTypes.PrimarySid).Value}{DatabaseSpecials.UniqueDatabaseNameSpliterCharecture}{database}";
        }
        #endregion
    }
}
