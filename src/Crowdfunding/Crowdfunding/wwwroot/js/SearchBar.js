// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    var activeSystemClass = $('.list-group-item.active');
    $('#system-search').keyup(function () {
        var that = this;

        var tableBody = $('.table-list-search tbody');
        var tableRowsClass = $('.table-list-search tbody tr');
        $('.search-sf').remove();
        tableRowsClass.each(function (i, val)
        {
            var rowText = $(val).text().toLowerCase();
            var inputText = $(that).val().toLowerCase();
            if (inputText != '') {
                $('.search-query-sf').remove();
                tableBody.prepend('<tr class="search-query-sf"><td colspan="6"><strong>Searching for: "'
                    + $(that).val()
                    + '"</strong></td></tr>');
            }
            else {
                $('.search-query-sf').remove();
            }

            if (rowText.indexOf(inputText) == -1) {
                //hide rows
                tableRowsClass.eq(i).hide();

            }
            else {
                $('.search-sf').remove();
                tableRowsClass.eq(i).show();
            }
        }
    }
};
    //{
    //    var inputSearch = $('.search-input').val();
       
    //    var Projects = document.getElementById ("").getElementsByTagName("");

    //    for (i = 0; i < Projects.length; i++) {

    //        if (inputSearch == $('.card-title') 
    //        {
    //            $('.card-title').filter('.' + value).show('3000');
    //            $('.card-title').not('.' + value).hide('3000');
    //        }
    //        else if (inputSearch == $('.card-text')
    //        {
    //            $('.card-text').filter('.' + value).show('3000');
    //            $('.card-text').not('.' + value).hide('3000');
    //        }
    //        else
    //        {
    //            alert ('project does not exist')
    //        }
        
    //});

});