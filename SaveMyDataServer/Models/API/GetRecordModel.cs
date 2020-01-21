using MongoDB.Bson;
using System.Collections.Generic;

namespace SaveMyDataServer.Models
{
    public class GetRecordModel
    {
        #region Properties
        /// <summary>
        /// The database to connect to
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// The table to get the  data from
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// If any filters where supplyed
        /// </summary>
        public string Filters { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public GetRecordModel()
        {

        }
        #endregion
    }
}
