namespace SaveMyDataServer.Static
{
    /// <summary>
    /// Urls for Ajax requests
    /// </summary>
    public static class ServerAjaxURLs
    {
        //Restful enpoint
        public const string DatabaseEndpoint = "/database";
        public const string CollectionEndPoint = "/collection";
        public const string RecordEndPoint = "/record";
        //DataCenter controller
        public const string ExportData =          "/Datacenter/exportData";
        //Auth controller
        public const string EmailConfirmation = "/Auth/EmailConfirmation";
    }   
}
