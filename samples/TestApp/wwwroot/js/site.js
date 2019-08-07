// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var onSuccess = function (token) {
    document.getElementById('token').value = token;
    document.getElementById('submitBtn').disabled = false;
    document.getElementById('tokenDiv').style.display = 'block';
}
var onExpired = function () {
    document.getElementById('token').value = 'Token expired! Please try again.';
    document.getElementById('submitBtn').disabled = true;
}