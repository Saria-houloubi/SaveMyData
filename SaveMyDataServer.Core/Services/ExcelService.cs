using MongoDB.Bson;
using OfficeOpenXml;
using SaveMyDataServer.Core.Extensions;
using SaveMyDataServer.Core.IServices;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SaveMyDataServer.Core.Services
{
    /// <summary>
    /// The implemenation class for the IExcelService interface
    /// </summary>
    public class ExcelService : IExcelService
    {

        #region Properties
        /// <summary>
        /// The normal font size to use
        /// </summary>
        public float FontSizeNormal { get; set; } = 11;
        public float FontSizeLarg => FontSizeNormal * 1.2f;

        public string HeaderFontFamily { get; set; } = "Helvetica LT Std Cond Blk";
        #endregion
        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public ExcelService()
        {

        }
        #endregion
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
                        var columnNames =  data[i].GetKeyNameValue("");
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

            //Loop throw the cells in the sheet
            for (int i = 1; i <= sheet.Cells.Columns; i++)
            {
                //Get teh header cell that we are on 
                var cell = sheet.Cells[1, i];
                //If that is not the end cell
                if (cell.Value != null)
                {
                    if (cell.Value.ToString() == keyValue.Key)
                    {
                        //Assgin the value in the wanted row
                        sheet.Cells[currentRow, i].Value = keyValue.Value;
                        break;
                    }
                }
                else
                {
                    //if header is not added then add it
                    cell.Value = keyValue.Key;
                    //Set the header styling
                    StyleCells(cell, FontSizeLarg, HeaderFontFamily, Color.LightGray);
                    //Just add the value
                    sheet.Cells[currentRow, i].Value = keyValue.Value;
                    break;
                }

            }

        }

        public void StyleCells(ExcelRange cells, float fontSize, string fontFamily, Color fillColor)
        {
            //Loop throw each cell
            foreach (var item in cells)
            {
                item.Style.Font.SetFromFont(new Font(fontFamily, fontSize));
                item.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.MediumGray;
                item.Style.Fill.BackgroundColor.SetColor(fillColor);
            }
        }
     

    }
}
