focoJsCaptura = function () {
    $(".js_Captura").on("focusout", function () {
        var elemento = $(this);
        if ($(this).val().length > 0 && $.inArray($(this).val(), $(this).data("dataSource")) == -1) {
            $(this).parent().parent().parent().find(".js_Descripcion").val("");
            $(this).val("").focus();
            $(this).parent().parent().parent().nextAll().find(".js_Captura").attr("disabled", "disabled").val("").typeahead('destroy');
            $(this).parent().parent().parent().nextAll().find(".js_Descripcion").val("");
            $("#Id_Cuenta").empty();
            setTimeout(function () { elemento.focus(); }, 100);
        }
    });

}
var eventFocus = function () {

    $.each($(".js_Captura"), function (key, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
    });
    $(".js_Captura").off("focusout");
    ajaxJson(urlFocusOAreas, { Id_Fuente_Filtro: $("#Id_Fuente_Filtro").val() }, "POST", true, function (response) {
        changeCOG();
        focoJsCaptura();
        $("#Id_Area").typeahead({ source: response.Data });
        $("#Id_Area").data("dataSource", response.Ids);
        if (response.Data.length == 1) {
            $("#Id_Area").val(response.Data[0]);
            $("#Id_Area").trigger('change');
        }
        $("#Id_Area").removeAttr("disabled");
    });

}

var eventFocusCRI = function () {
    $("body").off("change", "#Id_Fuente");
    $.each($(".js_Captura"), function (key, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
    });

    $(".js_Captura").off("focusout");

    ajaxJson("/Tesoreria/FocusOut/CentroRecaudador", {}, "POST", true, function (response) {
        changeCRI();
        focoJsCaptura();
        $("#Id_CentroRecaudador").typeahead({ source: response.Data });
        $("#Id_CentroRecaudador").data("dataSource", response.Ids);
        if (response.Data.length == 1) {
            $("#Id_CentroRecaudador").val(response.Data[0]);
            $("#Id_CentroRecaudador").trigger('change');
        }
        $("#Id_CentroRecaudador").removeAttr("disabled");
    });
}

