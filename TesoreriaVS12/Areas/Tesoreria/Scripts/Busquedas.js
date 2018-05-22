var buscarArea = function (modalSize) {
    $(".js_BuscarCentroGestor").on("click", function () {
        customModal(urlBusquedaAreas,
                    {},
                    "GET",
                    modalSize,
                    tblAreas,
                    "",
                    "Buscar",
                    "Cerrar",
                    "Buscar centro gestor",
                    $(this).attr("id"));
        return false;
    });
}

var tblAreas = function () {
    $("#resultsAreas").ajaxLoad({ url: "/Tesoreria/Busquedas/Tbl_Area", method: "POST", data: { BDescripcionArea: $("#BDescripcionArea").val() } });
}

var seleccionarArea = function (selectors, modalSelectorClass) {
    $("body").on("click", "." + modalSelectorClass + " .js_SeleccionarArea", function () {
        var id = $(this).data("idarea");
        var desc = $(this).data("descripcion");        
        var valores = [id, desc];
        $.each(selectors, function (key, value) {
            $("#" + value).val(valores[key]);
        });
        $("." + modalSelectorClass).modal("hide");
        return false;
    });
}
    

var seleccionarArea__ = function (modalSelectorClass) {
    $("body").on("click", "." + modalSelectorClass + " .js_SeleccionarBeneficiario", function () {
        seleccionarBenfeciario({ Id_Beneficiario: $(this).data("idbeneficiario"), Beneficiario: $(this).data("nombre") }, "ModalBusBeneficiario");
        return false;
    });
}