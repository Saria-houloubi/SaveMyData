namespace SaveMyDataServer.Models.API
{
    /// <summary>
    /// The record full details model
    /// </summary>
    public class RecordDetailModel<T>
    {
        #region Properties
        /// <summary>
        /// The database the record belongs to
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// The table that the record is saved in
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// The id of the record in the database
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The record data
        /// </summary>
        public T Data { get; set; }
        public object ErrorMessage { get; internal set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public RecordDetailModel()
        {
        }
        #endregion

    }
}
