$(document).ready(function () {
    $('input[type=checkbox]').click(function () {
        thisCheckbox = $(this);
        if (thisCheckbox.attr('checked'))
            thisCheckbox.attr('checked', '');
        else
            thisCheckbox.attr('checked', 'checked');
    });

    $('table tr').click(function () {
        checkBox = $(this).children('td').children('input[type=checkbox]');
        if (checkBox.attr('checked'))
            checkBox.attr('checked', '');
        else
            checkBox.attr('checked', 'checked');
    });

    $('.check-all').click(function () {
        checkBox = $('table tr').children('td').children('input[type=checkbox]');
        if ($(this).attr('checked')) {
            $(this).attr('checked', '');
            checkBox.attr('checked', '');
        } else {
            $(this).attr('checked', 'checked');
            checkBox.attr('checked', 'checked');
        }
    });
});