namespace SaveMyDataServer.Static
{
    /// <summary>
    /// Some redirect URLs for the server
    /// </summary>
    public static class ServerRedirectsURLs
    {
        public const string Index = "/";
        /// <summary>
        /// The path to home page
        /// </summary>
        public const string Home = "/home";
        public const string Guid = "/home/guid";
        public const string Contact = "/home/contact";
        /// <summary>
        /// The path to the login page for unauthorized users
        /// </summary>
        public const string Login = "/auth/login";
        public const string Logout = "/auth/logout";
        public const string Register = "/auth/register";
        /// <summary>
        /// The path to authinticate an email
        /// </summary>
        public const string EmailAuth = "/auth/emailauth";

    }
}
