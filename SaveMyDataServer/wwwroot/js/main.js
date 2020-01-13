//
// The main.js file holds refrence to all the javascript files
//

//
//Show an error alert box in the top of the page
// message: message to put in the alert
// isError: a flag to check if the message is an error show it in alert-danger
//
function showAlertMessage(message, isError) {
    //Get the top message part to show alert to the user
    var navbar = document.getElementById("top-page-messages-part");
    //Check wither the message is an error or a sucess message
    if (isError)
        navbar.innerHTML = '<div class="alert alert-danger text-center m-4" role="alert">' + message + '</div>';
    else
        navbar.innerHTML = '<div class="alert alert-success text-center m-4" role="alert">' + message + '</div>';
}

function getLoadingGrowingSpinner() {
    return '<div class="d-flex justify-content-center m-5">'
        + '<div class="spinner-grow" role="status">'
        + '<span class="sr-only"> Loading...</span>'
        + '</div>'
        + '</div>';
}

//
//Unflattens a string seperated with a dot to a json object
// main : the old json object assign the values to
// pathArray : the path to the value such as path[0].path[1].path[2] = value
// value : the value to set at the end of the path
//
function unflattenJSON(main, pathArray, value) {
    //Get the path name from the path array
    var field = pathArray.pop();
    //If we still need to go down the path then..
    if (pathArray.length > 0) {
        //Check if the path exists in the object..
        if (main[field] === undefined) {
            //else create a new object with that path name
            main[field] = unflattenJSON({}, pathArray, value);
        } else {
            //If the path in json is already defined then continue going deeper
            main[field] = unflattenJSON(main[field], pathArray, value);

        }
    }
    //If it is the last item then we arrived at the key: value taget
    else {
        //once we reeach the value end of the path set it
        main[field] = value;
    }
    return main;
}
