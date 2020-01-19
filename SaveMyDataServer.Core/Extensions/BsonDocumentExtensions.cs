using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace SaveMyDataServer.Core.Extensions
{
    /// <summary>
    /// Some extensions method for <see cref="BsonDocumnet"/>
    /// </summary>
    public static class BsonDocumentExtensions
    {

        /// <summary>
        /// Gets the name of the property and its value from a bsond document
        /// </summary>
        /// <param name="document">The document to extract data from</param>
        /// <param name="prepend">if there is any prepend to it for umflattining</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetKeyNameValue(this BsonDocument document, string prepend)
        {
            Dictionary<string, string> namesAndValues = new Dictionary<string, string>();


            foreach (var item in document)
            {
                if (item.Value.IsBsonDocument)
                {
                    namesAndValues = namesAndValues.Concat(GetKeyNameValue(item.Value.AsBsonDocument, $"{item.Name}.")).ToDictionary(x => x.Key, x => x.Value);
                }
                else
                {
                    namesAndValues.Add($"{prepend}{item.Name}", item.Value.ToString());
                }
            }

            return namesAndValues;
        }

    }
}
