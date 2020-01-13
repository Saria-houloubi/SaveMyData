using SaveMyDataServer.Models.Base;
using System;

namespace SaveMyDataServer.Models
{
    /// <summary>
    /// The model to be sent to delete a record from the database
    /// </summary>
    public class DeteleRecordModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// The table name the record is on
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// The database the user is working in
        /// </summary>
        public string Database { get; set; }
        #endregion


        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public DeteleRecordModel()
        {

        }
        #endregion
    }
}
