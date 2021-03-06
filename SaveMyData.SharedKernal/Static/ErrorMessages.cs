﻿using System;
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
        public static string EmailNotConfirmed = "Email is not confirmed please check your inbox";
        public static string EmailNotSent = "Can not send an email to the wanted address due to invalid data plase check and try again";
        public static string NotSupported = "This feature is not supported yet!";
        public static string InvaildPermation = "You have no permision to do this operation";
        public static string ServerError = "Something wrong happened plase try again, if error still showing please contact us!";
    }
}
