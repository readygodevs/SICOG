function asignarCheque() {
    if ($(".jqx-grid-content .jqx-checkbox-check-checked").length > 0) {
        if ($("#CuentaBancaria").val() !="" && $("#CuentaBancaria").val() > 0) {
            //Vista asigancion cheque
            if ($("#h_Vista").val() == 1) {
                ajaxJson("AsignarCheque", { IdCuentaBancaria: $("#CuentaBancaria").val() }, "POST", true, function (result) {
                    if (result.Exito) {
                        if (result.Foliador == 1)
                            ConfirmCustom("La asignación de cheques se realizará por un rango especifico. ¿Desea Continuar?", ValidarCuentaManual);
                        if (result.Foliador == 2)
                            ValidarCuentaAutomatica();
                    }
                    else
                        ErrorCustom(result.Mensaje, "");
                });
            }//Vista transferencia electronica
            else {
                if ($("#FechaPago").val() != "") {
                    var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                    var exito = true;
                    for (var i = 0; i < getselectedrowindexes.length;i++)
                    {
                        selectedRowData = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[i]);
                        if (getDate($("#FechaPago").val()) < selectedRowData.Fecha_Ejercido)
                        {
                            ErrorCustom("Alguna de las fechas de ejercido es mayor que la facha de pago, favor de verificarlo.","");
                            exito = false;
                        }
                    }
                    if (exito)
                    {
                        ajaxJson("ValidarMes", { FechaPago: $("#FechaPago").val() }, "POST", true, function (result) {
                            if (result.Exito) {
                                ValidarCuentaAutomatica();
                            }
                            else
                                ErrorCustom(result.Mensaje, "");
                        });
                    }
                    
                } else
                    ErrorCustom("Debe seleccionar una fecha de pago.", "");

            }

        }
        else
            ErrorCustom("Debe seleccionar una cuenta bancaria.");

    } else {
        ErrorCustom("Debe seleccionar al menos al menos un contrarecibo.");
    }

}
function Buscar()
{
    if ($("#CuentaBancaria").val() == "" || $("#CuentaBancaria").val() == null)
        ErrorCustom("Debe seleccionar una cuenta bancaria.")
    else
    {
        DestroyGrid();
        url = "/Tesoreria/Egresos/ListaCheques?FechaDesde=" + $("#FechaDesde").val() + "&FechaHasta=" + $("#FechaHasta").val() + "&IdCta=" + $("#CuentaBancaria").val();
        ConstruirGrid();
    }
    
}
function getDate(date) {
    var parts = date.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}
function ValidarCuentaAutomatica() {
    ajaxJson("ValidarSaldo", { IdCuentaBancaria: $("#CuentaBancaria").val(), totalImporte: total }, "GET", true, function (result) {
        if (result.Exito) {
            if (result.Importe) {
                //Asignacion
                if ($("#h_Vista").val() == 1)
                    ModalNextChequeAutomatico();
                else
                    ModalNextChequeAutomatico();
            }
            else {
                //Transferencia
                if ($("#h_Vista").val() == 1)
                    ConfirmCustom(result.Mensaje, ModalNextChequeAutomatico);
                else
                    ConfirmCustom(result.Mensaje, ModalNextChequeAutomatico);
            }
                
        } else {
            ErrorCustom(result.Mensaje, "");
        }
    });
}
function ModalNextChequeAutomatico() {
    var numero = $('#jqxgrid').jqxGrid('getselectedrowindexes');
    ajaxJson("ChequesDisponibles", { IdCuentaBancaria: $("#CuentaBancaria").val(), numero: numero.length }, "POST", true, function (response) {
        if (response.Exito)
        {
            if ($("#h_Vista").val() == 1)
                customModal("ModalNextChequeAutomatico", { IdCuentaBancaria: $("#CuentaBancaria").val() }, "GET", "sm", NextChequeAutomatico, "", "Aceptar", "Cancelar", "Inicio No cheque", "ModalNextChequeAutomatico");
            else
                NextChequeAutomatico();
        }
            
        else
            ErrorCustom(response.Mensaje);
    });
}

