//The table the user has selected to show
var selectedTable = "";
//
//Initializes the the table once it is selectd
//
function initializeTable() {

    //Get the selected table from the select tag
    selectedTable = document.getElementById("dbTable").value;

    //set all the table holder name
    var holders = document.getElementsByClassName('selected-table-name-holder');
    for (var i = 0; i < holders.length; i++)
        holders[i].setAttribute('value', selectedTable);

    //Set the selected title
    document.getElementById("selected-table-title").innerText = selectedTable;
    //Get the count of rows in that table
    var tableRowCount = document.getElementById(selectedTable.concat("-count")).innerText;
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
    var tableDiv = document.getElementById("selected-table-content");
    //Set a loading spinner until the data is fetched
    tableDiv.innerHTML = getLoadingGrowingSpinner();
    $.getJSON(
        `${collectionEndPoint}/get`,
        data = {
            "table": selectedTable, "database": workingDatabase, "pagination": getPaginationObject(pageNumber, recordsCountInTable)
        },
        function (result) {
            //Set the table data
            tableDiv.innerHTML = result;
            //Bind the edit buttons into the modal
            bindEditButtonModel();
        }
    ).fail(function (error) {
        setTopPageAlertMessage(error.responseText, 'alert-danger');
    });
}
//
//Creats a row for that holds the the fields names and thier values
// field : the name of the field in db (can not be edited)
// value : the value stored in db
//
function createFormFiledValueRow(field, value) {
    //Create the row outter div
    var row = document.createElement('div');
    //add the needed class
    row.classList.add('input-group', 'mb-3');
    //Create the prepend inner field text div
    var fieldDiv = document.createElement('div');
    fieldDiv.classList.add('input-group-prepend');
    //Crate the span for the field text
    var fieldInput = document.createElement('input');
    fieldInput.classList.add('form-control');
    fieldInput.setAttribute('type', 'text');
    fieldInput.setAttribute('value', field);

    //Add the span to the inneer div
    fieldDiv.appendChild(fieldInput);
    //Create the field input text element
    var valueInput = document.createElement('input');
    valueInput.classList.add('form-control');
    valueInput.setAttribute('type', 'text');
    valueInput.value = value;

    //Create delete field button
    var deleteButton = document.createElement('button');
    deleteButton.classList.add('btn', 'btn-outline-danger', 'ml-2');
    deleteButton.setAttribute('onclick', 'deleteRow(this)');
    deleteButton.innerText = 'X';

    //Add to the row element
    row.appendChild(fieldDiv);
    row.appendChild(valueInput);
    row.appendChild(deleteButton);

    return row;
}
//
//Delets the row that is parent to the element
// element : the DOM element that did the action
//
function deleteRow(element) {
    element.parentElement.remove();
}

//
//The function that bind the edit button to the model
//
function bindEditButtonModel() {
    //Get all the edit buttons that are shown in the page
    var editButtons = document.getElementsByClassName("fa-edit");

    //Loop throw the items
    for (var i = 0; i < editButtons.length; i++) {
        //get the parent link element
        var linkParent = editButtons[i].parentElement;
        $(linkParent).bind('click', showEditModal);
    }
}
//
//Shows the edit modal and fills it with information
// element: the DOM elemen that sent the request
//
function showEditModal(element, newElment = false) {
    //get the model object
    var editModalGrid = document.getElementById("edit-record-modal-body-grid");
    //clear anything in the modal
    editModalGrid.innerHTML = "";
    if (!newElment) {
        //Stop the default action of it
        element.preventDefault();
        //Move from the a -> td -> tr
        var tableRow = element.currentTarget.parentElement.parentElement;
        tableRowToGrid(editModalGrid, tableRow, 1, "");
        //Show the edit buttons and hide the add
        document.getElementById('edit-mode-buttons').style.display = "block";
        document.getElementById('add-mode-buttons').style.display = "none";

    } else {
        //Show the add buttons and hide the edit
        document.getElementById('edit-mode-buttons').style.display = "none";
        document.getElementById('add-mode-buttons').style.display = "block";
    }
    document.getElementById('record-table-name').setAttribute('value', document.getElementById('selected-table-title').innerText);
    document.getElementById('record-database-name').setAttribute('value', document.getElementById('selected-database-name').innerText);
    //Show the model to the user
    $('#edit-record-modal').modal('show');
}
//
//Moves a table row to agrid key value pair
// grid: the grid to set the data in
// tableRow: the row to get the key values from
// startIndex: the start index in the table row to move to the grid
// prependFieldText: any value to preprend the field text part
//
function tableRowToGrid(grid, tableRow, startIndex, prependFieldText) {
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

    //The last element as the main row has an export button at the end
    var endIndex = prependFieldText === "" ? tableRow.cells.length - 1 : tableRow.cells.length;
    //Move throw the row cells
    for (var j = startIndex; j < endIndex; j++) {
        //Get the cell
        var cell = tableRow.cells[j];
        //Get the first child of the cell
        var cellFirstChild = cell.firstElementChild;
        //If them first child is a link to another row
        if (cellFirstChild.getAttribute('href') !== null) {
            //add the row and prepend its fields with the link text
            tableRowToGrid(grid, document.querySelector(cellFirstChild.getAttribute('href')), 0, cellFirstChild.innerText + '.');
            continue;
        }
        var field = prependFieldText + cellFirstChild.innerText;
        var value = cell.children[1].innerText;

        grid.appendChild(createFormFiledValueRow(field, value));
    }

    //Do it only once at the first row
    if (prependFieldText === "") {
        //Set the id input to disabled
        var idInpute = grid.getElementsByTagName('input')[0];
        idInpute.setAttribute('disabled', true);
        //Set the row as selected
        tableRow.classList.add('selected-row');
        //Set the hidden input for delete and update
        document.getElementById('record-id').setAttribute('value', idInpute.value);
        document.getElementById('delete-record-id').setAttribute('value', idInpute.value);
    }
}

