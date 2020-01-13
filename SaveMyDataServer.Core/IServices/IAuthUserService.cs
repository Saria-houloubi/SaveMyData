using SaveMyDataServer.Database.Models.Users;
using System;
using System.Threading.Tasks;

namespace SaveMyDataServer.Core.IServices
{
    /// <summary>
    /// The authintication user service 
    ///     register and check for passwrods
    /// </summary>
    public interface IAuthUserService
    {

        /// <summary>
        /// Registers a user into the database
        /// </summary>
        /// <param name="email">The user email for confirmation</param>
        /// <param name="password">The user password</param>
        /// <param name="DOB">The user date of birth</param>
        /// <returns></returns>
        Task<UserModel> Register(string fullName, string email, string password, DateTime DOB);
        /// <summary>
        /// Checks the user password with the sent email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passowrd"></param>
        /// <returns></returns>
        Task<UserModel> ConfirmUserPassword(string email, string passowrd);
        /// <summary>
        /// Confirms the user email 
        /// </summary>
        /// <param name="id">The id of the user that is confirming the email</param>
        /// <param name="sentMail">The id to check that is unique to each email sent</param>
        /// <returns></returns>
        Task<UserModel> ConfirmUserEmail(Guid id,Guid sentMail);

    }
}
