using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SaveMyDataServer.Controllers.APIs.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.ExteintionMethods;
using SaveMyDataServer.Helpers;
using SaveMyDataServer.Models.API;
using SaveMyDataServer.SharedKernal.Static;
using SaveMyDataServer.ViewModels.Auth;
using System;
using System.Threading.Tasks;

namespace SaveMyDataServer.Controllers.APIs
{
    /// <summary>
    /// The authintication API controller to login,register....
    /// </summary>
    public class AuthAPIController : BaseAPIController
    {
        #region Services
        /// <summary>
        /// The service to work with authintication a user
        /// </summary>
        public IAuthUserService AuthUserService { get; set; }
        /// <summary>
        /// The configuration Manger to get some data from the configuration files
        /// </summary>
        public IConfiguration Configuration { get; set; }
        #endregion
        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public AuthAPIController(IAuthUserService authUserService, IConfiguration configuration)
        {
            AuthUserService = authUserService;
            Configuration = configuration;
        }
        #endregion

        #region POST requests
        /// <summary>
        /// Checks if the user can login and issues JWT token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            //If the data was not sent corectly
            if (!ModelState.IsValid)
            {
                //Set the error message
                model.ErrorMessage = ErrorMessages.InvalidData;
                //Return the view to show
                return BadRequest(model);
            }
            //Check if the sent information is correct
            var user = await AuthUserService.ConfirmUserPassword(model.Email, model.Password);
            if (user == null)
            {
                //Set the error message
                model.ErrorMessage = ErrorMessages.LoginFail;
                return StatusCode(StatusCodes.Status401Unauthorized, model);
            }
            //Check if the user has confirmed his/her email
            if (!user.IsMailConfirmed)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ErrorMessages.EmailNotConfirmed);
            }

            //Create the response object
            var response = new UserAPIModel
            {
                DOB = user.DOB,
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                IsMailConfirmed = user.IsMailConfirmed,
            };
            //Add the JWT token to the object
            response.Token = JWTTokenHelpers.GenerateJwtToken(response, DateTime.UtcNow.AddMonths(2));

            //Redirect the user to the home page
            return Ok(response);
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid || (model.Password != model.ConfirmPassword))
            {
                //Set the error message
                return BadRequest(ModelState.GetValidationErrors());
            }
            try
            {
                //Add the user into the database
                var user = await AuthUserService.Register(model.Name, model.Email, model.Password, model.DOB);
            }
            catch (MongoWriteException)
            {
                //If the registerd email is already stored
                return BadRequest(ErrorMessages.DuplicateEmail);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.DuplicateEmail);
            }
            //Redirect the user to confirm his/her email
            return Ok(SuccessMessages.OperationSuccess);
        }
        #endregion



    }
}