//
// Adds a new filed value row with empty values
//
function addNewFieldValueEmptyRow() {
    //Create the row
    var row = createFormFiledValueRow("", "");
    //Get the edit molel
    var editModal = document.getElementById("edit-record-modal-body-grid");
    //Append the row to the end of the model
    editModal.appendChild(row);
}

//
//Ask the user to make sure before deleting
//
function askConfirmation(id = null) {
    if (id !== null) {
        document.getElementById('delete-record-id').setAttribute('value', id);
    }
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
            `${recordEndPoint}/delete`, {
                data: {
                    Id: document.getElementById('record-id').value,
                    Table: document.getElementById('record-table-name').value,
                    Database: document.getElementById('record-database-name').value
                },
                method: 'DELETE',
                success: function (data, status, jqXHR) {
                    //Remove the row from the grid
                    document.getElementsByClassName('selected-row')[0].remove();
                    //Hide the model from the user
                    $('#edit-record-modal').modal('hide');

                    //decrement the table row count
                    var counter = document.getElementById(document.getElementById('record-table-name').value + '-count');
                    counter.innerText = parseInt(counter.innerText, 10) - 1;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    setTopPageAlertMessage(errorThrown, 'alert-danger');
                }
            }
        ).always(function () {
            enableAndHideSpinner(element, loadingSpinner);
        });
    }
}

//
//Edits a record in the database
//
function editRecord(element) {
    //Create the spinner
    var loadingSpinner = disableAndShowSpinner(element, 'text-info');
    if (loadingSpinner !== null) {

        //Ready up the data part
        //Get the grid modal
        var grid = document.getElementById('edit-record-modal-body-grid');
        //The data object that will hold the values
        var jsonData = {};
        //Loop throw the grid items and add the key value pair
        for (var i = 0; i < grid.children.length; i++) {
            var elementGrid = grid.children[i];
            unflattenJSON(jsonData, elementGrid.getElementsByClassName('form-control')[0].value.split('.').reverse(), elementGrid.getElementsByClassName('form-control')[1].value);
        }
        //Send the delete request
        $.ajax(
            `${recrodEndPoint}/put`, {
                data: {
                    Id: document.getElementById('record-id').value,
                    Table: document.getElementById('record-table-name').value,
                    Database: document.getElementById('record-database-name').value,
                    data: JSON.stringify(jsonData)
                },
                method: 'PUT',
                success: function (data, status, jqXHR) {
                    //Reupdate the table content
                    getTableData(document.getElementsByClassName('pagination-selected')[0].innerText);
                    //Hide the model from the user
                    $('#edit-record-modal').modal('hide');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    setTopPageAlertMessage(errorThrown, 'alert-danger');
                }
            }
        ).always(function () {
            enableAndHideSpinner(element, loadingSpinner);
        });
    }
}


