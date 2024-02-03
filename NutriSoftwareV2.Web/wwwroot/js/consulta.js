$(document).ready(function () {
    painesConsultaCompleCadastro('none', 'block')
    paineisPnlCadastroVisiveis('block', 'none', 'none');
});

function InserirAtualizarDados(url,frmName,pnlResultadoId,message,tipoMessage=1)
{
    let formulario = $('form[name="' + frmName + '"]').serialize();
    $.ajax({
        url: url,
        type: 'POST',
        data: formulario,
        async: true,
        success: function (response) {
            $("#" + pnlResultadoId).empty();
            $("#" + pnlResultadoId).append(response);

            if (tipoMessage == 2) {
                toastr.info(message, "Informação")
            }
            else {
                toastr.success(message, "Sucesso")
            }
            //painesConsultaCompleCadastro('none', 'block');
            //paineisPnlCadastroVisiveis('none', 'block', 'none');
            //$('input[mask="formAval"]').mask("00,0", { reverse: false });
        },
        error: function (response) {
            toastr.error("Houve um erro ao abrir formulário." + response, "Error");
        }
    });
}

function MenuConsulta(url,btnId, codigoConsulta,pnlResultadoId,pacienteId, passo = null) {

    $.get(url, {
        Codigo: codigoConsulta,
        PacienteId: pacienteId
    }, function (data) {
       
    }).done(function (data) {
        $("#" + pnlResultadoId).html(data)
        $(".btn-menus-consulta").removeClass('btn-menu-consulta-selecionado');
        $("#" + btnId).addClass('btn-menu-consulta-selecionado')
        $('input[mask="hora"]').mask('00:00');
        
        }).fail(function (error) {
            // Tratar erros aqui
            console.error('Erro na requisição:', error);
        });
   
}

//Passo 1 Anotaçoes
function inserirAnotacoes() {

    var formulario = $("#frmObservacaoConslta").serialize();
    $.ajax({
        url: '/Consulta/PrimeiroPassoObservacoes',
        type: 'POST',
        data: formulario,
        async: true,
        success: function (response) {
            $("#pnlTotal").empty();
            $("#pnlTotal").append(response);
            painesConsultaCompleCadastro('none', 'block');
            paineisPnlCadastroVisiveis('none', 'block', 'none');
            $('input[mask="formAval"]').mask("00,0", { reverse: false });
        },
        error: function (response) {
            toastr.error("Houve um erro ao abrir formulário." + response, "Error");
        }
    });
}

function CalcularAvaliacao() {

    if (validaCamposAvaliacao(false)) {
        $("#blnInserir").val(false);
        var editar = $("#txtEditar").val()
        var formularioAvalicao = $("#frmCadastrarAvaliacaoConsulta").serialize();
        $.ajax({
            url: '/Consulta/CalcularAvaliacao',
            type: 'POST',
            data: formularioAvalicao,
            async: false,
            success: function (response) {
                $("#pnlCadastroAvaliacao").empty();
                $("#pnlCadastroAvaliacao").append(response);
                painesConsultaCompleCadastro('none', 'block');
                paineisPnlCadastroVisiveis('none', 'block', 'none');

                if (editar) {
                    $("#pnlBotoesAvaliacao").css('display', 'none');
                    $("#pnlBotaoSalvarAlteracoesAvaliacao").css('display', 'none');
                    $("#pnlBotaoEditarAvaliacao").css('display', 'block');
                }
                else {
                    $("#btnInserirAvaliacao").css('display', 'block');
                    $("#btnCalcular").css('display', 'none');
                }
                $("#pnlBtnSalvarConsulta").css('display', 'block');
                //$("#prtListaAlimentos").css('margin-top', '235px')
            },
            error: function (response) {
                toastr.error("Houve um erro ao abrir formulário." + response, "Error");
            }
        });
    }
}


