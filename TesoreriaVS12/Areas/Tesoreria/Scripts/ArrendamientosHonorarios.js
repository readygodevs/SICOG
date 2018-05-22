var ligarContrarecibo = function () {

    var elemento = $("input[name=Compromiso]:checked");
    if (elemento.data("foliocompromiso") != undefined) {
        ajaxJson("/Tesoreria/Contrarecibos/GuardarComprimisoCR", { FolioCompromiso: elemento.data("foliocompromiso"), TipoCR: elemento.data("tipocr"), FolioCR: elemento.data("foliocr"), TipoCompromiso: elemento.data("tipocompromiso") }, "POST", true, function (response) {
            if (response.Exito == true) {
                $(".IdModal").modal("hide");
                recargarMenuLateral(["bNuevo", "bEditar", "bBuscar", "bDetalles", "bCompromisos", "bDocumentos","bAgregarImporte", "bSalir"]);
                llenarMaestro(response.Registro);
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    } else {
        ErrorCustom("Favor de seleccionar un Compromiso");
    }
    
}

var saveArrendamiento = function () {
    if ($("#frmContrarecibos").valid()) {
        var desabilitados = $("input:disabled, select:disabled, textarea:disabled");
        desabilitados.removeAttr("disabled");
        ajaxJson("/Tesoreria/Contrarecibos/GuardarContrarecibo", $("#frmContrarecibos").serialize(), "POST", true, function (response) {
            if (response.Exito == false)
                ErrorCustom(response.Mensaje, "");
            else {
                ExitoCustom("El registro se ha guardado correctamente", function () {
                    llenarMaestro(response.Registro);
                    //desabilitados.attr("disabled", "disabled");
                    $("input[type=text], textarea").attr("disabled", "disabled");
                    recargarMenuLateral(response.Registro.Botonera);
                });
            }

        });
    }
}
var agregarImporte = function () {
    customModal("/Tesoreria/Contrarecibos/AgregarImporte", { FolioCR: $("#Id_FolioCR").val(), TipoCR: $("#Id_TipoCR").val() }, "GET", "lg", function () {
      //  debugger;
        if ($("#frmImporte").valid()) {
            $("#Importe").val($("#Importe").autoNumeric("get"));
            ajaxJson("/Tesoreria/Contrarecibos/AgregarImporte", $("#frmImporte").serialize(), "POST", true, function (response) {
                if (response.Exito) {
                    ExitoCustom(response.Mensaje, function () {
                        $(".IdModal").modal("hide");
                        llenarMaestro(response.Registro);
                       
                        $("#Ca_Cuentas_FR.Descripcion2").val(response.Registro.Descripcion2);
                        $("#Ca_Cuentas_FR.Descripcion3").val(response.Registro.Descripcion3);
                        $("#Ca_Cuentas_FR.Descripcion4").val(response.Registro.Descripcion4);

                        $("#Cargos").autoNumeric();
                        $("#Importe_AH").autoNumeric();
                        $("#Importe_CH").autoNumeric();
                        recargarMenuLateral(response.Registro.Botonera);
                    });
                } else {
                    ErrorCustom(response.Mensaje);
                }
            });
        }
    }, "", "Aceptar", "Cancelar", "Agregar Importe", "IdModal")
}

var nuevoArrendamiento = function () {
    ajaxJson("/Tesoreria/Contrarecibos/NuevoArrendamiento", {idTipoCR:$("#Id_TipoCR").val()}, "POST", true, function (response) {
        $("input[type=text], textarea").val("");
        llenarMaestro(response.Registro);
        $("#FechaCR").val(response.fActual);
        $("#FechaCR").datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            startDate: response.fMin,
            endDate: response.fMax
        });
        $("#FechaCR").datepicker("setStartDate", response.fMin);
        $("#FechaCR").datepicker("setEndDate", response.fMax);
    });
    $(".container #Descripcion, #FechaCR").val("");
    $(".container #Descripcion, #FechaCR").removeAttr("disabled");

    recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
}

var selectArrendamiento = function () {
    var elemento = $(this);
    ajaxJson("/Tesoreria/Contrarecibos/GetContrarecibo", { IdFolio: elemento.data("folio"), IdTipoCR: elemento.data("tipo") }, "POST", true, function (response) {
        if (response.Exito == true) {
            llenarMaestro(response.Registro);
            $(".Modal1").modal("hide");
            $("#js_mCompromisos").removeClass("hide");
            createBotonera(response.Registro.Botonera);
            $("#Cargos").formatCurrency({ symbol: '' });
            $("#Importe_AH").formatCurrency({ symbol: '' });
            $("#Importe_CH").formatCurrency({ symbol: '' });
            //recargarMenuLateral(["bNuevo", (response.Registro.StateEdit != -1 ? "bEditar" : ""), "bBuscar", (response.Detalles > 0 ? "bDetalles" : ""), (response.Detalles > 0 ? "bDocumentos" : ""), "bCompromisos", (response.Registro.StateCancel > 0 ? "bCancelar" : ""), (response.hasDocuments > 0 ? "bReportes" : ""), "bSalir"]);
        }
        else
            ErrorCustom(response.Mensaje, "");
        return false;
    });
}

var cancelarArrendamiento = function () {
    customModal("/Tesoreria/Contrarecibos/CancelarContrarecibo", { FolioCR: $("#Id_FolioCR").val(), TipoCR: $("#Id_TipoCR").val() }, "GET", "size", function () {
        if ($("#FechaCancelacionCR1").val() != "") {
            ajaxJson("/Tesoreria/Contrarecibos/CancelarContrarecibo", $("#frmCancelaContra").serialize(), "POST", true, function (response) {
                if (response.Exito) {
                    ExitoCustom("Contrarecibo cancelado correctamente", function () {
                        $(".IdModal").modal("hide");
                        llenarMaestro(response.Registro);
                        recargarMenuLateral(["bNuevo", "bBuscar", "bSalir"]);
                    });
                }
            });
        }
        else
            return false;
    }, "", "Aceptar", "Cancelar", "Cancelar Cuenta por Liquidar", "IdModal")
}

var activarEdicion = function () {
    switch (parseInt($("#StateEdit").val())) {
        case 1:
            $("#Descripcion, #FechaVen, #Spei, #FechaCR").removeAttr("disabled");
            break;
        case 2:
            $("#Descripcion, #Spei").removeAttr("disabled");
            break;
        case 3:
            $("#Descripcion, #Spei").removeAttr("disabled");
            //$("#FechaVen").datepicker();
            break;
    }
    setFechas();
    recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
}

var imprimirContraRecibo = function () {
    var Mensaje = _.template($('#js_msjImprimir').html());
    ConfirmCustom(Mensaje({ TipoCR: $("#Ca_TipoCompromisos_Descripcion").val(), FolioCR: $("#Id_FolioCR").val(), Total: $("#Cargos").val() }), function () {
        ajaxJson("/Tesoreria/Contrarecibos/ImprimirContraRecibo", { TipoCR: $("#Id_TipoCR").val(), FolioCR: $("#Id_FolioCR").val() }, "POST", true, function (response) {
            if (response.Exito == true) {
                if (typeof response.Registro != "undefined")
                    llenarMaestro(response.Registro);
                window.open("/Tesoreria/Contrarecibos/Reporte_ContraRecibo?TipoCR=" + $("#Id_TipoCR").val() + "&FolioCR=" + $("#Id_FolioCR").val(), '_blank');
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }, "", "Aceptar", "Cancelar");
}
