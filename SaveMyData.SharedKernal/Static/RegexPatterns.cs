namespace SaveMyDataServer.SharedKernal.Static
{
    /// <summary>
    /// Regex patterns for global use
    /// </summary>
    public static class RegexPatterns
    {
        /// <summary>
        /// Gets any spaces in the end of all the properties and values of the json string
        /// </summary>
        public const string SpaceJsonEnd = "\\s+(?=\")";
        /// <summary>
        /// Gets any spaces in the start of all the properties and values of the json string
        /// </summary>
        public const string SpaceJsonStart = "(?<=\")\\s+";
    }
}
