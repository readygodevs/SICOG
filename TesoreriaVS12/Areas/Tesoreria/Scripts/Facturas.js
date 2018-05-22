/**
* Facturas.js
* @author: Ing. Julio César Carbajal
**/
var params = getUrlVars();
var subirArchivo = function () {
    customModal("/Tesoreria/Contrarecibos/UploadFile", {}, "GET", "lg", submitFile, "", "Subir", "Cancelar", "Subir Archivo", "IdModal");

}

var submitFile = function () {
    $("#fileinfo").submit();
}
var newFactura = function () {
    ajaxJson("GetContrarecibo", { IdFolio: params.folio, IdTipoCR: params.tipo }, "POST", true, function (response) {
        if (response.Exito == true) {
            llenarMaestro(response.Registro);
            var idTipoCR = response.Registro.Id_TipoCR;
            switch (idTipoCR) {
                case 1:
                case 8:
                case 9:
                case 10:
                case 11:
                    $("#BeneficiarioName").val(response.Registro.Ca_Beneficiarios.NombreCompleto);
                    break;

                default:
                    $("#BeneficiarioName").val(response.Registro.Ca_Cuentas_FR.Descripcion);
            }
            $("#MontoCR").val(response.Registro.Cargos);
            $("#Id_Proveedor").val(response.Registro.Id_Beneficiario);
            $("#ProveedorName").val(response.Registro.NombreCompleto);
        }
        $("#No_docto").val("");
        $("#Editado").val(0);
        /*url, data, metodo, asincrono, div, seleccion, callback*/
        //ajaxSelect("/Tesoreria/Listas/List_TipoDocto", {}, "POST", true, "Id_TipoDocto", 1, callBackLlenarSelect);
        //ajaxSelect("/Tesoreria/Listas/List_Impuesto", {}, "POST", true, "Id_Impuesto", 1, callBackLlenarSelect);
        //ajaxSelect("/Tesoreria/Listas/List_Deduccion", {}, "POST", true, "Id_Deduccion", 1, callBackLlenarSelect);
        recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
        $("input[type=text], select").removeAttr("disabled");
        $("input[type=text], select").removeAttr("readonly");
        $(".caluclos, #TOTAL").val("");
        $("#Id_TipoCR, #Id_FolioCR,#BeneficiarioName, #TOTAL").attr("readonly", "readonly");
    });
}

var saveFactrura = function () {
    $(".caluclos, #TOTAL").toNumber();
    $("#TipoDocumento").val($("#Id_TipoDocto option:selected").text());
    if ($("#frmFactruras").valid()) {
        ajaxJson("GuardarDocumentos", $("#frmFactruras").serialize(), "POST", true, function (response) {
            if (response.Exito == true) {
                /*var Acciones = _.template($('#jsActionsource').html());
                response.Registro.Acciones = Acciones({ IdFactura: response.Registro.Id_Factura, FolioCR: response.Registro.Id_FolioCR, TipoCR: response.Registro.Id_TipoCR, Proveedor: response.Registro.Id_Proveedor });
                if (response.Editado == false)
                {
                    AddTbl(response.Registro, "_tblFacturas");
                }
                else{
                    //var elemento = $(".js_eliminar[data-factura='" + $("#Id_Factura").val() + "'][data-foliocr='" + $("#FolioCR").val() + "'][data-tipocr='" + $("#TipoCR").val() + "'][data-proveedor='" + $("#Id_Proveedor").val() + "']");
                    UpdateTbl(response.Registro, "_tblFacturas", aPos);
                }
                ultimo = false;*/
                $(".caluclos, #TOTAL").formatCurrency();
                $("#frmFactruras input, texarea").attr("readonly", "readonly");
                $("select").attr("disabled", "disabled");
                recargarTablaDocumentos();
                llenarMaestro(response.Registro);
                ExitoCustom("El registro se editó correctamente", function () {
                    recargarMenuLateral(["bNuevo", "bEditar", "bEliminar", "bUploadFiles", "bSalir"]);
                });
            }
            else {
                ErrorCustom(response.Mensaje, "");
            }
        });
    }
    else
        $(".caluclos, #TOTAL").formatCurrency();

}