//Passo 2 Avaliaçao
function inserirAvaliacao() {
    if (validaCamposAvaliacao()) {
        $("#blnInserir").val(true);
        var formularioAvalicao = $("#frmCadastrarAvaliacaoConsulta").serialize();
        $.ajax({
            url: '/Consulta/SegundoPassoAvaliacao',
            type: 'POST',
            data: formularioAvalicao,
            async: false,
            success: function (response) {
                $("#pnlTotal").empty();
                $("#pnlTotal").append(response);
                painesConsultaCompleCadastro('none', 'block');
                paineisPnlCadastroVisiveis('none', 'none', 'block');
                //$("#prtListaAlimentos").css('margin-top', '235px')
                inicializacao();
                $("#pnlBtnSalvarConsulta").css('display', 'none');
            },
            error: function (response) {
                toastr.error("Houve um erro ao abrir formulário." + response, "Error");
            }
        });
    }
}


//Passo 3 Dieta
function inserirDieta() {


    var formularioAvalicao = $("#frmCadastrarDietaConsulta").serialize();
    $.ajax({
        url: '/Consulta/TerceiroPassoDieta',
        type: 'POST',
        data: formularioAvalicao,
        async: false,
        success: function (response) {
            $("#pnlTotal").empty();
            $("#pnlTotal").append(response);
            painesConsultaCompleCadastro('none', 'block');
            paineisPnlCadastroVisiveis('block', 'block', 'block');
            camposParaPreencherEditarDieta(false)
            HabilitarDesabilitarCmapos(true);
            $("#pnlBtnSalvarConsulta").css('display', 'block');

        },
        error: function (response) {
            toastr.error("Houve um erro ao abrir formulário." + response, "Error");
        }
    });

}

function VoltarEtapasConsulta(value, codigo) {
    // 2 -> voltar de cadastro de avaliação para cadastro de anotações
    // 3 -> voltar de cadastoro dieta para cadastro de avaliação

    if (value > 0) {
        $.ajax({
            url: '/Consulta/PassoAnterior',
            type: 'POST',
            data: { Passo: value, Codigo: codigo },
            async: false,
            success: function (response) {
                $("#pnlTotal").empty();
                $("#pnlTotal").append(response);
                painesConsultaCompleCadastro('none', 'block');
                switch (value) {
                    case 2:
                        paineisPnlCadastroVisiveis('block', 'none', 'none');
                        break;

                    case 3:
                        paineisPnlCadastroVisiveis('none', 'block', 'none');
                        $("#pnlBtnSalvarConsulta").css('display', 'block');
                        break;
                }

                $('input[mask="formAval"]').mask("00,0", { reverse: false });
            },
            error: function (response) {
                toastr.error("Houve um erro ao abrir formulário." + response, "Error");
            }
        });

    }

}

function cancelarAlteracaoConsulta() {

    painesConsultaCompleCadastro('none', 'block');
    paineisPnlCadastroVisiveis('block', 'block', 'block');
    camposParaPreencherEditarDieta(false)
    HabilitarDesabilitarCmapos(true);
    $("#pnlBtnSalvarConsulta").css('display', 'block');
    $("#pnlBotaoSalvarEdicaoObservacaoConsulta").css('display', 'none');
    $("#pnlBotaoSalvarAlteracoesAvaliacao").css('display', 'none');
    $("#pnlBotaoSalvarAlteracaoDieta").css('display', 'none');

}

function validarCamposDigitados(campoHiddeId, campotextId) {
    console.log("id Hidden " + campoHiddeId);
    console.log("id text " + campotextId);
    console.log("valor Hidden " + $("#" + campoHiddeId).val());
    console.log("valor text " + $("#" + campotextId).val());


    if ($("#" + campoHiddeId).val() == "" && $("#" + campotextId).val() != "") {
        $("#" + campotextId).val("");
    }
}


// funçoes para paineis
function painesConsultaCompleCadastro(CssDisplayPnlConsultaCompleta, CssDisplayPnlCadastro) {
    $("#pnlConsultaCompleta").css('display', CssDisplayPnlConsultaCompleta);
    $("#pnlCadastro").css('display', CssDisplayPnlCadastro);
}

