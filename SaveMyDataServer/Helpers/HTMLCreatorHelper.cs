using MongoDB.Bson;
using SaveMyDataServer.SharedKernal;
using SaveMyDataServer.SharedKernal.Enums;
using System.Collections.Generic;
using System.Text;

namespace SaveMyDataServer.Helpers
{
    /// <summary>
    /// A static class for some html tags creation helper
    /// </summary>
    public static class HTMLCreatorHelper
    {

        /// <summary>
        /// Gets an html table from bson documents with no headers each table cell contains the name and value
        /// </summary>
        /// <param name="records">The elements to change into a table</param>
        /// <returns></returns>
        public static string GetMongoTableBson(List<BsonDocument> records, string tableName, bool enableEdit = true)
        {
            //Create the first tag for the table
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.AppendLine("<table class=\"table\">");
            //The row count for the table start from 1 to records.count
            var rowCount = 1;
            //The id for each row
            var rowId = $"{tableName}-r-{rowCount}";
            //Go throw the records
            foreach (var item in records)
            {
                //Set the id
                rowId = $"{tableName}-r-{rowCount++}";
                //Creeat the rows
                htmlTable.AppendLine(CreateExpandableTableRow(item, rowId, enableEdit: enableEdit));
            }

            return htmlTable.ToString();
        }

        /// <summary>
        /// Creates an expandalbe custom row
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rowId"></param>
        /// <param name="isCollapse"></param>
        /// <param name="enableEdit"></param>
        /// <returns></returns>
        public static string CreateExpandableTableRow(BsonDocument element, string rowId, bool isCollapse = false, bool enableEdit = true, bool exportCSV = true)
        {
            //Check if the row has any classes
            var rowClass = isCollapse ? "class = \"collapse table-secondary\"" : "";
            //Create the row variable
            string row = $"<tr id={rowId} {rowClass}>";
            //If the edit option is enabled then
            if (enableEdit && !isCollapse)
            {
                //add the edit button to the beginning of the row
                row = string.Concat(row, "<td><a class=\"btn\" ><i class=\"far fa-edit text-info\"></i></a></td>");
            }
            //The expanders rows
            string rowExpander = "";
            //The number of expandation in the same row depth
            int rowExpanderCount = 1;
            //loop throw the element for its properties
            foreach (var item in element.Elements)
            {
                //If the items contains more data inside it
                if (item.Value.IsBsonDocument)
                {
                    //the inner row id
                    var innerRowId = string.Concat(rowId, $"-{rowExpanderCount++}");
                    //add a dropdown button to expand and show more data
                    row = string.Concat(row, $"<td ><a href=\"#{innerRowId}\" data-toggle=\"collapse\" role=\"button\" aria-expanded=\"false\" class=\"text-secondary\"><i class=\"far fa-caret-square-down align-middle\"></i><b class=\"m-1\">{item.Name}</b></a></td>");
                    //Add the inner row to it
                    rowExpander = string.Concat(rowExpander, CreateExpandableTableRow(item.Value.AsBsonDocument, innerRowId, isCollapse: true));
                }
                else
                {
                    var fieldName = item.Name == MongoTableBaseFieldNames.Id ? "Id" : item.Name;
                    //Add the table data cell for the property
                    row = string.Concat(row, $"<td><b>{fieldName}</b>: <span>{item.Value}</span></td>");
                }
            }
            //Add the export csv button
            if (exportCSV && !isCollapse)
            {
                row = string.Concat(row, $"<td><a class=\"btn\" onclick=\"exportRecord('{element[0]}','{SupportedExportFileTypes.CSV}')\" ><i class=\"fas fa-file-csv text-info\"></i></a></td>");
            }
            //Close top most row
            row = string.Concat(row, "</tr>");
            //Add the expandation at the end
            row = string.Concat(row, rowExpander);
            return row;
        }


        /// <summary>
        /// Creates an HTML table 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records">The records to fill the table with</param>
        /// <param name="columns">The column names for the records</param>
        /// <returns></returns>
        public static string CreateTable<T>(List<T> records, string[] columns)
        {
            //Create the first tag for the table
            string htmlTable = "<table class=\"table\"><thead class='thead-light'><tr>";
            //Add the table columns
            foreach (var col in columns)
            {
                htmlTable = string.Concat(htmlTable, $"<td>{col}</td>");
            }
            //Close the row
            htmlTable = string.Concat(htmlTable, $"</tr></thead>");
            //Get the sent type properties
            var typeProperties = typeof(T).GetProperties();
            //add the rows
            foreach (var item in records)
            {
                //Create the row
                htmlTable = string.Concat(htmlTable, $"<tr>");
                //Loop throw the object properties
                foreach (var prop in typeProperties)
                {
                    //Get the property value
                    htmlTable = string.Concat(htmlTable, "<td>", prop.GetValue(item, null), "</td>");
                }


                htmlTable = string.Concat(htmlTable, $"</tr>");
            }
            //Close the table
            htmlTable = string.Concat(htmlTable, $"</table>");

            return htmlTable;
        }
    }
}
