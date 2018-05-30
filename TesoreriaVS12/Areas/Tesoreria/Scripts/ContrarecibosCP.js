var search = function () {
    customModal(urlCompromisos,
                {},
                "GET",
                "lg",
                function () {
                    ajaxLoad($("#frmOrden").attr("action"), $("#frmOrden").serialize(), "resultsCompromisos", "POST", "");
                },
                "",
                "Buscar",
                "Cerrar",
                "Buscar orden de compra",
                "ModalBusCompromisos");
}

var getNew = function () {
    ajaxJson("getCancelacion_Pasivos", { TipoCr: $("#Id_TipoCR").val() }, "POST", true, function (response) {
        if (!response.Error) {
            cleanInputs();
            llenarMaestro(response.Data);           
        }
        else
            ErrorCustom(response.Message);
    });
}

var get = function () {
    ajaxJson("OrdenCompraJson", { TipoCompromiso: $("#Id_TipoCompromiso").val(), FolioCompromiso: $("#Id_FolioCompromiso").val() }, "POST", true, function (response) {
        if (response.Exito) {
            llenarMaestro(response.Data);
            botonera = [];
            $.each(response.Data.Botonera, function (key, value) {
                botonera[key] = value;
            });
            recargarMenuLateral(botonera);            
        }
        else {
            ErrorCustom(response.Mensaje);
        }
    });
}

var save = function () {  
    if ($("#frmCancelacionPasivos").valid()) {
        ajaxJson($("#frmCancelacionPasivos").attr("action"), $("#frmCancelacionPasivos").serialize(), "POST", true, function (response) {
            if (!response.Exito) {
                ErrorCustom(response.Mensaje);
            }
            else {
                createBotonera(response.Botonera);
                disableInputs();
                $("#Id_FolioCR").val(response.FolioCR);
                $("#FechaVen").val(response.FechaVen);
            }
        });
    } else {
        NonEditableInputs();
    }
}

var edit = function () {    
    if (($("#Id_MesPolizaCH").val() != "" || ($("#Id_MesPolizaCH").val() == "") && $("#Impreso_CR").is(":checked") == true)) {
        $(".Editable_PCH").removeAttr("readonly");
        $(".Editable_PCH").removeAttr("disabled");
    }
    else if ($("#Id_MesPolizaCH").val() == "" && $("#Impreso_CR").is(":checked") == false) {
        enableInputs();
        NonEditableInputs();
        eventFocusBalance();
    }
    createBotonera(["bGuardar", "bCancelar"]);
}