function paineisPnlConsultaVisiveis(CssDisplaypPnlAnotacoes, CssDisplayPnlAvaliacao, CssDisplayPnlDieta) {
    $("#pnlAnotacoes").css('display', CssDisplaypPnlAnotacoes);
    $("#pnlAvaliacao").css('display', CssDisplayPnlAvaliacao);
    $("#pnlDieta").css('display', CssDisplayPnlDieta);
}

function paineisPnlCadastroVisiveis(CssDisplayPnlCadastroAnotacoes, CssDisplayPnlCadastroAvaliacao, CssDisplayPnlCadastroDieta) {
    $("#pnlCadastroAnotacoes").css('display', CssDisplayPnlCadastroAnotacoes);
    $("#pnlCadastroAvaliacao").css('display', CssDisplayPnlCadastroAvaliacao);
    $("#pnlCadastroDieta").css('display', CssDisplayPnlCadastroDieta);
}
//

//Validições de campos

//Validações dos campos de Avaliação

function HabilitarDesabilitarCmapos(habilitar) {


    $("#txtNumAvaliacao").prop('readonly', habilitar);
    $("#txtDataAvaliacao").prop('readonly', habilitar);
    $("#txtPeso").prop('readonly', habilitar);
    $("#txtCircCintura").prop('readonly', habilitar);
    $("#txtCircAbdominal").prop('readonly', habilitar);
    $("#txtCircQuadril").prop('readonly', habilitar);
    $("#txtCircPeito").prop('readonly', habilitar);
    $("#txtCircBracoDireito").prop('readonly', habilitar);
    $("#txtCircBracoEsquerdo").prop('readonly', habilitar);
    $("#txtCircCoxadireita").prop('readonly', habilitar);
    $("#txtCircCoxaEsquerda").prop('readonly', habilitar);
    $("#txtPanturrilhaD").prop('readonly', habilitar);
    $("#txtPanturrilhaE").prop('readonly', habilitar);
    $("#txtDCTriceps").prop('readonly', habilitar);
    $("#txtDCEscapular").prop('readonly', habilitar);
    $("#txtDCSupraIliaca").prop('readonly', habilitar);
    $("#txtDCAbdominal").prop('readonly', habilitar);
    $("#txtDCAxilar").prop('readonly', habilitar);
    $("#txtDCPeitoral").prop('readonly', habilitar);
    $("#txtDCCoxa").prop('readonly', habilitar);

    $(".txaObservacoes").prop('readonly', habilitar);
    $("#txtAnotacoesIniciaisConsulta").prop('readonly', habilitar);

}

function EditarPassoConsulta(passo, codigo, editar) {

    if (passo > 0) {
        $.ajax({
            url: '/Consulta/PassoAnterior',
            type: 'POST',
            data: { Passo: passo, Codigo: codigo, Editar: editar },
            async: false,
            success: function (response) {
                $("#pnlTotal").empty();
                $("#pnlTotal").append(response);
                $('input[mask="formAval"]').mask("00,0", { reverse: false });
            },
            error: function (response) {
                toastr.error("Houve um erro ao abrir formulário." + response, "Error");
            }
        });
    }
    switch (passo) {
        case 1:
            paineisPnlCadastroVisiveis('block', 'none', 'none');
            $("#pnlBotaoSalvarEdicaoObservacaoConsulta").css('display', 'block');
            $("#pnlBotaoEditarObservacaoConsulta").css('display', 'none');
            $("#btnInserirAnotacoes").css('display', 'none');
            break;

        case 2:
            paineisPnlCadastroVisiveis('none', 'block', 'none');
            $("#pnlBotaoSalvarAlteracoesAvaliacao").css('display', 'block');
            $("#pnlBotaoEditarAvaliacao").css('display', 'none');
            $("#pnlBotaoSalvarAlteracoesAvaliacao").css('display', 'none');
            break;

        case 3:
            paineisPnlCadastroVisiveis('none', 'none', 'block');
            $("#pnlBotaoSalvarAlteracaoDieta").css('display', 'block');
            $("#pnlBotaoEditarDieta").css('display', 'none');
            $("#pnlBotoesAvancar").css('display', 'none');
            break;
    }
    camposParaPreencherEditarDieta(true)
    HabilitarDesabilitarCmapos(false);
    botoesSalvarAlteracoes('block', 'none');

    inicializacao();

}

