using SaveMyDataServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaveMyDataServer.ViewModels.Auth
{
    /// <summary>
    /// The authintication view model to login or register a user
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Properties

        /// The email to login with
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The user password to login
        /// </summary>
        public string Password { get; set; }


        #endregion


        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public LoginViewModel()
        {
        
        }
        #endregion
    }
}
