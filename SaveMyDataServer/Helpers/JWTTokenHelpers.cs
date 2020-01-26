using Microsoft.IdentityModel.Tokens;
using SaveMyDataServer.Models.API;
using SaveMyDataServer.Sercret;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaveMyDataServer.Helpers
{
    /// <summary>
    /// A helper class to create jwt tokens
    /// </summary>
    public static class JWTTokenHelpers
    {
        /// <summary>
        /// Generates a token for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(UserAPIModel user,DateTime expires)
        {
            //Get the jwt values from the configuration file
            var audience = SecretJWTTokenInformation.Audience;
            var issuer = SecretJWTTokenInformation.Issuer;
            var SecretKey = SecretJWTTokenInformation.SecretKey;

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
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