function SalvarAlteracoes(form, action) {
    if ($(".txtEditar").val() != true) {
        $(".txtEditar").val(true);
    }
    var formulario = $(form).serialize();
    $.ajax({
        url: '/Consulta/' + action,
        type: 'POST',
        data: formulario,
        async: true,
        success: function (response) {
            $("#pnlTotal").empty();
            $("#pnlTotal").append(response);
            $('input[mask="formAval"]').mask("00,0", { reverse: false });

            painesConsultaCompleCadastro('none', 'block');
            paineisPnlCadastroVisiveis('block', 'block', 'block');
            camposParaPreencherEditarDieta(false)
            HabilitarDesabilitarCmapos(true);
            botoesSalvarAlteracoes('none', 'block');
        },
        error: function (response) {
            toastr.error("Houve um erro ao abrir formulário." + response, "Error");
        }
    });
}

function validaCamposAvaliacao(showMessage = true) {

    if ($("#txtNumAvaliacao").val() == "") {
        if (message) {
            toastr.error("O campo 'Avaliação' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtDataAvaliacao").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Data Avaliação' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtPeso").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Peso' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtCircCintura").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Cintura' deve ser preenchido.", "Error");
        }
        return false;
    }


    if ($("#txtCircAbdominal").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Abdominal' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtCircQuadril").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Quadril' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtCircPeito").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Peito' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtCircBracoDireito").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Braço D' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtCircBracoEsquerdo").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Braço E' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtCircCoxadireita").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Coxa D' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtCircCoxaEsquerda").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'Circ. Coxa E' deve ser preenchido.", "Error");
        }
        return false;

    }

    if ($("#txtDCTriceps").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'DC. Triceps' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtDCEscapular").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'DC. Escapular' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtDCSupraIliaca").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'DC. Supra Iliaca' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtDCAbdominal").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'DC. Abdominal' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtDCAxilar").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'DC. Axilar' deve ser preenchido.", "Error");
        }
        return false;
    }


    if ($("#txtDCPeitoral").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'DC. Peitoral' deve ser preenchido.", "Error");
        }
        return false;
    }

    if ($("#txtDCCoxa").val() == "") {
        if (showMessage) {
            toastr.error("O campo 'DC. Coxa' deve ser preenchido.", "Error");
        }
        return false;
    }
    return true;



    //$("#txtCircPanturrilhaDireita").val()
    //$("#txtCircPanturrilhaEsquerda").val()


}


//Dieta
function inicializacao() {
    var hora = $("#horario").val();
    var alimento = $("#txtAlimento").val();
    var alimentoId = $("#AlimentoId").val();
    var qtdAlimento = $("#txtQuantidadeAlimento").val();
    var tipoMedida = $("#txtTpMedidaAlimento").val();
    var tipoMedidaId = $("#TipoMedidaAlimentoId").val();

    var subAlimento = $("#txtSubstituicao").val();
    var subAlimentoId = $("#txtSubstituicaoId").val();
    var subQuantidade = $("#quantidadeSubAlimento").val();
    var tipoSubMedida = $("#txtTpMedidaSubsAlimento").val();
    var tipoSubMedidaId = $("#txtTpMedidaSubsAlimentoId").val();

    var habilitarBotaoAlimentos = !(
        hora != "" &&
        alimento != "" &&
        alimentoId != "" &&
        qtdAlimento != "" &&
        tipoMedida != "" &&
        tipoMedidaId != "");

    var habilitarBotaoSubAlimentos = !(
        hora != "" &&
        subAlimento != "" &&
        subAlimentoId != "" &&
        subQuantidade != "" &&
        tipoSubMedida != "" &&
        tipoSubMedidaId != "");

    $('input[mask="hora"]').mask('00:00');
    $('input[mask="Qtd"]').mask("#,##0.00", { reverse: true });
    $("#btnInserirAlimentos").prop('disabled', habilitarBotaoAlimentos);
    $("#btnInseriSubstituicoes").prop('disabled', habilitarBotaoSubAlimentos);

    habilitarBotoesInserirIntervalos();

    $(function () {
        $("#txtAlimento").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Dieta/AutoComplete",
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
                $("#txtAlimentoId").val(i.item.val);
            },
            minLength: 1
        });
    });

    $(function () {
        $("#txtSubstituicao").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Dieta/AutoComplete",
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
                $("#txtSubstituicaoId").val(i.item.val);
            },
            minLength: 1
        });

    });

    $(function () {
        $("#txtTpMedidaAlimento").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Dieta/AutoCompleteTipoMedidas",
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
                $("#txtTpMedidaAlimentoId").val(i.item.val);
            },
            minLength: 1
        });

    });

    $(function () {
        $("#txtTpMedidaSubsAlimento").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Dieta/AutoCompleteTipoMedidas",
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
                $("#txtTpMedidaSubsAlimentoId").val(i.item.val);
            },
            minLength: 1
        });
    });
}

