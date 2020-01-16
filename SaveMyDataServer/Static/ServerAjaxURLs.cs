namespace SaveMyDataServer.Static
{
    /// <summary>
    /// Urls for Ajax requests
    /// </summary>
    public static class ServerAjaxURLs
    {
        //Home Conroller
        public const string GetDatabaseTable = "/Home/GetDatabaseData";
        //DataCenter controller
        public const string GetCollectionTabble = "/DataCenter/GetTableData";
        public const string ExportData =          "/Datacenter/exportData";
        //Auth controller
        public const string EmailConfirmation = "/Auth/EmailConfirmation";
    }   
}
