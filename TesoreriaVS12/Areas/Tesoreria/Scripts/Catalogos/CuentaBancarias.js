function DetallesCuenta() {
    customModal("DetallesCuentasBancarias", { Id: $(this).data("id") }, "GET", "md", "", "", "", "Cerrar", "Detalles Cuenta Bancaria", "ModalDetalles");
}
function EliminarCuenta() {
    var elemento = $(this);
    ConfirmCustom("¿Seguro de eliminar?", function () {
        ajaxJson("EliminarCuenta", { IdCuenta: elemento.data("id") }, "POST", true, function (result) {
            if (result.Exito == true) {
                aPos = tabla.fnGetPosition(elemento.parent().parent().get(0));
                tabla.fnDeleteRow(aPos);
                aPos = -1;
            }
            else {
                ErrorCustom(result.Mensaje, "")
            }
        })
    });
}
function changeFoliador() {
    if ($(this).val() == 1) {
        customModal("V_CuentasBancariasValidar", {}, "GET", "sm", validar, "", "Validar", "", "Validar", "ModalValidarCuenta");
        $("#TipoFoliador").val("2");
    } else {
        $(".folios").addClass("hide");
        $("#NoChequeIni").val("");
        $("#NoChequeFin").val("");
    }
}
function validar() {
    ajaxJson("ValidarCuenta", { password: $("#validar").val() }, "POST", true, function (result) {
        if (result.Exito) {
            $(".ModalValidarCuenta").modal("hide");
            $(".folios").removeClass("hide");
            $("#TipoFoliador").val("1");
        }
        else {
            ErrorCustom(result.Mensaje, "");
        }
    });
}
function ModalEditarCuenta() {
    aPos = tabla.fnGetPosition($(this).parent().parent().get(0));
    customModal("V_CuentasBancariasEditar", { IdCuenta: $(this).data("id") }, "GET", "lg", Editar, "", "Guardar", "Cancelar", "Editar Cuenta Bancaria", "ModalEditarCuenta");
}
function Editar() {
    if ($("#frmEditar").valid()) {
        ajaxJson("V_CuentasBancariasEditar", $("#frmEditar").serialize(), "POST", true, function (response) {
            if (response.Exito) {
                ExitoCustom(response.Mensaje, "");
                $(".ModalEditarCuenta").modal("hide");
                if ($('#' + SourceAction).length > 0) {
                    var Acciones = _.template($('#' + SourceAction).html());
                    var js = jQuery.parseJSON(Propiedades);
                    _.map(js, function (value, key) {
                        valor = eval(value);
                        eval('js.' + key + ' = ' + value);
                    });
                    response.Registro.Acciones = Acciones(js);
                }
                UpdateTbl(response.Registro, TableId, aPos);
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }
    else
        $("#frmEditar").validate();
}
function eliminarCheque() {
    var elemento = $(this);
    ConfirmCustom("¿Seguro de eliminar?", function () {
        ajaxJson("EliminarCheque", { idAsignacion: elemento.data("id"), idCta: elemento.data("idcta") }, "POST", true, function (result) {
            if (result.Exito == true) {
                aPos = TablaCheque.fnGetPosition(elemento.parent().parent().get(0));
                TablaCheque.fnDeleteRow(aPos);
                aPos = -1;
            }
            else {
                ErrorCustom(result.Mensaje, "")
            }
        })
    });
}
function ModalNuevoCheque() {
    customModal("V_NuevoCheque", { IdCuenta: $("#js_idCtaBancaria").val() }, "GET", "md", GuardarCheque, "", "Guardar", "Cancelar", "Control de cheques", "ModalNuevoCheque");
}
function GuardarCheque() {
    if ($("#frmCheque").valid()) {
        ajaxJson("GuardarCheque", $("#frmCheque").serialize(), "POST", true, function (response) {
            if (response.Exito) {
                ExitoCustom(response.Mensaje, "");
                $(".ModalNuevoCheque").modal("hide");
                if ($('#jsActionsourceCheque').length > 0) {
                    var Acciones = _.template($('#jsActionsourceCheque').html());
                    var js = jQuery.parseJSON(Propiedades);
                    _.map(js, function (value, key) {
                        valor = eval(value);
                        eval('js.' + key + ' = ' + value);
                    });
                    response.Registro.Acciones = Acciones(js);
                }
                AddTbl(response.Registro, "tbl_cheques");
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }
    else
        $("#frm").validate();

}
function ModalCheques() {
    customModal("V_ControlCheques", { IdCuenta: $(this).data("id") }, "GET", "lg", "", "", "", "Cancelar", "Control de Cheques", "ModalControlCheques");
}
function changeBanco() {
    ajaxJson("ObetenerCuenta", { idBanco: $(this).val() }, "POST", true, function (result) {
        $("#Id_Cuenta").val(result.Id);
    });
}
function Guardar() {
    if ($("#frm").valid()) {
        ajaxJson("GuardarCuenta", $("#frm").serialize(), "POST", true, function (response) {
            if (response.Exito) {
                ExitoCustom(response.Mensaje, "");
                $(".ModalAgregar").modal("hide");
                if ($('#' + SourceAction).length > 0) {
                    var Acciones = _.template($('#' + SourceAction).html());
                    var js = jQuery.parseJSON(Propiedades);
                    _.map(js, function (value, key) {
                        valor = eval(value);
                        eval('js.' + key + ' = ' + value);
                    });
                    response.Registro.Acciones = Acciones(js);
                }
                AddTbl(response.Registro, TableId);
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }
    else
        $("#frm").validate();
}
function focusout()
{
    var Campo = $(this).attr("id");
    if ($(this).val() != "") {
        ajaxJson(urlGetDescripcion, { Id: $("#" + Campo).val(), objeto: 12 }, "POST", true, function (result) {
            var textbox = "#Desc_" + Campo;
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
    else
        $("#" + Campo).focus();
}