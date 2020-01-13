using SaveMyDataServer.ViewModels.Base;
using System.Collections.Generic;

namespace SaveMyDataServer.ViewModels.DataCenter
{
    public class DataCenterHomeViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// The table names and there row count
        /// </summary>
        public Dictionary<string,long> TablesAndCount{ get; set; }
        /// <summary>
        /// The database that the user is working in
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// The table that is displayed to the user
        /// </summary>
        public string DisplayedTable { get; set; }
        /// <summary>
        /// The table that selected by the user to display
        /// </summary>
        public string SelectedTable { get; set; }

        #endregion

        #region Costructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public DataCenterHomeViewModel()
        {

        }
        #endregion
    }
}
