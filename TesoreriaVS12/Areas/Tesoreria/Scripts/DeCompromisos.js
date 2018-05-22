var nuevoDetalleCompromiso = function () {
    $("input[type=text], select").val("");
    $("#Id_Registro").val("0");
    $("#Importe").removeAttr("disabled");
    recargarMenuLateral(["bGuardar", "bCancelar"]);
    $("#Id_Movimiento").val("1");
    $("#Movimiento").val("CARGO");
    eventFocus();
}

var editarDetalleCompromiso = function () {
    $("#Importe").removeAttr("disabled");
    recargarMenuLateral(["bGuardar", "bCancelar"]);
    eventFocus();
}
var getNuevoDetalleCompromiso = function () {
    ajaxJson("getNuevoDetalleCompromiso", { TipoCompromiso: $("#Id_TipoCompromiso").val(), FolioCompromiso: $("#Id_FolioCompromiso").val() }, "POST", true, function (response) {
        cleanInputs();
        llenarMaestro(response.Data);
        botonera = [];
        $.each(response.Botonera, function (key, value) {
            botonera[key] = value;
        });
        recargarMenuLateral(botonera);
        disableInputs();        
    });
}

var eliminarDetalleCompromiso = function () {
    ajaxJson("deleteDetalleCompromiso", { TipoCompromiso: $("#Id_TipoCompromiso").val(), FolioCompromiso: $("#Id_FolioCompromiso").val(), Registro: $("#Id_Registro").val() }, "POST", true, function (response) {
        if (!response.Exito)
            ErrorCustom(response.Mensaje);
        else
            $("#frmRecarga").submit();
    });
}

var guardarDetalleCompromiso = function () {
    enableInputs();
    NonEditableInputs();
    $("#Importe").toNumber();    
    if ($("#frmDetalleCompromiso").valid()) {
        var cve = $("#Id_Area").val() + $("#Id_Funcion").val() + $("#Id_Actividad").val() + $("#Id_ClasificacionP").val() + $("#Id_Programa").val() + $("#Id_Proceso").val() + $("#Id_TipoMeta").val() + $("#Id_ActividadMIR").val() + $("#Id_Accion").val() + $("#Id_Alcance").val() + $("#Id_TipoG").val() + $("#Id_Fuente").val() + $("#AnioFin").val() + $("#Id_ObjetoG").val();
        ajaxJson("getDisponibilidad", { cve: cve, importe: $("#Importe").val(), fecha: $("#Fecha_Orden").val() }, "POST", true, function (response) {
            if (!response.Error) {
                if (response.Data.Disponibilidad) {
                    ajaxJson($("#frmDetalleCompromiso").attr("action"), $("#frmDetalleCompromiso").serialize(), "POST", true, function (response) {
                        //recargar botonera Mandar a guardar detalles
                        if (!response.Exito) {
                            ErrorCustom(response.Mensaje);
                        }
                        else {
                            $("#CanSaldar").val("true");
                            $("#frmRecarga").submit();
                        }
                    });
                }
                else if (response.Data.SinDisponibilidad == false && response.Data.Disponibilidad == false) {
                    ErrorCustom("Esta clave presupuestal no cuenta con disponibilidad.", "");
                }
                else if (response.Data.SinDisponibilidad && response.Data.Disponibilidad == false) {
                    ConfirmCustom("Esta clave presupuestal no cuenta con disponibilidad. ¿Desea continuar?", function () {
                        ajaxJson($("#frmDetalleCompromiso").attr("action"), $("#frmDetalleCompromiso").serialize(), "POST", true, function (response) {
                            //recargar botonera Mandar a guardar detalles
                            if (!response.Exito) {
                                ErrorCustom(response.Mensaje);
                            }
                            else {
                                $("#CanSaldar").val("true");
                                $("#frmRecarga").submit();
                            }
                        });
                    }, "", "Continuar", "Cancelar");
                }
            }
        });
    }
}

var saldarDetalleCompromiso = function () {
    ajaxJson("saldarMovimientos", { TipoCompromiso: $("#Id_TipoCompromiso").val(), FolioCompromiso: $("#Id_FolioCompromiso").val() }, "POST", true, function (response) {
        if (!response.Exito)
            ErrorCustom(response.Mensaje);
        else {
            $("#CanSaldar").val("false")
            $("#frmRecarga").submit();
        }
    });
}

var salir = function () {
    ajaxJson("generarPolizaComprometido", { TipoCompromiso: $("#Id_TipoCompromiso").val(), FolioCompromiso: $("#Id_FolioCompromiso").val() }, "POST", true, function (response) {
        if (!response.Exito)
            ErrorCustom(response.Mensaje);
        else
            $("#frmRegreso").submit();
    });
}

var seleccionarDetalle = function (params) {
    ajaxJson("getDetalleCompromiso", params, "POST", true, function (response) {
        cleanInputs();
        llenarMaestro(response.Data);
        $("#Id_Registro").val(response.Data.Id_Registro);
        if (response.Data.Id_Movimiento == 1)
            $("#Movimiento").val("CARGO");
        else if (response.Data.Id_Movimiento == 2)
            $("#Movimiento").val("ABNONO");

        $("#Importe").val(response.Data.Importe);
        if (response.Data.Disponibilidad)
            $("#Disponiblidad").attr("checked", true);
        else
            $("#Disponiblidad").attr("checked", false);

        botonera = [];
        $.each(response.Botonera, function (key, value) {
            botonera[key] = value;
        });
        recargarMenuLateral(botonera);
        $("#Importe").formatCurrency({ symbol: "" });
    });
}

var elimiarDetalle = function (params) {
    ajaxJson("deleteDetalleCompromiso", params, "POST", true, function (response) {
        if (!response.Exito)
            ErrorCustom(response.Mensaje);
        else {
            $("#CanSaldar").val(response.CanSaldar);
            $("#frmRecarga").submit();
        }
    });
}

var NonEditableInputs = function () {      
    $(".js_Descripcion").attr("disabled", "disabled");
}

var disableInputs = function () {
    $(".container input[type=text], textarea, select").attr("disabled", "disabled");    
}

var enableInputs = function () {
    $(".container input[type=text], textarea, select").removeAttr("disabled");    
}

var cleanInputs = function () {
    $(".container input[type=text], textarea").val("");
    $(".container select").val("");
}

var createBotonera = function (botones) {
    botonera = [];
    $.each(botones, function (key, value) {
        botonera[key] = value;
    });
    recargarMenuLateral(botonera);
}
