/*
*Sección -> 1 : Presupuesto de Egresos, 2 -> Presupuesto de Ingresos
*Si el tipo es 1 se busca en los catálogos, si es dos busca en el maestro 
*
*/

$(document).ready(function () {
    $("input[type=text]").keydown(function (e) {
        console.log("keypress onfocus:" + e.keyCode);
        if (e.keyCode == 27) {
            $(this).val("");
        }
    });
    $("input[type=text]").blur(function (e) {
        if ($(this).attr("class").indexOf("js_Captura") > -1) {
            console.log("si es js_captura");
            nextDescripcion = $(this).closest('form').find(':input');
            console.log(nextDescripcion.eq(nextDescripcion.index(this) + 1));
            if (nextDescripcion.eq(nextDescripcion.index(this) + 1).val() == "") {
                console.log("esta vacio");
                $(this).val("");
            }
        }

    });
});

function FocusOut(Campo, Siguiente, Longitud, Atributos, Tipo, Seccion) {
    $("#" + Campo).on("keyup", function () {
        var texto = $(this).val().trim();
        if (texto.length == parseInt(Longitud)) {
            if ($("#" + Siguiente).length > 0)
                $("#" + Siguiente).focus();
            else
                $("#" + Campo).focusout();
        }
        else {
            var textbox = "#desc_" + Campo;
            $(textbox).val("");
        }

    });

    $("#" + Campo).on("focusout", function () {
        var texto = $(this).val().trim();
        var arrAtributos = "";
        if (texto.length == Longitud) {
            if (typeof Atributos != "undefined") {
                var lista = Atributos.split(",");
                for (i = 0; i < lista.length; i++) {
                    arrAtributos += $("#" + lista[i]).val() + ",";
                }
                arrAtributos = arrAtributos.substr(0, arrAtributos.length - 1);
            }
            ajaxJson(Seccion == 1 ? urlGetDescripcion : urlGetDescripcionIngresos, { Id: $("#" + Campo).val(), objeto: $("#" + Campo).attr("objeto"), parametros: arrAtributos, Tipo: Tipo }, "POST", true, function (result) {
                var textbox = "#desc_" + Campo;
                if (result == null) {
                    $(textbox).val("");
                    $("#" + Campo).val("");
                    $("#" + Campo).focus();
                }
                else if (result.Descripcion == "") {
                    $(textbox).val("");
                    $("#" + Campo).val("");
                    $("#" + Campo).focus();
                }
                else {
                    $(textbox).val(result.Descripcion);
                }
            });
        }
    });
}
var ReiniciarFormulario = function () {
    $("#frmPrespuesto")[0].reset();
    $(".js_Captura").attr("readonly", true);
}
var change = function (Campo, Tipo) {
    console.log("Entrar a change.Campo:" + Campo + ",Tipo:" + Tipo);
    $("body").on("change", "#" + Campo, function () {
        if ($(this).val().split('-').length == 2) {
            console.log("Valores:" + $(this).val().split('-')[0] + "," + $(this).val().split('-')[1]);

            if (Tipo == 2)
                $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            else
                $(this).parent().parent().parent().find(".js_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if (Tipo == 2) {
                //$(this).next(".textBoxModal").focus();
                $(this).parent().parent().parent().next().children().next().find(".textBoxModal").focus(); //$(".textDescripcion");
                //console.log($(this).next("input[type=text]").attr("id"));
                //input.focus();
            }
            else {
                var input = $(".js_Captura");

                input.eq(input.index(this) + 1).attr("readonly", false);
                input = input.eq(input.index(this) + 1).focus();
            }

            setTimeout(function () {
                $('.typeahead').each(function () {
                    console.log("hide drop");
                    $(this).css('display', 'none');
                })
            }, 1000);




        }
    });
}
var focoJsCaptura = function (Campo) {
    $("#" + Campo).on("focusout", function () {
        if (typeof $(this).data("datasource") == "undefined" || $(this).data("datasource").length == 0) {
            $(this).parent().parent().parent().find(".js_Descripcion").val("");
            $(this).val("").focus();
            setTimeout(function () { $("#" + Campo).focus(); }, 100);
        }
        else if ($(this).val().split('-').length > 0 && $.inArray($(this).val(), $(this).data("datasource")) == -1) {
            $(this).parent().parent().parent().find(".js_Descripcion").val("");
            $(this).val("").focus();
            setTimeout(function () { $("#" + Campo).focus(); }, 100);
        }
        if ($(this).val().length == 0)
            $(this).val("");
    });

}
var EventFocus = function (Campo, Tipo) {
    console.log("focus mayusculas");
    $.each($(".js_Captura"), function (key, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
    });
    $("#" + Campo).off("focusout");
    $("#" + Campo).off("change");
    if (Campo == "Id_ActividadMIR") {
        $('#' + Campo).typeahead({
            source: function (query, process) {
                $.ajax({
                    url: urlGetDescripcion,
                    type: "POST",
                    data: { id: $("#Id_Proceso").val(), objeto: 8, tipo: Tipo },
                    dataType: "JSON",
                    async: true,
                    success: function (results) {
                        $("#" + Campo).data("datasource", results.Ids);
                        change(Campo, Tipo);
                        if (Tipo != 2)
                            focoJsCaptura(Campo);
                        return process(results.Data);
                    }
                });
            }
        });
    }
    else if (Campo == "Id_Accion") {
        $('#' + Campo).typeahead({
            source: function (query, process) {
                $.ajax({
                    url: urlGetDescripcion,
                    type: "POST",
                    data: { id: $("#Id_Accion").val(), objeto: 9, parametros: $("#Id_Proceso").val() + "," + $("#Id_ActividadMIR").val(), tipo: Tipo },
                    dataType: "JSON",
                    async: true,
                    success: function (results) {
                        $("#" + Campo).data("datasource", results.Ids);
                        change(Campo, Tipo);
                        if (Tipo != 2)
                            focoJsCaptura(Campo);
                        return process(results.Data);
                    }
                });
            }
        });
    }
    else {
        //console.log("Entra");
        $('#' + Campo).typeahead({
            source: function (query, process) {
                $.ajax({
                    url: urlGetDescripcion,
                    type: "POST",
                    data: { id: $("#" + Campo).val(), objeto: $("#" + Campo).attr("objeto"), tipo: Tipo },
                    dataType: "JSON",
                    async: true,
                    success: function (results) {
                        $("#" + Campo).data("datasource", results.Ids);
                        change(Campo, Tipo);
                        if (Tipo != 2)
                            focoJsCaptura(Campo);
                        return process(results.Data);
                    }
                });
            }, autoSelect: true,
            minLength: 0, items: 15
        });
    }
}

