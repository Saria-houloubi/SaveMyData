using MongoDB.Bson;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace SaveMyDataServer.Core.IServices
{
    /// <summary>
    /// The service layer to work with excel files
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// Create an excel file from the sent data
        /// </summary>
        /// <param name="workSheetName">The name of the sheet </param>
        /// <param name="data">The lis to of data</param>
        /// <returns></returns>
        byte[] CreateExcelFile(string workSheetName, List<BsonDocument> data);

        /// <summary>
        /// Adds a new row the sent keyvalue paris as cell header and value
        /// </summary>
        /// <param name="sheet">The sheet to add the row to</param>
        /// <param name="keyValue">The row header and value pairs</param>
        /// <param name="currentRow">The current row to add it in</param>
        void AddNewRow(ExcelWorksheet sheet, KeyValuePair<string, string> keyValue, int currentRow);

    }

}
