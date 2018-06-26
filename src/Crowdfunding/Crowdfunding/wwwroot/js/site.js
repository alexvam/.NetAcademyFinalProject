// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    $(".filter-button").click(function () {
        var value = $(this).attr('data-filter');

        if (value == "all") {
            //$('.filter').removeClass('hidden');
            $('.filter').show('1000');
        }
        else {
            //            $('.filter[filter-item="'+value+'"]').removeClass('hidden');
            //            $(".filter").not('.filter[filter-item="'+value+'"]').addClass('hidden');
            $(".filter").not('.' + value).hide('3000');
            $('.filter').filter('.' + value).show('3000');

        }
    });

    if ($(".filter-button").removeClass("active")) {
        $(this).removeClass("active");
    }
    $(this).addClass("active");

});

$('#submit-project').on('click', function (event) {
    event.preventDefault();

    let formData = $('#project-created-form').serialize();
    $(this).val('Please wait ...')
        .attr('disabled', 'disabled');

    //debugger;
    $.ajax({
        url: 'https://localhost:44330/Projects/Create',
        type: 'post',
        data: formData
    }).done(function () {
        //alert('done');
        $('#project-created-form').hide();
        $('#success-created').append('Project Created Succesfully').show();
        $('#success-created').removeClass('hidden');
    }).fail(function () {
        //alert('fail');
        $('#project-created-form').hide();
        $('#failed-created').append('Project Created failure').show();
        $('#failed-created').removeClass('hidden');
    })
});

$('#submit-reward').on('click', function (event) {
    event.preventDefault();

    let formData = $('#reward-created-form').serialize();
    $(this).val('Please wait ...')
        .attr('disabled', 'disabled');

    //debugger;
    $.ajax({
        url: 'https://localhost:44330/Rewards/Create',
        type: 'post',
        data: formData
    }).done(function () {
        //alert('done');
        $('#reward-created-form').hide();
        $('#success-created-reward').append('Reward Created Succesfully').show();
        $('#success-created-reward').removeClass('hidden');
    }).fail(function () {
        //alert('fail');
        $('#reward-created-form').hide();
        $('#failed-created-reward').append('reward Created failure').show();
        $('#failed-created-reward').removeClass('hidden');
    })
});