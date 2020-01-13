namespace SaveMyDataServer.Database.Static
{

    /// <summary>
    /// The names of the base tables for our database
    /// </summary>
    public static class DatabaseTableNames
    {
        /// <summary>
        /// The user table hold <see cref="UserModel"/>
        /// </summary>
        public static string Users = "users";
        public static string UserDatabases = "userdatabases";
    }
}
