using MongoDB.Bson;
using System.Collections.Generic;
using System.IO;

namespace SaveMyDataServer.Core.IServices
{
    /// <summary>
    /// The service layer to work with excel files
    /// </summary>
    public interface IExcelService
    {
        byte[] CreateExcelFile(string workSheetName, List<BsonDocument> data);

    }
}
