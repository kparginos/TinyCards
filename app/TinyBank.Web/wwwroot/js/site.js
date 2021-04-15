// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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
                              </button>
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
