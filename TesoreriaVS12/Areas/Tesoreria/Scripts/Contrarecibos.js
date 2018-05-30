var ligarContrarecibo = function () {
    
	var elemento = $("input[name=Compromiso]:checked");
	ajaxJson(urlGuardarCompCR, { FolioCompromiso: elemento.data("foliocompromiso"), TipoCR: elemento.data("tipocr"), FolioCR: elemento.data("foliocr"), TipoCompromiso: elemento.data("tipocompromiso") }, "POST", true, function (response) {
	    if (response.Exito == true) {
	        $(".IdModal").modal("hide");
	        recargarMenuLateral(["bNuevo", "bEditar", "bBuscar", "bDetalles", "bCompromisos", "bDocumentos", "bSalir"]);
	        llenarMaestro(response.Registro);
	    }
	    else
	        ErrorCustom(response.Mensaje, "");
	});
}

var saveContraRecibo = function () {
    if ($("#frmContrarecibos").valid())
    {
        var desabilitados = $("input:disabled, select:disabled, textarea:disabled");
        desabilitados.removeAttr("disabled");
        ajaxJson(urlGuardarContraR, $("#frmContrarecibos").serialize(), "POST", true, function (response) {
            if (response.Exito == false)
                ErrorCustom(response.Mensaje, "");
            else
            {
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

var newContraRecivo = function () {
    ajaxJson(urlNuevoContraR, {}, "POST", true, function (response) {
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

var selectContraRecibo = function () {
    var elemento = $(this);
    ajaxJson(urlGetContraR, { IdFolio: elemento.data("folio"), IdTipoCR: elemento.data("tipo") }, "POST", true, function (response) {
        if (response.Exito == true) {
            llenarMaestro(response.Registro);
            $(".Modal1").modal("hide");
            $("#js_mCompromisos").removeClass("hide");
            createBotonera(response.Registro.Botonera);
            $("#Cargos").formatCurrency({ symbol: '' });
            //recargarMenuLateral(["bNuevo", (response.Registro.StateEdit != -1 ? "bEditar" : ""), "bBuscar", (response.Detalles > 0 ? "bDetalles" : ""), (response.Detalles > 0 ? "bDocumentos" : ""), "bCompromisos", (response.Registro.StateCancel > 0 ? "bCancelar" : ""), (response.hasDocuments > 0 ? "bReportes" : ""), "bSalir"]);
        }
        else
            ErrorCustom(response.Mensaje, "");
        return false;
    });
}

var canclearContrarecibo = function () {
    customModal(urlCancContraR, { FolioCR: $("#Id_FolioCR").val(), TipoCR: $("#Id_TipoCR").val() }, "GET", "size", function () {
        if($("#FechaCancelacionCR1").val() != "")
        {
            ajaxJson(urlCancContraR, $("#frmCancelaContra").serialize(), "POST", true, function (response) {
			    if (response.Exito) {
			        ExitoCustom("Contrarecibo cancelado correctamente", function () {
			            $(".IdModal").modal("hide");
			            llenarMaestro(response.Registro);
			            recargarMenuLateral(["bNuevo", "bBuscar", "bSalir"]);
			        });
				}
				else
					ErrorCustom(response.Mensaje, "");
			});
		}
		else
			return false;
		
    },"","Aceptar","Cancelar","Cancelar Cuenta por Liquidar","IdModal")
}

var activarEdicion = function () {
    switch (parseInt($("#StateEdit").val())) {
        case 1:
            $("#Id_Beneficiario, #Descripcion, #FechaVen, #Spei, #FechaCR").removeAttr("disabled");
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
    ConfirmCustom(Mensaje({TipoCR: $("#Ca_TipoCompromisos_Descripcion").val(), FolioCR: $("#Id_FolioCR").val(),  Total: $("#Cargos").val()}), function () {
        ajaxJson(urlImpContraR, { TipoCR: $("#Id_TipoCR").val(), FolioCR: $("#Id_FolioCR").val() }, "POST", true, function (response) {
            if (response.Exito == true) {
                if (typeof response.Registro != "undefined")
                    llenarMaestro(response.Registro);
                window.open(urlRptContraR+"?TipoCR=" + $("#Id_TipoCR").val() + "&FolioCR=" + $("#Id_FolioCR").val(),'_blank');
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }, "", "Aceptar", "Cancelar");
}
