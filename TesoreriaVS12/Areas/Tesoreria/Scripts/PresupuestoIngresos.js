var seleccionado = 0;
var DeshabilitarElementos = function () {
    $(".js_Descripcion").attr("disabled", true);
    $(".js_Captura").attr("disabled", true);
    $("#AnioFin").attr("disabled", true);
}
var BotoneraGuardar = function () {
    recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
}
var BotoneraNuevo = function () {
    recargarMenuLateral(["bNuevo", "bBuscar", "bSalir"]);
}
var HabilitarElementos = function () {
    $(".textBox").attr("disabled", false);
    $(".js_Captura").attr("readonly", false);
    $("#AnioFin").attr("disabled", false);
    $(".js_radio").attr("disabled", false);
    $(".importe").attr("readonly", false);
    $("#TotalC").attr("readonly", "readonly");
    $("#porcentaje").attr("readonly", false);
    $("#Id_CentroRecaudador").trigger("focus");
}
var InicializarFormulario = function () {
    $("#frmPresupuesto")[0].reset();
    $("#TotalC").val(0);
    $("#Total").val(0);
}
var HabilitarDivsImporte = function () {
    seleccionado = $(this).val();
    if (seleccionado == 1) {
        $("#div_porcentaje").hide();
        $("#div_mesesIguales").hide();
        $(".js_mes").attr("readonly", false);
        $(".js_mes").focusout();
        $("#Estimado01").val("0");
        $("#Estimado01").focus();
    } else if (seleccionado == 2) {
        $("#div_porcentaje").hide();
        $("#div_mesesIguales").show();
        $(".js_mes").attr("readonly", true);
        $("#importeMeses").focus();
    } else {
        $("#div_porcentaje").show();
        $("#div_mesesIguales").hide();
        $("#importe").focus();
        $(".js_mes").attr("readonly", true);
    }
}
var Cancelar = function () {
    DeshabilitarElementos();
    InicializarFormulario();
    //HabilitarElementos();
    BotoneraNuevo();
}
var Buscar = function () {

}
var GuardarP = function () {
    $.each($(".js_mes, #TotalC"), function (i, value) {
        $(this).val($(this).autoNumeric("get"));
    });
    if ($("#frmPresupuesto").valid()) {
        ajaxJson("/Tesoreria/PresupuestosIngresos/GuardarPresupuesto", $("#frmPresupuesto").serialize(), "POST", true, function (response) {
            if (response.Exito == false) {
                ErrorCustom(response.Mensaje);
            } else {
                ExitoCustom(response.Mensaje, function () {
                    recargarMenuLateral(["bNuevo", "bBuscar", "bEliminar", "bSalir"]);
                    $("#FolioPoliza").val(response.Poliza);
                    $("#Id_MesPO_Estimado").val(response.Id_MesPO_Estimado);
                    $("#Id_FolioPO_Estimado").val(response.Id_FolioPO_Estimado);
                    $(".js_mes").focusout();
                    $(".js_radio").attr("disabled", true);
                    $(".js_Captura").attr("readonly", "readonly");
                    $("#AnioFin").attr("disabled", "disabled");
                    $(".importe").attr("readonly", "readonly");
                    $(".importe").focusout();
                    $("#porcentaje").attr("readonly", "readonly");
                    $("#idClavePresupuesto").val(response.Id_ClavePresupuesto);
                });
            }
        });
    }
}
var GuardarPresupuesto = function () {
    if ($("#TotalC").autoNumeric("get") == 0 || $("#TotalC").val() == "")
        $("#TotalC").val(0);
    if ($("#TotalC").autoNumeric("get") == 0) {
        ConfirmCustom("<div class='text-justify'><b>Esta Clave Presupuestaria se registrará en el Presupuesto Estimado con $0.00 y no se podrá editar posteriormente para ingresarle importes a los meses. " +
            "Para actualizar algún importe en el futuro, tendrá que hacerlo por medio de Ampliaciones y Reducciones, lo que afectará al Presupuesto Modificado.</b> <br/>¿Desea continuar?</div>", GuardarP, "", "Si", "No");
    } else {
        GuardarP();
    }

}
var sumar = function () {
    var total = parseFloat($("#Estimado01").autoNumeric("get"));
    var o = 0;
    for (i = 2; i < 13; i++) {
        if (i < 10) {
            o = i;
            total += parseFloat($("#Estimado0" + o).autoNumeric("get"));
        } else {
            total += parseFloat($("#Estimado" + i).autoNumeric("get"));
        }
    }
    $("#TotalC").autoNumeric("set", total);
    $("#Total").val(total);
};
var InicializarNumeric = function () {
    $("#TotalC").val(0);
    $("#importeMeses").autoNumeric({ aSign: "$" });
    $("#importe").autoNumeric({ aSign: "$" });
    $("#TotalC").autoNumeric({ aSign: "$" });
    $("#TotalC").attr("readonly", true);
    $(".js_mes").autoNumeric("init");
}
var Previsualizar = function () {
    if (seleccionado > 0) {
        if (seleccionado == 2) {
            var total = $("#importeMeses").autoNumeric("get");
            var importe = (total / 12).toFixed(2);
            var resto = (total - (importe * 11)).toFixed(2);
            for (i = 0; i < 12; i++) {
                if (i < 10)
                    $("#Estimado0" + i).val(importe);
                else
                    $("#Estimado" + i).val(importe);
            }
            $("#Estimado12").val(resto);
            $("#TotalC").autoNumeric("set", total);
            $("#Total").val(total);
            $(".js_mes").attr("readonly", "readonly");
            $("#TotalC").attr("readonly", "readonly");
        } else if (seleccionado == 3) {
            var importe = $("#importe").autoNumeric("get");
            var porcentaje = $("#porcentaje").val().trim();
            porcentaje = (importe * porcentaje) / 100;
            var calculo = parseInt(importe) + porcentaje;
            $("#Estimado01").val(importe);
            var o = 0;
            for (i = 2; i < 13; i++) {
                if (i < 10) {
                    o = i;
                    $("#Estimado0" + o).val(calculo);
                    var valor = parseInt($("#Estimado0" + o).val());
                    calculo = valor + porcentaje;
                } else {
                    $("#Estimado" + i).val(calculo);
                    var valor = parseInt($("#Estimado" + i).val());
                    calculo = valor + porcentaje;
                }
            }
            sumar();
            $(".js_mes").attr("readonly", "readonly");
            $("#TotalC").attr("readonly", "readonly");
        }
        $(".js_mes").focusout();
        $("#TotalC").focusout();
    }
}
var setEventoBuscar = function () {
    $("body").off("click", "#MyModal1 .js_btnOk");
    $("body").on("click", "#MyModal1 .js_btnOk", function () {
        ajaxLoad("/Tesoreria/PresupuestosIngresos/BuscarPresupuestos", $("#frmBuscarPresupuesto").serialize(), "div_resultados", "POST",
                function () {
                    ConstruirTabla("tabla_presupuestos");
                });
    });
}

