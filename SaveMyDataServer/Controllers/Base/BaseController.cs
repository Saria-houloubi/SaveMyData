using Microsoft.AspNetCore.Mvc;
using SaveMyDataServer.ExteintionMethods;
using SaveMyDataServer.SharedKernal.Static;
using SaveMyDataServer.Static;
using System.Security.Claims;

namespace SaveMyDataServer.Controllers.Base
{
    /// <summary>
    /// The base controller class for the endpoints
    /// </summary>
    public abstract class BaseController : Controller
    {
        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public BaseController()
        {

        }
        #endregion

        #region Overrides
        public override ViewResult View(object model)
        {
            SetMessages();
            SetUserDetailes();

            return base.View(model);
        }
        public override ViewResult View()
        {
            SetMessages();
            SetUserDetailes();

            return base.View();
        }
        /// <summary>
        /// Sets a message to show to the user after some operations
        ///     either shows an error message
        ///     or a sucess message
        ///     TODO:Future change to enum and display messages based on it for the message type    
        /// </summary>
        protected void SetMessages()
        {
            if (TempData.TryGetValue(TempDataDictionaryKeys.ErrorMessage, out object error))
            {
                ViewBag.ErrorMessage = error;
            }
            else if (TempData.TryGetValue(TempDataDictionaryKeys.SucessMessage, out object success))
            {
                ViewBag.SuccessMessage = success;
            }
        }
        /// <summary>
        /// Sets the user information if authorized
        /// </summary>
        private void SetUserDetailes()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.Email = User.GetClaim(ClaimTypes.Email).Value;
                ViewBag.Id = User.GetClaim(ClaimTypes.PrimarySid).Value;
                ViewBag.Name = User.GetClaim(ClaimTypes.Name).Value;
            }
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
        /// Redirect the user to a page with a sent message in it to display
        /// </summary>
        /// <param name="redirect">The redirect url</param>
        /// <param name="message">The message to send</param>
        /// <param name="isError">If the message is error show in an error box</param>
        /// <returns></returns>
        public RedirectResult RedirectWithMessage(string redirect, string message, bool isError)
        {
            TempData[isError ? TempDataDictionaryKeys.ErrorMessage : TempDataDictionaryKeys.SucessMessage] = message;
            return Redirect(redirect);
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
