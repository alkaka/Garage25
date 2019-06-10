// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var createButton = document.getElementById("createButton");
var inputForm = document.getElementById("inputForm");
var searchField = document.getElementById("searchField");
var selectButton = document.getElementById("selectButton");
var searchButton = document.getElementById("searchButton");
var resetButton = document.getElementById("resetButton");
var messageField = document.getElementById("messageField");

//checking the length of the value of message and assigning to a variable(checkField) on load
var searchFieldLength = 0;
if (searchField != null && searchField.value != null)
    searchFieldLength = searchField.value.length;

function enableDisableButton() {
    if (searchField == null ||
        messageField == null ||
        searchButton == null ||
        resetButton == null) {
        return;
    }

    if (messageField.innerText.includes("match")) {
        searchField.setAttribute("disabled", "disabled");
        searchButton.setAttribute("disabled", "disabled");
        if (selectButton != null)
            selectButton.setAttribute("disabled", "disabled");
        if (createButton != null)
            createButton.setAttribute("disabled", "disabled");
    }
    else {
        searchField.removeAttribute("disabled");
        if (selectButton != null)
            selectButton.removeAttribute("disabled");
        if (createButton != null)
            createButton.removeAttribute("disabled");

        if (searchFieldLength > 0) {
            searchButton.removeAttribute("disabled");
            resetButton.removeAttribute("disabled");
        }
        else {
            searchButton.setAttribute("disabled", "disabled");
            resetButton.setAttribute("disabled", "disabled");
        }
    }
}

function updateSearchFieldLength() {
    if (searchField != null && searchField.value != null) {
        searchFieldLength = searchField.value.length;
        enableDisableButton();
    }
}

document.addEventListener('keyup', keyUpEvent);

function keyUpEvent() {
    updateSearchFieldLength();
}

document.addEventListener('change', changeEvent);

function changeEvent() {
    updateSearchFieldLength();
}

//calling enableDisableButton() function on load
enableDisableButton();

//function setSelectedIndex(s, valsearch) {

//    // Loop through all the items in drop down list
//    for (i = 0; i< s.options.length; i++) { 
//        if (s.options[i].value == valsearch) {

//            // Item is found. Set its property and exit
//            s.options[i].selected = true;
//            break;
//        }
//    }

//    return;
//}

//console.log("ViewData Value: " + viewdataSelect);
//var str = '@ViewData["Select"]';
//alert(@ViewData["Select"]);
//var str = @Html.Raw(Json.Encode(ViewData["Select"]));
//var str = '<%= ViewData["Select"] %>';
//var str = ViewData["Select"];
//setSelectedIndex(selectButton, viewdataSelect);
//enableDisableButton();        



// this code for Print option 
document.querySelector('#printLink').addEventListener('click', printDiv);
printDivCSS = new String('<link href="myprintstyle.css" rel="stylesheet" type="text/css">');
function printDiv() {
    let printDiv = document.querySelector('#print').innerHTML;
    let = output = printDivCSS + printDiv;
    window.frames["print_frame"].document.body.innerHTML = output;
    window.frames["print_frame"].window.focus();
    window.frames["print_frame"].window.print();
}