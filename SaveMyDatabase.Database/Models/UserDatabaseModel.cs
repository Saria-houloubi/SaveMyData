using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace SaveMyDataServer.Database.Models.Users
{
    /// <summary>
    /// The model that holds the user information 
    /// </summary>
    public class UserDatabaseModel
    {
        #region Properties
        /// <summary>
        /// The unique id of the user
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateUTC { get; set; }
        public Guid Key { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public UserDatabaseModel()
        {

        }
        #endregion
    }
}