//
//adds a record to the database
//
function addRecord(element) {
    //Create the spinner
    var loadingSpinner = disableAndShowSpinner(element, 'text-success');
    if (loadingSpinner !== null) {

        //Ready up the data part
        //Get the grid modal
        var grid = document.getElementById('edit-record-modal-body-grid');
        //The data object that will hold the values
        var jsonData = {};
        //Loop throw the grid items and add the key value pair
        for (var i = 0; i < grid.children.length; i++) {
            var elementGrid = grid.children[i];
            unflattenJSON(jsonData, elementGrid.getElementsByClassName('form-control')[0].value.split('.').reverse(), elementGrid.getElementsByClassName('form-control')[1].value);
        }
        //Send the delete request
        $.ajax(
            `${recrodEndPoint}/post`, {
                data: {
                    Id: null,
                    Table: document.getElementById('record-table-name').value,
                    Database: document.getElementById('record-database-name').value,
                    data: JSON.stringify(jsonData)
                },
                method: 'POST',
                success: function (data, status, jqXHR) {
                    //Reupdate the table content
                    getTableData(document.getElementsByClassName('pagination-selected')[0].innerText);
                    //Hide the model from the user
                    $('#edit-record-modal').modal('hide');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    setTopPageAlertMessage(errorThrown, 'alert-danger');
                }
            }
        ).always(function () {
            enableAndHideSpinner(element, loadingSpinner);
        });
    }
}

//
//Deletes a table from thre database
// element: the button to disable
//
function deleteTable(element) {
    //Create the spinner
    var loadingSpinner = disableAndShowSpinner(element, 'text-danger');

    if (loadingSpinner !== null) {
        //Send the delete request
        $.ajax(
            `${collectionEndPoint}/delete`, {
                data: {
                    Id: null,
                    Table: selectedTable,
                    Database: workingDatabase
                },
                method: 'DELETE',
                success: function (data, status, jqXHR) {
                    location.reload();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    setTopPageAlertMessage(jqXHR.responseText, 'alert-danger');
                }
            }
        ).always(function () {
            enableAndHideSpinner(element, loadingSpinner);
            //Hide the model from the user
            $('#delete-table-confirmation-modal').modal('hide');
        });
    }
}
//
//Edits a collection name 
//
function editTable(element) {
    //Create the spinner
    var loadingSpinner = disableAndShowSpinner(element, 'text-info');

    if (loadingSpinner !== null) {
        //Send the delete request
        $.ajax(
            `${collectionEndPoint}/put`, {
                data: {
                    Name: selectedTable,
                    NewName: $('#table-new-name').val(),
                    Database: workingDatabase
                },
                method: 'PUT',
                success: function (data, status, jqXHR) {
                    location.reload();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    setTopPageAlertMessage(jqXHR.responseText, 'alert-danger');
                }
            }
        ).always(function () {
            enableAndHideSpinner(element, loadingSpinner);
            //Hide the model from the user
            $('#edit-table-modal').modal('hide');
        });
    }
}
//
//Adds a new empty table
//
function addTable(element) {
    //Create the spinner
    var loadingSpinner = disableAndShowSpinner(element, 'text-success');

    if (loadingSpinner !== null) {
        //Send the delete request
        $.ajax(
            `${collectionEndPoint}/Post`, {
                data: {
                    Name: null,
                    NewName: $('#table-new-name').val(),
                    Database: workingDatabase
                },
                method: 'POST',
                success: function (data, status, jqXHR) {
                    location.reload();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    setTopPageAlertMessage(jqXHR.responseText, 'alert-danger');
                }
            }
        ).always(function () {
            enableAndHideSpinner(element, loadingSpinner);
            //Hide the model from the user
            $('#edit-table-modal').modal('hide');
        });
    }
}
function shoEditTableModal(addNew = false) {
    if (!addNew) {
        //Show the edit buttons and hide the add
        var elements = document.getElementsByClassName('edit-table-modal-buttons');
        for (var i = 0; i < elements.length; i++) {
            elements[i].style.display = "block";
        }

        document.getElementById('add-table-modal-buttons').style.display = "none";

    } else {

        //Show the add buttons and hide the edit
        var elements2 = document.getElementsByClassName('edit-table-modal-buttons');
        for (var j = 0; j < elements2.length; j++) {
            elements2[j].style.display = "none";
        }
        document.getElementById('add-table-modal-buttons').style.display = "block";
    }
    $("#edit-table-modal").modal().show();
}
function showDeleteTableConfirmation() {
    $('#delete-table-confirmation-modal').modal().show();
}