$(function () {
    $("#txtIntervalo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dieta/AutoComplete",
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
            $("#txtIntervaloId").val(i.item.val);
        },
        minLength: 1
    });


});

$(function () {
    $("#txtTpIntervalo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dieta/AutoCompleteTipoMedidas",
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
            $("#txtTpIntervaloId").val(i.item.val);
        },
        minLength: 1
    });
});
function verificaHorarioCorreto(horario) {
    if (horario != "" && horario.length < 5) {
        $("#btnInserirAlimentos").prop('disabled', true);
        toastr.error("Por favor preencha o horario corretamente", "Error");
        $("#horario").focus();
        return false;
    }

    if (horario != "" && horario.substr(0, 2) > 23) {
        $("#btnInserirAlimentos").prop('disabled', true);
        toastr.error("Por favor preencha o horario corretamente", "Error");
        $("#horario").focus();
        return false;
    }
}

function habilitarbotoes(botao, id = null) {
    var hora = $("#horario").val();

    verificaHorarioCorreto(hora);

    switch (botao) {
        case 1:
            var alimento = $("#txtAlimento").val();
            var alimentoId = $("#AlimentoId").val();
            var qtdAlimento = $("#txtQuantidadeAlimento").val();
            var tipoMedida = $("#txtTpMedidaAlimento").val();
            var tipoMedidaId = $("#txtTpMedidaAlimentoId").val();
            validarCamposDigitados('txtAlimentoId', 'txtAlimento');
            validarCamposDigitados('txtTpMedidaAlimentoId', 'txtTpMedidaAlimento');
            var habilitarBotaoAlimentos = !(
                hora != "" &&
                alimento != "" &&
                alimentoId != "" &&
                qtdAlimento != "" &&
                tipoMedida != "" &&
                tipoMedidaId != "");

            $("#btnInserirAlimentos").prop('disabled', habilitarBotaoAlimentos);
            break;

        case 2:
            var subAlimento = $("#txtSubstituicao").val();
            var subAlimentoId = $("#txtSubstituicaoId").val();
            var subQuantidade = $("#quantidadeSubAlimento").val();
            var tipoSubMedida = $("#txtTpMedidaSubsAlimento").val();
            var tipoSubMedidaId = $("#txtTpMedidaSubsAlimentoId").val();

            var habilitarBotaoSubAlimentos = !(
                hora != "" &&
                subAlimento != "" &&
                subAlimentoId != "" &&
                subQuantidade != "" &&
                tipoSubMedida != "" &&
                tipoSubMedidaId != "");

            $("#btnInseriSubstituicoes").prop('disabled', habilitarBotaoSubAlimentos);
            break;

        case 3:

            var IntervaloAlimento = $("#txtIntervalo_" + id).val();
            var IntervaloAlimentoId = $("#txtIntervaloId_" + id).val();
            var IntervaloQuantidade = $("#quantidadeIntervaloAlimento_" + id).val();
            var IntervalotipoSubMedida = $("#txtTpIntervalo_" + id).val();
            var IntervalotipoSubMedidaId = $("#txtTpIntervaloId_").val();

            validarCamposDigitados('txtIntervaloId_' + id, 'txtIntervalo_' + id)

            var habilitarBotaoSubAlimentos = !(
                hora != "" &&
                IntervaloAlimento != "" &&
                IntervaloAlimentoId != "" &&
                IntervaloQuantidade != "" &&
                IntervalotipoSubMedida != "" &&
                IntervalotipoSubMedidaId != "");

            $("#btnInserirIntervalos_" + id).prop('disabled', habilitarBotaoSubAlimentos);
            break;
    }
}

