﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let stateIcons =
    [
        '',
        'Active-Icon.bmp',
        'Inactive-Icon.bmp',
        'Suspended-Icon.bmp'
    ];
let itemVal = '';
let iban = '';

$(document).ready(function () {
    $('.js-back').hide();

    $(".js-expmonth").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $("#errmsgMonth").html("Digits Only").show().fadeOut(2000);
            return false;
        }
    });
    $(".js-expmonth").on("change", function () {
        var val = parseInt(this.value);
        if (val > 12 || val < 1) {
            $("#errmsgMonth").html("Accepted values 1-12").show().fadeOut(2000);
            this.value = '';
            return false;
        }
    });

    $(".js-expyear").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $("#errmsgYear").html("Digits Only").show().fadeOut(2000);
            return false;
        }
    });
    $(".js-expyear").on("change", function () {
        if ($(this).val().length != 4) {
            $("#errmsgYear").html("Year must have a size of 4 digits").show().fadeOut(2000);
            this.value = '';
            return false;
        }
    });

    $('.js-amount').on('change', function () {
        if (!$.isNumeric($(this).val())) {
            $("#errmsgAmount").html('Amount must be a number').show().fadeOut(2000);
            this.value = '';
            return false;
        }
    });
});

$('.js-update-customer').on('click',
    (event) => {
        debugger;
        let firstName = $('.js-first-name').val();
        let lastName = $('.js-last-name').val();
        let customerId = $('.js-customer-id').val();

        console.log(`${firstName} ${lastName}`);

        let data = JSON.stringify({
            firstName: firstName,
            lastName: lastName
        });

        // ajax call
        $.ajax({
            url: `/customer/${customerId}`,
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('Update was successful');
            // success
        }).fail(failure => {
            // fail
            console.log('Update failed');
        });
    });

$('.js-customers-list tbody tr').on('click',
    (event) => {
        console.log($(event.currentTarget).attr('id'));
    });

$('.accounts-table tbody tr').on('click',
    (event) => {
        //debugger;
        currRow = event.currentTarget;
        currRowIndex = currRow.rowIndex;
        icon = currRow.cells[0];
        iban = currRow.cells[1].innerText;
        balance = currRow.cells[2].innerText;
        created = currRow.cells[3].innerText;
        state = currRow.cells[4].innerText;

        $('.js-view-account-id').text(iban);
        $('.js-view-account-balance').text(balance);
        $('.js-view-account-created').text(created);
        $('.js-view-account-state').text(state);

        //debugger;
        $('#exampleModal').modal('show');
    });

$('.dropdown-item').on('click',
    (event) => {
        itemVal = parseInt(event.currentTarget.getAttribute('data-account-state'));
        $('.js-view-account-state').text(event.currentTarget.innerText);
    });

$('.btn-secondary').on('click',
    (event) => {
        $('.js-result').empty();
        $('.js-result')
            .append(`<div class="alert alert-warning alert-dismissible fade show" role="alert">
                              Account State update canceled
                              <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                              </button>
                            </div>`);
    });

$('.js-update-account-state').on('click',
    (event) => {
        // update account details
        //itemVal = parseInt($('.js-account-state-selector option:selected').attr('value'));
        let data = JSON.stringify({
            State: itemVal
        });

        //debugger;
        $.ajax({
            url: `/account/${iban}`,
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            // success
            // show an alert
            $('.js-result').empty();
            $('.js-result')
                .append(`<div class="alert alert-success alert-dismissible show" role="alert" >
                              Account State updated successfully
                              <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            </div>`);
            $('.js-result').fadeOut(5000);
            //debugger;
            // try to refresh icon
            //debugger;
            //$('.js-state-image').attr("src", `"/Images/${stateIcons[itemVal]}"`);
        }).fail(failure => {
            // failure
            //console.log('Update failed');
            $('.js-result').empty();
            $('.js-result')
                .append(`<div class="alert alert-danger alert-dismissible fade show" role="alert">
                              Account State update failed
                              <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                              </button>
                            </div>`);
        });
    });

$('.js-pay-card').on('click',
    (event) => {
        let data = JSON.stringify({
            CardNumber: $('.js-cardnumber').val(),
            ExpirationMonth: parseInt($('.js-expmonth').val()),
            ExpirationYear: parseInt($('.js-expyear').val()),
            Amount: parseFloat($('.js-amount').val())
        });

        // lock the form
        $('.js-card-form :input').prop("disabled", true);
        $('.js-back').show();

        // ajax call
        $.ajax({
            url: '/card/checkout',
            method: 'POST',
            contentType: 'application/json',
            data: data
        }).done(response => {
            // success
            $('.js-back').hide();
            $('.js-card-form').toggle();
            console.log("Payment Successfull");
            $('.js-result').empty();
            $('.js-result')
                .append(`<div class="alert alert-success alert-dismissible fade show" role="alert">
                              <h4 class="alert-heading">You payment was successfull!</h4>
                            </div>`);
        }).fail(failure => {
            // failure
            $('.js-card-form').toggle();
            $('.js-result').empty();
            $('.js-result')
                .append(`<div class="alert alert-danger alert-dismissible fade show" role="alert">
                              <h4 class="alert-heading">Your payment failed!</h4>
                              <hr>
                              ${failure.status} - ${failure.responseText}
                            </div>`);
            $('.js-card-form :input').prop("disabled", false);
        });
    });

$('.js-back').on('click',
    (event) => {
        $('.js-result').empty();
        $('.js-back').hide();
        $('.js-card-form').toggle();
    });