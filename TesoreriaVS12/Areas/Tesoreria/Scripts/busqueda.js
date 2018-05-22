var construirCampo = function (Campo,Url) {
    $('#' + Campo).typeahead({
        source: function (query, process) {
            buscar(query, process, Campo,Url);
        },
        autoSelect: true,
        minLength: 0, items: 15
    });
}
var construirCampo2 = function (Campo, Campo2, Url) {
    $('#' + Campo).typeahead({
        source: function (query, process) {
            buscar2(query, process, Campo, Campo2, Url);
        },
        autoSelect: true,
        minLength: 0, items: 15
    });
}
var buscar2 = _.debounce(function (query, process, Campo, Campo2, Url) {
    ajaxJson(Url, { descripcion: $("#" + Campo).val(), descripcion2: $("#" + Campo2).val() }, "POST", true, function (results) {
        $("#" + Campo).data("datasource", results.Ids);
        return process(results.Data);
    });
}, 300);
var buscar = _.debounce(function (query, process, Campo,Url) {
    ajaxJson(Url, { descripcion: $("#" + Campo).val() }, "POST", true, function (results) {
        $("#" + Campo).data("datasource", results.Ids);
        return process(results.Data);
    });
}, 300);
var change = function (Campo) {
    //debugger;
    $("body").on("change", "#" + Campo, function () {
        if ($("#" + Campo).val().split('-').length != 1) {
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").val($("#" + Campo).val().split('-')[1]);
            $("#" + Campo).val($("#" + Campo).val().split('-')[0]);
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").focus();
        } else if ($.inArray($("#" + Campo).val(), $("#" + Campo).data("datasource")) == -1) {
            $("#" + Campo).val("");
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").val("");
        }
        $("#" + Campo).next;
    });
}
var change2 = function (Campo) {
    $("body").on("change", "#" + Campo, function () {
        if ($("#" + Campo).val().split('|').length != 1) {
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").val($("#" + Campo).val().split('|')[1]);
            $("#" + Campo).val($("#" + Campo).val().split('|')[0]);
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").focus();
        } else if ($.inArray($("#" + Campo).val(), $("#" + Campo).data("datasource")) == -1) {
            $("#" + Campo).val("");
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").val("");
        }
        $("#" + Campo).next;
    });
}



var change3 = function (Campo, campo2) {
    $("body").on("change", "#" + Campo, function () {

        if ($("#" + Campo).val().split('-').length != 1) {
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").val($("#" + Campo).val().split('-')[1]);
            $("#" + campo2).val($("#" + Campo).val().split('-')[1]);
            $("#" + Campo).val($("#" + Campo).val().split('-')[0]);
    

            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").focus();
        } else if ($.inArray($("#" + Campo).val(), $("#" + Campo).data("datasource")) == -1) {
            $("#" + Campo).val("");
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").val("");
        }
        $("#" + Campo).next;
    });
}
var focusOut = function (Campo) {
    $("body").on("focusout", "#" + Campo, function () {
        var clave = $(this).val().trim();
        if (clave.length == 0) {
            $("#" + Campo).parent().parent().parent().siblings().find(".js_Descripcion").val("");
        }
    });
}