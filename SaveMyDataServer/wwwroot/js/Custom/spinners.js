//
//Creates a bootstrap spinner div
// text_info : is the color of the spinner
//
function createSpinnerElement(text_info) {
    //Create the div element
    var spinner = document.createElement('div');
    //Add the needed classes
    spinner.classList.add('spinner-border', text_info, 'align-middle','m-1');
    //Optional screen reader for acciblity
    var screenReader = document.createElement('span');
    screenReader.classList.add('sr-only');
    screenReader.innerText = 'loading...';
    //Add the screen reader to the spinner div
    spinner.appendChild(screenReader);
    return spinner;
}
//
//Disables an element and shows a loading spinner next to it
// element: the element to disable
// spinnerTextInfo: the color of the spinner to show in
//
//Returns the load spinner if is has not yet been disabled
//
function disableAndShowSpinner(element, spinnerTextInfo) {

    //if the element is already disabled do not do anything
    if ($(element).hasClass('disabled')) {
        return null;
    }
    //set the elment into disabled
    $(element).addClass('disabled');
    //Create the spinner
    var loadingSpinner = createSpinnerElement(spinnerTextInfo);
    //Show the loading elemnt to the user
    element.parentElement.appendChild(loadingSpinner);

    return loadingSpinner;
}

//
//enables the element and hides the spinner
// element : the element to enable
// spinner : the load spinner element to hide
//
function enableAndHideSpinner(element, spinner) {
    spinner.remove();
    //set the elment into disabled
    $(element).toggleClass('disabled');
}

//
//Create a growing spinner 
//
function getLoadingGrowingSpinner() {
    return '<div class="d-flex justify-content-center m-5">'
        + '<div class="spinner-grow" role="status">'
        + '<span class="sr-only"> Loading...</span>'
        + '</div>'
        + '</div>';
}
