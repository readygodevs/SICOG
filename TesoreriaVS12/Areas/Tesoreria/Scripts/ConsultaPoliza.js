var ConsultaPoliza = function (tipo, folio, mes) {
    /*Detalle/DetalleDePolizas  Byte IdTipo, Int16 IdFolio, Byte IdMes fijos 3,4
    var params = "?IdTipo=" + tipo + "&IdFolio=" + folio + "&IdMes=" + mes;
    window.location.href = "/Tesoreria/Polizas/DetalleDePolizas" + params;    */
    console.log("folio:" + folio + ",mes:" + mes);
    if (folio != "" && mes != "")
        window.open(urlPoliza + "?TipoPoliza=" + tipo + "&FolioPoliza=" + folio + "&MesPoliza=" + mes, '_blank');
}