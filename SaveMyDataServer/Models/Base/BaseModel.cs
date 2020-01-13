using System;

namespace SaveMyDataServer.Models.Base
{
    /// <summary>
    /// The base model class
    /// </summary>
    public class BaseModel
    {

        #region Properties
        /// <summary>
        /// The id of the object in the database
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The date the object has been created and added to the database
        /// </summary>
        public DateTime CreationDateUTC { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public BaseModel()
        {

        }
        #endregion
    }
}
