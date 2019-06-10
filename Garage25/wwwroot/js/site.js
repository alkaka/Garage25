// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.





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