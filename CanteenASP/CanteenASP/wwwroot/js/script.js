// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.addEventListener('DOMContentLoaded', event => {

    $('#password, #password_confirmation').on('keyup', () => {
        if ($('#password').val() == $('#password_confirmation').val()) {
            $('#passwordMessage').removeClass('text-danger');
            $('#passwordMessage').html('Password is pair').addClass('text-success');
        } else {
            $('#passwordMessage').removeClass('text-success');
            $('#passwordMessage').html('Password is not pair').addClass('text-danger');
        }
    });

})