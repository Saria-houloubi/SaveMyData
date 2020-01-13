using SaveMyDataServer.Database.Models.Users;
using SaveMyDataServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaveMyDataServer.ViewModels.Home
{
    /// <summary>
    /// The view model for the home page 
    /// </summary>
    public class IndexViewModel : BaseViewModel
    {

        #region Properties
        /// <summary>
        /// The databases the user created
        /// </summary>
        public List<UserDatabaseModel> Databases{ get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public IndexViewModel()
        {
            //Create the collection
            Databases = new List<UserDatabaseModel>();
        }
        #endregion
    }
}