function adicionarAlimentos(tipo, habilitarEdit = "False") {
    let editar = habilitarEdit == "True" ? true : false;
    var dados = "";
    var horario = $("#horario").val();
    $(".hora").val(horario)
    var substituir = false

    if ($("#substituir").prop('checked')) {
        $("#tipoAlimento").val(2);
    }
    else {
        $("#tipoAlimento").val(1);
    }
    $("#txtEditVal").val(editar);
    dados = $("#frmInserirAlimentos").serialize();

    switch (tipo) {
        case 1:
            var qtdSub = $("#txtQuantidadeAlimento").val().replaceAll(".", ",")
            $("#txtQuantidadeAlimento").val(qtdSub);
            dados = $("#frmInserirAlimentos").serialize();
            break;
        case 2:
            var qtdSub = $("#quantidadeSubAlimento").val().replaceAll(".", ",")
            $("#quantidadeSubAlimento").val(qtdSub);
            dados = $("#frmInserirSubAlimentos").serialize();
            break;
    }

    $.ajax({
        url: '/Consulta/AdicionarAlimento',
        type: 'POST',
        data: dados,
        async: false,
        success: function (response) {
            //alert(response)
            $("#prtListaAlimentos").empty();
            $("#prtListaAlimentos").append(response);
            $("#txtAlimento").val("");
            $("#txtAlimentoId").val("");
            $("#txtQuantidadeAlimento").val("");
            $("#TipoMedidaAlimentoId").val("");
            $("#txtTpMedidaAlimento").val("");
            $("#txtSubstituicao").val("");
            $("#txtSubstituicaoId").val("");
            $("#quantidadeSubAlimento").val("");
            $("#txtTpMedidaSubsAlimento").val("");
            $("#txtTpMedidaSubsAlimentoId").val("");

            habilitarbotoes(tipo);
            habilitarBotoesInserirIntervalos()
            if (editar) {
                console.log("Teste entrou no if")
                $("#pnlBotoesAvancar").css('display', 'none');
                $("#pnlBotaoSalvarAlteracaoDieta").css('display', 'block');
            }
            console.o


            // $('html, body').animate({ scrollTop: $("#linha_" + horario.replaceAll(":", "")).offset().top }, 'slow');
            //$('html, body').animate({ scrollTop: $("#linha_" + horario.replaceAll(":", "")).position().top }, 'slow');

            if ($("#linha_" + horario.replaceAll(":", "")) !== "") {
                // Prevent default anchor click behavior
                event.preventDefault();

                // Store hash
                var hash = $("#linha_" + horario.replaceAll(":", ""));

                // Using jQuery's animate() method to add smooth page scroll
                // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area
                $('html, #prtListaAlimentos').animate({
                    scrollTop: $(hash).offset().top
                }, 800, function () {

                    // Add hash (#) to URL when done scrolling (default click behavior)
                    window.location.hash = hash;
                });
            } // End if


        },
        error: function (jqXHR, status, error) {

            var posicaoInicio = jqXHR.responseText.indexOf(":") + 1;
            var posicaoFinal = jqXHR.responseText.indexOf("/");
            /*var err = eval("(" + jqXHR.responseText + ")");*/
            var mensagemErro = jqXHR.responseText.substring(posicaoInicio, posicaoFinal);
            toastr.error(mensagemErro, "Error");
            switch (tipo) {
                case 1:
                    $("#txtAlimento").val("");
                    $("#AlimentoId").val("");
                    $("#txtQuantidadeAlimento").val("");
                    $("#txtTpMedidaAlimento").val("");
                    $("#TipoMedidaAlimentoId").val("");
                    break;
                case 2:
                    $("#txtSubstituicao").val("");
                    $("#txtSubstituicaoId").val("");
                    $("#quantidadeSubAlimento").val("");
                    $("#txtTpMedidaSubsAlimento").val("");
                    $("#txtTpMedidaSubsAlimentoId").val("");
                    break;
            }

        }

    });

}

