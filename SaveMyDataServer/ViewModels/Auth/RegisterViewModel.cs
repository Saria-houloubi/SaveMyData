using SaveMyDataServer.ViewModels.Base;
using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        /// <summary>
        /// The user email to  with
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// The password the user will  with
        /// </summary>
        [Required]
        [RegularExpression("^(?=.*[A-Z]+)(?=.*[a-z]+)(?=.*[0-9]+).{8,}$", ErrorMessage = "Must contain uppercase, lowercase and digits with length over 7 characters")]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password),ErrorMessage = "Password dose not match")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// The DOB for the user to  with 
        /// </summary>
        [Required]
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
