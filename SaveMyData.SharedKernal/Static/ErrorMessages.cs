using System;
using System.Collections.Generic;
using System.Text;

namespace SaveMyDataServer.SharedKernal.Static
{
    /// <summary>
    /// A static class holds the error messages the server can give
    /// </summary>
    public static class ErrorMessages
    {
        public static string InvalidData = "The data that was sent is invaild";
        public static string MissingData = "Some importent data is missing from the request";
        public static string DuplicateEmail = "The email you used has already an account connected to it";
        public static string LoginFail = "Email or password is incorrect please check and try again";
    }
}