function habilitarBotoesInserirIntervalos() {
    var existeTxtIntervalo = false;
    var existeTxtIntervaloId = false;
    var existeQuantidadeIntervalo = false;
    var existeTipoMedidaIntervalo = false;
    var existeTipoMedidaIntervaloId = false;

    $.each($('.linhasIntervalos'), function (index, value) {

        if ($(value).children().children().children().children().is("#txtIntervalo_" + (index + 1))) {
            var txtIntervalo = $(value).children().children().children().children("#txtIntervalo_" + (index + 1)).val();
            existeTxtIntervalo = txtIntervalo != "";
        }
        if ($(value).children().children().children().children().is("#txtIntervaloId_" + (index + 1))) {
            var txtIntervaloId = $(value).children().children().children().children("#txtIntervaloId_" + (index + 1)).val();
            existeTxtIntervaloId = txtIntervaloId != "";
        }
        if ($(value).children().children().children().children().is("#quantidadeIntervaloAlimento_" + (index + 1))) {
            var txtQtdIntervalo = $(value).children().children().children().children("#quantidadeIntervaloAlimento_" + (index + 1)).val();
            existeQuantidadeIntervalo = txtQtdIntervalo != "";
        }
        if ($(value).children().children().children().children().is("#txtTpIntervalo_" + (index + 1))) {
            var txtTipoItervalo = $(value).children().children().children().children("#txtTpIntervalo_" + (index + 1)).val();
            existeTipoMedidaIntervalo = txtTipoItervalo != "";
        }
        if ($(value).children().children().children().children().is("#txtTpIntervaloId_" + (index + 1))) {
            var txtTipoItervaloId = $(value).children().children().children().children("#txtTpIntervaloId_" + (index + 1)).val();
            existeTipoMedidaIntervaloId = txtTipoItervaloId != "";
        }

        var habilitaCampo = !(
            existeTxtIntervalo &&
            existeTxtIntervaloId &&
            existeQuantidadeIntervalo &&
            existeTipoMedidaIntervalo &&
            existeTipoMedidaIntervaloId);

        if ($(value).children(".dvbtnAdicionar").children().is("#btnInserirIntervalos_" + (index + 1))) {
            $(value).children(".dvbtnAdicionar").children("#btnInserirIntervalos_" + (index + 1)).prop('disabled', habilitaCampo)
        }

    });

}

function buscarIntervalos(id) {
    console.log("hora " + id);

    $("#txtIntervalo_" + id).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dieta/AutoComplete",
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
            $("#txtIntervaloId_" + id).val(i.item.val);
        },
        minLength: 1
    });
}

function buscarTipoMedidaIntevalos(id) {
    console.log("Teste " + id);
    $("#txtTpIntervalo_" + id).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dieta/AutoCompleteTipoMedidas",
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
            $("#txtTpIntervaloId_" + id).val(i.item.val);
        },
        minLength: 1
    });
};

function inserirObservacaoDieta(formId) {
    var dados = $("#frmObservacao_" + formId).serialize();
    $.ajax({
        url: '/Consulta/CadastrarObservacao',
        type: 'POST',
        data: dados,
        async: false,
        success: function (response) {
            $("#prtListaAlimentos").empty();
            $("#prtListaAlimentos").append(response);
        },
        error: function (response) {
            toastr.error("Erro na operação", "Error");
        }
    });
}

