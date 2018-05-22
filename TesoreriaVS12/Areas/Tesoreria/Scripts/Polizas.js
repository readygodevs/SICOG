var ultimo = false;
var nuevoDePolizas = function () {
    var opcion = parseInt($('input[name=TipoCaptura]:checked').val());
    console.log(typeof opcion);
    if (typeof opcion != "number")
        opcion = 2;
    switch (opcion) {
        case 1:
            $("#_detalles_polizas").ajaxLoad({
                url: urlCveCRI, callback: function () {
                    eventFocusCRI();
                    $(".container input[type=text], textarea").val("");
                    $(".container select").attr("disabled", "disabled");
                    $("#_detalles_polizas input[type=text],select,textarea").attr("disabled", "disabled");
                    $(".js_MovBancario").addClass("hide");
                    $("#Id_Movimiento").val(2);
                    $("input[type=text], select, #Id_Registro").val("");
                }
            });
            break;
        case 2:
            var Id_Fuente_Filtro = $("#Id_Fuente_Filtro").val();
            $("#_detalles_polizas").ajaxLoad({
                url: urlCve, callback: function () {
                    $("#Id_Fuente_Filtro").val(Id_Fuente_Filtro);
                    eventFocus();
                    $(".js_Captura").parent().parent().parent().show();
                    $("#_detalles_polizas input[type=text],select,textarea").attr("disabled", "disabled");
                    $("#Id_Cuenta, .js_Descripcion").attr("readonly", "readonly");
                    $(".container input[type=text], textarea").val("");
                    $(".container select").attr("disabled", "disabled");
                    $("#Id_Area").removeAttr("disabled");
                    $("#Id_Area").removeAttr("readonly");
                    $(".js_MovBancario").addClass("hide");
                    $("input[type=text], select, #Id_Registro").val("");
                }
            });
            break;
        case 3:
            $("#_detalles_polizas").ajaxLoad({
                url: urlCve, callback: function () {
                    eventFocusBalance();
                    $(".js_Captura").parent().parent().parent().hide();
                    $("#AnioFin").parent().parent().hide();
                    $("#Id_Cuenta").removeAttr("disabled");
                    $("input[type=text], #Id_Registro").val("");
                    $(".js_Descripcion").attr("disabled", "disabled");
                    $(".js_CuentaHiden").show();
                }
            });
            break;
    }
    $(".js_Descripcion").attr("disabled","disabled");
    recargarMenuLateral(["bCancelar", "bGuardar", "bSalir"]);
    
}

var editarDePolizas = function () {
    switch (parseInt($('input[name=TipoCaptura]:checked').val())) {
        case 1:
            ErrorCustom("Por el momento no puedes capturar detalles del tipo CRI", "");
            break;
        case 2:
            ajaxJson(urlFocusOAreas, {}, "POST", true, function (response) {
                $("#Id_Area").typeahead({ source: response.Data });
            });
            $("#Id_Cuenta, .js_Descripcion").attr("readonly", "readonly");
            $(".container select").attr("disabled", "disabled");
            $("#Id_Area").removeAttr("disabled");
            $("#Id_Area").removeAttr("readonly");
            addButtons(["bGuardar","bCancelar"]);
            eventFocus();
            break;
        case 3:
            $(".container select, input[type=text]").attr("readonly", "readonly");
            $("#Id_Area").attr("disabled","disabled");
            $("#Id_Cuenta, #Importe, #Id_Movimiento, #DescripcionMP").removeAttr('disabled');
            $("#Id_Cuenta, #Importe, #Id_Movimiento, #DescripcionMP").removeAttr('readonly');
            addButtons(["bGuardar","bCancelar"]);
            break;
    }
}

