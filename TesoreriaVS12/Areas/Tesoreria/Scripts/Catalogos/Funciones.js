var Nuevo = function () {
    /*url, data, div, metod, callback*/
    ajaxLoad(RutaAgregar, {}, "modal-content1", "GET", openModal);
}
var Detalles = function () {
    ajaxLoad(RutaDetalles, { IdFuncion: $(this).data('id') }, "modal-content1", "GET", openModal);
}

var Guardar = function () {
    if ($("#frm").valid()) {
        /*url, data, metodo, asincrono, callback*/
        ajaxJson(RutaGuardar, $("#frm").serialize(), "POST", true, callBackSave);
    }
}
var Obtener = function () {
    aPos = tabla.fnGetPosition($(this).parent().parent().get(0));
    ajaxLoad(RutaAgregar, { IdFuncion: $(this).data('id') }, "modal-content1", "GET", openModal);

}
var Eliminar = function () {
    aPos = tabla.fnGetPosition($(this).parent().parent().get(0));
    var id = $(this).data('id');
    var eliminar = function () {
        ajaxJson(RutaEliminar, { IdFuncion: id }, "POST", true, function (response) {
            if (response.Exito) {
                ExitoCustom(response.Mensaje, closeModal);
                tabla.fnDeleteRow(aPos);
            }
            else
                ErrorCustom(response.Mensaje, "");
        });
    }
    ConfirmCustom("¿ Seguro que desea eliminar la función?", eliminar, "", "Si", "No");
}
var callBackSave = function (response) {
    if (response.Exito) {
        ExitoCustom(response.Mensaje, closeModal);
        if (response.Nuevo == true) {
            var Acciones = _.template($('#jsActionsource').html());
            response.Registro.Acciones = Acciones({ Id: response.Registro.IdFuncion });
            AddTbl(response.Registro, "tbl");
        }
        else {
            var Acciones = _.template($('#jsActionsource').html());
            response.Registro.Acciones = Acciones({ Id: response.Registro.IdFuncion });
            UpdateTbl(response.Registro, "tbl", aPos);
        }
    }

    else
        ErrorCustom(response.Mensaje, "");
}