function adicionarIntervalos(formId) {

    var dados = $("#frmInserirIntervalos_" + formId).serialize();
    $.ajax({
        url: '/Consulta/AdicionarAlimento',
        type: 'POST',
        data: dados,
        async: false,
        success: function (response) {
            $("#prtListaAlimentos").empty();
            $("#prtListaAlimentos").append(response);
            $("#txtIntervalo_" + formId).val("");
            $("#txtIntervaloId_" + formId).val("");
            $("#quantidadeAlimento_" + formId).val("");
            $("#txtTpIntervalo_" + formId).val("");
            $("#txtTpIntervaloId_" + formId).val("");

            habilitarBotoesInserirIntervalos()

        },
        error: function (response) {
            toastr.error("Erro na operação", "Error");
        }
    });

}

function removerItem(form) {

    if (form == "" || form == null) {
        toastr.error("Erro ao localizer formulario de remoção", "Error");
        return false;
    }

    var form = $("#" + form).serialize();
    $.ajax({
        url: '/Consulta/RemoverItemDieta',
        type: 'POST',
        data: form,
        async: false,
        success: function (response) {
            $("#prtListaAlimentos").empty();
            $("#prtListaAlimentos").append(response);
            habilitarBotoesInserirIntervalos();
        },
        error: function (response) {
            toastr.error("Erro na operação", "Error");
        }
    });
}

function camposParaPreencherEditarDieta(ativo) {
    if (ativo) {
        $("#pnlCamposSuperiores").css('display', 'block');
        $("#pnlcamposIntevalos").css('display', 'block');
        $("#pnlBotoesAvancar").css('display', 'block');
        $(".btnRemoveItemAlimentos").css('display', 'block');
        $(".btnRemoveItemSubstituicoes").css('display', 'block');
        $(".linhasIntervalos").css('display', 'block');
        $(".btnRemoveItemIntervalos").css('display', 'block');

    }
    else {
        $("#pnlCamposSuperiores").css('display', 'none');
        $("#pnlBotoesAvancar").css('display', 'none');
        $(".btnRemoveItemAlimentos").css('display', 'none');
        $(".btnRemoveItemSubstituicoes").css('display', 'none');
        $(".linhasIntervalos").css('display', 'none');
        $(".btnRemoveItemIntervalos").css('display', 'none');
    }

    botoesEditarVisiveis(!ativo);

}

function botoesSalvarAlteracoes(boteosSalvarAlteracoes, botoesEditar) {


    $("#pnlBotaoSalvarAlteracoesAvaliacao").css('display', boteosSalvarAlteracoes);
    $("#pnlBotaoSalvarEdicaoObservacaoConsulta").css('display', boteosSalvarAlteracoes);
    $("#pnlBotaoSalvarAlteracaoDieta").css('display', boteosSalvarAlteracoes);

    $("#pnlBotaoEditarAvaliacao").css('display', botoesEditar);
    $("#pnlBotaoEditarDieta").css('display', botoesEditar);
    $("#pnlBotaoEditarObservacaoConsulta").css('display', botoesEditar);
    $("#pnlBotoesAvancar").css('display', 'none');
    $("#pnlBotoesAvaliacao").css('display', 'none');
    $("#plnBotaoAnotacoes").css('display', 'none');

}

function botoesEditarVisiveis(visivel) {
    if (visivel) {
        $("#pnlBotaoEditarAvaliacao").css('display', 'block');
        $("#pnlBotaoEditarDieta").css('display', 'block');
        $("#pnlBotaoEditarObservacaoConsulta").css('display', 'block');

        $("#pnlBotoesAvancar").css('display', 'none');
        $("#pnlBotoesAvaliacao").css('display', 'none');
        $("#plnBotaoAnotacoes").css('display', 'none');
    }
}

function confirm(formId, messageConfirm)
{
    Swal.fire({
        title: messageConfirm,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim'

    }).then((result) => {
        if (result.value) {
            $("#"+formId).submit();
        }
    });

}