var seleccionarDetalle= function () {
    ajaxJson(urlGetDetPoliza, { IdTipo: $(this).data("idtipo"), IdFolio: $(this).data("idfolio"), IdMes: $(this).data("idmes"), IdRegistro: $(this).data("registro") }, "POST", true, function (response) {
        var maestro = response;
        switch (maestro.Tipo) {
            case 1:
                $('input[name=TipoCaptura][value=1]').trigger("click");
                //$('input[name=TipoCaptura][value=1]').parent().addClass("active").attr("checked");
                //$('input[name=TipoCaptura][value!=1]').parent().removeClass("active").removeAttr("checked");
                $("#_detalles_polizas").ajaxLoad({
                    url: urlCveCRI, callback: function () {
                        $(".container input[type=text], textarea").val("");
                        $(".container select").attr("disabled", "disabled");
                        $("#frmDePolizas input[type=text],select,textarea").attr("disabled", "disabled");
                        llenarMaestro(maestro.Registro);
                        $("#Importe").formatCurrency();
                        $(".js_MovBancario").addClass("hide");
                    }
                });
                break;
            case 2:
                $('input[name=TipoCaptura][value=2]').trigger("click");
                //$('input[name=TipoCaptura][value=2]').parent().addClass("active").attr("checked");
                //$('input[name=TipoCaptura][value!=2]').parent().removeClass("active").removeAttr("checked");
                $("#_detalles_polizas").ajaxLoad({
                    url: urlCve, callback: function () {
                        $(".js_Captura").parent().parent().parent().show();
                        $("#_detalles_polizas input[type=text],select,textarea").attr("disabled", "disabled");
                        $("#Id_Cuenta, .js_Descripcion").attr("readonly", "readonly");
                        $(".container input[type=text], textarea").val("");
                        $(".container select").attr("disabled", "disabled");
                        $(".js_MovBancario").addClass("hide");
                        $("#frmDePolizas input[type=text],select,textarea").attr("disabled", "disabled");
                        llenarMaestro(maestro.Registro);
                        $("#Importe").formatCurrency();
                        
                    }
                });
                break;
            case 3:
                $('input[name=TipoCaptura][value=3]').trigger("click");
                //$('input[name=TipoCaptura][value=3]').parent().addClass("active").attr("checked");
                //$('input[name=TipoCaptura][value!=3]').parent().removeClass("active").removeAttr("checked");
                $("#_detalles_polizas").ajaxLoad({
                    url: urlCve, callback: function () {
                        $(".js_Captura").parent().parent().parent().hide();
                        $("#AnioFin").parent().parent().hide();
                        $("#frmDePolizas input[type=text],select,textarea").attr("disabled", "disabled");
                        llenarMaestro(maestro.Registro);
                        $("#Importe").formatCurrency();
                        $(".js_CuentaHiden").show();
                        
                    }
                });
                break;
        }
        recargarBotoneraDetalles();
    });
}

var guardarDetalle = function () {
    var cve = $("#Id_Area").val() + $("#Id_Funcion").val() + $("#Id_Actividad").val() + $("#Id_ClasificacionP").val() + $("#Id_Programa").val() + $("#Id_Proceso").val() + $("#Id_TipoMeta").val() + $("#Id_ActividadMIR").val() + $("#Id_Accion").val() + $("#Id_Alcance").val() + $("#Id_TipoG").val() + $("#Id_Fuente").val() + $("#AnioFin").val() + $("#Id_ObjetoG").val();
    $("#Importe").toNumber();
    $(".js_Descripcion").attr("disabled", "disabled");
    if ($("#Id_Movimiento").val() == 1)
        guardarCargo(cve);
    else
        guardarAbono(cve);
}
var guardarAbono = function (cve) {
	if($("#frmDePolizas").valid())
	{
		ajaxJson($("#frmDePolizas").attr("action"), $("#frmDePolizas").serialize(), "POST", true, function (response) {
        //recargar botonera Mandar a guardar detalles
        if (!response.Exito) {
            ErrorCustom(response.Mensaje);
        }
        else {
            //var Acciones = _.template($('#jsActionsource').html());
            //response.Registro.Acciones = Acciones({ Id_Folio: response.Registro.Id_FolioPoliza, Id_Tipo: response.Registro.Id_TipoPoliza, Id_Mes: response.Registro.Id_MesPoliza, Id_Registro: response.Registro.Id_Registro });
            //if (response.Registro.Id_Movimiento == 1) {
            //    response.Registro.Cargos = response.Registro.Importe;
            //    response.Registro.Abonos = 0;
            //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
            //}
            //else {
            //    response.Registro.Abonos = response.Registro.Importe;
            //    response.Registro.Cargos = 0;
            //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
            //}
            //AddTbl(response.Registro, "tbldetalles");
            ExitoCustom("El registro se ha guardado correctamente", function () {
                llenarMaestro(response.Registro);
                $("#Importe").formatCurrency({symbol:''});
                $("input[type=text], select").attr("disabled", "disabled");
                recargarMenuLateral(["bNuevo", "bEliminar", "bSalir"]);
                cargarTablaDePolizas();
            });
		}
		});
	}
    
}

