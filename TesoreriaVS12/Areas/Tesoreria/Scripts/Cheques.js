
var buscarCheques = function () {
    if ($("#frmBusqueda").valid()) {
        ajaxLoad($("#frmBusqueda").attr("action"), $("#frmBusqueda").serialize(), "cheques", "POST", function (response) { });
    }
}

var imprimirCheques = function () {
    if ($("#frmBusqueda").valid())
        if ($("#frmImpresion").valid()) {
            var parameters = $("#frmBusqueda").serialize() + "&" + $("#frmImpresion").serialize();
            ajaxJson($("#frmImpresion").attr("action"), parameters, "POST", true, function (response) {
                if (!response.Exito) {
                    ErrorCustom(response.Mensaje);
                }
                else {
                    $("#js_mBuscar").trigger("click");
                    window.open("/Tesoreria/Cheques/" + response.UrlRpt + "?Formato=" + response.Formato, "_blank");
                }
            });
        }
}