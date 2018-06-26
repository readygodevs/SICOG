function Eliminar() {
    ConfirmCustom("¿Realmente deseas borrar esta Persona?", callBackBorrar, "", "Eliminar", "Cancelar");
}
function Editar() {
    Btn = "Editar";
    Desbloquear();
    recargarMenuLateral(["bGuardar", "bCancelar"]);

}
function Buscar() {
    customModal(RutaBuscar, {}, "get", "lg", callBackBuscarBeneficiario, "", "Buscar", "Cancelar", "Buscar Contribuyente", "IdModal")
}
function Guardar() {
    if ($('#IdCalle').is(':hidden')) {
        ErrorCustom("No se puede agregar si no termina de guardar la Calle");
        return false;
    }
    if ($('#IdColonia').is(':hidden')) {
        ErrorCustom("No se puede agregar si no termina de guardar la Colonia");
        return false;
    }
   // debugger;
    //ajaxJson("V_Personas", $("#frmBen").serialize(), "POST", false, callbackGuardar)

    if ($("#frmBen").valid()) {
      //  debugger;
        if (Btn == "Nuevo")
            ajaxJson("V_Personas", $("#frmBen").serialize(), "POST", false, callbackGuardar)
        else
            ajaxJson("V_PersonasEdit", $("#frmBen").serialize(), "POST", false, callbackGuardar)
    }


}
function Cancelar() {
    Bloquear();
    if (Btn == "Nuevo")
        recargarMenuLateral(["bNuevo", "bBuscar", "bImprimir", "bSalir"]);
    else
        recargarMenuLateral(["bNuevo", "bEditar", "bEliminar", "bBuscar", "bSalir"]);
    ColoniaHide();
    CalleHide();
    $(".js_Colonia").hide();
    $(".js_Calle").hide();
}
function Nuevo() {
  //  debugger;
    Btn = "Nuevo";
    Desbloquear();
    recargarMenuLateral(["bGuardar", "bCancelar"]);
    LimpiarCampos();
    $("#IdPersona").val("0");
    $(".js_Colonia").show();
    $(".js_Calle").show();
}
function Bloquear() {
    $("#frmBen input, #frmBen select, #frmBen textarea").attr("disabled", "disabled");
    $(".js_Agregar").attr("disabled", "disabled");
    $("#tblclasificacion .js_eliminar").attr("style", "display:none");
    $("#tblgiros .js_eliminar").attr("style", "display:none");
    $("#tblcontactos .js_eliminar").attr("style", "display:none");
}

