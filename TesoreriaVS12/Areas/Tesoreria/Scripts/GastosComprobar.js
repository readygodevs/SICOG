var ligarContrarecibo = function () {
	var elemento = $("input[name=Compromiso]:checked");
	ajaxJson("GuardarComprimisoCR", { FolioCompromiso: elemento.data("foliocompromiso"), TipoCR: elemento.data("tipocr"), FolioCR: elemento.data("foliocr"), TipoCompromiso: elemento.data("tipocompromiso") }, "POST", true, function (response) {
	    if (response.Exito == true) {
	        $("#IdModal").modal("hide");
	        recargarMenuLateral(["bNuevo", "bEditar", "bBuscar", "bDocumentos", "bSalir"]);
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
        ajaxJson("/Tesoreria/Contrarecibos/GuardarContrarecibo", $("#frmContrarecibos").serialize(), "POST", true, function (response) {
            if (response.Exito == false)
                ErrorCustom(response.Mensaje, "");
            desabilitados.attr("disabled", "disabled");
            recargarMenuLateral(["bNuevo", "bEditar", "bBuscar", "bCompromisos","bSalir"]);
        });
    }
}

var newContraRecivo = function () {
    ajaxJson("/Tesoreria/Contrarecibos/NuevoContrarecibo", {}, "POST", true, function (response) {
        llenarMaestro(response.Registro);
    });
    $(".container #Id_Beneficiario, #Descripcion, #FechaCR").val("");
    $(".container #Id_Beneficiario, #Descripcion, #FechaCR").removeAttr("disabled");
    recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
}

var selectContraRecibo = function () {
    var elemento = $(this);
    ajaxJson("/Tesoreria/Contrarecibos/GetContrarecibo", { IdFolio: elemento.data("folio"), IdTipoCR: elemento.data("tipo") }, "POST", true, function (response) {
        if (response.Exito == true) {
            llenarMaestro(response.Registro);
            $(".Modal1").modal("hide");
            $("#js_mCompromisos").removeClass("hide");
            recargarMenuLateral(["bNuevo", (response.Registro.StateEdit != -1 ? "bEditar" : ""), "bBuscar", (response.Registro.Detalles > 0 ? "bDetalles" : ""), (response.Registro.Detalles > 0 ? "bDocumentos" : ""), "bCompromisos", (response.Registro.hasDocuments > 0 ? "bReportes" : ""), "bSalir"]);
        }
        else
            ErrorCustom(response.Mensaje, "");
        return false;
    });
}

var activarEdicion = function () {
    switch (parseInt($("#StateEdit").val())) {
        case 1:
            $("#Id_Beneficiario, #Descripcion").removeAttr("disabled");
            break;
        case 2:
            $("#Descripcion").removeAttr("disabled");
            break;
        case 3:
            $("#Descripcion, #FechaVen").removeAttr("disabled");
            //$("#FechaVen").datepicker();
            break;
    }
    var buttons = [];
    $.each($("#menu-lateral ul li"), function (item, value) {
        buttons[item] = $(value).data("name");
    });
    buttons[$("#menu-lateral ul li").length] = "bGuardar";
    recargarMenuLateral(buttons);
}