$("body").on("click", "#js_mEliminar", function () {
    ConfirmCustom("El Presupuesto de Egreso será eliminado ¿Desea continuar?", modalEliminarPresupuesto, "", "Si", "No");
});
var change = function (Campo) {
    $("body").off("change", "#" + Campo);
    $("body").on("change", "#" + Campo, function () {
        if ($(this).val().split('-').length == 2) {
            $(this).val();
            if (esModalBusqueda == 1) {
                $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
                var input = $(".textBoxModal");
                input.eq(input.index(this) + 1).attr("readonly", false);
            }else {
                $(this).parent().parent().parent().find(".js_Descripcion").val($(this).val().split('-')[1]);
                var input = $(".js_Captura");
                input.eq(input.index(this) + 1).attr("readonly", false);
                input = input.eq(input.index(this) + 1).focus();
                if (Campo == "Id_Concepto")
                    $("#Ca_ConceptosIngresos_Descripcion").focus();
                else
                    input = input.eq(input.index(this) + 1).focus();
            }
            $(this).val($(this).val().split('-')[0]);
            $(this).focusout();
        }
        else if($.inArray($(this).val(),$(this).data("datasource")) ==-1)    {
            $(this).val("");
            $(this).parent().parent().parent().find(".js_Descripcion").val("");
        }
    });
}
var EventFocus = function (Campo, tipo) {
    $("#" + Campo).off("focusout");
    $('#' + Campo).typeahead({
        source: function (query, process) {
            $.ajax({
                url: urlGetDescripcionIngresos,
                type: "POST",
                data: { id: $("#" + Campo).val(), objeto: $("#" + Campo).attr("objeto"), parametros: "", tipo: tipo },
                dataType: "JSON",
                async: true,
                success: function (results) {
                    $("#" + Campo).data("datasource", results.Ids);
                    return process(results.Data);
                }
            });
        },
        autoSelect: true,
        minLength: 0, items: 15
    });
}