var returnContraRecibos = function () {
    if (parseFloat($(".js_totalfacturas").text().substr(1)) > parseFloat($("#MontoCR").val())) {
        //mensaje, callbackOk, callbackCancel, TxtBtnOk, TxtBtnFail
        ConfirmCustom("La suma de los documentos es mayor al importe del Cuenta por Liquidar. ¿Realmente deseas salir?", function () {
            if ($("#Id_TipoCR").val() == 1)
                $("#frmSalir").submit();
            else
                $("#frmSalirFG").submit(function () {
                    $("#Tipo").val($("#Id_TipoCR").val());
                    $("#Folio").val($("#Id_FolioCR").val());
                }).submit();
        }, "", "Si", "No");
    }
    else if (parseFloat($(".js_totalfacturas").text().substr(1)) < parseFloat($("#MontoCR").val())) {
        ConfirmCustom("La suma de los documentos es menor al importe de la Cuenta por Liquidar. ¿Realmente deseas salir?", function () {
            if ($("#Id_TipoCR").val() == 1)
                $("#frmSalir").submit();
            else
                $("#frmSalirFG").submit(function () {
                    $("#Tipo").val($("#Id_TipoCR").val());
                    $("#Folio").val($("#Id_FolioCR").val());
                }).submit();
        }, "", "Si", "No");
    }
    else {
        var idTipoCR = parseInt($("#Id_TipoCR").val());
        switch (idTipoCR) {
            case 1:
                $("#frmSalir").attr("action", ligaContrarecibos);
                $("#frmSalir").submit();
                break;
            case 8:
                $("#frmSalir").attr("action", ligaArrendamientos);
                $("#frmSalir").submit();
                break;
            case 9:
                $("#frmSalir").attr("action", ligaHonorarios);
                $("#frmSalir").submit();
                break;
            case 10:
                $("#frmSalir").attr("action", ligaCancelacionActivos);
                $("#frmSalir").submit();
                break;
            case 11:
                $("#frmSalir").attr("action", ligaHonorariosAsimilables);
                $("#frmSalir").submit();
                break;
            default:
                $("#frmSalirFG").submit(function () {
                    $("#Tipo").val($("#Id_TipoCR").val());
                    $("#Folio").val($("#Id_FolioCR").val());
                }).submit();
                break;
        }
    }
}

var doCalc = function () {
    if ($(this).val().trim() == "")
        $(this).val(0);
    $("#TOTAL").val($("#SubTotal").asNumber() + $("#IVA").asNumber() + $("#SubTotal").asNumber() + $("#Otros").asNumber() - $("#Ret_ISR").asNumber() - $("#Ret_IVA").asNumber() - $("#SubTotal").asNumber() - $("#Ret_Obra").asNumber() - $("#Ret_Otras").asNumber());
    $("#TOTAL").formatCurrency();
}

var editFactura = function () {
    var desabilitados = $("input[readonly='readonly'], select:disabled");
    desabilitados.removeAttr("readonly");
    desabilitados.removeAttr("disabled");
    $("#Id_TipoCR, #Id_FolioCR,#BeneficiarioName").attr("readonly", "readonly");
    recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
    $("#Editado").val(1);
}

var detailsFactura = function () {
    var element = $(this);
    ajaxJson("GetDocumento", { FolioCR: $(this).data("foliocr"), TipoCR: $(this).data("tipocr"), Factura: $(this).data("factura"), Proveedor: $(this).data("proveedor") }, "POST", true, function (response) {
        //ajaxSelect("/Tesoreria/Listas/List_TipoDocto", {}, "POST", true, "Id_TipoDocto", 1, callBackLlenarSelect);
        //ajaxSelect("/Tesoreria/Listas/List_Impuesto", {}, "POST", true, "Id_Impuesto", 1, callBackLlenarSelect);
        //ajaxSelect("/Tesoreria/Listas/List_Deduccion", {}, "POST", true, "Id_Deduccion", 1, callBackLlenarSelect);
        llenarMaestro(response.Registro);
        //recargarMenuLateral(response.Registro.Botonera);
        if ($("#js_mNuevo").length > 0)
            addButtons(["bEditar", "bEliminar"]);
        $(".caluclos, #TOTAL").formatCurrency();
        $("input[type=text], select").attr("readonly", "readonly");
        aPos = $("#" + TableId).dataTable().fnGetPosition(element.parent().parent().get(0));
    });
}

var deleteFactura = function () {
    var elemento = $(".js_eliminar[data-factura='" + $("#Id_Factura").val() + "'][data-foliocr='" + $("#FolioCR").val() + "'][data-tipocr='" + $("#TipoCR").val() + "'][data-proveedor='" + $("#Id_Proveedor").val() + "']");
    if ($("#_tblFacturas tbody tr").length == 1)
        ultimo = true;
    Eliminar({ FolioCR: $("#Id_FolioCR").val(), TipoCR: $("#Id_TipoCR").val(), Factura: $("#Id_Factura").val(), Proveedor: $("#Id_Proveedor").val() }, elemento, cancelar);
    //cancelar();

}
var cancelar = function () {
    $("input[type=text], select").val("").attr("disabled", "disabled");
    recargarMenuLateral(["bNuevo", "bUploadFiles", "bSalir"]);
}
//var focusBeneficiario = function () {
//    if ($(this).val().length > 0)
//        $("#Id_Beneficiario").focusOut({
//            url: "/Tesoreria/FocusOut/Beneficiario",
//            data: { IdBeneficiario: $(this).val() },
//            campos: [{ Base: "NombreCompleto", Campo: "ProveedorName" }]
//        });
//}

var recargarTablaDocumentos = function () {
    $("#_containerTblDoctos").ajaxLoad({ url: "tblDocumentos", method: "POST", data: { tipo: $("#TipoCR").val(), folio: $("#FolioCR").val() } });
}

var recargarTablaArchivos = function () {
    $("#_tblFiles").ajaxLoad({ url: "tblArchivos", method: "POST", data: { TipoCR: $("#TipoCR").val(), FolioCR: $("#FolioCR").val() } });
}

