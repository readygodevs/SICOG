
var llenarCuentasBanco = function () {
    ajaxSelect("/Tesoreria/Listas/List_CtaBancaria", { Id_Banco: $(this).val() }, "POST", true, "Id_CtaBancaria", "", callBackLlenarSelect);
}

var desabilitarCampos = function (opcion) {
    //Op = 1 (Nuevo)
    switch (opcion) {
        case 1:
            $('#Id_CajaR, #Fecha, #IdContribuyente, #Observaciones,#Id_CtaBancaria,#Id_Banco').removeAttr('disabled');
            break;
        default:
            $('input[type=text],input[type=checkbox],select, textarea').removeAttr('disabled');
            break;
    }
}

var habilitarCampos = function (opcion) {
    //Op = 1 (despues de Guardar)
    switch (opcion) {
        case 1:
            $('#Id_CajaR, #Fecha, #IdContribuyente, #Observaciones,#Id_CtaBancaria,#Banco').attr('disabled','disabled');
            break;
        default:
            $('input[type=text],input[type=checkbox],select, textarea').removeAttr('disabled');
            break;
    }
}

var newReciboIngresos = function () {
    ajaxJson("newReciboIngresos", {}, "POST", true, function (response) {
        $("input[type=text],select").val("");
        llenarMaestro(response.Registro);
        desabilitarCampos(1);
        $("#Fecha").datepicker({
            format: "dd/mm/yyyy",
            altFormat: "dd/mm/yyyy",
            autoclose: true,
            startDate: response.fMin,
            endDate: response.fMax
        });
        $("#Fecha").datepicker("setStartDate", response.fMin);
        $("#Fecha").datepicker("setEndDate", response.fMax);
        //ajaxSelect("/Tesoreria/Listas/List_Bancos", {}, "POST", true, "Banco", "", callBackLlenarSelect);
        recargarMenuLateral(["bGuardar","bCancelar","bSalir"]);
    });
}
var saveRecibo = function () {
    if ($("#frmRecibo").valid()) {
        $("#frmRecibo input[type=text]").removeAttr('disabled');
        ajaxJson("GuardarRecibo", $("#frmRecibo").serialize(), "POST", true, function (response) {
            if (response.Exito) {
                llenarMaestro(response.Registro);
                recargarMenuLateral(response.Registro.Botonera);
                $('input[type=text],input[type=checkbox],select, textarea').attr('disabled', 'disabled');
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }
    else
        ErrorCustom("Antes de guardar, llena los campos correctamente", "");
}

var chageContribuyente = function () {
    if ($(this).val().split('-').length != 1) {
        $(this).val($(this).val().split('-')[1]);
        if ($(this).val().length > 0 && $.inArray($(this).val(), $(this).data("dataSource")) == -1) {
            ajaxJson("getContribuyenteData", { IdPersona: $(this).val() }, "POST", true, function (response) {
             //   debugger;
                if (response.Exito) {
                    $('#DomicilioContribuyente').val(response.persona.Domicilio);
                    $('#NombreContribuyente').val(response.persona.NombreCompleto);
                    $("#Observaciones").focus();
                } else {
                    ErrorCustom(response.Mensaje, "");

                }
            });
        }
    }
}

var chageBeneficiario = function () {
    if ($(this).val().split('-').length != 1) {
        $(this).val($(this).val().split('-')[1]);
        if ($(this).val().length > 0 && $.inArray($(this).val(), $(this).data("dataSource")) == -1) {
            ajaxJson("getContribuyenteData", { IdPersona: $(this).val() }, "POST", true, function (response) {
               // $('#DomicilioContribuyente').val(response.Domicilio);
                $('#Ca_Beneficiarios_NombreCompleto').val(response.Nombre);
                $("#Observaciones").focus();
            });
        }
    }
}

var callBackBuscarBeneficiario = function () {
    ajaxLoad("/Tesoreria/Busquedas/Tbl_Personas", { BDescripcionPersona: $("#BDescripcionBeneficiario").val() }, "resultsBeneficiarios", "POST", function () { })
}

var BuscarContribuyente = function () {
    if(!$(this).isSiblingDisabled())
        customModal("/Tesoreria/Busquedas/Buscar_Beneficiario", {}, "get", "lg", callBackBuscarBeneficiario, "", "Buscar", "Cancelar", "Buscar Contribuyente", "IdModal");
}

var selectContribuyente = function(){
    $('#IdContribuyente').val($(this).data('idbeneficiario'));
    $(".IdModal").modal('hide');
    ajaxJson("getContribuyenteData", { IdPersona: $(this).data('idbeneficiario') }, "POST", true, function (response) {
        $('#DomicilioContribuyente').val(response.Domicilio);
        $('#NombreContribuyente').val(response.NombreCompleto);
        $("#Observaciones").focus();
    });
}

var buscarRecibo = function () {
    customModal("BuscarRecibo", {}, "GET", "lg", function () {
        ajaxLoad("tblResultadosRecibos", $('#frmBusqueda').serialize(), "resultadosRecibos", "POST", "");
    }, "", "Buscar", "Cancelar", "Buscar Recibo", "IdModal");
    return false;
}
var selectRecibo = function () {
    ajaxJson("getRecibo", { IdRecibo: $(this).data("idrecibo") }, "POST", true, function (response) {
        if(response.Exito)
        {
            $(".IdModal").modal("hide");
            llenarMaestro(response.Registro);
            recargarMenuLateral(response.Registro.Botonera);
            $("#Fecha").datepicker({
                format: "dd/mm/yyyy",
                autoclose: true,
                startDate: response.fMin,
                endDate: response.fMax
            });
            $("#Fecha").datepicker("setStartDate", response.fMin);
            $("#Fecha").datepicker("setEndDate", response.fMax);
        }
    });
    return false;
}

var guardarDetalle = function () {
   if($("#frmDetallesRecibos").valid()){
	   $("#Id_Movimiento").removeAttr("disabled");
		ajaxJson($("#frmDetallesRecibos").attr("action"), $("#frmDetallesRecibos").serialize() + "&" + $("#frmDetallesReturn").serialize(), "POST", true, function (response) {
			if (!response.Exito) {
				ErrorCustom(response.Mensaje);
			}
			else {
				cargarTabla();
				ultimo = false;
				desHabilitar();
				llenarMaestro(response.Registro);
				$("#Importe").formatCurrency({ symbol: '' });
				recargarMenuLateral(["bNuevo","bEditar","bEliminar","bSalir"]);
			}
		});
   }
}

var buildFiltros = function () {
    var f = {};
    switch (parseInt($('input[name=TipoCaptura]:checked').val())) {
        case 3:
            if ($("#IdCuenta").length > 0)
                f.IdCuenta = $("#IdCuenta").val();
            else
                f.IdCuenta = $("#Id_Cuenta").val();
            if ($("#Descripcion_filtro").length > 0)
                f.Descripcion = $("#Descripcion_filtro").val();
            else
                f.Descipcion = $("#Descripcion").val();
            f.selectUltimoNivel = true;
            f.RestringirCuentas = true;
            f.ParametroCuentas = "Cuentas_ReciboIngresos";
            break;
    }

    return f;
}

var cargarTabla = function () {
    $("#container_DetallesRecibos").ajaxLoad({
        url: "/Tesoreria/Ingresos/TablaDetallesRecibos",
        data: { Folio: $("#Folio").val() },
        method: "POST"
    });
}

var desHabilitar = function () {
    $("input[type=text], select").attr('disabled', 'disabled');
}

var newDetalle = function () {
    switch (parseInt($('input[name=TipoCaptura]:checked').val())) {
        case 1:
            $("#_detalles_ReciboIgresos").ajaxLoad({
                url: "/Tesoreria/Cuentas/partialClavePresupuestalCRI", callback: function () {
                    eventFocusCRI();
                    $("input[type=text], textarea,select").val("");
                    $("select").attr("disabled", "disabled");
                    $("#_detalles_ReciboIgresos input[type=text],select,textarea").attr("disabled", "disabled");
                    $(".js_MovBancario").addClass("hide");
                    $('#Id_Movimiento').val(2);
                    $("#IdRegistro").val("");
                }
            });
            break;
        case 3:
            $("#_detalles_ReciboIgresos").ajaxLoad({
                url: "/Tesoreria/Cuentas/partialClavePresupuestal", callback: function () {
                    eventFocusBalance();
                    $(".js_Captura").parent().parent().parent().hide();
                    $("#AnioFin").parent().parent().hide();
                    $('#Id_Movimiento').val(1).attr("disabled", "disabled");
                    $(".js_Descripcion").attr("disabled", "disbled");
                    $('#Id_Movimiento').val(1);
                    $("#IdRegistro").val("");
                }
            });
            break;
    }
    $("input[type=text], select").val("");
    
    recargarMenuLateral(["bGuardar", "bCancelar"]);
}

var seleccionarDetalle = function () {
    ajaxJson("/Tesoreria/Ingresos/getDetalleRecibo", { Folio: $(this).data("idfolio"), IdRegistro: $(this).data("registro") }, "POST", true, function (response) {
        if (response.Registro.Id_ClavePresupuestoIng != null) {
//            $('input[name=TipoCaptura][value!=1]').removeAttr("checked").parent().removeClass("active");
            $('input[name=TipoCaptura][value=1]').trigger("click");
            $("#_detalles_polizas").ajaxLoad({
                url: "/Tesoreria/Cuentas/partialClavePresupuestalCRI", callback: function () {
                    $(".container input[type=text], textarea").val("");
                    $(".cri").show();
                    $(".container select").attr("disabled", "disabled");
                    llenarMaestro(response.Registro);
                    $("#Importe").formatCurrency();
                    $(".js_Descripcion").attr("disabled", "disabled");
                    $('input[type=text],input[type=checkbox],select, textarea').attr('disabled', 'disabled');
                }
            });
        }
        else
        {
            //$('input[name=TipoCaptura][value!=3]').removeAttr("checked").parent().removeClass("active");
            //$('input[name=TipoCaptura][value=3]').attr("checked", "checked").parent().addClass("active");
            $('input[name=TipoCaptura][value=3]').trigger("click");
            $("#_detalles_polizas").ajaxLoad({
                url: "/Tesoreria/Cuentas/partialClavePresupuestal", callback: function () {
                    $(".cri").hide();
                    llenarMaestro(response.Registro);
                    $("#Importe").formatCurrency();
                    $('input[type=text],input[type=checkbox],select, textarea').attr('disabled', 'disabled');
                }
            });
        }
        recargarMenuLateral(response.Registro.Botonera);
    });
}

var editarDetalle = function () {
    if (parseInt($('input[name=TipoCaptura]:checked').val()) == 1) {
        eventFocusCRI();
        $("#Id_CentroRecaudador").removeAttr("disabled");
    }
    else {
        eventFocusBalance();
        $("#Id_Cuenta").removeAttr("disabled");
    }
    $(".js_Captura, .js_Descripcion, #Importe, #Id_Cuenta").val("");
    recargarMenuLateral(["bCancelar", "bGuardar", "bSalir"]);
}

var cancelarDetalle = function () {
    $("input[type=text]").val("").attr("disabled", "disabled");
    $("#IdRegistro").val("");
    recargarMenuLateral(["bNuevo", "bSalir"]);
}

var eliminarDetalle = function () {
    ConfirmCustom("¿Está seguro de eliminar el detalle?", function () {
        ajaxJson("/Tesoreria/Ingresos/EliminarDeRecibo", { Folio: $("#Folio").val(), Registro: $("#IdRegistro").val() }, "POST", true, function (response) {
            if (response.Exito == true) {
                ErrorCustom("El detalle se eliminó correctamente", function () {
                    var table = $("#tbldetalles").dataTable();
                    if ($("#tbldetalles tbody tr").length == 1)
                        ultimo = true;
                    else
                        ultimo = false;
                    table.fnDeleteRow(table.fnGetPosition($(".js_Eliminar[data-registro='" + $("#IdRegistro").val() + "'][data-idfolio='" + $("#Folio").val() + "']").parent().parent().get(0)));
                    limpiarDetalle();
                    recargarMenuLateral(["bNuevo", "bSalir"]);
                    $('input[name=TipoCaptura][value=1]').trigger("click");
                });
            }
        });
    }, "", "Si", "Cancelar");

    //ajaxJson("/Tesoreria/Ingresos/EliminarDeRecibo", { Folio: $("#Folio").val(), Registro: $("#IdRegistro").val() }, "POST", true, function (response) {

    //});
}

var limpiarDetalle = function () {
    $("input[type=text], select").val("").attr("disabled", "disabled");
}

var salirDetalle = function () {
    $("#frmDetallesReturn").submit();
}

var activarEdicionMaestro = function () {
    $("#Id_CajaR,#Fecha,#IdContribuyente,#Observaciones,#Id_Banco,#Id_CtaBancaria").removeAttr("disabled");
    recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
}

var cancelarRecibo = function () {
    customModal("/Tesoreria/Ingresos/CancelarRecibo", { FechaCancelacion: $("#Fecha").val() }, "GET", "", function () {
        if ($("#FechaCancelacion2").val() != "") {
            ajaxJson("/Tesoreria/Ingresos/CancelarRecibo", { Folio: $("#Folio").val(), FechaCancelacion: $("#FechaCancelacion2").val() }, "POST", true, function (response) {
                if (response.Exito == true)
                {
                    llenarMaestro(response.Registro);
                    recargarMenuLateral(response.Registro.Botonera);
                    $(".IdModal").modal("hide");
                }
                else
                {
                    ErrorCustom(response.Mensaje, "");
                    return false;
                }
            });
        }
        else
            return false;
    }, "", "Aceptar", "Cancelar", "Cancelar Recibo de Ingresos", "IdModal");
}

var imprimirRecibo = function () {
    /*customModal("/Tesoreria/Ingresos/ImprimirRecibo", { FechaRecaudacion: $("#Fecha").val() }, "GET", "", function () {
        if ($("#F_Recaudacion").val() != "") {*/
    ajaxJson("/Tesoreria/Ingresos/ImprimirRecibo", { Folio: $("#Folio").val(), FechaRecaudacion: $("#Fecha").val() }, "POST", true, function (response) {
                if (response.Exito != false) {
                    llenarMaestro(response.Registro);
                    recargarMenuLateral(response.Registro.Botonera);
                    $(".IdModal").modal("hide");
                    window.open("/Tesoreria/Ingresos/ReporteReciboIngresos?Folio=" + $("#Folio").val(), '_blank');
                }
                else {
                    ErrorCustom(response.Mensaje, "");
                    return false;
                }
            });
        /*}
        else
            return false;
    }, "", "Aceptar", "Cancelar", "Imprimr Recibo de Ingresos", "IdModal");*/
}

function recaudar()
{
    customModal("/Tesoreria/Ingresos/ImprimirRecibo", { FechaRecaudacion: $("#Fecha").val() }, "GET", "", function () {
        if ($("#F_Recaudacion").val() != "") {
            ajaxJson("/Tesoreria/Ingresos/RecaudarRecibo", { Folio: $("#Folio").val(), FechaRecaudacion: $("#Fecha").val() }, "POST", true, function (response) {
                if (response.Exito != false) {
                    llenarMaestro(response.Registro);
                    recargarMenuLateral(response.Registro.Botonera);
                    $(".IdModal").modal("hide");
                    //window.open("/Tesoreria/Ingresos/ReporteReciboIngresos?Folio=" + $("#Folio").val(), '_blank');
                }
                else {
                    ErrorCustom(response.Mensaje, "");
                    return false;
                }
            });
            }
            else
                return false;
        }, "", "Aceptar", "Cancelar", "Recaudar Recibo de Ingresos", "IdModal");

}