var eventFocusBalance = function () {
    $.each($(".js_Captura"), function (key, value) {
        $("body").off("change", "#" + ($(value).attr("id")));
    });

    var filtros = {};
    if (typeof buildFiltros != "undefined")
        filtros = buildFiltros();
    else
        filtros = { GeneroStr: "1,2,3,6,7,9", selectUltimoNivel: true, UltimoNivel: true };
    ajaxJson("/Tesoreria/FocusOut/SearchCuentasBalance", filtros, "POST", true, function (response) {
        focoJsCaptura();
        $("#Id_Cuenta").removeAttr("disabled");
        $("#Id_Cuenta").typeahead({ source: response.Data, items: 15 });
        $("#Id_Cuenta").data("dataSource", response.Ids);
        $("#Id_Cuenta").data("dataBancos", response.cuentasBancos);
        if (response.Data.length == 1) {
            $("#Id_Cuenta").val(response.Data[0]);
            $("#Id_Cuenta").trigger('change');
        }
    });

    $("body").on("change", "#Id_Cuenta", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_Cuentas_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 20) {
                $("#Importe").removeAttr('disabled').focus();
                $("#Id_Movimiento, #DescripcionMP").removeAttr('disabled');
                var cuenta = $(this).val();
                var banco = false;
                $.each($(this).data("dataBancos"), function (key, value) {
                    if (cuenta.substring(0, value.length) == value) {
                        banco = true;
                        return;
                    }
                });
                if (banco) {
                    $(".js_MovBancario").removeClass("hide");
                    $("#IdTipoMovB").removeAttr('disabled');
                    ajaxSelect("/Tesoreria/Polizas/ListTipoMovBancario", { TipoMov: $("#Id_Movimiento").val() }, "POST", true, "IdTipoMovB", "", callBackLlenarSelect);
                }
                else {
                    $(".js_MovBancario").addClass("hide");
                    $("#IdTipoMovB").val("");
                }
            }
        }
    });

    $("body").on("change", "#Id_Movimiento", function () {
        $("#IdTipoMovB").removeAttr('disabled');
        ajaxSelect("/Tesoreria/Polizas/ListTipoMovBancario", { TipoMov: $("#Id_Movimiento").val() }, "POST", true, "IdTipoMovB", "", callBackLlenarSelect);
    });
}
var changeCOG = function () {
    $.each($(".js_Captura"), function (key, elemento) {

        $("body").on("change", "#" + $(elemento).attr("id"), function () {
            $("#Id_Cuenta").empty();
            if ($(elemento).val().split('-').length != 1) {
                $(elemento).parent().parent().siblings().find(".js_Descripcion").val($(elemento).val().split('-')[1])
                //$("#Ca_Areas_Descripcion").val($(this).val().split('-')[1]);
                $(elemento).val($(elemento).val().split('-')[0]);
                if ($(elemento).val().length == $(elemento).data("length")) {
                    //$("#Id_Funcion").val("").removeAttr('disabled');
                    var input = $('.js_Captura');
                    var nextElement = input.eq(input.index(elemento) + 1)
                    nextElement.val("").removeAttr("disabled").focus();
                    ajaxJson(urlFocusOGenSearCve, $(".js_frmClavePresupuestal").serialize() + "&Id_Actual=" + $(elemento).attr("id"), "POST", true, function (response) {
                        nextElement.typeahead('destroy');
                        nextElement.typeahead({ source: response.Data });
                        nextElement.data("dataSource", {});
                        nextElement.data("dataSource", response.Ids);
                        if ($(elemento).attr("id") == "Id_ObjetoG") {
                            if ($("#Id_FolioPoliza").length > 0)
                                $("#Importe, #Id_Movimiento,#DescripcionMP").val("").removeAttr("disabled");

                            $("#Importe").focus();
                            $("#Id_Cuenta").removeAttr("readonly");
                        }
                        if (response.Data.length == 1) {
                            nextElement.val(response.Data[0]);
                            nextElement.trigger('change');
                        }
                    });
                }
            }
        });
    });



    //$("body").on("change", "#Id_Funcion", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_Funciones_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 3) {
    //            $("#Id_Actividad").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/Actividad", { IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val() }, "POST", true, function (response) {
    //                $("#Id_Actividad").typeahead({ source: response.Data });
    //                $("#Id_Actividad").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_Actividad").val(response.Data[0]);
    //                    $("#Id_Actividad").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_Actividad", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_ActividadInst_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 2) {
    //            $("#Id_ClasificacionP").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/ClasificacionP", { IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val() }, "POST", true, function (response) {
    //                $("#Id_ClasificacionP").typeahead({ source: response.Data });
    //                $("#Id_ClasificacionP").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_ClasificacionP").val(response.Data[0]);
    //                    $("#Id_ClasificacionP").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_ClasificacionP", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_ClasProgramatica_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 1) {
    //            $("#Id_Programa").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/Programa", { IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_Programa").typeahead({ source: response.Data });
    //                $("#Id_Programa").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_Programa").val(response.Data[0]);
    //                    $("#Id_Programa").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_Programa", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_Programas_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 2) {
    //            $("#Id_Proceso").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/Proceso", { Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_Proceso").typeahead({ source: response.Data });
    //                $("#Id_Proceso").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_Proceso").val(response.Data[0]);
    //                    $("#Id_Proceso").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    ///*Revalidar ¿Ok?*/
    //$("body").on("change", "#Id_Proceso", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_Proyecto_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 4) {
    //            $("#Id_TipoMeta").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/TipoMeta", { Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_TipoMeta").typeahead({ source: response.Data });
    //                $("#Id_TipoMeta").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_TipoMeta").val(response.Data[0]);
    //                    $("#Id_TipoMeta").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_TipoMeta", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_TipoMeta_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 1) {
    //            $("#Id_ActividadMIR").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/ActividadMIR", { Id_TipoMeta: $("#Id_TipoMeta").val(), Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_ActividadMIR").typeahead({ source: response.Data });
    //                $("#Id_ActividadMIR").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_ActividadMIR").val(response.Data[0]);
    //                    $("#Id_ActividadMIR").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_ActividadMIR", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_Actividad_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 2) {
    //            $("#Id_Accion").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/Accion", { Id_ActividarMIR: $("#Id_ActividadMIR").val(), Id_TipoMeta: $("#Id_TipoMeta").val(), Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_Accion").typeahead({ source: response.Data });
    //                $("#Id_Accion").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_Accion").val(response.Data[0]);
    //                    $("#Id_Accion").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_Accion", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_Acciones_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 3) {
    //            $("#Id_Alcance").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/Alcance", { Id_Accion: $("#Id_Accion").val(), Id_ActividarMIR: $("#Id_ActividadMIR").val(), Id_TipoMeta: $("#Id_TipoMeta").val(), Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_Alcance").typeahead({ source: response.Data });
    //                $("#Id_Alcance").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_Alcance").val(response.Data[0]);
    //                    $("#Id_Alcance").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_Alcance", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_AlcanceGeo_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 1) {
    //            $("#Id_TipoG").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/TipoG", { Id_Alcance: $("#Id_Alcance").val(), Id_Accion: $("#Id_Accion").val(), Id_ActividarMIR: $("#Id_ActividadMIR").val(), Id_TipoMeta: $("#Id_TipoMeta").val(), Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_TipoG").typeahead({ source: response.Data });
    //                $("#Id_TipoG").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_TipoG").val(response.Data[0]);
    //                    $("#Id_TipoG").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});

    //$("body").on("change", "#Id_TipoG", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_TipoGastos_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 1) {
    //            $("#Id_Fuente").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/Fuente", { Id_TipoG: $("#Id_TipoG").val(), Id_Alcance: $("#Id_Alcance").val(), Id_Accion: $("#Id_Accion").val(), Id_ActividarMIR: $("#Id_ActividadMIR").val(), Id_TipoMeta: $("#Id_TipoMeta").val(), Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_Fuente").typeahead({ source: response.Data });
    //                $("#Id_Fuente").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_Fuente").val(response.Data[0]);
    //                    $("#Id_Fuente").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});

    //$("body").on("change", "#Id_Fuente", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $("#Ca_FuentesFin_Descripcion").val($(this).val().split('-')[1]);
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 4) {
    //            //$("#Id_ObjetoG").removeAttr('disabled');
    //            $("#AnioFin").val("").removeAttr('disabled');
    //            //url, data, metodo, asincrono, div, seleccion, callback
    //            //ajaxSelect("/Tesoreria/Listas/Lista_AnioFinanciamiento", {}, "POST", true, "AnioFin", "", callBackLlenarSelect);
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/AnioFin", { Id_Fuente: $("#Id_Fuente").val(), Id_TipoG: $("#Id_TipoG").val(), Id_Alcance: $("#Id_Alcance").val(), Id_Accion: $("#Id_Accion").val(), Id_ActividarMIR: $("#Id_ActividadMIR").val(), Id_TipoMeta: $("#Id_TipoMeta").val(), Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#AnioFin").typeahead({ source: response.Data });
    //                $("#AnioFin").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#AnioFin").val(response.Data[0]);
    //                    $("#AnioFin").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});

    //$('body').on('change', '#AnioFin', function () {
    //    if ($(this).val().split('-').length != 1) {
    //        $(this).val($(this).val().split('-')[0]);
    //        if ($(this).val().length == 2) {
    //            $("#Id_ObjetoG").val("").removeAttr('disabled');
    //            var input = $('.js_Captura');
    //            input.eq(input.index(this) + 1).focus();
    //            ajaxJson("/Tesoreria/FocusOut/ObjetoG", { IdAnio: $("#AnioFin").val(), Id_Fuente: $("#Id_Fuente").val(), Id_TipoG: $("#Id_TipoG").val(), Id_Alcance: $("#Id_Alcance").val(), Id_Accion: $("#Id_Accion").val(), Id_ActividarMIR: $("#Id_ActividadMIR").val(), Id_TipoMeta: $("#Id_TipoMeta").val(), Id_Proceso: $("#Id_Proceso").val(), Id_Programa: $("#Id_Programa").val(), IdArea: $("#Id_Area").val(), Id_Funcion: $("#Id_Funcion").val(), Id_Actividad: $("#Id_Actividad").val(), Id_ClasificacionP: $("#Id_ClasificacionP").val() }, "POST", true, function (response) {
    //                $("#Id_ObjetoG").typeahead({ source: response.Data });
    //                $("#Id_ObjetoG").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_ObjetoG").val(response.Data[0]);
    //                    $("#Id_ObjetoG").trigger('change');
    //                }
    //            });
    //        }
    //    }
    //});
    //$("body").on("change", "#Id_ObjetoG", function () {
    //    if ($(this).val().split('-').length != 1) {
    //        if ($(this).val().split('-')[0]) {
    //            $("#Ca_ObjetoGasto_Descripcion").val($(this).val().split('-')[1]);
    //            $(this).val($(this).val().split('-')[0]);
    //            ajaxJson("/Tesoreria/FocusOut/CuentasPorOG", { Id_ObjetoG: $("#Id_ObjetoG").val() }, "POST", true, function (response) {
    //                $("#Id_Cuenta").removeAttr('disabled');
    //                $("#Id_Cuenta").removeAttr('readonly');
    //                $("#Id_Cuenta").typeahead({ source: response.Data });
    //                $("#Id_Cuenta").data("dataSource", response.Ids);
    //                if (response.Data.length == 1) {
    //                    $("#Id_Cuenta").val(response.Data[0]);
    //                    $("#Id_Cuenta").trigger('change');
    //                    $("#Importe").focus();
    //                }
    //                else
    //                    $("#Id_Cuenta").focus();
    //            });
    //        }
    //    }

    //});

    $("body").on("change", "#Id_Cuenta", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_Cuentas_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 20) {
                $("#Id_Cuenta").removeAttr('disabled').focus();
                $("#Id_Movimiento,#Importe,#DescripcionMP").removeAttr('disabled');
                setTimeout(function () { $("#Importe").focus(); }, 100);
            }
        }
    });
}


