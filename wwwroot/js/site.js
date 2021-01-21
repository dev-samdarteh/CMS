// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        alert('clicked..')
        var url = $(this).data('url');
        alert(url);
         

        $.get(url, function (data) {
            $("#divModalBody_AddEdit_lg").html(data);             
        });

        $("#divPopModal_AddEdit_lg").modal('show');


        //$("#divModalBody_AddEdit_lg").load(url, function () {
        //    alert('show modal')
        //    $("#divPopModal_AddEdit_lg").modal('show');
        //});

        //$.get(url).done(function (data) {

        //    alert('load data..')
        //    PlaceHolderElement.html(data);

        //    alert('show modal now..')
        //    PlaceHolderElement.find('.modal').modal('show');
        //})
    })
})



$(function () {
     
    $('#btnAction3').click(function (event) {
        alert('mod clicked..')

        var url = $(this).data("url");
        $.ajax({
            type: "GET",
            url: url,
            dataType: 'json',
            success: function (res) {

                // get the ajax response data
                var data = res.body;
                // update modal content
                $('.modal-body').text(data.someval);
                // show modal
                $('#myModal').modal('show');

            },
            error: function (request, status, error) {
                console.log("ajax call went wrong:" + request.responseText);
            }
        });
    })
})


function displayBusyIndicator() {
    document.getElementById("loading").style.display = "block";
}