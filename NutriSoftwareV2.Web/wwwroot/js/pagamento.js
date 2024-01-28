$(document).ready(function () {
    $('input[mask = "valores"]').mask("#.##0,00", { reverse: true });
});

function VerDetalhesPagamento(idDetalhesConsulta, idDeltalhesObs, idIconDetalhes) {
    $("#" + idDetalhesConsulta).toggle();
    $("#" + idDeltalhesObs).toggle();

    if ($("#" + idDetalhesConsulta).is(":visible")) {
        $("#" + idIconDetalhes).removeClass("fa-angle-right")
        $("#" + idIconDetalhes).addClass("fa-angle-down")
    }
    else {
        $("#" + idIconDetalhes).addClass("fa-angle-right")
        $("#" + idIconDetalhes).removeClass("fa-angle-down")
    }
}



function limparPesquisaPagamento() {
    $("#selLocalAtendimento").val(0)
    $("#selPaciente").val(0)
    $("#selSituacao").val(0)
    $("#txtDataIncial").val("")
    $("#txtDataFinal").val("")
    $("#resultaPagamento").empty();
}

function validarPesquisaPagamento(dataIncial, dataFinal) {
    if (dataIncial == "" || dataIncial == null) {
        toastr.error("A data inicial não pode ser nula.", "Error");
        return false;
    }
    if (dataFinal == "" || dataFinal == null) {
        toastr.error("A data final não pode ser nula.", "Error");
        return false;
    }
    if (dataIncial > dataFinal) {
        toastr.error("A data inicial não pode ser maior que a data final.", "Error");
        return false;
    }
    return true;
}