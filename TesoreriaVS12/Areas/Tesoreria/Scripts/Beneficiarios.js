var buscarBeneficiario = function (modalSize) {
    $("body").on("click", ".js_Beneficiario", function () {
        if (!$(this).isSiblingDisabled())
            customModal("/Tesoreria/Busquedas/Buscar_Beneficiario",
                    {},
                    "GET",
                    modalSize,
                    buscarBeneficiarioTbl,
                    "",
                    "Buscar",
                    "Cerrar",
                    "Buscar beneficiario",
                    $(this).attr("id"));
    });
}

var buscarBeneficiarioTbl = function () {
    $("#resultsBeneficiarios").ajaxLoad({ url: "/Tesoreria/Busquedas/Tbl_Beneficiario", method: "POST", data: { BDescripcionBeneficiario: $("#BDescripcionBeneficiario").val() } });
}

var seleccionarBeneficiario = function (selectors, modalSelectorClass, callBack, paramsCallback) {
    $("body").on("click", "." + modalSelectorClass + " .js_SeleccionarBeneficiario", function () {
        var id = $(this).data("idbeneficiario");
        var desc = $(this).data("nombre");
        var valores = [id, desc];        
        $.each(selectors, function (key, value) {
            $("#" + value).val(valores[key]);
        });
        $("." + modalSelectorClass).modal("hide");
        if (callBack != "") {
            paramsCallback.postId = id;
            var call = $.Callbacks();
            call.add(callBack(paramsCallback));
            call.fire();
        }
        return false;
    });
}

var getClasificaBeneficiario = function (params) {
    ajaxSelect("/Tesoreria/Listas/getClasificacionBeneficiario", { beneficiario: params.postId }, "POST", true, params.targetSelector, params.optionSelected, callBackLlenarSelectClasificacion);
}

var callBackLlenarSelectClasificacion = function (result, dr, seleccion) {
    $("#" + dr).removeAttr("disabled");
    var selected = 0;
    $("#" + dr).empty();
    if (result.length != 0) {
        $("#" + dr).append("<option value='0'>--Selecciona una opción--</option>");
        $.each(result, function (i, item) {
            selected = item.Value;
            if (item.Selected == true)
                $("#" + dr).append("<option selected='true' value='" + item.Value + "'>" + item.Text + "</option>");
            else
                $("#" + dr).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
        });
        //$("#selectbox option[value=3]").attr("selected", true);   
        if (result.length == 1) {
            $("#" + dr).val(selected);
            $("#" + dr).attr("disabled", "disabled");
        }
    }
    else {
        $("#" + dr).append("<option value=''>--Sin Resultados--</option>");
    }
}