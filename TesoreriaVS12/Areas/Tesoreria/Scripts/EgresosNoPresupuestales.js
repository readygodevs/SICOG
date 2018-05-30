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
    ajaxJson("getEgresosNoPresupuestales", {}, "POST", true, function (response) {
        if (!response.Error) {
            cleanInputs();
            llenarMaestro(response.Data);
            LoadTablaSaldos();
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
                llenarMaestro(response.Data);
                disableInputs();
                $("#Id_FolioCR").val(response.FolioCR);
                $(".detallesHide").removeClass('hide');
                LoadTablaSaldos();
            }
        });
    } else {
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
                $("#Impreso_CR").prop("checked", true);
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
            $("#CargoMes").val(response.Data.Cargo);
            $("#AbonoMes").val(response.Data.Abono);
            $(".js_Importe").trigger("blur");
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
            $(".detallesHide").removeClass('hide');
            LoadTablaSaldos()
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

    ajaxJson(urlCancPas, { generos: generos }, "POST", true, function (response) {
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
                //$("#Id_Movimiento").removeAttr('disabled');
                var cuenta = $(this).val();
                var banco = false;
                $.each($(this).data("dataBancos"), function (key, value) {
                    if (cuenta.substring(0, value.length) == value) {
                        banco = true;
                        return;
                    }
                });
                if (banco) {
                    $(".js_MovBancario").removeClass("hide");
                    $("#IdTipoMovB").removeAttr('disabled');
                    ajaxSelect(urlListMovBanc, { TipoMov: $("#Id_Movimiento").val() }, "POST", true, "IdTipoMovB", "", callBackLlenarSelect);
                }
                else {
                    $(".js_MovBancario").addClass("hide");
                    $("#IdTipoMovB").val("");
                }
            }
        }
    });

    $("body").on("change", "#Id_Movimiento", function () {
        $("#IdTipoMovB").removeAttr('disabled');
        ajaxSelect(urlListMovBanc, { TipoMov: $("#Id_Movimiento").val() }, "POST", true, "IdTipoMovB", "", callBackLlenarSelect);
    });
}

var buildFiltros = function () {
    var f = {};
    f.GeneroStr = generos;
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
}

var NonEditableInputs = function () {
    $("input[type=text].NonEditable, textarea.NonEditable").attr("readonly", "readonly");
    $("select.NonEditable,input[type=checkbox]").attr("disabled", "disabled");
}

var disableInputs = function () {
    $("input[type=text]:not('.detalles'), textarea").attr("disabled", "disabled");
    $("select,input[type=checkbox]").attr("disabled", "disabled");
    $(".detallesHide").addClass('hide');
}

var enableInputs = function () {
    $("input[type=text], textarea").removeAttr("readonly");
    $("input[type=text], textarea").removeAttr("disabled");
    $("select,input[type=checkbox]").removeAttr("disabled");
     $(".detallesHide").addClass('hide');
}

var cleanInputs = function () {
    $("input[type=text], textarea").val("");
    $("select").val("");
    $("input[type=checkbox]").prop("checked", false);
}

var chageContribuyente = function () {
    if ($(this).val().split('-').length != 1) {
        var nombre = $(this).val().split('-')[0];
        $(this).val($(this).val().split('-')[1]);
        $('#Ca_Persona_NombreCompleto').val(nombre);
        $("#Id_CuentaFR").focus();
    }
}
var callBackBuscarBeneficiario = function () {
    ajaxLoad(urlBuscPersonas, { BDescripcionPersona: $("#BDescripcionBeneficiario").val() }, "resultsBeneficiarios", "POST", function () { })
}

var BuscarContribuyente = function () {
    if (!$(this).isSiblingDisabled())
        customModal(urlBuscBene, {}, "get", "lg", callBackBuscarBeneficiario, "", "Buscar", "Cancelar", "Buscar Contribuyente", "IdModal");
}
var selectContribuyente = function () {
    $('#IdPersona_ENP').val($(this).data('idbeneficiario'));
    $(".IdModal").modal('hide');
    ajaxJson(urlGetContrData, { IdPersona: $(this).data('idbeneficiario') }, "POST", true, function (response) {
        $('#Ca_Persona_NombreCompleto').val(response.NombreCompleto);
    });
}

