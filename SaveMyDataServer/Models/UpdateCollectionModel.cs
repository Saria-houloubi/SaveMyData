using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaveMyDataServer.Models
{
    public class UpdateCollectionModel
    {
        #region Propertise
        /// <summary>
        /// The old name of the colleciton
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The new name to edit the collection to
        /// </summary>
        public string NewName { get; set; }
        /// <summary>
        /// THe database the collection is in
        /// </summary>
        public string Database { get; set; }
        #endregion
        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public UpdateCollectionModel()
        {

        }
        #endregion
    }
}