function Desbloquear() {
    $("input, select, textarea").removeAttr("disabled");
    $(".js_Agregar").removeAttr("disabled");

}
function MunicipioSave() {
    if ($("#IdEstado").val() == "") {
        ErrorCustom("Favor de Ingresar los Campos Anteriores");
        return false;
    }
    ajaxJson(RutaMpioGuardar, { Estado: $("#IdEstado").val(), Municipio: $("#Municipio").val() }, "POST", true, function (Data) {
        callBackLlenarSelect(Data, "IdMunicipio", "");
        $(".js_MunicipioCancelar").click();
    });
}
function MunicipioHide() {
    $("#Municipio").hide();
    $("#IdMunicipio").show();
    $(".js_MunicipioGuardar").hide();
    $(".js_MunicipioCancelar").hide();
    $(".js_Municipio").show();
}
function MunicipioShow() {
    $("#Municipio").show();
    $("#IdMunicipio").hide();
    $(".js_MunicipioGuardar").show();
    $(".js_MunicipioCancelar").show();
    $(".js_Municipio").hide();
}
function LocalidadSave() {
    if ($("#IdEstado").val() == "" || $("#IdMunicipio").val() == "") {
        ErrorCustom("Favor de Ingresar los Campos Anteriores");
        return false;
    }
    ajaxJson(RutaLocGuardar, { Estado: $("#IdEstado").val(), Municipio: $("#IdMunicipio").val(), Localidad: $("#Localidad").val() }, "POST", true, function (Data) {
        callBackLlenarSelect(Data, "IdLocalidad", "");
        $(".js_LocalidadCancelar").click();
    });
}
function LocalidadHide() {
    $("#Localidad").hide();
    $("#IdLocalidad").show();
    $(".js_LocalidadGuardar").hide();
    $(".js_LocalidadCancelar").hide();
    $(".js_Localidad").show();
}
function LocalidadShow() {
    $("#Localidad").show();
    $("#IdLocalidad").hide();
    $(".js_LocalidadGuardar").show();
    $(".js_LocalidadCancelar").show();
    $(".js_Localidad").hide();
}
function ColoniaSave() {
    if ($("#IdEstado").val() == "" || $("#IdMunicipio").val() == "" || $("#IdLocalidad").val() == "") {
        ErrorCustom("Favor de Ingresar los Campos Anteriores");
        return false;
    }
    ajaxJson(RutaColGuardar, { Estado: $("#IdEstado").val(), Municipio: $("#IdMunicipio").val(), Localidad: $("#IdLocalidad").val(), Colonia: $("#Colonia").val() }, "POST", true, function (Data) {
        callBackLlenarSelect(Data, "IdColonia", "");
        $(".js_ColoniaCancelar").click();
    });
}
function ColoniaHide() {
    $("#Colonia").hide();
    $("#IdColonia").show();
    $(".js_ColoniaGuardar").hide();
    $(".js_ColoniaCancelar").hide();
    $(".js_Colonia").show();
}
function ColoniaShow() {
    $("#Colonia").show();
    $("#IdColonia").hide();
    $(".js_ColoniaGuardar").show();
    $(".js_ColoniaCancelar").show();
    $(".js_Colonia").hide();
}
function CalleSave() {
    if ($("#IdEstado").val() == "" || $("#IdMunicipio").val() == "" || $("#IdLocalidad").val() == "") {
        ErrorCustom("Favor de Ingresar los Campos Anteriores");
        return false;
    }
    ajaxJson(RutaCallGuardar, { Estado: $("#IdEstado").val(), Municipio: $("#IdMunicipio").val(), Localidad: $("#IdLocalidad").val(), Calle: $("#Calle").val() }, "POST", true, function (Data) {
        callBackLlenarSelect(Data, "IdCalle", "");
        $(".js_CalleCancelar").click();
    });
}
function CalleHide() {
    $("#Calle").hide();
    $("#IdCalle").show();
    $(".js_CalleGuardar").hide();
    $(".js_CalleCancelar").hide();
    $(".js_Calle").show();
}
function CalleShow() {
    $("#Calle").show();
    $("#IdCalle").hide();
    $(".js_CalleGuardar").show();
    $(".js_CalleCancelar").show();
    $(".js_Calle").hide();
}
function callbackGuardar(data) {
    if (data.Exito == false) {
        $("#IdPersona").val(data.IdPersona);
        ErrorCustom(data.Mensaje, "");
    }
    else {
        $("#IdPersona").val(data.IdPersona);
        ExitoCustom("Guardado correctamente", "");
        Bloquear();
        recargarMenuLateral(["bNuevo", "bEditar", "bEliminar", "bBuscar", "bSalir"]);
    }
}

function callBackEliminar(data) {
    if (data.Exito == false) {
        ErrorCustom(data.Mensaje, "");
    }
    else {
        ExitoCustom(data.Mensaje, "");
        LimpiarCampos();
        Bloquear();
        recargarMenuLateral(["bNuevo", "bBuscar", "bSalir"]);
    }
}
function callBackBuscarBeneficiario() {
    //resultsBeneficiarios
    ajaxLoad(RutaResultPersonas, { BDescripcionPersona: $("#BDescripcionBeneficiario").val() }, "resultsBeneficiarios", "POST", function () { })
}
function callBackBorrar() {
    ajaxJson("V_PersonasDelete", { IdPersona: $("#IdPersona").val() }, "POST", true, callBackEliminar)
}

function LimpiarCampos() {
    $("input, texbox").val("");
    if ($(".js_TipoCaptura.active").data("val") == 1)
        $("#PersonaFisica").val(true);
    else
        $("#PersonaFisica").val(false);
    $('#tblclasificacion').DataTable().clear().draw();
    $('#tblgiros').DataTable().clear().draw();
    $('#tblcontactos').DataTable().clear().draw();
    $("#IdEstado").val("").change();
    $("#lid").html("0");
}