using MongoDB.Bson;
using OfficeOpenXml;
using SaveMyDataServer.Core.IServices;
using System.Collections.Generic;
using System.Linq;

namespace SaveMyDataServer.Core.Services
{
    /// <summary>
    /// The implemenation class for the IExcelService interface
    /// </summary>
    public class ExcelService : IExcelService
    {
        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public ExcelService()
        {

        }
        #endregion
        /// <summary>
        /// Create an excel file from the sent data
        /// </summary>
        /// <param name="workSheetName">The name of the sheet </param>
        /// <param name="data">The lis to of data</param>
        /// <returns></returns>
        public byte[] CreateExcelFile(string workSheetName, List<BsonDocument> data)
        {
            //Create a new package
            using (var package = new ExcelPackage())
            {
                //Add the new sheet with the sent name
                var workSheet = package.Workbook.Worksheets.Add(workSheetName);
                //Check if there is any data sent
                if (data.Count > 0)
                {
                    //Loop through each item
                    for (int i = 0; i < data.Count; i++)
                    {
                        //Get the column names of this item
                        var columnNames = GetKeyNameValue(data[i], "");
                        foreach (var item in columnNames)
                        {
                            AddNewRow(workSheet, item, i + 2);
                        }
                        workSheet.Cells.AutoFitColumns(0);
                    }
                }

                return package.GetAsByteArray();
            }

        }

        public void AddNewRow(ExcelWorksheet sheet, KeyValuePair<string, string> keyValue, int currentRow)
        {
            for (int i = 1; i <= sheet.Cells.Columns; i++)
            {
                var cell = sheet.Cells[1, i];
                if (cell.Value != null)
                {
                    if (cell.Value.ToString() == keyValue.Key)
                    {
                        sheet.Cells[currentRow, i].Value = keyValue.Value;
                        break;
                    }
                }
                else
                {
                    cell.Value = keyValue.Key;
                    sheet.Cells[currentRow, i].Value = keyValue.Value;
                    break;
                }
            }
        }

        public Dictionary<string, string> GetKeyNameValue(BsonDocument document, string prepend)
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
