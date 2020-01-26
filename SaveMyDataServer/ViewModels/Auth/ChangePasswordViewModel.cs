using SaveMyDataServer.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace SaveMyDataServer.ViewModels.Auth
{
    /// <summary>
    /// THe change password model for requesting a change link and changing it
    /// </summary>
    public class ChangePasswordViewModel : BaseViewModel
    {
        #region Properties
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
        [Compare(nameof(Password), ErrorMessage = "Password dose not match")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// A flag to check if the change is authorized to the user
        /// </summary>
        public bool ChangeAuthorized { get; set; } = false;
        /// <summary>
        /// The change token that was issued to the user
        /// </summary>
        [Required]
        public string ChangeToken { get; set; }
        #endregion


        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public ChangePasswordViewModel()
        {
        }
        #endregion
    }
}