var HabilitarBusqueda = function (seccion) {
    $("body").off("click", ".js_Buscar");
    $("body").on("click", ".js_Buscar", function () {
        if (!$(this).parent().hasClass("date")) {
            if (!$(this).isSiblingDisabled()) {
                var elemento = $(this).parent().find("input");
                var label = $(this).parent().parent().parent().children()[0];
                label = $(label).children().text();
                var hijo = $(this).parent().parent().parent().children()[1];
                hijo = $(hijo).children()[0];
                hijo = $(hijo).children()[0];
                customModal(seccion == 1 ? urlBusquedaCatPres : urlBusquedaCatPresIng, { tipo: $(hijo).attr("objeto"), label: label, id: $(hijo).attr("id") }, "GET", "lg", function () {
                    ajaxLoad(seccion == 1 ? urlBusquedaPres : urlBusquedaPresIng, { objeto: $("#tipo_busqueda").val(), clave: $("#text_clave").val().trim(), descripcion: $("#text_descripcion").val().trim() }, "div_tabla", "GET",
                            function () {
                                ConstruirTabla("tabla_resultados");
                            });
                }, "", "Buscar", "Cancelar", "Búsqueda de " + label, "MyModal1");
            }
        }
    });
}

var InicializarIngresos = function (a1, a2) {
    $(".js_mes").attr("readonly", true);
    $(".js_Descripcion").attr("disabled", true);
    $("#AnioFin").attr("disabled", true);
    $(".textBox").attr("disabled", true);
    var textbox = $(".js_Captura");
    textbox.each(function (index, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
        EventFocus($(value).attr("id"));
        change($(value).attr("id"));
    });
    $("#Fecha_Estimado").datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true
    });
    DeshabilitarElementos();
    InicializarNumeric();
    $(".js_radio").on("click", HabilitarDivsImporte);
    $(".js_previsualizar").on("click", Previsualizar);
    $(".js_mes").on("blur, keyup", function () {
        sumar();
    });
    var anio = a1;
    var anio1 = a2;
    anio = anio - 2;
    anio1 = anio1 - 2;
    $(".js_mes").on("focusout", function () {
        if ($(this).val().trim().length == 0) {
            $(this).val("0");
            $(".js_mes").focusout();
        }
    });
    $("#AnioFin").append("<option value='" + anio + "'>" + anio1 + "</option>");
    for (i = 0; i < 4; i++) {
        anio1 = anio1 + 1;
        anio = anio + 1;
        $("#AnioFin").append("<option value='" + anio + "'>" + anio1 + "</option>");
    }
    $("#AnioFin").val(a1);
    $("body").on("click", ".js_Buscar", function () {
        seccion = 2;
        if (!$(this).parent().hasClass("date")) {
            if (!$(this).isSiblingDisabled()) {
                var elemento = $(this).parent().find("input");
                var label = $(this).parent().parent().parent().children()[0];
                label = $(label).children().text();
                var hijo = $(this).parent().parent().parent().children()[1];
                hijo = $(hijo).children()[0];
                hijo = $(hijo).children()[0];
                $("#TipoBusqueda").val(0);
                customModal(seccion == 1 ? urlBusquedaCatPres : urlBusquedaCatPresIng, { tipo: $(hijo).attr("objeto"), label: label, id: $(hijo).attr("id") }, "GET", "lg", function () {
                    ajaxLoad(seccion == 1 ? urlBusquedaPres : urlBusquedaPresIng, { objeto: $("#tipo_busqueda").val(), clave: $("#text_clave").val().trim(), descripcion: $("#text_descripcion").val().trim(), tipoBusqueda: $("input[name=tipoBusqueda]:checked").val() }, "div_tabla", "GET",
                            function () {
                                ConstruirTabla("tabla_resultados");
                            });
                }, "", "Buscar", "Cancelar", "Búsqueda de " + label, "MyModal1");
            }
        }
    });
    $("body").on("click", "#js_mNuevo", function () {
        ajaxJson("/Tesoreria/PresupuestosIngresos/NuevoPresupuesto", {}, "GET", true, function (datos) {
            if (datos.Exito) {
                InicializarFormulario();
                HabilitarElementos();
                BotoneraGuardar();
                $("#AnioFin").val(a1);
            }
        });
    });
    $("body").on("click", "#js_mGuardar", function () {
        if ($("#frmPresupuesto").valid()) {
            GuardarPresupuesto();
        }
    });
    $("body").on("click", "#js_mCancelar", Cancelar);
    $("body").on("click", "#js_mBuscar", function () {
        ajaxLoad("/Tesoreria/PresupuestosIngresos/ModalPresupuestoIng", {}, "modal-body1", "GET", function () {
            openModal({ Modal: "MyModal1", Size: "lg" });
            var textbox = $(".textBoxModal");
            esModalBusqueda = 1;
            $("#TipoBusqueda").val(1);
            textbox.each(function (index, value) {
                $("body").off("change", "#" + $(value).attr("id"));
                EventFocus($(value).attr("id"), 2);
                change($(value).attr("id"));
            });
            var anio = a1;
            var anio1 = a2;
            anio = anio - 2;
            anio1 = anio1 - 2;
            $("#AnioFinModal").append("<option value='" + anio + "'>" + anio1 + "</option>");
            for (i = 0; i < 4; i++) {
                anio1 = anio1 + 1;
                anio = anio + 1;
                $("#AnioFinModal").append("<option value='" + anio + "'>" + anio1 + "</option>");
            }
            $("#AnioFinModal").val(a1);
            HabilitarBusqueda(2);
            setEventoBuscar();
            esModalBusqueda = 1;
        });
    });
    $("body").on("click", ".MyModal1 .js_seleccionar", function () {
        seleccionadoModal = 1;
        var id = $(this).data("id");
        var descripcion = $(this).data("descripcion");
        var selector = $("#label").val();
        $("#" + selector).val(id + "-" + descripcion);
        $(".MyModal1").modal("hide");
        if (typeof $("#TipoBusqueda").val() == "undefined" || $("#TipoBusqueda").val() == 0) {
            var input = $(".js_Captura");
            input.eq($("#" + selector).index(".js_Captura") + 1).attr("readonly", false);
            input = input.eq($("#" + selector).index(".js_Captura") + 1).focus();
        }
        else {
            var input = $(".textBoxModal");
            input.eq($("#" + selector).index(".textBoxModal") + 1).attr("disabled", false);
            input = input.eq($("#" + selector).index(".textBoxModal") + 1).focus();
        }
        $("#" + selector).trigger('change');
    });
    $("body").on("click", "#MyModal1 .js_seleccionarPresupuesto", function () {
        $("#frmPresupuesto")[0].reset();
        var Id_ClavePresupuesto = $(this).data("clavepresupuesto");
        ajaxJson("/Tesoreria/PresupuestosIngresos/GetPresupuesto", { Id_ClavePresupuesto: Id_ClavePresupuesto }, "POST", true, function (data) {
            if (data.Exito == true) {
                recargarMenuLateral(["bNuevo", "bBuscar", "bEliminar", "bsalir"]);
                $(".textBox").attr("disabled", false);
                $(".textDescripcion").attr("disabled", false);
                llenarMaestro(data.Presupuesto);
                $("#Id_CentroRecaudador").val(data.Presupuesto.Id_CentroRecaudador);
                $("#Id_Fuente").val(data.Presupuesto.Id_Fuente);
                $("#AnioFin").val(data.Presupuesto.AnioFin);
                $(".js_radio").attr("disabled", true);
                $("#Id_Alcance").val(data.Presupuesto.Id_Alcance);
                $("#Id_Concepto").val(data.Presupuesto.Id_Concepto);
                $(".textBox").focusout();
                $("#TotalC").attr("readonly", "readonly");
                $("#Estimado01").val(data.Presupuesto.Estimado01);
                $("#Estimado02").val(data.Presupuesto.Estimado02);
                $("#Estimado03").val(data.Presupuesto.Estimado03);
                $("#Estimado04").val(data.Presupuesto.Estimado04);
                $("#Estimado05").val(data.Presupuesto.Estimado05);
                $("#Estimado06").val(data.Presupuesto.Estimado06);
                $("#Estimado07").val(data.Presupuesto.Estimado07);
                $("#Estimado08").val(data.Presupuesto.Estimado08);
                $("#Estimado09").val(data.Presupuesto.Estimado09);
                $("#Estimado10").val(data.Presupuesto.Estimado10);
                $("#Estimado11").val(data.Presupuesto.Estimado11);
                $("#Estimado12").val(data.Presupuesto.Estimado12);
                $("#idClavePresupuesto").val(data.Presupuesto.Id_ClavePresupuesto);
                $("#Id_Poliza").val(data.Presupuesto.FolioPoliza);
                $("#TotalC").val(data.Presupuesto.Total);
                $("#Total").val(data.Presupuesto.Total);
                closeModal();
                $(".textBox").attr("disabled", true);
                $(".textDescripcion").attr("disabled", true);
                $(".js_mes").focusout();
                $(".js_mes").attr("disabled", true);
                $("#TotalC").focusout();
            } else {
                ErrorCustom(data.Mensaje);
            }
        });
    });
}
var setEventos = function () {
    $("body").off("click", "#MyModal1 .js_btnOk");
    $("body").on("click", "#MyModal1 .js_btnOk", function () {
        var fecha = $("#fechaCancelacion").val().trim();
        ajaxJson("/Tesoreria/PresupuestosIngresos/CancelarPresupuesto", {
            Id_ClavePresupuesto: $("#idClavePresupuesto").val(), Fecha: fecha
        }, "POST", true, function (data) {
            if (data.Exito) {
                ExitoCustom(data.Mensaje, function () {
                    InicializarFormulario();
                    BotoneraNuevo();
                });
                closeModal();
            } else
                ErrorCustom(data.Mensaje);
        });
    });
}
var modalEliminarPresupuesto = function () {
    ajaxLoad("/Tesoreria/PresupuestosIngresos/ModalEliminar", {
    }, "modal-body1", "GET", function () {
        openModal({
            Modal: "MyModal1"
        });
        $('#fechaCancelacion').datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            todayHighlight: true
        });
        setEventos();
    })
}
