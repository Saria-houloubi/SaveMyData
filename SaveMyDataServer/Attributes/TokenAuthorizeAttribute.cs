using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace SaveMyDataServer.Attributes
{
    /// <summary>
    /// JWT token authintication attribute
    /// </summary>
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public TokenAuthorizeAttribute()
        {
            //Set the schema to Bearer
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
        #endregion
    }
}