var guardarCargo = function (cve) {
    if ($("#frmDePolizas").valid())
    {
        if (parseInt($('input[name=TipoCaptura]:checked').val()) == 2) {
            ajaxJson(urlGetDisp, { cve: cve, importe: $("#Importe").val(), fecha: $("#Fecha").val() }, "POST", true, function (response) {
                if (!response.Error) {
                    if (response.Data.Disponibilidad) {
                        ajaxJson($("#frmDePolizas").attr("action"), $("#frmDePolizas").serialize(), "POST", true, function (response) {
                            //recargar botonera Mandar a guardar detalles
                            if (!response.Exito) {
                                ErrorCustom(response.Mensaje);
                            }
                            else {
                                //var Acciones = _.template($('#jsActionsource').html());
                                //response.Registro.Acciones = Acciones({ Id_Folio: response.Registro.Id_FolioPoliza, Id_Tipo: response.Registro.Id_TipoPoliza, Id_Mes: response.Registro.Id_MesPoliza, Id_Registro: response.Registro.Id_Registro });
                                //if (response.Registro.Id_Movimiento == 1) {
                                //    response.Registro.Cargos = response.Registro.Importe;
                                //    response.Registro.Abonos = 0;
                                //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
                                //}
                                //else {
                                //    response.Registro.Abonos = response.Registro.Importe;
                                //    response.Registro.Cargos = 0;
                                //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
                                //}
                                //AddTbl(response.Registro, "tbldetalles");
                                ExitoCustom("El registro se ha guardado correctamente", function () {
                                    cargarTablaDePolizas();
                                    llenarMaestro(response.Registro);
                                    $("#Importe").formatCurrency({ symbol: '' });
                                    $("input[type=text], select").attr("disabled", "disabled");
                                    recargarMenuLateral(["bNuevo", "bEliminar", "bSalir"]);
                                });
                            }
                        });
                    }
                    else if (response.Data.SinDisponibilidad == false && response.Data.Disponibilidad == false) {
                        ErrorCustom("Esta clave presupuestal no cuenta con disponibilidad.", "");
                    }
                    else if (response.Data.SinDisponibilidad && response.Data.Disponibilidad == false) {
                        ConfirmCustom("Esta clave presupuestal no cuenta con disponibilidad. ¿Desea continuar?", function () {
                            ajaxJson($("#frmDePolizas").attr("action"), $("#frmDePolizas").serialize(), "POST", true, function (response) {
                                //recargar botonera Mandar a guardar detalles
                                if (!response.Exito) {
                                    ErrorCustom(response.Mensaje);
                                }
                                else {
                                    //var Acciones = _.template($('#jsActionsource').html());
                                    //response.Registro.Acciones = Acciones({ Id_Folio: response.Registro.Id_FolioPoliza, Id_Tipo: response.Registro.Id_TipoPoliza, Id_Mes: response.Registro.Id_MesPoliza, Id_Registro: response.Registro.Id_Registro });
                                    //if (response.Registro.Id_Movimiento == 1) {
                                    //    response.Registro.Cargos = response.Registro.Importe;
                                    //    response.Registro.Abonos = 0;
                                    //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
                                    //}
                                    //else {
                                    //    response.Registro.Abonos = response.Registro.Importe;
                                    //    response.Registro.Cargos = 0;
                                    //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
                                    //}
                                    //AddTbl(response.Registro, "tbldetalles");
                                    ExitoCustom("El registro se ha guardado correctamente", function () {
                                        cargarTablaDePolizas();
                                        llenarMaestro(response.Registro);
                                        $("#Importe").formatCurrency({ symbol: '' });
                                        $("input[type=text], select").attr("disabled", "disabled");
                                        recargarMenuLateral(["bNuevo", "bEliminar", "bSalir"]);
                                    });
                                }
                            });
                        }, "", "Continuar", "Cancelar");
                    }
                }
            });
        }
        else {
            ajaxJson($("#frmDePolizas").attr("action"), $("#frmDePolizas").serialize(), "POST", true, function (response) {
                //recargar botonera Mandar a guardar detalles
                if (!response.Exito) {
                    ErrorCustom(response.Mensaje);
                }
                else {
                    //var Acciones = _.template($('#jsActionsource').html());
                    //response.Registro.Acciones = Acciones({ Id_Folio: response.Registro.Id_FolioPoliza, Id_Tipo: response.Registro.Id_TipoPoliza, Id_Mes: response.Registro.Id_MesPoliza, Id_Registro: response.Registro.Id_Registro });
                    //if (response.Registro.Id_Movimiento == 1) {
                    //    response.Registro.Cargos = response.Registro.Importe;
                    //    response.Registro.Abonos = 0;
                    //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
                    //}
                    //else {
                    //    response.Registro.Abonos = response.Registro.Importe;
                    //    response.Registro.Cargos = 0;
                    //    response.Registro.Id_Cuenta = response.Registro.Ca_Cuentas.Id_CuentaFormato;
                    //}
                    //AddTbl(response.Registro, "tbldetalles");
                    ExitoCustom("El registro se ha guardado correctamente", function () {
                        cargarTablaDePolizas();
                        llenarMaestro(response.Registro);
                        $("#Importe").formatCurrency({ symbol: '' });
                        $("input[type=text], select").attr("disabled", "disabled");
                        recargarMenuLateral(["bNuevo", "bEliminar", "bSalir"]);
                    });
                }
            });
        }
    }
}

