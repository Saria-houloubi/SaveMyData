namespace SaveMyDataServer.Static
{
    /// <summary>
    /// What is the count of elements to show in each page depaniding on the size of it 
    /// </summary>
    public static class ServerPaginationCount
    {
        /// <summary>
        /// The amount records to get in a large page
        /// </summary>
        public const int LargePage = 100;
        /// <summary>
        /// The amount of records to get in a normal page
        /// </summary>
        public const int NormalPage = 20;
    }
}
