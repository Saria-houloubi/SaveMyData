
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
        `${databaseEndPoint}/get`,
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

//
//Ask the user to make sure before deleting
//
function askConfirmation(element, id) {

    //Get the previous selected row
    var previousSelectedRows = document.getElementsByClassName('selected-row');
    //Check if there was any selected
    if (previousSelectedRows.length !== 0) {
        //Unselect all of them
        //Should always be one but for safty
        for (var i = 0; i < previousSelectedRows.length; i++) {
            previousSelectedRows[i].classList.remove('selected-row');
        }
    }
    //Mark the row as selected
    element.parentElement.parentElement.classList.add('selected-row');
    //Set teh id value
    $('#delete-record-id').val(id);
    //show the confirmation model
    $('#delete-confirmation-modal').modal('show');
}
///
//Deletes a record from the database
// element: the delete button to disable untile the server response is given
//
function deleteRecord(element) {
    //Create the spinner
    var loadingSpinner = disableAndShowSpinner(element, 'text-danger');

    if (loadingSpinner !== null) {
        //Send the delete request
        $.ajax(
            `${databaseEndPoint}/delete`, {
                data: {
                    id: document.getElementById('delete-record-id').value
                },
                method: 'DELETE',
                success: function (data, status, jqXHR) {
                    //Remove the row from the grid
                    document.getElementsByClassName('selected-row')[0].remove();

                    //decrement the table row count
                    var counter = document.getElementById('database-count');
                    counter.innerText = parseInt(counter.innerText, 10) - 1;
                    //Show sucess message
                    setTopPageAlertMessage(data, 'alert-success');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    setTopPageAlertMessage(errorThrown, 'alert-danger');
                }
            }
        ).always(function () {
            //Hide the model from the user
            $('#delete-confirmation-modal').modal('hide');
            enableAndHideSpinner(element, loadingSpinner);
        });
    }
}