function NextChequeAutomatico() {
    jsonCheques = {};
    no_Cheque = 0;
    $(".ModalNextChequeAutomatico").modal("hide");
    var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
    if (getselectedrowindexes.length > 0) {
        var rows = getselectedrowindexes.length;
        var error = false;
        var index = 0;
        while (rows > 0 && !error) {
            selectedRowData = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[rows-1]);
            //Vista asigancion cheque
            if ($("#h_Vista").val() == 1) {
                ajaxJson("GuardaAsignacionChequeAutomatico", { Id_TipoCR: selectedRowData.Id_TipoCR, Id_FolioCR: selectedRowData.Id_FolioCR, IdCuentaBancaria: $("#CuentaBancaria").val() }, "POST", false, function (result) {
                    if (!result.Exito) {
                        error = true;
                        ErrorCustom(result.Mensaje, "");
                    } else {
                        no_Cheque = result.NoCheque;
                        selectedRowData.No_Cheque = no_Cheque;
                        jsonCheques[index] = selectedRowData;
                        DestroyGridSave();
                        ConstruirGridSave();
                        index++;
                    }
                    

                });
            }//Vista transferencia electronica
            else {
                ajaxJson("GuardaAsignacionChequeAutomaticoTE", { Id_TipoCR: selectedRowData.Id_TipoCR, Id_FolioCR: selectedRowData.Id_FolioCR, IdCuentaBancaria: $("#CuentaBancaria").val(), FechaPago: $("#FechaPago").val() }, "POST", false, function (result) {
                    if (!result.Exito) {
                        error = true;
                        ErrorCustom(result.Mensaje, "");
                    }
                    else{
                        no_Cheque = result.NoCheque;
                        selectedRowData.No_Cheque = no_Cheque;
                        jsonCheques[index] = selectedRowData;
                        DestroyGridSave();
                        ConstruirGridSave();
                        index++;
                    }
                    
                });
            }
            rows--;
        }
        //ExitoCustom("Ultimo cheque agregado: " + no_Cheque);
        DestroyGrid();
        reset();
        $("#js_divImporte").addClass("hide");
    }
}
function ValidarCuentaManual() {
    var numeroChecks = $(".jqx-grid-content .jqx-checkbox-check-checked").length;
    ajaxJson("ValidarCuentaManual", { IdCuentaBancaria: $("#CuentaBancaria").val(), numeroChecks: numeroChecks, totalImporte: total }, "POST", true, function (result) {
        if (result.Exito) {
            if (result.Importe)
                ModalNextChequeManual();
            else
                ConfirmCustom(result.Mensaje, ModalNextChequeManual);
        } else {
            ErrorCustom(result.Mensaje, "");
        }
    });
}

function NextChequeManual() {
    jsonCheques = {};
    var noCheque = $("#js_noIniciocheque").html();
    $(".ModalNextChequeManual").modal("hide");
    var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
    if (getselectedrowindexes.length > 0) {
        var rows = getselectedrowindexes.length;
        var error = false;
        var index = 0;
        while (rows > 0 && !error) {
            var selectedRowData = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[rows-1]);
            ajaxJson("GuardaAsignacionCheque", { Id_TipoCR: selectedRowData.Id_TipoCR, Id_FolioCR: selectedRowData.Id_FolioCR, IdCuentaBancaria: $("#CuentaBancaria").val(), NoCheque: noCheque }, "POST", true, function (result) {
                if (!result.Exito) {
                    error = true;
                    ErrorCustom(result.Mensaje, "");
                }
                no_Cheque = result.NoCheque;
                selectedRowData.No_Cheque = no_Cheque;
                jsonCheques[index] = selectedRowData;
                index++;
            });
            rows--;
            noCheque++;
        }
        DestroyGrid();
        ConstruirGridSave();
        reset();
        $("#js_divImporte").addClass("hide");
    }
}

function ModalNextChequeManual() {
    customModal("ModalNextChequeManual", { IdCuentaBancaria: $("#CuentaBancaria").val() }, "GET", "sm", NextChequeManual, "", "Aceptar", "Cancelar", "Inicio No cheque", "ModalNextChequeManual");
}
function llenarCuentasBanco() {
    ajaxSelect("/Tesoreria/Listas/List_CtaBancaria", { Id_Fuente: $(this).val() }, "POST", true, "CuentaBancaria", "", callBackLlenarSelect);
}
function FocusoutProveedor() {
    if ($(this).val().length > 0) {
        $("#IdBeneficiario").focusOut({
            url: "/Tesoreria/FocusOut/Beneficiario",
            data: { IdBeneficiario: $(this).val() },
            campos: [{ Base: "NombreCompleto", Campo: "DescripcionBeneficiario" }]
        });
        DestroyGrid();
        url = "/Tesoreria/Egresos/ListaCheques?Fecha=" + $("#FechaVen").val() + "&radio=" + radio + "&Id_Bene=" + $("#IdBeneficiario").val();
        ConstruirGrid();
    }

}
function reset() {
    $("#FechaVen").prop("selectedIndex", 0);
    $("#Banco").prop("selectedIndex", 0);
    $("#CuentaBancaria").empty();
    $("#FuenteFin").prop("selectedIndex", 0);

}

