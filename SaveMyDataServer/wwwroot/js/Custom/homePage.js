
//
//Initializes the the table once it is selectd
//
function initializeTable() {

    //Get the count of rows in that table
    var tableRowCount = document.getElementById("database-count").innerText;
    //Create the page
    createHTMLPagination(tableRowCount, recordsCountInTable, getTableData);
    //Load the table data
    getTableData(1);
}
//
//Gets the data that is saved in the selected table 100 element at a time
//
function getTableData(pageNumber) {
    //Get the part where the table should be inseterd in
    var tableDiv = document.getElementById("database-table-holder");
    //Set a loading spinner until the data is fetched
    tableDiv.innerHTML = getLoadingGrowingSpinner();
    $.getJSON(
        dataEndPoint,
        data = {
             "pagination": getPaginationObject(pageNumber, recordsCountInTable)
        },
        function (result) {
            //Set the table data
            tableDiv.innerHTML = result;
        }
    ).fail(function (error) {
        setTopPageAlertMessage(error.responseText, 'alert-danger');
    });
}


