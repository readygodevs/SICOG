var buscarCompromisos = function () {
    customModal(urlBusquedaCompromisos,
                {},
                "GET",
                "lg",
                function () {
                    ajaxLoad($("#frmOrden").attr("action"), $("#frmOrden").serialize(), "resultsCompromisos", "POST", "");
                },
                "",
                "Buscar",
                "Cerrar",
                "Buscar Compromiso",
                "ModalBusCompromisos");
}

var getNuevoCompromiso = function () {
    ajaxJson("getNuevoCompromiso", {}, "POST", true, function (response) {
        if (!response.Error) {
            cleanInputs();
            llenarMaestro(response.Data);
            enableInputs();
            NonEditableInputs();
            recargarMenuLateral(["bGuardar", "bCancelar"]);
        }
        else
            ErrorCustom(response.Message);
    });
}

var getCompromiso = function () {
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

var guardarCompromiso = function () {
    if ($("#frmOrdenCompra").valid()) {
        ajaxJson($("#frmOrdenCompra").attr("action"), $("#frmOrdenCompra").serialize(), "POST", true, function (response) {
            //recargar botonera Mandar a guardar detalles
            if (!response.Exito) {
                ErrorCustom(response.Mensaje);
            }
            else {
                //recargarMenuLateral(["bNuevo", "bEditar", "bCancelar", "bBuscar", "bDetalles", "bRecibido", "bSalir"]);
                createBotonera(response.Botonera);
                disableInputs();
                $("#Id_TipoCompromiso").val(response.TipoCompromiso);
                $("#Id_FolioCompromiso").val(response.FolioCompromiso);
            }
        });
    }
}

var irDetalles = function () {
    $("#TipoCompromiso").val($("#Id_TipoCompromiso").val());
    $("#FolioCompromiso").val($("#Id_FolioCompromiso").val());
    $("#frmDetalles").submit();
}

var recibirCompromiso = function () {
    customModal("Recibir", { tcompromiso: $("#Id_TipoCompromiso").val(), fcompromiso: $("#Id_FolioCompromiso").val() }, "GET", "",
                function () {
                    if ($("#frmRecibido").valid()) {
                        ajaxJson($("#frmRecibido").attr("action"), $("#frmRecibido").serialize(), "POST", true, function (response) {
                            if (!response.Exito) {
                                ErrorCustom(response.Mensaje);
                            }
                            else {
                                //recargarMenuLateral(["bNuevo", "bEditar", "bCancelar", "bBuscar", "bDetalles", "bRecibido", "bSalir"]);  
                                getCompromiso();
                                $(".recibeCompromiso").modal("hide");
                            }
                        });
                    }
                }, "", "Aceptar", "Cancelar", "Recibir compromiso", "recibeCompromiso");
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

var NonEditableInputs = function () {
    $("input[type=text].NonEditable, textarea.NonEditable").attr("readonly", "readonly");
    $("select.NonEditable").attr("disabled", "disabled");
}

var disableInputs = function () {
    $(".container input[type=text], textarea").attr("disabled", "disabled");
    $(".container select").attr("disabled", "disabled");
}

var enableInputs = function () {
    $(".container input[type=text], textarea").removeAttr("disabled");
    $(".container select").removeAttr("disabled");
}

var cleanInputs = function () {
    $(".container input[type=text], textarea").val("");
    $(".container select").val("");
}