function adicionar() {
    if ($("#Id_FolioCR").val() != 0) {
        if ($("#Id_EstatusCR").val() == 1) {
            $("#Importe").toNumber();
            ajaxJson("SaveDetails", { IdFolioCR: $("#Id_FolioCR").val(), IdTipoCR: $("#Id_TipoCR").val(), Importe: $("#Importe").val(), IdCuenta: $("#Id_CuentaFR").val() }, "POST", true, function (result) {
                if (result.Exito == true) {
                    var a = tablaM.fnAddData([result.Registro.Ca_Cuentas.Id_CuentaFormato, result.Registro.Ca_Cuentas.Descripcion, result.importe,
                                 ' <a class="fa fa-trash-o js_eliminar cursorPointer" data-idfolio="' + result.Registro.Id_FolioCR + '"  data-idtipo="' + result.Registro.Id_TipoCR + '" data-id="' + result.Registro.Id_Registro + '"  title="Eliminar" data-toggle="tooltip" data-placement="top"></a>']);
                    var nTr = tablaM.fnSettings().aoData[a[0]].nTr;
                    $('td', nTr)[3].setAttribute('class', 'acciones');
                    $('td', nTr)[2].setAttribute('class', 'text-right');
                    $(".js_autoNumeric").formatCurrency();
                    $("#Importe").formatCurrency({ symbol: "" });
                    elimino = false;
                    cleanSaveDetails();
                    $("#Cargos").val(result.Cargo);
                    createBotonera(result.Botonera);
                } else {
                    $("#Importe").formatCurrency({ symbol: "" });
                    ErrorCustom(result.Mensaje, "");
                }

            });
        }
        else
            ErrorCustom("No puede agregar mas detalles, favor verificar el estatus.", "");
    }
    else
        ErrorCustom("Debe seleccionar un Cuenta por Liquidar.", "");
}
function cleanSaveDetails()
{
    $("#Id_CuentaFR").val(""); $("#Ca_Cuentas_FR_Descripcion").val(""); $("#Importe").val("");
}
function deleteDetails() {
    var elemento = $(this);
    if ($("#Id_EstatusCR").val() == 1) {
        if ($("#tblSaldos tbody tr").length == 1)
            elimino = true;
        ConfirmCustom("¿Seguro de eliminar?", function () {
            ajaxJson("DeleteDetails", { IdFolioCR: elemento.data("idfolio"), IdTipoCr: elemento.data("idtipo"), IdRegistro: elemento.data("id") }, "POST", true, function (result) {
                if (result.Exito == true) {
                    aPos = tablaM.fnGetPosition(elemento.parent().parent().get(0));
                    tablaM.fnDeleteRow(aPos);
                    aPos = -1;
                    $(".js_autoNumeric").formatCurrency();
                    createBotonera(result.Botonera);
                }
                else {
                    ErrorCustom(result.Mensaje, "")
                }
            })
        });
    }
    else
        ErrorCustom("No puede eliminar detalles, favor verificar el estatus.", "");

}

function LoadTablaSaldos() {
    ajaxLoad("TablaSaldos", { IdTipoCR: $("#Id_TipoCR").val(), IdFolioCR: $("#Id_FolioCR").val() }, "div_tablaSaldos", "GET", function () {
        tablaM = ConstruirTablaSaldos("tblSaldos", "No hay registros...");
        $(".js_autoNumeric").formatCurrency();
    });
}
var elimino = false;
var ConstruirTablaSaldos = function (div, mensaje) {
    if (mensaje == undefined || mensaje == "")
        mensaje = "No hay registros a mostrar";
    var tabla = $("#" + div).dataTable({
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;
            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                    i : 0;
            };
            // Total over all pages
            if ($("#tblSaldos tbody tr").length > 0) {
                if (!elimino) {
                    totalCargo = api
                   .column(2)
                   .data()
                   .reduce(function (a, b) {
                       return intVal(a) + intVal(b);
                   });
                    $(api.column(2).footer()).html(
                        '<span class="js_autoNumeric">' + totalCargo + '</span>'
                    );
                }
                else {
                    $(api.column(2).footer()).html(
                       '$0.00'
                   );
                }
            }
            else {
                $(api.column(2).footer()).html(
                    '$0.00'
                );
            }
        },
        "sPaginationType": "full_numbers",
        "oLanguage": {
            "oPaginate": {
                "sPrevious": "Anterior",
                "sNext": "Siguiente",
                "sLast": "Última",
                "sFirst": "Primera"
            },
            "sLengthMenu": '<div id="combo_datatable">Mostrar <select>' +
            '<option value="5">5</option>' +
            '<option value="10">10</option>' +
            '<option value="20">20</option>' +
            '<option value="30">30</option>' +
            '<option value="40">40</option>' +
            '<option value="50">50</option>' +
            '<option value="-1">Todos</option>' +
            '</select> registros',
            "sInfo": "Mostrando del _START_ a _END_ (Total: _TOTAL_ resultados)",
            "sInfoFiltered": " - filtrados de _MAX_ registros",
            "sInfoEmpty": "No hay resultados de búsqueda",
            "sZeroRecords": mensaje,
            "sProcessing": "Espere, por favor...",
            "sSearch": "<div id='div_buscar'><i class='fa fa-search'></i>Buscar:</div>"
        }
    });
    return tabla;
}