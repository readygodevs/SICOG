function Salir() {
    var ampliacion = $("#totalAm").text();
    var reduccion = $("#totalRed").text();
    if (ampliacion == reduccion)
        $("#frmSalir").submit();
    else
        ErrorCustom("No puede salir porque aún no están cuadrados las ampliaciones y reducciones.");
}
function Cancelar() {
    Reset();
}
function GetDisponibilidad()
{
    ajaxJson("GetDisponibilidadTransferencia", $("#frmDeTransferencia").serialize(), "POST", true, function (result) {
        $.each(result.Meses, function (i, item) {
            if (item.Mes < 10) {
                $("#Dis0" + item.Mes).val(item.importeFormato);
            } else {
                $("#Dis" + item.Mes).val(item.importeFormato);
            }
        });
        $("#total").val(result.total);
        $(".js_Captura").attr("readonly", "readonly");
        $(".js_dis").autoNumeric("init");
        $("#total").autoNumeric({ aSign: "$" });
    });
}
function eliminarDeTransferenciasM() {
    var idT = $("#Id_Transferencia").val();
    var idC = $("#Id_Consecutivo").val();
    ConfirmCustom("¿Esta seguro de eliminar el detalle?", function () {
        //$(".js_Eliminar[data-registro='6'][data-idmes='12'][data-idtipo=''][data-idfolio='']")
        ajaxJson("EliminarDeTransferencia", { Id_Transferencia: idT, Id_Consecutivo: idC }, "POST", true, function (response) {
            if (response.Exito == true) {
                ExitoCustom(response.Mensaje, function () {
                    Reset();
                });
            }
        });
    }, "", "Si", "No");
}
function Reset() {
    $("#_detalles_polizas input,select,textarea").attr("disabled", "disabled");
    $("#_distribucion input").attr("disabled", "disabled");
    $("#_detalles_polizas input[type=text], textarea").val("");
    $("#_distribucion input[type=text], textarea").val("0");
    $("#Id_Consecutivo").val("0");
    recargarMenuLateral(["bNuevo", "bSalir"]);
    ajaxLoad("V_TablaDetallesTransferencia", { Id_Transferencia: $("#Id_Transferencia").val() }, "container_DetallesTransferencias", "POST", function (result) {
        tablaDetalles = ConstruirTabla("tblDetallesTransferencia", "No hay resultado para mostrar.");
    });
}
function eliminarDeTransferencias() {
    var elemento = $(this);
    ConfirmCustom("¿Esta seguro de eliminar el detalle?", function () {
        //$(".js_Eliminar[data-registro='6'][data-idmes='12'][data-idtipo=''][data-idfolio='']")
        ajaxJson("EliminarDeTransferencia", { Id_Transferencia: elemento.data("idtrans"), Id_Consecutivo: elemento.data("idcon") }, "POST", true, function (response) {
            if (response.Exito == true) {
                ExitoCustom(response.Mensaje, function () {
                    ajaxLoad("V_TablaDetallesTransferencia", { Id_Transferencia: $("#Id_Transferencia").val() }, "container_DetallesTransferencias", "POST", function (result) {
                        tablaDetalles = ConstruirTabla("tblDetallesTransferencia", "No hay resultado para mostrar.");
                    });
                    Reset();
                });
            }
        });
    }, "", "Si", "No");
}
function seleccionarDetalle() {
    ajaxJson("getDetalleTransferencia", { Id_Transferencia: $(this).data("idtrans"), Id_Consecutivo: $(this).data("idcon") }, "POST", true, function (response) {
        llenarMaestro(response.Registro);
        if ($("#Id_Estatus").val() == 1)
            recargarMenuLateral(["bNuevo", "bEliminar", "bSalir"]);
        GetDisponibilidad();
    });
}
function guardarDetalle() {
    if ($("#frmDeTransferencia").valid()) {
        $.each($(".js_mes, #Importe"), function (i, value) {
            $(this).val($(this).autoNumeric("get"));
        });
        ajaxJson("GuardarDeTransferencia", $("#frmDeTransferencia").serialize(), "POST", true, function (response) {
            $(".js_mes").autoNumeric("init");
            $("#Importe").autoNumeric({ aSign: "$" });
            if (response.Exito == true) {
                recargarMenuLateral(["bNuevo", "bEliminar", "bSalir"]);
                $("#Id_Consecutivo").val(response.Registro.Id_Consecutivo);
                $("#container_DetallesTransferencias").empty();
                ajaxLoad("V_TablaDetallesTransferencia", { Id_Transferencia: $("#Id_Transferencia").val() }, "container_DetallesTransferencias", "POST", function (result) {
                    tablaDetalles = ConstruirTabla("tblDetallesTransferencia", "No hay resultado para mostrar.");
                });
                $("#_detalles_polizas input,select,textarea").attr("readonly", "readonly");
                $("#_distribucion input").attr("disabled", "disabled");
                $("select").attr("readonly", "readonly");
                ExitoCustom(response.Mensaje, "");
            } else
                ErrorCustom(response.Mensaje, "");
        });
    } else
        $("#frmDeTransferencia").validate();
}
function sumar() {
    var total = parseFloat($("#Pre01").autoNumeric("get"));
    var o = 0;
    for (i = 2; i < 13; i++) {
        if (i < 10) {
            o = i;
            total += parseFloat($("#Pre0" + o).autoNumeric("get"));
        } else {
            total += parseFloat($("#Pre" + i).autoNumeric("get"));
        }
    }
    $("#Importe").autoNumeric("set", total);
};

