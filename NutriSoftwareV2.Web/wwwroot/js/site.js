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


function autoComplete(url, fieldSearch, inputHideId) {

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

function AbrirFormularioCadastroEdicao(url, pacienteId, pnlResultado, objetoId = null) {
    $.get(url, {
        Id: pacienteId,
        ObjetoId: objetoId
        // Adicione mais parâmetros conforme necessário
    }, function (data) {
        // Manipular os dados de resposta (data) aqui
        console.log(data);
    })
        .done(function (response) {
            $("#" + pnlResultado).html(response)
        })
        .fail(function (error) {
            // Tratar erros aqui
            console.error('Erro na requisição:', error);
        });
}

function CadastrarEditarDados(url, formName, pnlResultadoId, inputhiddenId, messageInsert, messageEdit) {
    var dados = $('form[name="' + formName + '"]').serialize();

    var mensagem = inputhiddenId > 0 ? messageEdit : messageInsert;

    $.ajax({
        url: url,
        type: 'POST',
        data: dados,
        async: true,
        success: function (response) {
            $("#" + pnlResultadoId).empty();
            $("#" + pnlResultadoId).append(response);
            $("#" + modalId).modal('hide')
            toastr.success(mensagem, "Sucesso");
        },
        error: function (response) {
            toastr.error(response, "Error");
        }
    });
}

function UploadJs(url, formName, pnlResultado, modalId) {

    var formData = new FormData($('form[name="' + formName + '"]')[0]);
    console.log(formData)



    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {

            $("#" + pnlResultado).html(data);
            toastr.success("Arquivo inserido com sucesso", "Sucesso");
            $("#loadingLogin").hide();


            $('#' + modalId).modal('hide')
        },
        error: function (xhr, status, error) {
            $("#" + modalId).modal('show');
            $("#loadingLogin").hide();
            toastr.error("Erro ao enviar Arquivo", "Error")
        }
    });

}

function GetRequest(url, id, mensagem) {

    var arquivoId = 1; // ID do arquivo que você quer baixar
    var downloadUrl = url+"?Id="+id;
    window.location = downloadUrl;

    //$.ajax({
    //    url: url,
    //    data: { Id: id },
    //    type: "GET",
    //    success: function () {
    //        // Arquivo baixado com sucesso
    //    },
    //    error: function () {
    //        // Tratar erros, se necessário
    //    }
    //});

    //$.ajax({
    //    url: url,
    //    data: {Id:id},
    //    method: 'GET',
    //    xhrFields: {
    //        responseType: 'blob' // Define o tipo de resposta como blob (binário)
    //    },
    //    success: function (data) {
    //        var urlteste = window.URL.createObjectURL(data);
    //        console.log(data);
    //        console.log(urlteste);
    //        // Aqui você pode fazer qualquer outra coisa com o arquivo,
    //        // como mostrá-lo em um elemento HTML ou manipulá-lo de outra forma.
    //    },
    //    error: function (xhr, status, error) {
    //        console.error('Erro ao baixar o arquivo:', error);
    //    }
    //});

    //$.get(url, {
    //    Id: id,
    //    // Adicione mais parâmetros conforme necessário
    //}, function (data) {
    //    // Manipular os dados de resposta (data) aqui
    //    console.log(data);
    //})
    //    .done(function () {
    //        toastr.success(mensagem, "Sucesso")
    //    })
    //    .fail(function (error) {
    //        // Tratar erros aqui
    //        console.error('Erro na requisição:', error);
    //    });
}