var changeCRI = function () {

    $("body").on("change", "#Id_CentroRecaudador", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_CentroRecaudador_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 6) {
                $("#Id_Fuente").val("").removeAttr('disabled');
                var input = $('.js_Captura');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/FuenteCRI", { IdCentroR: $("#Id_CentroRecaudador").val() }, "POST", true, function (response) {
                    $("#Id_Fuente").typeahead({ source: response.Data });
                    $("#Id_Fuente").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_Fuente").val(response.Data[0]);
                        $("#Id_Fuente").trigger('change');
                    }
                });
            }
        }
    });

    $("body").on("change", "#Id_Fuente", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_FuentesFin_Ing_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 4) {
                $("#Id_Alcance").val("").removeAttr('disabled');
                $("#AnioFin").removeAttr('disabled');
                //ajaxSelect("/Tesoreria/Listas/Lista_AnioFinanciamiento", {}, "POST", true, "AnioFin", "", callBackLlenarSelect);
                var input = $('.js_Captura');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/AnioFinCRI", { IdCentroR: $("#Id_CentroRecaudador").val(), IdFuente: $("#Id_Fuente").val() }, "POST", true, function (response) {
                    $("#AnioFin").typeahead({ source: response.Data });
                    $("#AnioFin").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#AnioFin").val(response.Data[0]);
                        $("#AnioFin").trigger('change');
                    }
                });
                //ajaxJson("/Tesoreria/FocusOut/AlcanceCRI", { IdCentroR: $("#Id_CentroRecaudador").val(), IdFuente: $("#Id_Fuente").val() }, "POST", true, function (response) {
                //    $("#Id_Alcance").typeahead({ source: response.Data });
                //    $("#Id_Alcance").data("dataSource", response.Ids);
                //    if (response.Data.length == 1) {
                //        $("#Id_Alcance").val(response.Data[0]);
                //        $("#Id_Alcance").trigger('change');
                //    }
                //});
            }
        }
    });

    $('body').on('change', '#AnioFin', function () {
        if ($(this).val().split('-').length != 1) {
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 2) {
                $("#Id_ObjetoG").val("").removeAttr('disabled');
                var input = $('.js_Captura');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/AlcanceCRI", { IdAnio: $('#AnioFin').val(), IdCentroR: $("#Id_CentroRecaudador").val(), IdFuente: $("#Id_Fuente").val() }, "POST", true, function (response) {
                    $("#Id_Alcance").typeahead({ source: response.Data });
                    $("#Id_Alcance").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_Alcance").val(response.Data[0]);
                        $("#Id_Alcance").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_Alcance", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_AlcanceGeo_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 1) {
                $("#Id_Concepto").val("").removeAttr('disabled');
                var input = $('.js_Captura');
                input.eq(input.index(this) + 1).focus();
                ajaxJson("/Tesoreria/FocusOut/ConceptoCRI", { IdAlcance: $("#Id_Alcance").val(), IdCentroR: $("#Id_CentroRecaudador").val(), IdFuente: $("#Id_Fuente").val() }, "POST", true, function (response) {
                    $("#Id_Concepto").typeahead({ source: response.Data });
                    $("#Id_Concepto").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_Concepto").val(response.Data[0]);
                        $("#Id_Concepto").trigger('change');
                    }
                });
            }
        }
    });
    $("body").on("change", "#Id_Concepto", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_ConceptoIngresos_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 3) {
                $("#IdCur").val("").removeAttr('disabled');
                ajaxJson("/Tesoreria/FocusOut/CUR", { IdConcepto: $("#Id_Concepto").val() }, "POST", true, function (response) {
                    $("#IdCur").typeahead({ source: response.Data });
                    $("#IdCur").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#IdCur").val(response.Data[0]);
                        $("#IdCur").trigger('change');
                    }
                });
                ajaxJson("/Tesoreria/FocusOut/CuentasPorCRI", { IdConcepto: $("#Id_Concepto").val() }, "POST", true, function (response) {
                    $("#Id_Cuenta").removeAttr('disabled');
                    $("#Id_Cuenta").typeahead({ source: response.Data });
                    $("#Id_Cuenta").data("dataSource", response.Ids);
                    if (response.Data.length == 1) {
                        $("#Id_Cuenta").val(response.Data[0]);
                        $("#Id_Cuenta").trigger('change');
                        $("#Importe").focus();
                    }
                    else
                        $("#Id_Cuenta").focus();
                });
            }
        }
    });
    $("body").on("change", "#IdCur", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_Cur_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 3) {
                $("#Id_Cuenta").removeAttr('disabled').focus();
                $("#Importe").removeAttr('disabled');
            }
        }
    });
    $("body").on("change", "#Id_Cuenta", function () {
        if ($(this).val().split('-').length != 1) {
            $("#Ca_Cuentas_Descripcion").val($(this).val().split('-')[1]);
            $(this).val($(this).val().split('-')[0]);
            if ($(this).val().length == 20) {
                $("#Id_Cuenta").removeAttr('disabled').focus();
                $("#Importe").removeAttr('disabled').focus();
                $("#Id_Movimiento, #DescripcionMP").removeAttr('disabled');
            }
        }
    });
}