var buildFiltros = function () {
    var f = {};
    switch (parseInt($('input[name=TipoCaptura]:checked').val())) {
        case 1:
            f.GeneroStr = "4";
            if ($("#IdCuenta").length > 0)
                f.IdCuenta = $("#IdCuenta").val();
            else
                f.IdCuenta = $("#Id_Cuenta").val();
            if ($("#Descripcion_filtro").length > 0)
                f.Descripcion = $("#Descripcion_filtro").val();
            else
                f.Descipcion = $("#Descripcion").val();
            f.selectUltimoNivel = true;
            f.IdCri = $("#Id_Concepto").val();
            break;
        case 2:
            break;
        case 3:
            f.GeneroStr = "1,2,3,6,7,9";
            if ($("#IdCuenta").length > 0)
                f.IdCuenta = $("#IdCuenta").val();
            else
                f.IdCuenta = $("#Id_Cuenta").val();
            if ($("#Descripcion_filtro").length > 0)
                f.Descripcion = $("#Descripcion_filtro").val();
            else
                f.Descipcion = $("#Descripcion").val();
            f.selectUltimoNivel = true;
            break;
    }
    
    return f;
}

var validacionesDePolizas = function () {
    if ($("#Ca_ClasificaPolizas_Automatica").val() == "True")
        $("#_detalles_polizas input,select,textarea").attr("disabled", "disabled");
}

var seleccionarCuenta = function(){
    var elemento = $(this);
    $("#Id_Cuenta").val(elemento.data("idcuenta"));
    $("#Ca_Cuentas_Descripcion").val(elemento.data("descripcion"));
    $(".IdModal").modal("hide");
    $("#Id_Movimiento").removeAttr("disabled");
}

var eleiminarDePolizas = function(){
    var elemento = $(this);
    ConfirmCustom("¿Está seguro de eliminar el detalle?",function(){
        ajaxJson(urlElimPoliza, { IdTipo: $("#Id_TipoPoliza").val(), IdFolio: $("#Id_FolioPoliza").val(), IdMes: $("#Id_MesPoliza").val(), IdRegistro: $("#Id_Registro").val() }, "POST", true, function (response) {
            if(response.Exito == true){
                ExitoCustom("El detalle se eliminó correctamente",function(){
                    var table = $("#tbldetalles").dataTable();
                    if ($("#tbldetalles tbody tr").length == 1)
                        ultimo = true;
                    else
                        ultimo = false;
                    //table.fnDeleteRow(table.fnGetPosition($(".js_Eliminar[data-registro='" + $("#Id_Registro").val() + "'][data-idmes='" + $("#Id_MesPoliza").val() + "'][data-idtipo='" + $("#Id_TipoPoliza").val() + "'][data-idfolio='" + $("#Id_FolioPoliza").val() + "']").parent().parent().get(0)));
                    cargarTablaDePolizas();
                    limpiarDetalle();
                    recargarMenuLateral(["bNuevo","bSalir"]);
                });
            }
        });
    },"","Si","Cancelar");
    
}

var recargarBotoneraDetalles = function () {
    if ($("#js_mNuevo").length > 0) {
        recargarMenuLateral(["bNuevo", "bEliminar", "bSalir"]);
        if ($("#Id_Area").length > 0) {
            $("#Id_Area").removeAttr("disabled");
            $("#Id_Area").removeAttr("readonly");
        }
        if ($("#Id_CentroRecaudador").length > 0) {
            $("#Id_CentroRecaudador").removeAttr("disabled");
            $("#Id_CentroRecaudador").removeAttr("readonly");
        }
        if ($("#Id_Cuenta").length > 0 && parseInt($('input[name=TipoCaptura]:checked').val()) == 3)
            $("#Id_Cuenta").removeAttr("disabled");
    }
}

var limpiarDetalle = function () {
    $("input[type=text], select").val("").attr("disabled", "disabled");
}

var cargarTablaDePolizas = function () {
    var parameters = getUrlVars();
    $("#container_DetallesPolizas").ajaxLoad({
        url: urlTbaDetPoliza,
        data: { IdTipo: parameters.IdTipo, IdFolio: parameters.IdFolio, IdMes: parameters.IdMes },
        method: "POST"
    });
}