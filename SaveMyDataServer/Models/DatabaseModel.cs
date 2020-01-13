using SaveMyDataServer.Models.Base;
using System;

namespace SaveMyDataServer.Models
{
    /// <summary>
    /// The model for the databsae information
    /// </summary>
    public class DatabaseModel : BaseModel
    {

        #region Properties
        /// <summary>
        /// The full unique name for the database
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// The name of the database the user entered
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The private key for the user to work with the database
        /// </summary>
        public Guid Key { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public DatabaseModel()
        {

        }
        #endregion
    }
}
