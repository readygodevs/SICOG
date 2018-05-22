var Nuevo = function () {
    /*url, data, div, metod, callback*/
    ajaxLoad(RutaAdd, { Id: $(this).data("id") }, "MyModal1 .modal-body", "GET", openModal);
}
var Obtener = function () {
    /*url, data, div, metod, callback*/
    aPos = tabla.fnGetPosition($(this).parent().parent().get(0));
    ajaxLoad(RutaObtener, { Id: $(this).data("id") }, "MyModal1 .modal-body", "GET", openModal);
}
var Detalles = function (data) {
    /*url, data, div, metod, callback*/
    if (typeof data.originalEvent === "undefined")
        ajaxLoad(RutaDetalles, data, "MyModal1 .modal-content1", "GET", openModal);
    else
        ajaxLoad(RutaDetalles, { Id: $(this).data("id") }, "MyModal1 .modal-body", "GET", openModal);
}
var Guardar = function () {
    if ($("#" + FrmId).valid()) {
        /*url, data, metodo, asincrono, callback*/
        ajaxJson(RutaSave, $("#" + FrmId).serialize(), "POST", true, callBackSave);
    }
}
var Editar = function () {
    if ($("#" + FrmId).valid()) {
        /*url, data, metodo, asincrono, callback*/
        ajaxJson(RutaEdit, $("#" + FrmId).serialize(), "POST", true, callBackEdit);
    }
}
var ExportarPDFCatalogo = function () {
    window.open(RutaExportarPDF,"_blank");
}
var callBackEdit = function (response) {
    if (response.Exito) {
        ExitoCustom(response.Mensaje, closeModal);
        if ($('#' + SourceAction).length > 0) {
            var Acciones = _.template($('#' + SourceAction).html());
            var js = jQuery.parseJSON(Propiedades);
            _.map(js, function (value, key) {
                valor = eval(value);
                eval('js.' + key + ' = ' + value);
            });
            response.Registro.Acciones = Acciones(js);
        }
        UpdateTbl(response.Registro, TableId, aPos);
    }
    else
        ErrorCustom(response.Mensaje, "");

}

var callBackSave = function (response) {
    if (response.Exito == false)
        ErrorCustom(response.Mensaje, "");
    else {
        ExitoCustom("", closeModal);
        var Acciones = _.template($('#jsActionsource').html());
        response.Datos.Acciones = Acciones({ Id_Area: response.Datos.Id_Area });
        AddTbl(response.Datos, "tbl");
    }
    //if (response.Exito) {
    //    ExitoCustom(response.Mensaje, closeModal);
    //    if ($('#' + SourceAction).length > 0) {
    //        var Acciones = _.template($('#' + SourceAction).html());
    //        var js = jQuery.parseJSON(Propiedades);
    //        _.map(js, function (value, key) {
    //            valor = eval(value);
    //            eval('js.' + key + ' = ' + value);
    //        });
    //        response.Registro.Acciones = Acciones(js);
    //    }
    //    AddTbl(response.Registro, TableId);

    //}
    //else
    //    ErrorCustom(response.Mensaje, "");
}

var Eliminar = function () {
    var elemento = $(this);
    ConfirmCustom(MsjEliminar,
        function () {
            ajaxJson(RutaDelete, { Id: elemento.data("id") }, "GET", true, function (response) {
                if (response.Exito == true)
                    ExitoCustom(response.Mensaje, function () {
                        aPos = $("#" + TableId).dataTable().fnGetPosition(elemento.parent().parent().get(0));
                        $("#" + TableId).dataTable().fnDeleteRow(aPos);
                        aPos = -1;
                    });
                else
                    ErrorCustom(response.Mensaje, "");
            });
        }, "", "Aceptar", "Cancelar");
}