/*
 Helper functions to work with colors
 */

//
//Generates a random color
//
function GenerateRandomColor() {
    return `#${hex(GenerateRandomNumber(0, 255))}${hex(GenerateRandomNumber(0, 255))}${hex(GenerateRandomNumber(0, 255))}`;
}
//
//Get the hex value with padding from an int
// d : the value to get convert
//
function hex(d) {
    return Number(d).toString(16).padStart(2, '0')
}

//
//Generates a random number between two numbers
// from : the min value
// to : the max value
//
function GenerateRandomNumber(min, max) {
    return Math.floor(Math.random() * max) + min;
}