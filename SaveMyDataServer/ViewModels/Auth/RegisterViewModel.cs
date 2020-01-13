using SaveMyDataServer.ViewModels.Base;
using System;

namespace SaveMyDataServer.ViewModels.Auth
{
    /// <summary>
    /// The authintication view model to register a user
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        #region Properties

        /// <summary>
        /// The name of the user that wants to 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The user email to  with
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The password the user will  with
        /// </summary>
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// The DOB for the user to  with 
        /// </summary>
        public DateTime DOB { get; set; }

        #endregion


        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public RegisterViewModel()
        {
            //Default the data to some value
            DOB = new DateTime(1996, 11, 4);
        }
        #endregion
    }
}
