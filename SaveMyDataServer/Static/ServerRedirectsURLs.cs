﻿namespace SaveMyDataServer.Static
{
    /// <summary>
    /// Some redirect URLs for the server
    /// </summary>
    public static class ServerRedirectsURLs
    {
        public const string MainHost = "https://savemydata.sariahouloubi.com";
        public const string DevHost = "https://localhost:5001";
        public const string Index = "/";

        public const string Home =    "/home";
        public const string ApiGuid =    "/home/apiguid";
        public const string Contact = "/home/contact";

        public const string Login =     "/auth/login";
        public const string Logout =    "/auth/logout";
        public const string Register =  "/auth/register";
        public const string EmailAuth = "/auth/emailauth";
        public const string ChangePassword = "/auth/changepassword";
        



    }
}
