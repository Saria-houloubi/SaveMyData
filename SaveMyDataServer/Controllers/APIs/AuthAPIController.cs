using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaveMyDataServer.SharedKernal.Static;
using SaveMyDataServer.Controllers.APIs.Base;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Models.API;
using SaveMyDataServer.ViewModels.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            response.Token = GenerateJwtToken(response);

            //Redirect the user to the home page
            return Ok(response);
        }

        #endregion


        #region Methods
        /// <summary>
        /// Generates a token for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateJwtToken(UserAPIModel user)
        {
            //Get the jwt values from the configuration file
            var audience = Configuration["Jwt:Audience"];
            var issuer = Configuration["Jwt:Issuer"];
            var SecretKey = Configuration["Jwt:SecretKey"];

            //Set the claims for the token
            var claims = new[]
            {
                //Set a unique key for the clam
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                
                //Set the username for use in the httpcontext
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.FullName),
                //The name ientifier that will be assigned before any database name owned by the user
                new Claim(ClaimTypes.PrimarySid,user.Id.ToString()),

            };
            //Create the credentials that are used for the token
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            //Create the jwt token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMonths(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
