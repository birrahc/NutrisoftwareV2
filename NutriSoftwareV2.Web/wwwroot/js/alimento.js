function AbrirModalParaCadastroAlimento() {

    $.ajax({
        url: '/Alimento/Create',
        type: 'GET',
        async: true,
        success: function (response) {
            $("#formularioAlimento").empty();
            $("#formularioAlimento").append(response);
            $('#modalAlimento').modal('show')
        },
        error: function (response) {
            toastr.error("Houve um erro ao abrir formulário." + response, "Error");
        }
    });
}

function CadastrarEditarAlimento() {

    var dados = $("#formularioCadastrarAlimento").serialize();

    $.ajax({
        url: '/Alimento/Create',
        type: 'POST',
        data: dados,
        async: true,
        success: function (response) {
            $("#listaDeAlimentos").empty();
            $("#listaDeAlimentos").append(response);
            $('#modalAlimento').modal('hide');
            toastr.success("Dados cadastrados com sucesso.", "Secesso.");
        },
        error: function (response) {
            toastr.error("Houve um erro ao abrir formulário." + response, "Error");
        }
    });
}