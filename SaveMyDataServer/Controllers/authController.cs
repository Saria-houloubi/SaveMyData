using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SaveMyDataServer.Controllers.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.SharedKernal.Static;
using SaveMyDataServer.Static;
using SaveMyDataServer.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers
{
    /// <summary>
    /// The authintication controller to register and login users
    /// </summary>
    public class AuthController : BaseController
    {
        #region Properties
        /// <summary>
        /// The service to work with authintication a user
        /// </summary>
        public IAuthUserService AuthUserService { get; set; }
        /// <summary>
        /// The mail service layer
        /// </summary>
        public IMailService MailService { get; set; }

        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public AuthController(IAuthUserService authUserService, IMailService mailService)
        {
            AuthUserService = authUserService;
            MailService = mailService;
        }
        #endregion

        #region GET Requests
        /// <summary>
        /// The GET request to show the login page for the user
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            //If the user is already authinticated
            if (User.Identity.IsAuthenticated)
            {
                //Send him/her to the home page
                return Redirect(ServerRedirectsURLs.Home);
            }
            return View(new LoginViewModel());
        }
        /// <summary>
        /// The GET request to register to show the registeration page to the user
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {

            //If the user is already authinticated
            if (User.Identity.IsAuthenticated)
            {
                //Send him/her to the home page
                return Redirect(ServerRedirectsURLs.Home);
            }
            return View(new RegisterViewModel());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult EmailAuth()
        {
            //The email the user regiterd in
            var registerEmail = "";
            //Check if the we have been redirected from the register controller
            if (TempData.TryGetValue(TempDataDictionaryKeys.Email, out object email))
            {
                //Set the email the user registerd in
                registerEmail = email.ToString();
            }
            return View(new RegisterViewModel
            {
                Email = registerEmail
            });
        }
        /// <summary>
        /// Confirms a user email
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <param name="id2">The unique id of the email that was sent to the user</param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [HttpGet]
        public async Task<IActionResult> EmailConfirmaiton([FromQuery] Guid id, [FromQuery]Guid id2)
        {
            //Check if the sent id is null
            if (id == Guid.Empty || id2 == Guid.Empty)
            {
                return RedirectWithMessage(ServerRedirectsURLs.EmailAuth, ErrorMessages.InvalidData, true);
            }

            try
            {
                var result = await AuthUserService.ConfirmUserEmail(id, id2);

                if (result)
                {
                    return RedirectWithMessage(ServerRedirectsURLs.EmailAuth, ErrorMessages.InvalidData, true);
                }
                //Set a message for sucess
                return RedirectWithMessage(ServerRedirectsURLs.Login, SuccessMessages.EmailConfirmed, false);
            }
            catch (Exception ex)
            {

                return RedirectWithMessage(ServerRedirectsURLs.EmailAuth, ex.Message, true);
            }
        }
        /// <summary>
        /// Logs the user out and deletes the cookies
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //Sign the user out by deleting the cookies
            await HttpContext.SignOutAsync();
            //redirect to login page
            return Redirect(ServerRedirectsURLs.Login);
        }
        #endregion

        #region POST Requests
        /// <summary>
        /// The POST request to register a user
        /// </summary>
        /// <param name="model">The user data</param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            //If the data was not sent corectly
            if (!ModelState.IsValid)
            {
                //Set the error message
                SetViewBagErrorMessage(ErrorMessages.InvalidData);
                //Return the view to show
                return View(model);
            }
            //Check if the sent information is correct
            var user = await AuthUserService.ConfirmUserPassword(model.Email, model.Password);
            if (user == null)
            {
                //Set the error message
                SetViewBagErrorMessage(ErrorMessages.LoginFail);
                return View(model);
            }
            if (!user.IsMailConfirmed)
            {
                //Set the error message
                SetViewBagErrorMessage(ErrorMessages.EmailNotConfirmed);
                return View(model);
            }

            //Create the user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
            };
            //Create the identity and assing the schema for using cookies
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            //Assing the cookie and sign the user in
            await HttpContext.SignInAsync(principal);
            //Redirect the user to the home page
            return Redirect(ServerRedirectsURLs.Home);
        }

        /// <summary>
        /// The POST request to register a user in the database
        /// </summary>
        /// <param name="model">The model that holds the user information</param>
        /// <returns></returns>
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid || (model.Password != model.ConfirmPassword))
            {
                //Set the error message
                SetViewBagErrorMessage(ErrorMessages.InvalidData);
                //Return the view to show
                return View(model);
            }
            try
            {
                //Add the user into the database
                var user = await AuthUserService.Register(model.Name, model.Email, model.Password, model.DOB);
            }
            catch (MongoWriteException)
            {
                //If the registerd email is already stored
                SetViewBagErrorMessage(ErrorMessages.DuplicateEmail);
                return View(model);
            }
            catch (Exception ex)
            {
                SetViewBagErrorMessage(ex.Message);
                return View(model);
            }
            //Set the temp data before redirect email auth
            TempData[TempDataDictionaryKeys.Email] = model.Email;
            //Redirect the user to confirm his/her email
            return Redirect(ServerRedirectsURLs.EmailAuth);
        }
        /// <summary>
        /// sends a confirmation email to the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EmailConfirmation([FromForm]string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessages.MissingData);
            }

            try
            {
                //Try to send the user an email
                var user = await AuthUserService.SendConfirmationEmail(email);

                if (user == null)
                {
                    return BadRequest(ErrorMessages.InvalidData);
                }

                return Ok(user.IsMailConfirmed ? SuccessMessages.EmailConfirmed : SuccessMessages.EmailSent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