var print = function () {
    var Mensaje = _.template($('#js_msjImprimir').html());
    ConfirmCustom(Mensaje({ TipoCR: $("#Ca_TipoContrarecibos_Descripcion").val(), FolioCR: $("#Id_FolioCR").val(), Total: $("#Cargos").val() }), function () {
        ajaxJson(urlImpContrarecibo, { TipoCR: $("#Id_TipoCR").val(), FolioCR: $("#Id_FolioCR").val() }, "POST", true, function (response) {
            if (response.Exito == true) {
                window.open(urlRptContrarecibo + "?TipoCR=" + $("#Id_TipoCR").val() + "&FolioCR=" + $("#Id_FolioCR").val(), "_bank");
                llenarMaestro(response.Registro);
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }, "", "Aceptar", "Cancelar");
}

var cancel = function () {
    customModal("CancelarCRCP", { TipoCR: $("#Id_TipoCR").val(), FolioCR: $("#Id_FolioCR").val() }, "GET", "",
                    function () {
                        if ($("#frmCancelarCR_CP").valid()) {
                            ajaxJson($("#frmCancelarCR_CP").attr("action"), $("#frmCancelarCR_CP").serialize(), "POST", true, function (response) {
                                if (!response.Exito) {
                                    ErrorCustom(response.Mensaje);
                                }
                                else {
                                    cleanInputs();
                                    llenarMaestro(response.Data);
                                    disableInputs();
                                    createBotonera(response.Data.Botonera);
                                    $(".cancelaCRCP").modal("hide");
                                }
                            });
                        }
                    }, "", "Aceptar", "Cancelar", "Cancelar Cuenta por Liquidar", "cancelaCRCP");
}

var getCargos = function () {
    ajaxJson("getCargosAbonos", { Id_Cuenta: $("#Id_CuentaFR").val(), mes: $("#Id_Mes").val() }, "POST", true, function (response) {
        if (!response.Exito)
            ErrorCustom(response.Mensaje);
        else {
            var resta = response.Data.Abono - response.Data.Cargo;
            $("#CargoMes").val(resta);
            //$("#AbonoMes").val(response.Data.Abono);
            $("#CargoMes").autoNumeric('destroy');
            $("#CargoMes").autoNumeric('init', { nBracket: null, vMin: -999999999.99});
        }
    });
}

var searchCuentas = function (element) {
    if (!$(element).isSiblingDisabled()) {
        customModal(urlBuscarCuenta, {}, "GET", "lg", "", "", "", "Cancelar", "Buscar Cuentas", "busquedaCuentas");
    }
}

var getContras = function () {
    $("#Resultados").ajaxLoad({ url: urlTblContr, data: $("#frmSearchContra").serialize(), method: "POST" });
}

var selectContras = function (elemento) {    
    ajaxJson(urlGetContr, { IdFolio: elemento.data("folio"), IdTipoCR: elemento.data("tipo") }, "POST", true, function (response) {
        if (response.Exito == true) {
            cleanInputs();
            llenarMaestro(response.Registro);
            $(".busquedaContrarecibos").modal("hide");
            createBotonera(response.Registro.Botonera);
            $(".js_Importe").trigger("blur");
        }
        else
            ErrorCustom(response.Mensaje, "");        
    });
}

var selectCuentas = function (elemento) {
    $("#Id_CuentaFR").val($(elemento).data("idcuenta"));
    $("#Ca_Cuentas_FR_Descripcion").val($(elemento).data("descripcion"));
    $(".busquedaCuentas").modal("hide");
}

var irDetails = function () {
    $("#TipoCompromiso").val($("#Id_TipoCompromiso").val());
    $("#FolioCompromiso").val($("#Id_FolioCompromiso").val());
    $("#frmDetalles").submit();
}

var cancelarCompromiso = function (tituloModal) {
    customModal("Cancelar", { tcompromiso: $("#Id_TipoCompromiso").val(), fcompromiso: $("#Id_FolioCompromiso").val() }, "GET", "",
                function () {
                    if ($("#frmCancelar").valid()) {
                        ajaxJson($("#frmCancelar").attr("action"), $("#frmCancelar").serialize(), "POST", true, function (response) {
                            if (!response.Exito) {
                                ErrorCustom(response.Mensaje);
                            }
                            else {
                                //recargarMenuLateral(["bNuevo", "bEditar", "bCancelar", "bBuscar", "bDetalles", "bRecibido", "bSalir"]);     
                                getCompromiso();
                                $(".cancelaCompromiso").modal("hide");
                            }
                        });
                    }
                }, "", "Aceptar", "Cancelar", "Cancelar " + tituloModal, "cancelaCompromiso");
}

var focoJsCaptura = function () {
    $(".js_Captura").on("focusout", function () {
        var elemento = $(this);
        if ($(this).val().length > 0 && $.inArray($(this).val(), $(this).data("dataSource")) == -1) {
            $(this).parent().parent().parent().find(".js_Descripcion").val("");
            $(this).val("").focus();
            $(this).parent().parent().parent().nextAll().find(".js_Captura").attr("disabled", "disabled").val("").typeahead('destroy');
            $(this).parent().parent().parent().nextAll().find(".js_Descripcion").val("");
            $("#Id_Cuenta").empty();
            setTimeout(function () { elemento.focus(); }, 100);
        }
    });

}

var eventFocusBalance = function () {
    $.each($(".js_Captura"), function (key, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
    });
    var generos = getParametroCuentas($("#Id_TipoCR").val());
    ajaxJson(urlGastFond, { TipoCR: $("#Id_TipoCR").val() }, "POST", true, function (response) {
        focoJsCaptura();
        $("#Id_CuentaFR").typeahead({ source: response.Data, items: 15, });
        $("#Id_CuentaFR").data("dataSource", response.Ids);
        $("#Id_CuentaFR").data("dataBancos", response.cuentasBancos);
        if (response.Data.length == 1) {
            $("#Id_CuentaFR").val(response.Data[0]);
            $("#Id_CuentaFR").trigger('change');
        }
    });

    $("body").on("change", "#Id_CuentaFR", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_Cuentas_FR_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 20) {
                $("#Id_CuentaFR").removeAttr('disabled').focus();
                /*/$("#Id_Movimiento").removeAttr('disabled');
                var cuenta = $(this).val();
                var banco = false;

                console.log($(this).data("dataBancos"));
                $.each($(this).data("dataBancos"), function (key, value) {
                    if (cuenta.substring(0, value.length) == value) {
                        banco = true;
                        return;
                    }
                });
                if (banco) {
                    $(".js_MovBancario").removeClass("hide");
                    $("#IdTipoMovB").removeAttr('disabled');
                    ajaxSelect("/Tesoreria/Polizas/ListTipoMovBancario", { TipoMov: $("#Id_Movimiento").val() }, "POST", true, "IdTipoMovB", "", callBackLlenarSelect);
                }
                else {
                    $(".js_MovBancario").addClass("hide");
                    $("#IdTipoMovB").val("");
                }*/
            }
        }
    });

    $("body").on("change", "#Id_Movimiento", function () {
        $("#IdTipoMovB").removeAttr('disabled');
        ajaxSelect(urlListMovBanc, { TipoMov: $("#Id_Movimiento").val() }, "POST", true, "IdTipoMovB", "", callBackLlenarSelect);
    });
}

