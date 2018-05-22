function Afectar() {
    ajaxJson("AfectarTransferencia", { IdTransferencia: $("#h_IdTransferencia").val(), FechaAfectacion: $("#FechaAfectacion").val() }, "POST", true, function (result) {
        if (result.Exito) {
            ExitoCustom("Afectado correctamente");
            $(".ModalAfectar").modal("hide");
            llenarMaestro(result.Registro);
            recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bEliminar", "bSalir"]);
        }
        else
            ErrorCustom(result.Mensaje, "");
    });
}
function Agregar() {
    nuevo = true;
    ajaxJson("ValidarExistenciaArrastre", {}, "POST", true, function (result) {
        if (result.Exito) {
            typeahead();
            $("#frm")[0].reset();
            $(".jsEnabledAgregar").prop("disabled", false);
            recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
            cancelar = 1;
            $("#Id_Estatus").val("1");
        } else
            ErrorCustom(result.Mensaje, "");
    });
}
function Buscar() {
    ajaxLoad("V_TablaBusquedaAmpliacionReduccion", $("#frmB").serialize(), "tabla", "POST", function () {
        ConstruirTabla("tbl", "No hay resultado para mostrar.");
    });
}
function Cancelar() {
    if (cancelar == 1) {
        $("#frm")[0].reset();
        $("#Id_OGInicial").attr("readonly", true);
        $("#Id_OGFinal").attr("readonly", true);
        $(".jsEnabledAgregar").prop("disabled", true);
        recargarMenuLateral(["bNuevo", "bBuscar", "bSalir"]);
    }
    else {
        $("#Descrip").prop("disabled", true);
        $("#Descrip").val(tempDesc);
        $(".jsEnabledEditar").prop("disabled", true);
        recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bEditar", "bEliminar", "bSalir"]);
    }
}
function changeArea() {
    if ($(this).val().split('-').length == 2) {
        $("#Ca_Areas_Descripcion").val($(this).val().split('-')[1]);
        $(this).val($(this).val().split('-')[0]);
    }
}
function changeRadio() {
    if ($(this).val() == 1) {
        $(".js_porFolio").removeClass("hide");
        $(".js_porDesc").addClass("hide");
        $("#DescripciónBusqueda").val("");
    }
    if ($(this).val() == 2) {
        $(".js_porDesc").removeClass("hide");
        $(".js_porFolio").addClass("hide");
        $("#folioBusqueda").val("");
    }
}
function chekeado() {
    if ($(this).is(':checked')) {
        $(".js_rango").prop("readonly", false);
    } else {
        $(".js_rango").prop("readonly", true);
    }
}
function Detalles() {
    window.location.href = rutaVDeTrans + "?IdTransferencia=" + $("#Id_Transferencia").val() + "&origen=2";
}
function Editar() {
    if ($("#Id_Estatus").val() == 1) {
        nuevo = false;
        $(".jsEnabledEditar").prop("disabled", false);
        tempDesc = $("#Descrip").val();
        recargarMenuLateral(["bCancelar", "bGuardar", "bSalir"]);
        cancelar = 2;
        typeahead();
    }
    else
        ErrorCustom("Transferencia afectada, NO se puede editar.", "");
}
function ModalBuscarArea() {
    customModal(rutaBuscarCatalogo, {}, "GET", "md", ModalBuscarTabla, "", "Buscar", "Cancelar", "Buscar centro gestor", "ModalBuscarArea");
}
function ModalBuscarTabla() {
    ajaxLoad(rutaBuscar, { objeto: 1, clave: $("#text_clave").val().trim(), descripcion: $("#text_descripcion").val().trim() }, "div_tabla", "GET",
            function () {
                ConstruirTabla("tabla_resultados");
            });
}
function ModalBuscarObjeto() {
    if (!$(this).isSiblingDisabled()) {
        elemento = $(this).parent().find("input");
        var label = $(this).parent().parent().parent().children()[0];
        label = $(label).children().text();
        var hijo = $(this).parent().parent().parent().children()[1];
        hijo = $(hijo).children()[0];
        hijo = $(hijo).children()[0];
        customModal(rutaBuscarCatalogo, { tipo: $(hijo).attr("objeto"), label: label, id: $(hijo).attr("id") }, "GET", "lg", function () {
            ajaxLoad(rutaBuscar, { objeto: 14, clave: $("#text_clave").val().trim(), descripcion: $("#text_descripcion").val().trim() }, "div_tabla", "GET",
                    function () {
                        ConstruirTabla("tabla_resultados");
                    });
        }, "", "Buscar", "Cancelar", "Búsqueda de " + label, "MyModal1");
    }
}
function ModalEliminar() {
    ajaxJson("ValidarEliminarTransferencia", { IdTransferencia: $("#Id_Transferencia").val() }, "POST", true, function (result) {
        if (result.Exito) {
            Eliminar();
        }
        else
            ErrorCustom(result.Mensaje, "");
    });
}
function Eliminar() {
    var id = $("#Id_Transferencia").val();
    var confirmar = function () {
        ajaxJson("EliminarTransferencia2", { IdTransferencia: id, FechaCancelacion: $("#FechaCancelacion").val() }, "POST", true, function (result) {
            if (result.Exito) {
                ExitoCustom("Eliminado correctamente.");
                $(".ModalEliminar").modal("hide");
                $("#Id_Estatus").val("3");
                $(".jsEnabledAgregar").prop("disabled", true);
                recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bSalir"]);
            } else
                ErrorCustom(result.Mensaje, "");

        });
    };
    ConfirmCustom("Por favor confirme que desea eliminar de manera permanente el registro", confirmar, "", "Aceptar", "Cancelar");
}
function focusout() {
    if ($.inArray($(this).val(), $(this).data("dataSource")) == -1) {
        $(this).parent().parent().parent().find(".js_Descripcion").val("");
        $(this).val("").focus();
        setTimeout(function () { $("#Id_Area").focus(); }, 100);
    }
}
function focusoutRango() {

    var Campo = $(this).attr("id");
    var textbox = "#Desc_" + Campo;

    if ($(this).val().trim() == "") {
        $(textbox).val("");
        //$("#" + Campo).focus();
        return;
    }
    ajaxJson("GetDescripcion", { id: $("#" + Campo).val(), objeto: 14, parametros: "", tipo: "" }, "POST", true, function (result) {
        
        console.log("respuesta focusout:" + result.Data);
        if (result == null) {
            $(textbox).val("");
            $("#" + Campo).val("");
            $("#" + Campo).focus();
        }
        else if (result.Data == "") {
            $(textbox).val("");
            $("#" + Campo).val("");
            $("#" + Campo).focus();
        }
        else {
            $(textbox).val(result.Data[0].split("-")[1]);
        }
    });
}
function GenerarDetalles() {
    ajaxJson("GenerarDetallesArrastre", { Id_Transferencia: $("#Id_Transferencia").val() }, "POST", true, function (result) {
        if (result.Exito) {
            ExitoCustom(result.Mensaje, "");
            recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bEliminar", "bDetalles", "bSalir"]);
        }
        else
            ErrorCustom(result.Mensaje, "");
    });

}
function Guardar() {
    if ($("#frm").valid()) {
        /*url, data, metodo, asincrono, callback*/
        if (nuevo)//Nueva transferencia
        {
            if (ValidarTipo()) {
                ajaxJson("GuardarArrastre", $("#frm").serialize(), "POST", true, function (result) {
                    if (result.Exito) {
                        ExitoCustom(result.Mensaje, closeModal);
                        modelo = result.Registro;
                        $("#Id_Transferencia").val(result.Registro.Id_Transferencia);
                        $("#Id_Estatus").val("1");
                        $(".jsEnabledAgregar").prop("disabled", true);
                        $(".js_rango").prop("disabled", true);
                        if (modelo.Id_TipoT == 2)
                            recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bEliminar", "bGenerarPolizas", "bSalir"]);
                        else
                            recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bEliminar", "bDetalles", "bSalir"]);
                    }
                    else
                        ErrorCustom(result.Mensaje, closeModal);
                });
            }
            else
                ErrorCustom(mensajeError, "");
        }
        else {
            if (ValidarTipo()) {
                if (modelo.Id_TipoT == 2) {
                    if (modelo.Id_Area != $("#Id_Area").val() || modelo.Id_OGInicial != $("#Id_OGInicial").val() || modelo.Id_OGFinal != $("#Id_OGFinal").val() || modelo.Id_Mes != $("#Id_Mes").val() || modelo.Id_Mes_Origen != $("#Id_Mes_Origen").val()) {
                        ConfirmCustom("Se modificaron campos clave, por lo tanto se eliminarán los detalles generados", GuardarJson, "", "Aceptar", "Cancelar");
                    }
                    else
                        GuardarJson();
                }
                else
                    GuardarJson();
            }
            else
                ErrorCustom(mensajeError, "");

        }

    }
    else
        $("#frm").validate();
}
function GuardarJson() {
    ajaxJson("GuardarArrastre", $("#frm").serialize(), "POST", true, function (result) {
        if (result.Exito) {
            ExitoCustom(result.Mensaje, closeModal);
            modelo = result.Registro;
            $("#Id_Transferencia").val(result.Registro.Id_Transferencia);
            $(".jsEnabledAgregar").prop("disabled", true);
            $(".js_rango").prop("readonly", true);
            if (result.Afecta)
                recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bDetalles", "bAfectar", "bEliminar", "bSalir"]);
            else
                recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bGenerarPolizas", "bEliminar", "bSalir"]);
        }
        else
            ErrorCustom(result.Mensaje, closeModal);
    });
}
function ModalAfectar() {
    ajaxJson("ValidarAfectacionTransferencia", { IdTransferencia: $("#Id_Transferencia").val() }, "POST", true, function (result) {
        if (result.Exito)
            customModal("V_ModalAfectacion", { IdTransferencia: $("#Id_Transferencia").val(), IdTipo: 1 }, "GET", "md", Afectar, "", "Si", "No", "Transferencias", "ModalAfectar");
        else
            ErrorCustom(result.Mensaje, "");
    });
}
function ModalBuscar() {
    customModal("V_BuscarAmpliacionReduccion", { idPpto: 3 }, "GET", "lg", Buscar, "", "Buscar", "Cancelar", "Buscar Arrastre de Saldo", "ModalBuscar");
}
function focusOutArea() {
    ajaxJson(rutaFocusFunciones, { IdArea: $("#Id_Area").val() }, "POST", true, function (response) {
        if (response.Data.length == 1) {
            $("#Id_Funcion").val(response.Data[0]);
        }
    });
}
function Seleccionar() {
    ajaxJson("SeleccionarTransferencia", { IdTransferencia: $(this).data("id") }, "POST", true, function (result) {
        if (result.Exito) {
            $(".ModalBuscar").modal("hide");
            llenarMaestro(result.Registro);
            modelo = result.Registro;
            if (result.Registro.Id_OGInicial != null)
                $(".js_rango").trigger("focusout");
            if (result.Registro.Id_TipoT == 1) {
                $("#div_oculta").addClass("hide");
                $("#rManual").attr("checked", true);
            }
            else {
                $("#rAutomática").attr("checked", true);
                $("#div_oculta").removeClass("hide");
            }

            if (result.Afectar == 1) {
                if (result.Registro.Id_Estatus == 3)
                    recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bSalir"]);
                else
                    recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bDetalles", "bEliminar", "bAfectar", "bSalir"]);
            }
            else {
                if (result.Registro.Id_Estatus == 3)
                    recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bSalir"]);
                else {
                    if (result.Registro.Id_TipoT == 2 && result.Registro.Id_Estatus == 1)
                        recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bGenerarPolizas", "bEliminar", "bSalir"]);
                    else {
                        if (result.Registro.Id_Estatus == 1)
                            recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bDetalles", "bEliminar", "bSalir"]);
                        else
                            recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bEliminar", "bSalir"]);
                    }

                }

            }
        }
        else
            ErrorCustom(result.Mensaje, closeModal);
    });
}
var mensajeError = "";
function ValidarTipo() {
    if ($("#Id_Area").val() != "") {
        if ($("#js_check").is(':checked')) {
            if ($("#Id_OGInicial").val() != "" && $("#Id_OGFinal").val() != "") {
                if ($("#Id_OGInicial").val() < $("#Id_OGFinal").val()) {
                    if ($("#Id_Mes").val() != "" && $("#Id_Mes_Origen").val() != "")
                        return true;
                    else {
                        mensajeError = "Debe seleccionar los meses.";
                        return false;
                    }
                }
                else {
                    mensajeError = "El Objeto inicial debe ser menos que el Objeto final.";
                    return false;
                }

            } else {
                mensajeError = "Falta Especificar el rango de Objeto del Gasto.";
                return false;
            }
        }
        else {
            if ($("#Id_Mes").val() != "" && $("#Id_Mes_Origen").val() != "")
                return true;
            else {
                mensajeError = "Debe seleccionar los meses.";
                return false;
            }
        }
    }
    else {
        mensajeError = "Falta Especificar un Centro Gesto válido";
        return false;
    }
}
function typeahead() {
    ajaxJson(rutaFocusAreas, {}, "POST", true, function (response) {
        $("#Id_Area").typeahead({ source: response.Data });
        $("#Id_Area").data("dataSource", response.Ids);
        if (response.Data.length == 1) {
            $("#Id_Area").val(response.Data[0]);
            $("#Id_Area").trigger('change');
        }
        $("#Id_Area").removeAttr("disabled");
    });
}