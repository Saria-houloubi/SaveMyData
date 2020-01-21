using MongoDB.Bson;
using MongoDB.Driver;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Database.IServices;
using SaveMyDataServer.Database.Models.Users;
using SaveMyDataServer.Database.Static;
using SaveMyDataServer.SharedKernal;
using System;
using System.Threading.Tasks;

namespace SaveMyDataServer.Core.Services
{
    /// <summary>
    /// The implmentation of <see cref="IAuthUserService"/>
    /// </summary>
    public class AuthUserService : IAuthUserService
    {
        #region Properties
        /// <summary>
        /// The name of the hosting server
        /// </summary>
        public const string ServerName = "savemydata.sariahouloubi.com";//https://localhost:5001
        #endregion
        #region Services
        /// <summary>
        /// The database Access layer
        /// </summary>
        public IDatabaseService DatabaseService { get; private set; }
        /// <summary>
        /// The mail service to send confirmation emails
        /// </summary>
        public IMailService MailService { get; private set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public AuthUserService(IDatabaseService databaseService, IMailService mailService)
        {
            //Assign the injected value
            DatabaseService = databaseService;
            MailService = mailService;

        }
        #endregion
        public async Task<UserModel> Register(string fullName, string email, string password, DateTime DOB)
        {
            //Set to the main database until a user sends the database name
            DatabaseService.InitilizeDatabase(DatabaseNames.Main);
            //Try to add the user into the database
            var user = await DatabaseService.AddRecord<UserModel>(new UserModel
            {
                FullName = fullName,
                Email = email,
                DOB = DOB,
                Password = PasswordHashHelpers.HashPassword(password),
                ConfirmMailId = Guid.NewGuid(),
                IsMailConfirmed = false
            }, DatabaseTableNames.Users);

            //Send the confirmation emial to the user
            await SendConfirmationEmail(user.Email);

            return user;
        }

        public async Task<UserModel> SendConfirmationEmail(string userEmail)
        {
            //Set to the main database until a user sends the database name
            DatabaseService.InitilizeDatabase(DatabaseNames.Main);

            //Get the user from the database to make sure that it has been registerd
            var user = await DatabaseService.GetRecord<UserModel>(MongoTableBaseFieldNames.Email, userEmail, DatabaseTableNames.Users);
            //If the user is found
            if (user != null)
            {
                //Check if the email has not yet been confirmed
                if (!user.IsMailConfirmed)
                {
                    await MailService.SendEmail(new Models.EmailModel
                    {
                        ContentHTML = $"<h1>Thank you for registering with save my data</h1><p>To keep our data clean we need you to confirm your " +
                        $"email by clicking <a href={ServerName}/auth/emailconfirmaiton?id={user.Id}&id2={user.ConfirmMailId}>here</a>" +
                        $"<h5>Welcome to Save my data!</h5>",
                        Subject = "Email confirmation",
                        UserEmail = user.Email,
                        UserFullName = user.FullName
                    });
                }
            }

            return user;
        }
        public async Task<UserModel> ConfirmUserPassword(string email, string passowrd)
        {
            //Set to the main database until a user sends the database name
            DatabaseService.InitilizeDatabase(DatabaseNames.Main);
            //Get the user from the database
            var user = await DatabaseService.GetRecord<UserModel>(MongoTableBaseFieldNames.Email, email, DatabaseTableNames.Users);
            //If the email is not vaild or the user provided the wrong password then just return false
            if (user == null || !PasswordHashHelpers.VertifyPassword(user.Password, passowrd))
            {
                return null;
            }
            return user;
        }

        public async Task<bool> ConfirmUserEmail(Guid id, Guid mailId)
        {
            //Set to the main database until a user sends the database name
            DatabaseService.InitilizeDatabase(DatabaseNames.Main);
            //Update the user with the sent id
            var result = await DatabaseService.UpdateRecords(Builders<BsonDocument>.Filter.Eq(MongoTableBaseFieldNames.Id, id), Builders<BsonDocument>.Update.Set(MongoTableBaseFieldNames.IsMailConfirmed, true), DatabaseTableNames.UserDatabases);

            return result.ModifiedCount == 1;
        }
    }
}
