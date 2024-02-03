$(document).ready(function () {

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-full-width",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "none",
        "hideDuration": "none",
        "timeOut": "none",
        "extendedTimeOut": "none",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

})

function autoComplete(url,fieldSearch,inputHideId) {

    $("#" + fieldSearch).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url,
                type: "POST",
                data: { busca: request.term },
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#" + inputHideId).val(i.item.val);
        },
        minLength: 1
    });

  
}


function AbrirModal(modalId) {
    $("#" + modalId).modal('show');
}

function FecharModal(modalId) {
    $("#" + modalId).modal('hide');
}