var HabilitarBuscar = function (seccion) {
    $("body").on("click", ".js_Buscar", function () {
        if (!$(this).parent().hasClass("date")) {
            if (!$(this).isSiblingDisabled()) {
                var elemento = $(this).parent().find("input");
                var label = $(this).parent().parent().parent().children()[0];
                label = $(label).children().text();
                var hijo = $(this).parent().parent().parent().children()[1];
                hijo = $(hijo).children()[0];
                hijo = $(hijo).children()[0];
                var IdProyectoProceso = "";
                var IdActividadMIR = "";
                if (label == "Actividad MIR")
                    IdProyectoProceso = $("#Id_Proceso").val();
                if (label == "Acción u Obra") {
                    IdActividadMIR = $("#Id_ActividadMIR").val();
                    IdProyectoProceso = $("#Id_Proceso").val();
                }
                $("#TipoBusqueda").val(0);
                customModal(seccion == 1 ? urlBusquedaCatPres : urlBusquedaCatPresIng, { tipo: $(hijo).attr("objeto"), label: label, id: $(hijo).attr("id"), ProyectoProceso: IdProyectoProceso, ActividadMIR: IdActividadMIR }, "GET", "lg", function () {
                    ajaxLoad(seccion == 1 ? urlBusquedaPres : urlBusquedaPresIng, { objeto: $("#tipo_busqueda").val(), clave: $("#text_clave").val().trim(), descripcion: $("#text_descripcion").val().trim(), ProyectoProceso: $("#ProyectoProceso").val().trim(), ActividadMIR: $("#ActividadMIR").val().trim(), tipoBusqueda: $("input[name=tipoBusqueda]:checked").val() }, "div_tabla", "GET",
                            function () {
                                ConstruirTabla("tabla_resultados");
                            });
                }, "", "Buscar", "Cancelar", "Búsqueda de " + label, "MyModal1");
            }
        }
    });
}
var LoadPresupuestos = function (year, year1, tipo, seccion) {//Si el tipo es 1 se busca en los catálogos, si es dos busca en el maestro. seccion: 1->Egresos, 2->Ingresos
    var anio = year;
    var anio1 = year1;
    anio = anio - 2;
    anio1 = anio1 - 2;
    $("#AnioFin").append("<option value='" + anio + "'>" + anio1 + "</option>");
    for (i = 0; i < 4; i++) {
        anio1 += 1;
        anio += 1;
        $("#AnioFin").append("<option value='" + anio + "'>" + anio1 + "</option>");
    }
    $("#AnioFin").val(year);
    $("body").on("click", "#buscar", function () {
        ajaxLoad("/Tesoreria/Presupuestos/Buscar", { objeto: $("#tipo_busqueda").val(), clave: $("#text_clave").val().trim(), descripcion: $("#text_descripcion").val().trim() }, "div_tabla", "GET",
            function () {
                ConstruirTabla("tabla_resultados");
            });
    });
    $("body").on("click", ".MyModal2 .js_seleccionar", function () {
        seleccionadoModal = 1;
        var id = $(this).data("id");
        var descripcion = $(this).data("descripcion");
        var selector = $("#label").val();
        $("#" + selector).val(id);
        if (esModalBusqueda == 1) {
            $("#" + selector).parent().parent().parent().find(".textDescripcion").val(descripcion);
        }
        else
            $("#" + selector).parent().parent().parent().find(".js_Descripcion").val(descripcion);
        $(".MyModal2").modal("hide");
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
        totalModal--;
        console.log(totalModal);
        if (totalModal > 0) {
            setTimeout(function () { $("body").addClass("modal-open") }, 500);
        }
    });

    $("body").on("click", ".MyModal1 .js_seleccionar", function () {
        seleccionadoModal = 1;
        var id = $(this).data("id");
        var descripcion = $(this).data("descripcion");
        var selector = $("#label").val();
        $("#" + selector).val(id);
        if (esModalBusqueda == 1) {
            $("#" + selector).parent().parent().parent().find(".textDescripcion").val(descripcion);
        }
        else
            $("#" + selector).parent().parent().parent().find(".js_Descripcion").val(descripcion);
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

    $("#Id_ObjetoG").on("keyup", function () {
        var texto = $(this).val().trim();
        var num = texto[0];
        switch (num) {
            case "1":
            case "2":
            case "3":
            case "4":
            case "8":
                $("#Id_TipoG").val(1);
                $("#Id_TipoG").focusout();
                ajaxJson(rutaDescripcionTipoG, { Id_TipoG: $("#Id_TipoG").val() }, "POST", true, function (Datos) {
                    $("#Ca_TipoGastos_Descripcion").val(Datos.Descripcion);
                });
                break;
            case "5":
            case "6":
            case "7":
                $("#Id_TipoG").val(2);
                $("#Id_TipoG").focusout();
                ajaxJson(rutaDescripcionTipoG, { Id_TipoG: $("#Id_TipoG").val() }, "POST", true, function (Datos) {
                    $("#Ca_TipoGastos_Descripcion").val(Datos.Descripcion);
                });
                break;
            case "9":
                $("#Id_TipoG").val(3);
                $("#Id_TipoG").focusout();
                ajaxJson(rutaDescripcionTipoG, { Id_TipoG: $("#Id_TipoG").val() }, "POST", true, function (Datos) {
                    $("#Ca_TipoGastos_Descripcion").val(Datos.Descripcion);
                });
                break;
        }
        $("#Id_TipoG").attr("readonly", true);
    });
    $("#Id_ObjetoGModal").on("keyup", function () {
        var texto = $(this).val().trim();
        var num = texto[0];
        switch (num) {
            case "1":
            case "2":
            case "3":
            case "4":
            case "8":
                $("#Id_TipoGModal").val(1);
                $("#Id_TipoGModal").focusout();
                break;
            case "5":
            case "6":
            case "7":
                $("#Id_TipoGModal").val(2);
                $("#Id_TipoGModal").focusout();
                break;
            case "9":
                $("#Id_TipoGModal").val(3);
                $("#Id_TipoGModal").focusout();
        }
    });
}

/**Event Focus para el modal en buscar Presupuestos de Egresos*/
focoJsCaptura = function () {
    $(".textBoxModal").on("focusout", function () {
        var elemento = $(this);
        if ($(this).val().length > 0 && $.inArray($(this).val(), $(this).data("dataSource")) == -1) {
            $(this).parent().parent().parent().find(".textDescripcion").val("");
            $(this).val("").focus();
            $(this).parent().parent().parent().nextAll().find(".textBoxModal").attr("disabled", "disabled").val("").typeahead('destroy');
            $(this).parent().parent().parent().nextAll().find(".textDescripcion").val("");
            setTimeout(function () { elemento.focus(); }, 100);
        }
    });

}
var eventFocus = function () {
    console.log("event focus minusculas");
    $.each($(".textBoxModal"), function (key, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
    });
    $(".textBoxModal").attr("disabled", true);
    $(".textBoxModal").off("focusout");
    ajaxJson("/Tesoreria/FocusOut/Areas", {}, "POST", true, function (response) {
        changeCOG();
        focoJsCaptura();
        $("#Id_AreaModal").typeahead({ source: response.Data });
        $("#Id_AreaModal").data("dataSource", response.Ids);
        if (response.Data.length == 1) {
            $("#Id_AreaModal").val(response.Data[0]);
            $("#Id_AreaModal").trigger('change');
        }
        $("#Id_AreaModal").removeAttr("disabled");
    });

}

var changeCOG = function () {
    console.log("changeOC FocusOut.js");
    $("body").on("change", "#Id_AreaModal", function () {

        if ($(this).val().split('-').length == 2 || seleccionadoModal == 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 6) {
                $("#Id_FuncionModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();

                ajaxJson("/Tesoreria/FocusOut/Funciones", { IdArea: $("#Id_AreaModal").val() }, "POST", true, function (response) {
                    $("#Id_FuncionModal").typeahead({ source: response.Data });
                    $("#Id_FuncionModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_FuncionModal").val(response.Data[0]);
                        $("#Id_FuncionModal").trigger('change');
                    }
                });
            }
        }
        seleccionadoModal == 0;
    });


    $("body").on("change", "#Id_FuncionModal", function () {
        if ($(this).val().split('-').length != 1 || seleccionadoModal == 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 3) {
                $("#Id_ActividadModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/Actividad", { IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val() }, "POST", true, function (response) {
                    $("#Id_ActividadModal").typeahead({ source: response.Data });
                    $("#Id_ActividadModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_ActividadModal").val(response.Data[0]);
                        $("#Id_ActividadModal").trigger('change');
                    }
                });
            }
        }
        seleccionadoModal = 0;
    });
    $("body").on("change", "#Id_ActividadModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 2) {
                $("#Id_ClasificacionPModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/ClasificacionP", { IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val() }, "POST", true, function (response) {
                    $("#Id_ClasificacionPModal").typeahead({ source: response.Data });
                    $("#Id_ClasificacionPModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_ClasificacionPModal").val(response.Data[0]);
                        $("#Id_ClasificacionPModal").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_ClasificacionPModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 1) {
                $("#Id_ProgramaModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/Programa", { IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_ProgramaModal").typeahead({ source: response.Data });
                    $("#Id_ProgramaModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_ProgramaModal").val(response.Data[0]);
                        $("#Id_ProgramaModal").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_ProgramaModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 2) {
                $("#Id_ProcesoModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/Proceso", { Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_ProcesoModal").typeahead({ source: response.Data });
                    $("#Id_ProcesoModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_ProcesoModal").val(response.Data[0]);
                        $("#Id_ProcesoModal").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_ProcesoModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 4) {
                $("#Id_TipoMetaModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/TipoMeta", { Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_TipoMetaModal").typeahead({ source: response.Data });
                    $("#Id_TipoMetaModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_TipoMetaModal").val(response.Data[0]);
                        $("#Id_TipoMetaModal").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_TipoMetaModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 1) {
                $("#Id_ActividadMIRModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/ActividadMIR", { Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_ActividadMIRModal").typeahead({ source: response.Data });
                    $("#Id_ActividadMIRModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_ActividadMIRModal").val(response.Data[0]);
                        $("#Id_ActividadMIRModal").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_ActividadMIRModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 2) {
                $("#Id_AccionModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/Accion", { Id_ActividarMIR: $("#Id_ActividadMIRModal").val(), Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_AccionModal").typeahead({ source: response.Data });
                    $("#Id_AccionModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_AccionModal").val(response.Data[0]);
                        $("#Id_AccionModal").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_AccionModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 3) {
                $("#Id_AlcanceModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/Alcance", { Id_Accion: $("#Id_AccionModal").val(), Id_ActividarMIR: $("#Id_ActividadMIRModal").val(), Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_AlcanceModal").typeahead({ source: response.Data });
                    $("#Id_AlcanceModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_AlcanceModal").val(response.Data[0]);
                        $("#Id_AlcanceModal").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_AlcanceModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 1) {
                $("#Id_TipoGModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/TipoG", { Id_Alcance: $("#Id_AlcanceModal").val(), Id_Accion: $("#Id_AccionModal").val(), Id_ActividarMIR: $("#Id_ActividadMIRModal").val(), Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_TipoGModal").typeahead({ source: response.Data });
                    $("#Id_TipoGModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_TipoGModal").val(response.Data[0]);
                        $("#Id_TipoGModal").trigger('change');
                    }
                });
            }
        }
    });

    $("body").on("change", "#Id_TipoGModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 1) {
                $("#Id_FuenteModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/Fuente", { Id_TipoG: $("#Id_TipoGModal").val(), Id_Alcance: $("#Id_AlcanceModal").val(), Id_Accion: $("#Id_AccionModal").val(), Id_ActividarMIR: $("#Id_ActividadMIRModal").val(), Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_FuenteModal").typeahead({ source: response.Data });
                    $("#Id_FuenteModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_FuenteModal").val(response.Data[0]);
                        $("#Id_FuenteModal").trigger('change');
                    }
                });
            }
        }
    });

    $("body").on("change", "#Id_FuenteModal", function () {
        if ($(this).val().split('-').length != 1) {
            $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 4) {
                $("#Id_ObjetoGModal").removeAttr('disabled');
                $("#AnioFinModal").removeAttr('disabled');
                //url, data, metodo, asincrono, div, seleccion, callback
                ajaxSelect("/Tesoreria/Listas/Lista_AnioFinanciamiento", {}, "POST", true, "AnioFinModal", "", callBackLlenarSelect);
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/AnioFin", { Id_Fuente: $("#Id_FuenteModal").val(), Id_TipoG: $("#Id_TipoGModal").val(), Id_Alcance: $("#Id_AlcanceModal").val(), Id_Accion: $("#Id_AccionModal").val(), Id_ActividarMIR: $("#Id_ActividadMIRModal").val(), Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#AnioFinModal").typeahead({ source: response.Data });
                    $("#AnioFinModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#AnioFinModal").val(response.Data[0]);
                        $("#AnioFinModal").trigger('change');
                    }
                });
                //ajaxJson("/Tesoreria/FocusOut/ObjetoG", { Id_Fuente: $("#Id_FuenteModal").val(), Id_TipoG: $("#Id_TipoGModal").val(), Id_Alcance: $("#Id_AlcanceModal").val(), Id_Accion: $("#Id_AccionModal").val(), Id_ActividarMIR: $("#Id_ActividadMIRModal").val(), Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                //    $("#Id_ObjetoGModal").typeahead({ source: response.Data });
                //    $("#Id_ObjetoGModal").data("dataSource", response.Ids);
                //    if (response.Data.length == 1) {
                //        $("#Id_ObjetoGModal").val(response.Data[0]);
                //        $("#Id_ObjetoGModal").trigger('change');
                //    }
                //});
            }
        }
    });

    $('body').on('change', '#AnioFinModal', function () {
        if ($(this).val().split('-').length != 1) {
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 2) {
                $("#Id_ObjetoGModal").removeAttr('disabled');
                var input = $('.textBoxModal');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/ObjetoG", { IdAnio: $("#AnioFinModal").val(), Id_Fuente: $("#Id_FuenteModal").val(), Id_TipoG: $("#Id_TipoGModal").val(), Id_Alcance: $("#Id_AlcanceModal").val(), Id_Accion: $("#Id_AccionModal").val(), Id_ActividarMIR: $("#Id_ActividadMIRModal").val(), Id_TipoMeta: $("#Id_TipoMetaModal").val(), Id_Proceso: $("#Id_ProcesoModal").val(), Id_Programa: $("#Id_ProgramaModal").val(), IdArea: $("#Id_AreaModal").val(), Id_Funcion: $("#Id_FuncionModal").val(), Id_Actividad: $("#Id_ActividadModal").val(), Id_ClasificacionP: $("#Id_ClasificacionPModal").val() }, "POST", true, function (response) {
                    $("#Id_ObjetoGModal").typeahead({ source: response.Data });
                    $("#Id_ObjetoGModal").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_ObjetoGModal").val(response.Data[0]);
                        $("#Id_ObjetoGModal").trigger('change');
                    }
                });
            }
        }
    });

    $("body").on("change", "#Id_ObjetoGModal", function () {
        if ($(this).val().split('-').length != 1) {
            if ($(this).val().split('-')[0]) {
                $(this).parent().parent().parent().find(".textDescripcion").val($(this).val().split('-')[1]);
                $(this).val($(this).val().split('-')[0]);
                //ajaxJson("/Tesoreria/FocusOut/CuentasPorOG", { Id_ObjetoG: $("#Id_ObjetoGModal").val() }, "POST", true, function (response) {
                //    $("#Id_CuentaModal").removeAttr('disabled');
                //    $("#Id_CuentaModal").removeAttr('readonly');
                //    $("#Id_CuentaModal").typeahead({ source: response.Data });
                //    $("#Id_CuentaModal").data("dataSource", response.Ids);
                //    if (response.Data.length == 1) {
                //        $("#Id_CuentaModal").val(response.Data[0]);
                //        $("#Id_CuentaModal").trigger('change');
                //    }
                //    else
                //        $("#Id_Cuenta").focus();
                //});
            }
        }

    });

    //$("body").on("change", "#Id_Cuenta", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_Cuentas_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 20) {
    //            $("#Id_Cuenta").removeAttr('disabled').focus();
    //            $("#Id_Movimiento").removeAttr('disabled');
    //            $("#Importe").removeAttr('disabled');
    //            setTimeout(function () { $("#Importe").focus(); }, 100);
    //        }
    //    }
    //});
}