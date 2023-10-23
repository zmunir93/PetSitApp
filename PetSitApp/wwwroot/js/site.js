// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("#date").datepicker();
});

$(function () {
    $("#start-date").datepicker();
});

$(function () {
    $("#end-date").datepicker();
});

document.getElementById('boarding-check').addEventListener('change', function() {
    var rateInput = document.getElementById('boarding-rate');
    if (this.checked) {
        rateInput.removeAttribute('disabled');
    } else {
        rateInput.setAttribute('disabled', 'disabled');
    }
});

document.getElementById('home-check').addEventListener('change', function () {
    var rateInput = document.getElementById('home-rate');
    if (this.checked) {
        rateInput.removeAttribute('disabled');
    } else {
        rateInput.setAttribute('disabled', 'disabled');
    }
});