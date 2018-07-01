// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    $('.filter-button').click(function ()
    {   
        var value = $(this).attr('data-filter');

        if (value == "all")
        {     
            $('.card-filter').show('1000');
        }
        else
        {            
            $('.card-filter').not('.' + value).hide('3000');
            $('.card-filter').filter('.' + value).show('3000');
        }
    });

    if ($('.btn btn-default filter-button').removeClass("active"))
    {
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
        //url: '@this.Url.Action("Create","Projects")'
        //url: 'https://localhost:44330/Projects/Create',
        url: 'http://localhost:64710/Projects/Create',
        type: 'post',
        data: formData
    }).done(function (data) {
        //alert('done');
        window.location.href = data.redirectUrl;
        /*$('#project-created-form').hide();
        $('#success-created').append('Project Created Succesfully').show();
        $('#success-created').removeClass('hidden');*/
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
        //url: '@this.Url.Action("Create","Rewards")',
        url: 'http://localhost:64710/Rewards/Create',
        type: 'post',
        data: formData
    }).done(function (data) {
        //alert('done');
        window.location.href = data.redirectUrl;
        /*$('#reward-created-form').hide();
        $('#success-created-reward').append('Reward Created Succesfully').show();
        $('#success-created-reward').removeClass('hidden');*/
    }).fail(function () {
        //alert('fail');
        $('#reward-created-form').hide();
        $('#failed-created-reward').append('reward Created failure').show();
        $('#failed-created-reward').removeClass('hidden');
    })
});

$(".js-delete").on("click", function () {
    var button = $(this);
    if (confirm("Are you sure you want to delete this package?")) {
        $.ajax({
            url: "api/package/" + button.attr("data-package-id"),
            method: "DELETE",
            success: function () {
                console.log("success");
                button.parents("tr").remove();
            }
        });
    }
});

$('#password, #confirm-password').on('keyup', function () {
    if ($('#password').val() == $('#confirm-password').val() && $('#password').val() != "") {
        $('#message').html('Matching').css('color', 'green');
    } else
        $('#message').html('Not Matching').css('color', 'red');
});