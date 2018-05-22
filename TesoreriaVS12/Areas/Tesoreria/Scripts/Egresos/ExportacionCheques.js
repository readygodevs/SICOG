var Datos = function () {
    this.Id_TipoCR = "";
    this.Id_FolioCR = "";
}

var ExportarCheques = function () {
    var ListaDatos = [];
    var rows = $('#' + GridCheques).jqxGrid('getrows');
    var getselectedrowindexes = $('#' + GridCheques).jqxGrid('getselectedrowindexes');
    if (typeof getselectedrowindexes != "undefined") {
        if (getselectedrowindexes.length > 0) {
            for (i = 0; i < getselectedrowindexes.length; i++) {
                var selectedRowData = $('#' + GridCheques).jqxGrid('getrowdata', getselectedrowindexes[i]);
                var TempDatos = new Datos();
                TempDatos.Id_TipoCR = selectedRowData.Id_TipoCR;
                TempDatos.Id_FolioCR = selectedRowData.Id_FolioCR;
                ListaDatos.push(TempDatos);
            }
            $("#ListaCheques").val(JSON.stringify(ListaDatos));
            $("#ClaveCuenta").val($("#CuentaBancaria").val());
            $("#FormCheques").submit();
        }else
            ErrorCustom("Debe existir al menos un Cheque seleccionado");
    } else
        ErrorCustom("Debe existir al menos un Cheque seleccionado");
}
var GetCheques = function (CuentaBancaria) {
    ajaxLoad("/Tesoreria/Egresos/GetChequesCtaBancaria", { CuentaBancaria: CuentaBancaria }, "jqxgrid2", "POST", function () {

    });
}
function llenarCuentasBanco() {
    ajaxSelect("/Tesoreria/Listas/List_CtaBancaria", { Id_Banco: $(this).val() }, "POST", true, "CuentaBancaria", "", callBackLlenarSelect);
}
var DestroyGrid = function(Grid) {
    $('#'+Grid).jqxGrid('destroy');
    $(".js_"+Grid).append("<div id='"+Grid+"'></div>");
}
function ConstruirGrid(Grid,Source,Columns) {
    DestroyGrid(Grid);
    $("#"+Grid).empty();
    $("#js_divImporte").removeClass("hide");
    var Localization = {
        '/': "/",
        ':': ":",
        firstDay: 0,
        days: {
            names: ["Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo"],
            namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
        },
        months: {
            names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
            namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
        },
        AM: ["AM", "am", "AM"],
        PM: ["PM", "pm", "PM"],
        eras: [
        { "name": "A.D.", "start": null, "offset": 0 }
        ],
        twoDigitYearMax: 2029,
        patterns: {
            d: "M/d/yyyy",
            D: "dddd, MMMM dd, yyyy",
            t: "h:mm tt",
            T: "h:mm:ss tt",
            f: "dddd, MMMM dd, yyyy h:mm tt",
            F: "dddd, MMMM dd, yyyy h:mm:ss tt",
            M: "MMMM dd",
            Y: "yyyy MMMM",
            S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss"
        },
        percentsymbol: "%",
        currencysymbol: "$",
        currencysymbolposition: "antes",
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
    var source = Source;
    var dataAdapter = new $.jqx.dataAdapter(source);

    $("#"+Grid).jqxGrid(
    {
        width: "100%",
        height: 450,
        source: dataAdapter,
        sortable: true,
        filterable: false,
        ready: function () {
            $("#"+Grid).jqxGrid('localizestrings', Localization);
        },
        selectionmode: 'checkbox',
        theme: 'metro',
        columns: Columns
    });

}