function getFuente()
{
    ajaxJson("getFuente", { idCta: $(this).val() }, "POST", true, function (result) {
        $("#txtFuente").val(result.Mensaje);
    });
}
function SeleccionarProveedor() {
    $("#IdBeneficiario").val($(this).data("idbeneficiario"));
    $("#DescripcionBeneficiario").val($(this).data("nombre"));
    $(".MyModal1").modal("hide");
    return false;
}
function TablaBuscarProveedor() {
    $("#resultsBeneficiarios").ajaxLoad({ url: "/Tesoreria/FocusOut/Tbl_Beneficiario", data: { BDescripcionBeneficiario: $("#BDescripcionBeneficiario").val() }, method: "POST" });
}
function ModalBuscarProveedor() {
    customModal("/Tesoreria/FocusOut/Buscar_Beneficiario",{},"GET","lg",TablaBuscarProveedor,"","Buscar","Cancelar","Buscar Beneficiario","MyModal1")
}
function changeFecha() {
    $('#jqxgrid').jqxGrid('clear');
}

function Limpiar() {
    $(".js_radio").prop("checked", false);
    radio = 0;
    $("#FechaPago").val("");
    $("#IdBeneficiario").val("");
    $("#DescripcionBeneficiario").val("");
    reset();
    DestroyGrid();
    url = "/Tesoreria/Egresos/ListaCheques?Fecha=" + $("#FechaVen").val() + "&radio=" + radio;
    DestroyGridSave();
    //ConstruirGrid();
}

