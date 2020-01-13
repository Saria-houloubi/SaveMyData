using System;
using System.Collections.Generic;
using System.Text;

namespace SaveMyDataServer.Core.Models
{
    /// <summary>
    /// The email model the holds the information that are going to be sent to the user
    /// </summary>
    public class EmailModel
    {

        #region Properties
        /// <summary>
        /// The email subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The name of the user the email is sent to
        /// </summary>
        public string UserFullName { get; set; }
        /// <summary>
        /// The email of the user to send it to
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// The content of the email
        ///   TODO:  for this moment it is going to be plain string but will move to html template
        /// </summary>
        public string Content { get; set; }
        public string ContentHTML { get; set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public EmailModel()
        {

        }
        #endregion
    }
}