var getParametroCuentas = function (tipoCr) {
    var cta = "";
    ajaxJson(urlCuentaCG, { TipoCR: tipoCr }, "GET", false, function (response) {
        if (response.Exito == false)
            ErrorCustom(response.Mensaje);
        else {            
            cta = response.Data;
        }
    });
    return cta;
}
/*
var buildFiltros = function () {
    var f = {};
    f.GeneroStr = getParametroCuentas($("#Id_TipoCR").val());
    if ($("#IdCuenta").length > 0)
        f.IdCuenta = $("#IdCuenta").val();
    else
        f.IdCuenta = $("#Id_Cuenta").val();
    if ($("#Descripcion_filtro").length > 0)
        f.Descripcion = $("#Descripcion_filtro").val();
    else
        f.Descipcion = $("#Descripcion").val();
    f.selectUltimoNivel = true;
    return f;
}*/

var buildFiltros = function () {
    var f = {};
    var cta = ajaxJson(urlCuentaCG, { TipoCR: $("#Id_TipoCR").val() }, "get", false, function (data) {
        if (data.Exito == false)
            ErrorCustom(data.Mensaje);
        else
            f.IdCuenta = data.Data;
    });
    f.selectUltimoNivel = true;
    f.UltimoNivel = true;
    //f.Descripcion = $("#Descripcion_filtro").val();
    if ($("#IdCuenta").length > 0)
        f.IdCuenta = $("#IdCuenta").val();
    else
        f.IdCuenta = $("#Id_Cuenta").val();
    if ($("#Descripcion_filtro").length > 0)
        f.Descripcion = $("#Descripcion_filtro").val();
    else
        f.Descipcion = $("#Descripcion").val();
    return f;
}


var NonEditableInputs = function () {
    $("input[type=text].NonEditable, textarea.NonEditable").attr("readonly", "readonly");
    $("select.NonEditable,input[type=checkbox]").attr("disabled", "disabled");
}

var disableInputs = function () {
    $("input[type=text], textarea").attr("disabled", "disabled");
    $("select,input[type=checkbox]").attr("disabled", "disabled");
}

var enableInputs = function () {
    $("input[type=text], textarea").removeAttr("readonly");
    $("input[type=text], textarea").removeAttr("disabled");
    $("select,input[type=checkbox]").removeAttr("disabled");
}

var cleanInputs = function () {
    $("input[type=text], textarea").val("");    
    $("select").val("");
    $("input[type=checkbox]").prop("checked", false);
}