function ChangeRadio() {
    radio = $(this).data("id");
    //ajaxSelect("GetFechaVenc", { radio: $(this).data("id") }, "POST", true, "FechaVen", "", callBackLlenarSelect);
    if (radio == 1) {
        $("#div_proveedores").removeClass("hide");
        //$("#div_bancos").addClass("hide");
        radio = 1;
    }
    else {
        //$("#div_bancos").removeClass("hide");
        $("#div_proveedores").addClass("hide");
        radio = 2;
    }
    $('#Banco').prop('selectedIndex', 0);
    //setTimeout(function () { $('#FechaVen').prop('selectedIndex', 0); }, 1000);
    $('#Banco').prop('CuentaBancaria', 0);
    $("#IdBeneficiario").val("");
    $("#DescripcionBeneficiario").val("");
    /*DestroyGrid();
    url = "/Tesoreria/Egresos/ListaCheques?Fecha=" + $("#FechaVen").val() + "&radio=" + radio;
    ConstruirGrid();*/
}
function DestroyGrid() {
    $('#jqxgrid').jqxGrid('destroy');
    $(".js_grid").append("<div id='jqxgrid'></div>");
}
function DestroyGridSave() {
    $('#jqxgrid2').jqxGrid('destroy');
    $(".js_grid2").append("<div id='jqxgrid2'></div>");
}
var url = "/Tesoreria/Egresos/ListaCheques?Fecha=" + $("#FechaVen").val() + "&radio=" + radio;
function ConstruirGrid() {
    DestroyGridSave();
    $("#jqxgrid").empty();
    $("#js_divImporte").removeClass("hide");
    var Localization = {
        // separator of parts of a date (e.g. '/' in 11/05/1955)
        '/': "/",
        // separator of parts of a time (e.g. ':' in 05:44 PM)
        ':': ":",
        // the first day of the week (0 = Sunday, 1 = Monday, etc)
        firstDay: 0,
        days: {
            // full day names
            names: ["Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo"],
            // abbreviated day names
            namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            // shortest day names
            namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
        },
        months: {
            // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
            names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
            // abbreviated month names
            namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
        },
        // AM and PM designators in one of these forms:
        // The usual view, and the upper and lower case versions
        //      [standard,lowercase,uppercase]
        // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
        //      null
        AM: ["AM", "am", "AM"],
        PM: ["PM", "pm", "PM"],
        eras: [
        // eras in reverse chronological order.
        // name: the name of the era in this culture (e.g. A.D., C.E.)
        // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
        // offset: offset in years from gregorian calendar
        { "name": "A.D.", "start": null, "offset": 0 }
        ],
        twoDigitYearMax: 2029,
        patterns: {
            // short date pattern
            d: "M/d/yyyy",
            // long date pattern
            D: "dddd, MMMM dd, yyyy",
            // short time pattern
            t: "h:mm tt",
            // long time pattern
            T: "h:mm:ss tt",
            // long date, short time pattern
            f: "dddd, MMMM dd, yyyy h:mm tt",
            // long date, long time pattern
            F: "dddd, MMMM dd, yyyy h:mm:ss tt",
            // month/day pattern
            M: "MMMM dd",
            // month/year pattern
            Y: "yyyy MMMM",
            // S is a sortable format that does not vary by culture
            S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss"
        },
        percentsymbol: "%",
        currencysymbol: "$",
        currencysymbolposition: "before",
        decimalseparator: '.',
        thousandsseparator: ',',
        pagergotopagestring: "Ir a la ´página:",
        pagershowrowsstring: "Mostrar englones:",
        pagerrangestring: " de ",
        pagerpreviousbuttonstring: "anterior",
        pagernextbuttonstring: "siguiente",
        groupsheaderstring: "Drag a column and drop it here to group by that column",
        sortascendingstring: "Ascendente",
        sortdescendingstring: "Descendente",
        sortremovestring: "Quitar filtro",
        groupbystring: "Group By this column",
        groupremovestring: "Remove from groups",
        filterclearstring: "Clear",
        filterstring: "Filter",
        filtershowrowstring: "Show rows where:",
        filtershowrowdatestring: "Show rows where date:",
        filterorconditionstring: "Or",
        filterandconditionstring: "And",
        filterselectallstring: "(Select All)",
        filterchoosestring: "Please Choose:",
        filterstringcomparisonoperators: ['empty', 'not empty', 'contains', 'contains(match case)',
            'does not contain', 'does not contain(match case)', 'starts with', 'starts with(match case)',
            'ends with', 'ends with(match case)', 'equal', 'equal(match case)', 'null', 'not null'],
        filternumericcomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
        filterdatecomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
        filterbooleancomparisonoperators: ['equal', 'not equal'],
        validationstring: "Entered value is not valid",
        emptydatastring: "No hay datos para mostrar",
        filterselectstring: "Select Filter",
        loadtext: "Cargando...",
        clearstring: "Clear",
        todaystring: "Today"
    };
    var source =
        {
            dataType: "json",
            type: "POST",
            dataFields: [
                 {name:"Id_TipoCR",type:"number"},
                 { name: "TipoCR", type: "string" },
                 { name: "Id_FolioCR", type: "number" },
                 { name: "FechaVen", type: "date", },
                 { name: "Descripcion", type: "string" },
                 { name: "Fecha_Ejercido", type: "date" },
                 { name: "Importe", type: "float" }
            ],
            url: url,
            id: "id"
        };
    var dataAdapter = new $.jqx.dataAdapter(source);
    // create jqxgrid.

    $("#jqxgrid").jqxGrid(
    {
        width: "100%",
        height: 450,
        source: dataAdapter,
        sortable: true,
        filterable: false,
        ready: function () {
            $("#jqxgrid").jqxGrid('localizestrings', Localization);
            /*$('#jqxgrid').on('change', function (event) {
                console.log("evento");
            });*/
        },
        selectionmode: 'checkbox',
        theme: 'metro',
        columns: [
              { text: "Tipo", cellsAlign: "center", align: "center", dataField: "TipoCR" },
              { text: "Folio", cellsAlign: "center", align: "center", dataField: "Id_FolioCR" },
              { text: "Vencimiento", dataField: "FechaVen", cellsAlign: "center", align: "center", cellsformat: "dd/MM/yyyy" },
              { text: "Descripción", dataField: "Descripcion", cellsAlign: "center", align: "center" },
              { text: "Ejercido", dataField: "Fecha_Ejercido", cellsAlign: "center", align: "center", cellsformat: "dd/MM/yyyy" },
              { text: "Importe", dataField: "Importe", cellsAlign: "right", cellsformat: "c2" }
        ]
    });

}
function ConstruirGridSave() {
    $("#jqxgrid2").empty();
    var Localization = {
        // separator of parts of a date (e.g. '/' in 11/05/1955)
        '/': "/",
        // separator of parts of a time (e.g. ':' in 05:44 PM)
        ':': ":",
        // the first day of the week (0 = Sunday, 1 = Monday, etc)
        firstDay: 0,
        days: {
            // full day names
            names: ["Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo"],
            // abbreviated day names
            namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            // shortest day names
            namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
        },
        months: {
            // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
            names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
            // abbreviated month names
            namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
        },
        // AM and PM designators in one of these forms:
        // The usual view, and the upper and lower case versions
        //      [standard,lowercase,uppercase]
        // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
        //      null
        AM: ["AM", "am", "AM"],
        PM: ["PM", "pm", "PM"],
        eras: [
        // eras in reverse chronological order.
        // name: the name of the era in this culture (e.g. A.D., C.E.)
        // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
        // offset: offset in years from gregorian calendar
        { "name": "A.D.", "start": null, "offset": 0 }
        ],
        twoDigitYearMax: 2029,
        patterns: {
            // short date pattern
            d: "M/d/yyyy",
            // long date pattern
            D: "dddd, MMMM dd, yyyy",
            // short time pattern
            t: "h:mm tt",
            // long time pattern
            T: "h:mm:ss tt",
            // long date, short time pattern
            f: "dddd, MMMM dd, yyyy h:mm tt",
            // long date, long time pattern
            F: "dddd, MMMM dd, yyyy h:mm:ss tt",
            // month/day pattern
            M: "MMMM dd",
            // month/year pattern
            Y: "yyyy MMMM",
            // S is a sortable format that does not vary by culture
            S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss"
        },
        percentsymbol: "%",
        currencysymbol: "$",
        currencysymbolposition: "before",
        decimalseparator: '.',
        thousandsseparator: ',',
        pagergotopagestring: "Ir a la ´página:",
        pagershowrowsstring: "Mostrar englones:",
        pagerrangestring: " de ",
        pagerpreviousbuttonstring: "anterior",
        pagernextbuttonstring: "siguiente",
        groupsheaderstring: "Drag a column and drop it here to group by that column",
        sortascendingstring: "Ascendente",
        sortdescendingstring: "Descendente",
        sortremovestring: "Quitar filtro",
        groupbystring: "Group By this column",
        groupremovestring: "Remove from groups",
        filterclearstring: "Clear",
        filterstring: "Filter",
        filtershowrowstring: "Show rows where:",
        filtershowrowdatestring: "Show rows where date:",
        filterorconditionstring: "Or",
        filterandconditionstring: "And",
        filterselectallstring: "(Select All)",
        filterchoosestring: "Please Choose:",
        filterstringcomparisonoperators: ['empty', 'not empty', 'contains', 'contains(match case)',
            'does not contain', 'does not contain(match case)', 'starts with', 'starts with(match case)',
            'ends with', 'ends with(match case)', 'equal', 'equal(match case)', 'null', 'not null'],
        filternumericcomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
        filterdatecomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
        filterbooleancomparisonoperators: ['equal', 'not equal'],
        validationstring: "Entered value is not valid",
        emptydatastring: "No hay datos para mostrar",
        filterselectstring: "Select Filter",
        loadtext: "Cargando...",
        clearstring: "Clear",
        todaystring: "Today"
    };
    var source =
        {
            localdata: jsonCheques,
            dataType: "array",
            dataFields: [
                 { name: "TipoCR", type: "string" },
                 { name: "Id_FolioCR", type: "number" },
                 { name: "FechaVen", type: "date", },
                 { name: "Descripcion", type: "string" },
                 { name: "Fecha_Ejercido", type: "date" },
                 { name: "Importe", type: "money" },
                 { name: "No_Cheque", type: "number" }
            ],
            id: "id"
        };
    var dataAdapter = new $.jqx.dataAdapter(source);
    // create jqxgrid.

    $("#jqxgrid2").jqxGrid(
    {
        width: "100%",
        height: 450,
        source: dataAdapter,
        sortable: true,
        filterable: false,
        ready: function () {
            $("#jqxgrid2").jqxGrid('localizestrings', Localization);
            /*$('#jqxgrid').on('change', function (event) {
                console.log("evento");
            });*/
        },
        theme: 'metro',
        columns: [
              { text: "Tipo", cellsAlign: "center", align: "center", dataField: "TipoCR" },
              { text: "Folio", cellsAlign: "center", align: "center", dataField: "Id_FolioCR" },
              { text: "Vencimiento", dataField: "FechaVen", cellsAlign: "center", align: "center", cellsformat: "dd/MM/yyyy" },
              { text: "Descripcion", dataField: "Descripcion", cellsAlign: "center", align: "center" },
              { text: "Ejercido", dataField: "Fecha_Ejercido", cellsAlign: "center", align: "center", cellsformat: "dd/MM/yyyy" },
              { text: "Importe", dataField: "Importe", cellsAlign: "right", cellsformat: "c2" },
              { text: "No Cheque", dataField: "No_Cheque", cellsAlign: "right", align: "center" }
        ]
    });

}