var eventFocus = function () {
    console.log("focus minusculas");
    $.each($(".js_Captura"), function (key, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
    });
    $(".js_Captura").off("focusout");
    ajaxJson(urlFocusOutAreas, { Id_Fuente_Filtro: $("#Id_Fuente_Filtro").val() }, "POST", true, function (response) {
        changeCOG();
        focoJsCaptura();
        $("#Id_Area").typeahead({ source: response.Data });
        $("#Id_Area").data("dataSource", response.Ids);
        if (response.Data.length == 1) {
            $("#Id_Area").val(response.Data[0]);
            $("#Id_Area").trigger('change');
        }
        $("#Id_Area").removeAttr("disabled");
    });

}

var changeCOG = function () {
    $.each($(".js_Captura"), function (key, elemento) {

        $("body").on("change", "#" + $(elemento).attr("id"), function () {
            $("#Id_Cuenta").empty();
            if ($(elemento).val().split('-').length != 1) {
                $(elemento).parent().parent().siblings().find(".js_Descripcion").val($(elemento).val().split('-')[1])
                //$("#Ca_Areas_Descripcion").val($(this).val().split('-')[1]);
                $(elemento).val($(elemento).val().split('-')[0]);
                if ($(elemento).val().length == $(elemento).data("length")) {
                    //$("#Id_Funcion").val("").removeAttr('disabled');
                    var input = $('.js_Captura');
                    var nextElement = input.eq(input.index(elemento) + 1)
                    nextElement.val("").removeAttr("disabled").focus();
                    ajaxJson(urlGenericSearchClave, $(".js_frmClavePresupuestal").serialize() + "&Id_Actual=" + $(elemento).attr("id"), "POST", true, function (response) {
                        nextElement.typeahead('destroy');
                        nextElement.typeahead({ source: response.Data });
                        nextElement.data("dataSource", {});
                        nextElement.data("dataSource", response.Ids);
                        if ($(elemento).attr("id") == "Id_ObjetoG") {
                            if ($("#Id_FolioPoliza").length > 0)
                                $("#Importe, #Id_Movimiento,#DescripcionMP").val("").removeAttr("disabled");

                            $("#Importe").focus();
                            $("#Id_Cuenta").removeAttr("readonly");
                        }
                        if (response.Data.length == 1) {
                            nextElement.val(response.Data[0]);
                            nextElement.trigger('change');
                        }
                    });
                }
            }
        });
    });

    $("body").on("change", "#Id_Cuenta", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_Cuentas_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 20) {
                $("#Id_Cuenta").removeAttr('disabled').focus();
                $("#Id_Movimiento,#Importe,#DescripcionMP").removeAttr('disabled');
                setTimeout(function () { $("#Importe").focus(); }, 100);
            }
        }
    });
}

function nuevoDeTrans() {
    console.log("nuevo de tans");
    eventFocus();
    console.log("after de tans");
    ajaxJson(urlFocusOutAreas, {}, "POST", true, function (response) {
        $("#Id_Area").typeahead({ source: response.Data });
    });
    $("select").removeAttr("readonly").removeAttr("disabled")
    $("#_distribucion  input").removeAttr("disabled");
    $(".js_dis").prop("disabled", true);
    $("#Id_Movimiento").prop("disabled", false);
    $(".js_Captura").removeAttr("readonly");
    $("#Id_Cuenta").attr("readonly", "readonly");
    $("#_detalles_polizas input[type=text], textarea").val("");
    $("#_distribucion input[type=text], textarea").val("0");
    $("#Id_Consecutivo").val("0");
    $("#Id_Area").removeAttr("disabled");
    $("#Id_Area").removeAttr("readonly");
    recargarMenuLateral(["bGuardar", "bCancelar"]);
    //Valiar los meses cerrados
    ajaxJson("GetMesesCerradosTransferencia", { Id_Transferencia: $("#Id_Transferencia").val() }, "POST", true, function (result) {
        $(".js_mes").prop("disabled", true);
        $.each(result.Meses, function (i, item) {
            if (item.Id_Mes < 10) {
                $("#Pre0" + item.Id_Mes).prop("disabled", false);
            } else {
                $("#Pre" + item.Id_Mes).prop("disabled", false);
            }
        });
    });
}