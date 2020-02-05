/*
 Helper file to create and work with tables
 */

//
//Creates a normal html table
// data : the rows of the table
// columns : the header columns of the table
//
function CreateHTMLTable(data, columns) {
    //Create the table
    var table = document.createElement('table');
    //Add the default classes
    table.classList.add('table');
    //Create the table header
    var tableHeader = document.createElement('thead');
    //Add the default classes
    tableHeader.classList.add('thead-light','font-weight-bold');
    //Create the header row
    var headerRow = document.createElement('tr');
    for (var i = 0; i < columns.length; i++) {
        //Create the cell for the header
        var headerCell = document.createElement('td');
        //Add the wanted text
        headerCell.textContent = columns[i];
        //Add it to the header row
        headerRow.appendChild(headerCell);
    }
    tableHeader.appendChild(headerRow);
    //Add the header
    table.appendChild(tableHeader);
    //Add the row data
    for (var j = 0; j < data.length; j++) {
        //Create the data row
        var dataRow = document.createElement('tr');
        //Loop throw the object data
        for (var k = 0; k < data[j].length; k++) {
            //Create the cell for the data
            var dataCell = document.createElement('td');
            //Add the wanted text
            dataCell.innerHTML = data[j][k];
            //Add it to the header row
            dataRow.appendChild(dataCell);
        }
        //Add the row
        table.appendChild(dataRow);
    }

    return table;
}

//
//Create a table element with the sent data
//  data: the data to fill the table with
//
function CreateHTMLCustomExpandableNoColumnsTable(data) {
    //The start of the row count for ids
    var rowCount = 1;
   //Create the DOM element
    var table = document.createElement('table');
    //add the needed class
    table.classList.add('table', 'table-bordered');

    for (var j = 0; j < data.length; j++) {
        //The row array to hold before addting the items
        var rows = [];
        //Create and fill the rows array
        CreateExpandableTableRow(`r-${rowCount++}`, ['some'], data[j], rows);
        //Add teh elements into the table
        for (var k = 0; k < rows.length; k++) {
            table.appendChild(rows[k]);
        }
    }
    return table;
}
//
//Creates and expandeble row
// rowId : the id of the new row
// classList : any classes to add on the tr element
// data : the data to fill the row with
// rows : a that will hold main row and all its expandation
//
function CreateExpandableTableRow(rowId, classList, data, rows) {
    //Create the row
    var row = document.createElement('tr');
    row.id = rowId;
    //Add the needed class
    row.classList.add(...classList)

    var rowExpanderCount = 1;
    //Loop throw the data
    for (var i = 0; i < data.length; i++) {
        //If the data has some values inside it
        if (typeof (data[i].value) === 'object') {
            //Set the next row id
            var expanderRowId = `${rowId}-${rowExpanderCount++}`;
            //Create the expander table cell
            var expander = CreateAnchorIconTableCell(['align-middle'], ['text-secondary'], ['far', 'fa-caret-square-down', 'align-middle'], `Expand row with id of ${expanderRowId}`, data[i].name);
            //A way to fast identify which expandation is for which row
            var expandationFastUniqueIdentifcation = `thick solid ${GenerateRandomColor()}`;
            //Link the cell to open the collapsed rows
            expander.firstChild.href = `#${expanderRowId}`;
            expander.firstChild.setAttribute('data-toggle', 'collapse');
            expander.firstChild.setAttribute('role', 'button');
            expander.style.borderBottom = expandationFastUniqueIdentifcation;
            //add it to the row
            row.appendChild(expander);
            var expandationRow = CreateExpandableTableRow(expanderRowId, ['collapse'], data[i].value, rows)
            //Get the expandation row
            expandationRow.style.borderBottom = expandationFastUniqueIdentifcation;
            rows.push(expandationRow);
        }
        else {
            row.appendChild(CreateTextTableCell(['align-middle'], [], `${data[i].name} : `, data[i].value));
        }
    }

    //If the row is a main one
    if (!row.classList.contains('collapse')) {

        //Create the edit cell button
        var editCellButton = CreateAnchorIconTableCell([], ['btn', 'text-info'], ['fa', 'fa-edit'], 'Edit button');
        var exportCsvCellButton = CreateAnchorIconTableCell([], ['btn', 'text-info'], ['fas', 'fa-file-csv'], 'Export to CSV');
        //Set the export button onclick
        exportCsvCellButton.firstChild.setAttribute('onclick', `exportRecord("${data[0].value}","CSV")`);

        //Add the edit button as the first cell of the row
        row.appendChild(exportCsvCellButton)
        row.prepend(editCellButton)
        //Add the main row to the top of the array
        rows.unshift(row);
    }
    return row;
}
//
//Create cell with an icon and value next to it
// cellClassList : any class for the tr element
// anchorClassList : any class for the a element
// iconClassList : any class for the i element
// srText : screen reader text
// sideText : side text to the icon
//

function CreateAnchorIconTableCell(cellClassList, anchorClassList, iconClassList, srText, sideText = '') {
    //Create the cell DOM element
    var cell = document.createElement('td');
    //Crete the anchor DOM element
    var anchor = document.createElement('a');
    //Crete the icon DOM element
    var icon = document.createElement('i');
    //The side icon text
    var boldSideText = document.createElement('b');
    boldSideText.textContent = sideText;
    //Crete the screen reader DOM element
    var scrrenReader = document.createElement('span');
    scrrenReader.textContent = srText;
    //Add the needed classes
    cell.classList.add(...cellClassList);
    anchor.classList.add(...anchorClassList);
    icon.classList.add(...iconClassList);
    scrrenReader.classList.add('sr-only');
    boldSideText.classList.add('m-1')

    //Indent elements
    icon.appendChild(scrrenReader);
    anchor.appendChild(icon);
    //if the text is not empty add it
    if (sideText !== '')
        anchor.appendChild(boldSideText)
    cell.appendChild(anchor);

    return cell;
}
//
//Creates a table cell
// spanClassList : any class for the field and value elements
// field : the text value of the field
// value : the text value of the field value
//
function CreateTextTableCell(cellClassList, spanClassList, field, value) {
    //Create the cell DOM element
    var cell = document.createElement('td');
    //Crete the text DOM element
    var fieldText = document.createElement('b');
    var valueText = document.createElement('span');
    //Set the inner html
    fieldText.textContent = field;
    valueText.textContent = value;

    //Add the needed classes
    cell.classList.add(...cellClassList);
    valueText.classList.add(...spanClassList);
    fieldText.classList.add(...spanClassList);
    //Indent elements
    cell.appendChild(fieldText);
    cell.appendChild(valueText);

    return cell;
}
