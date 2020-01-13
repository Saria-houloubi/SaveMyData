namespace SaveMyDataServer.SharedKernal.Models
{
    /// <summary>
    /// The model for pagination abilities
    /// </summary>
    public class PaginationModel
    {

        #region Properties
        /// <summary>
        /// The number of the current page the user is in
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// The count of records that the page should get
        /// </summary>
        public int FetchRecordCount { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public PaginationModel()
        {

        }
        #endregion
    }
}
