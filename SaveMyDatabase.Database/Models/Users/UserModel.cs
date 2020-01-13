using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace SaveMyDataServer.Database.Models.Users
{
    /// <summary>
    /// The model that holds the user information 
    /// </summary>
    public class UserModel
    {
        #region Properties
        /// <summary>
        /// The unique id of the user
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        /// <summary>
        /// The id of the email for the confirmation
        /// </summary>
        [JsonIgnore]
        public Guid ConfirmMailId { get; set; }
        /// <summary>
        /// A flag checks if the user email is confirmed
        /// </summary>
        public bool IsMailConfirmed { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public UserModel()
        {

        }
        #endregion
    }
}
