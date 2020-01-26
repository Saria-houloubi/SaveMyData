using SaveMyDataServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <summary>
        /// The email to login with
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// The user password to login
        /// </summary>
        [Required]
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