var cargarCuentasCRI = function (IdCur) {
    ajaxSelect("/Tesoreria/FocusOut/CuentasPorCRI", { IdCur: IdCur }, "POST", true, "Id_Cuenta", "", function (response) {
        $("#Id_Cuenta").empty();
        $("#Id_Cuenta").append("<option value=''>--Selecciona una cuenta--</option>");
        $.each(response, function (i, item) {
            var idCuentaFormato = item.Text.split('$')[1];
            var descripcion = item.Text.split('$')[0];
            if (item.Selected == true) {
                $("#Id_Cuenta").append("<option selected='true' value='" + item.Value + "' data-descripcion='" + descripcion + "'>" + idCuentaFormato + "</option>");
                $("#Ca_Cuentas_Descripcion").val(descripcion);
            }
            else
                $("#Id_Cuenta").append("<option value='" + item.Value + "' data-descripcion='" + descripcion + "'>" + idCuentaFormato + "</option>");
        });
    });
}

var cargarCuetnasObjetoGasto = function (Id_ObjetoG) {
    ajaxSelect("/Tesoreria/FocusOut/CuentasPorOG", { Id_ObjetoG: Id_ObjetoG }, "POST", true, "Id_Cuenta", "", function (response) {
        $("#Id_Cuenta").empty();
        $("#Id_Cuenta").append("<option value=''>--Selecciona una cuenta--</option>");
        $.each(response, function (i, item) {
            var idCuentaFormato = item.Text.split('$')[1];
            var descripcion = item.Text.split('$')[0];
            if (item.Selected == true) {
                $("#Id_Cuenta").append("<option selected='true' value='" + item.Value + "' data-descripcion='" + descripcion + "'>" + idCuentaFormato + "</option>");
                $("#Ca_Cuentas_Descripcion").val(descripcion);
            }
            else
                $("#Id_Cuenta").append("<option value='" + item.Value + "' data-descripcion='" + descripcion + "'>" + idCuentaFormato + "</option>");
        });
    });
}


