//
// A pagination helper file creats a pagination by sending total amount of content and..
//  how many pages to show to the user 
//


//constant Id variables to use accross the file
const paginationPreviousId = "pagination-link-previous";
const paginationNextId = "pagination-link-next";
const paginationOrderAtribute = "pagination-order";
const paginationLimitAttribute = "pagination-limit";
const paginationSelectedClass = "pagination-selected";
const disabledClassName = "disabled";
//
// Creates a pagination bar under a table
// totalCount is the total count of elements in the full collection
// pageCount is the number of records to show in a page
//
function createHTMLPagination(totalCount, pageCount, loadDataCallbackFunction) {
    //as convention look for the element where the pagination should be set in
    // which should be a nav tag
    var element = document.getElementById("pagination-part");
    //Get the number of pages that the bar should hold
    var numberOfPages = Math.ceil(totalCount / pageCount);
    //The pagination html
    var pagination = "";
    //Create the unorder list
    pagination = '<ul class="pagination justify-content-center">';
    //Add the previous button
    pagination = pagination.concat(createPaginationListItem(['page-item', disabledClassName], ['page-link', 'text-info'], paginationPreviousId, 'Previous', loadDataCallbackFunction, [paginationLimitAttribute, 0]));

    for (var i = 1; i <= numberOfPages; i++) {
        //Add the numbers
        pagination = pagination.concat(createPaginationListItem(['page-item'], ['page-link', 'text-info', (i === 1 ? 'pagination-selected' : '')], 'pagination-' + numberOfPages + '-' + i + '', i, loadDataCallbackFunction, [paginationOrderAtribute, i]));
    }
    //Add the next button
    pagination = pagination.concat(createPaginationListItem(['page-item', (numberOfPages > 1 ? '' : disabledClassName)], ['page-link', 'text-info'], paginationNextId, 'Next', loadDataCallbackFunction, [paginationLimitAttribute, numberOfPages]));

    //Check if the element is found
    if (element !== null) {
        element.innerHTML = pagination;
    }
}

//
//Gets a stringfied object of the pagination model in the sharedKernal
// currentPage : the page the user is on
// nextPage : the page to fetch the data for
// fetchCount : the count of elements to get from the server
//
function getPaginationObject(currentPage, FetchCount) {
    return JSON.stringify({ CurrentPage: currentPage, FetchRecordCount: FetchCount })
}

//
//Creats a list item pagination tag
// li : list item
// a : anchor
// a_data_attribute : should be sent as [attributeName , value , attributeName , value....]
//
function createPaginationListItem(li_clasess, a_classes, a_id, a_content, a_onClick, a_data_attributes) {
    //The added data attributes if any
    var dataAttributes = "";
    //Loop throw the sent values
    for (var i = 0; i < a_data_attributes.length; i += 2) {
        //Set the attribute with its value
        dataAttributes = dataAttributes.concat(a_data_attributes[i] + ' = "' + a_data_attributes[i + 1] + '"');
    }
    return '<li class="' + li_clasess.join(' ') + '"><a id="' + a_id + '" class="' + a_classes.join(' ') + '"   onClick="' + basePaginationCallback.name + '(this,' + a_onClick.name + ')"' + dataAttributes + ' >' + a_content + '</a></li>'
}

//
//The base function to be called once a pagination request is fired
// element : the element that sent the request
// loadDataCallback : the callback function that is provieded by the user to loadData
//
function basePaginationCallback(element, loadDataCallback) {

    //If the element is already selected just reload the data
    if (!element.classList.contains(paginationSelectedClass)) {

        //Get next and previos buttons
        var nextElement = document.getElementById(paginationNextId);
        var previousElement = document.getElementById(paginationPreviousId);
        //The page number to get the user is wants to show
        var pageNumberToGet = 1;
        //The elemant that was previously selected
        var prevSelectedElement = document.getElementsByClassName(paginationSelectedClass)[0];
        //Remove the selection from the old one
        prevSelectedElement.classList.remove(paginationSelectedClass);
        //Get the value for the last selection
        var prevSelectedOrder = parseInt(prevSelectedElement.getAttribute(paginationOrderAtribute));

        //If the element id is next
        if (element.id === paginationNextId) {
            //Move to the next page
            pageNumberToGet = prevSelectedOrder + 1;
            //Selecte the next element
            prevSelectedElement.parentElement.nextElementSibling.firstChild.classList.add(paginationSelectedClass);

        } else if (element.id === paginationPreviousId) {
            //Move to the previous page
            pageNumberToGet = prevSelectedOrder - 1;
            //Selecte the previous  element
            prevSelectedElement.parentElement.previousElementSibling.firstChild.classList.add(paginationSelectedClass);

        } else {
            //Get the selected page number
            pageNumberToGet = element.getAttribute(paginationOrderAtribute);
            //add it to the element
            element.classList.add(paginationSelectedClass);
        }

        //Enable both the next and previous element
        if (nextElement.parentElement.classList.contains(disabledClassName))
            nextElement.parentElement.classList.remove(disabledClassName);
        if (previousElement.parentElement.classList.contains(disabledClassName))
            previousElement.parentElement.classList.remove(disabledClassName);


        //Get the pagination last limt to the next button
        var paginationLimitCount = parseInt(nextElement.getAttribute(paginationLimitAttribute));
        //Check if we reached the limit for the next button
        if (pageNumberToGet >= paginationLimitCount) {
            //disble the next button
            document.getElementById(paginationNextId).parentElement.classList.add(disabledClassName);
        }
        //if the pagination went back to the first page
        else if (pageNumberToGet <= 1) {
            //disable the previous button
            document.getElementById(paginationPreviousId).parentElement.classList.add(disabledClassName);
        }
    }

    //Call the user callback with the selected page
    loadDataCallback(pageNumberToGet);
}