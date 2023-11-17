function fnFormatDetails ( oTable, nTr )
{
    var aData = oTable.fnGetData( nTr );
    var sOut = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
    sOut += '<tr><td>Rendering engine:</td><td>'+aData[1]+' '+aData[4]+'</td></tr>';
    sOut += '<tr><td>Link to source:</td><td>Could provide a link here</td></tr>';
    sOut += '<tr><td>Extra info:</td><td>And any further details here (images etc)</td></tr>';
    sOut += '</table>';

    return sOut;
}

function paging() {
    $("img.small-thumb").thumbPopup({
        imgSmallFlag: "",
        imgLargeFlag: "",
        cursorTopOffset: 10,
        cursorLeftOffset: 10
    });
}

$(document).ready(function() {

    $('#dynamic-table').dataTable({} );

    $('.dataTables_paginate.paging_bootstrap.pagination').click(function () {
        paging();
    });
    /*
     * Insert a 'details' column to the table
     */
    var nCloneTh = document.createElement( 'th' );
    var nCloneTd = document.createElement( 'td' );
    nCloneTd.innerHTML = '<img src="/Content/backend/advanced-datatable/examples/examples_support/details_open.png">';
    nCloneTd.className = "center";

    // set class for input
    $("select[name='dynamic-table_length']").addClass("form-control");
    $("input[aria-controls='dynamic-table']").addClass("form-control");

} );