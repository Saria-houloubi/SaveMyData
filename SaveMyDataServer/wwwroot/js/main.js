//
// The main.js file holds refrence to all the javascript files
//


//
//Sets a top page alert message for an ajax request to give information to the user
// message : the message to show the user in
// background : the background of the alert box
// alertTime : the time to keep the alert shown to the use in miliseconds
//
function setTopPageAlertMessage(message, background, alertTime = 5000) {
    //Get the top message div in the layout
    var topMessageAlertDiv = document.getElementById('top-page-messages-part');
    //create the alert box
    var alertDiv = document.createElement('div');
    //add the needed classs
    alertDiv.classList.add('alert', 'text-center', 'm-4', background);
    alertDiv.setAttribute('role', 'alert');
    //Set the message
    alertDiv.innerText = message;
    //Show the alert to the user
    topMessageAlertDiv.appendChild(alertDiv);
    //Hid the alert after the sent alertTime
    setTimeout(function () {
        alertDiv.remove();
    }, alertTime);

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
