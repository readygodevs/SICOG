var Nuevo = function () {
    /*url, data, div, metod, callback*/
    nuevo = 1;
    ajaxLoad("/Tesoreria/Catalogos/V_AgregarCompromiso", {nuevo:nuevo}, "modal-content1", "GET", openModal);
}
var Detalles = function () {
    ajaxLoad("/Tesoreria/Catalogos/V_DetallesCompromiso", { IdCompromiso: $(this).data('id') }, "modal-content1", "GET", openModal);
}

var Guardar = function () {
    if ($("#frm").valid()) {
        /*url, data, metodo, asincrono, callback*/
        if ($("#h_nuevo").val() == 1)
            var url = "/Tesoreria/Catalogos/GuardarCompromiso"
        else
            var url = "/Tesoreria/Catalogos/ModificarCompromiso"
        ajaxJson(url, $("#frm").serialize(), "POST", true, callBackSave);
    }
}
var Obtener = function () {
    aPos = tabla.fnGetPosition($(this).parent().parent().get(0));
    nuevo = 2;
    ajaxLoad("/Tesoreria/Catalogos/V_AgregarCompromiso", { IdCompromiso: $(this).data('id'), nuevo: nuevo }, "modal-content1", "GET", openModal);

}
var Eliminar = function () {
    aPos = tabla.fnGetPosition($(this).parent().parent().get(0));
    var id = $(this).data('id');
    var eliminar = function () {
        ajaxJson("/Tesoreria/Catalogos/EliminarCompromiso", { IdCompromiso: id }, "POST", true, function (response) {
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
            response.Registro.Acciones = Acciones({ Id: response.Registro.Id_Actividad });
            AddTbl(response.Registro, "tbl");
        }
        else {
            var Acciones = _.template($('#jsActionsource').html());
            response.Registro.Acciones = Acciones({ Id: response.Registro.Id_Actividad });
            UpdateTbl(response.Registro, "tbl", aPos);
        }
    }

    else
        ErrorCustom(response.Mensaje, "");
}