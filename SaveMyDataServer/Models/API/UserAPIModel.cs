using SaveMyDataServer.Database.Models.Users;

namespace SaveMyDataServer.Models.API
{
    /// <summary>
    /// The model for the api response
    /// </summary>
    public class UserAPIModel : UserModel
    {

        #region Properties
        /// <summary>
        /// The JWT token for the user
        /// </summary>
        public string Token { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public UserAPIModel()
        {

        }
        #endregion
    }
}
