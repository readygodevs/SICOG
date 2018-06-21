var Nuevo = function (data) {
    /*url, data, div, metod, callback*/

    if (typeof data.originalEvent === "undefined") {

        if (typeof data.Modal === "undefined") {
            console.log("sie1");
            $("#MyModal1").openModal({ url: RutaAdd, data: data });
            //ajaxLoad(RutaAdd, data, "MyModal1 .modal-body", "GET", openModal);
        }
        else {
            console.log("sie2");
            $(data.Modal).openModal({ url: RutaAdd, data: data });
            //ajaxLoad(RutaAdd, data, data.Modal, "GET", openModal);
        }
    }
    else {
        //console.log("noee");
        $("#MyModal1").openModal({ url: RutaAdd, data: { Id: $(this).data("id") } });
        //ajaxLoad(RutaAdd, { Id: $(this).data("id") }, "MyModal1 .modal-body", "GET", openModal);
    }
}

var Editar = function (data, element) {
    /*url, data, div, metod, callback*/
    if (typeof data.originalEvent === "undefined") {
        if (typeof data.Modal === "undefined") {
            aPos = $("#" + TableId).dataTable().fnGetPosition($(element).parent().parent().get(0));
            ajaxLoad(RutaEdit, data, "MyModal1 .modal-body", "GET", openModal);
        }
        else {
            aPos = $("#" + TableId).dataTable().fnGetPosition($(this).parent().parent().get(0));
            ajaxLoad(RutaEdit, data, data.Modal, "GET", openModal);
        }
    }
    else {
        aPos = $("#" + TableId).dataTable().fnGetPosition($(this).parent().parent().get(0));
        ajaxLoad(RutaEdit, { Id: $(this).data("id") }, "MyModal1 .modal-body", "GET", openModal);
    }
}

var Detalles = function (data) {
    /*url, data, div, metod, callback*/
    if (typeof data.originalEvent === "undefined") {
        if (typeof data.Modal === "undefined")
            ajaxLoad(RutaDetalles, data, "MyModal1 .modal-body", "GET", openModal);
        else
            ajaxLoad(RutaDetalles, data, data.Modal, "GET", openModal);
    }
    else
        ajaxLoad(RutaDetalles, { Id: $(this).data("id") }, "MyModal1 .modal-body", "GET", openModal);
}

var Guardar = function (callback) {

    try {
        if ($(".jsValidate").length > 0) {
            if ($("#" + FrmId).valid()) {
                if (typeof callback.originalEvent === "undefined") {
                    ajaxJson(RutaAdd, $("#" + FrmId).serialize(), "POST", true, callback);
                }
                else {
                    //console.log("callBackSave->" + callBackSave);
                    ajaxJson(RutaAdd, $("#" + FrmId).serialize(), "POST", true, callBackSave);
                }

            }
        }
        else if (typeof callback.originalEvent === "undefined")
            ajaxJson(RutaAdd, $("#" + FrmId).serialize(), "POST", true, callback);
        else
            ajaxJson(RutaAdd, $("#" + FrmId).serialize(), "POST", true, callBackSave);
    }
    catch (ex) {
        ErrorCustom(ex.message, "");
    }
}

var Guardar2 = function (callback) {

    if ($(".jsValidate").length > 0) {
        if ($("#" + FrmId).valid()) {
            ajaxJson(RutaAdd, $("#" + FrmId).serialize(), "POST", true, callBackSave);

        }
    }
}

var GuardarEdit = function () {
    if ($("#" + FrmId).valid()) {
        /*url, data, metodo, asincrono, callback*/
        ajaxJson(RutaEdit, $("#" + FrmId).serialize(), "POST", true, callBackEdit);
    }
}
var callBackEdit = function (response) {
    if (response.Exito) {
        closeModal();
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

    if (response.Exito) {
        closeModal();
        ExitoCustom("El registro se guardó con éxito", closeModal);
        if ($('#' + SourceAction).length > 0) {
            var Acciones = _.template($('#' + SourceAction).html());
            var js = jQuery.parseJSON(Propiedades);
            _.map(js, function (value, key) {
                valor = eval(value);
                eval('js.' + key + ' = ' + value);
            });
            response.Registro.Acciones = Acciones(js);
        }
        AddTbl(response.Registro, TableId);
    }
    else
        ErrorCustom(response.Mensaje, "");
}

var Eliminar = function (data, elemento, callback) {
    var element = {};
    if (typeof elemento === "undefined")
        element = $(this);
    else
        element = elemento
    if (typeof callback === "undefined")
        callback = "";
    var call = $.Callbacks();
    ConfirmCustom(MsjEliminar,
        function () {
            if (typeof data.originalEvent === "undefined") {
                ajaxJson(RutaDelete, data, "GET", true, function (response) {
                    if (response.Exito == true)
                        ExitoCustom("El registro se eliminó con éxito", function () {
                            aPos = $("#" + TableId).dataTable().fnGetPosition(element.parent().parent().get(0));
                            $("#" + TableId).dataTable().fnDeleteRow(aPos);
                            aPos = -1;
                            if (callback != "") {
                                call.add(callback);
                                call.fire();
                            }
                        });
                    else
                        ErrorCustom(response.Mensaje, "");
                });
            }
            else {
                ajaxJson(RutaDelete, { Id: element.data("id") }, "GET", true, function (response) {
                    if (response.Exito == true)
                        ExitoCustom("El registro se eliminó con éxito", function () {
                            aPos = $("#" + TableId).dataTable().fnGetPosition(element.parent().parent().get(0));
                            $("#" + TableId).dataTable().fnDeleteRow(aPos);
                            aPos = -1;
                            if (callback != "") {
                                call.add(callback);
                                call.fire();
                            }
                        });
                    else
                        ErrorCustom(response.Mensaje, "");
                });
            }

        }, "", "Si", "No");
}