namespace SaveMyDataServer.Core.Sercret
{
    /// <summary>
    /// Secret data for connecting to the mail server
    /// </summary>
    public static class SecretMailInformation
    {
        /// <summary>
        /// The nama of the team the email is going to be sent in
        /// </summary>
        public const string TeamName = "Save my data team";
        /// <summary>
        /// The email address that is going to be sending the mails from
        /// </summary>
        public const string EmailAddress = "contact@sariahouloubi.com";
        /// <summary>
        /// The email password to connect with
        /// </summary>
        public const string Password = "JnY@nAAk5RmJJkn";
        /// <summary>
        /// The server port to connect to
        /// </summary>
        public const int Port = 465;
        /// <summary>
        /// A flag to use ssl
        /// </summary>
        public const bool UserSSL = true;
        /// <summary>
        /// THe hosting server provider
        /// </summary>
        public const string ServerName = "smtp.mail.us-east-1.awsapps.com";
    }
}
