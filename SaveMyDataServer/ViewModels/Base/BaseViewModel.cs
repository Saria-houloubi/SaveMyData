namespace SaveMyDataServer.ViewModels.Base
{
    /// <summary>
    /// The base view model for cross functions and data
    /// </summary>
    public class BaseViewModel
    {

        #region Properties
        /// <summary>
        /// A message error if there is
        /// </summary>
        public string ErrorMessage { get; set; }
        #endregion
        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public BaseViewModel()
        {

        }
        #endregion
    }
}
