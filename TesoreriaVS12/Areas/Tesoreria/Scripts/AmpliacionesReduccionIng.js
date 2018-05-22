﻿function Afectar() {
    ajaxJson("Afectar", { IdTransferencia: $("#h_IdTransferencia").val(), FechaAfectacion: $("#FechaAfectacion").val() }, "POST", true, function (result) {
        if (result.Exito) {
            ExitoCustom("Afectado correctamente");
            $(".ModalAfectar").modal("hide");
            llenarMaestro(result.Registro);
            recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bEliminar", "bSalir"]);
        }
        else
            ErrorCustom(result.Mensaje, "");
    });
}
function Agregar() {
    ajaxJson("ValidarExistencia", {}, "POST", true, function (result) {
        if (result.Exito) {
            $("#frm")[0].reset();
            $(".jsEnabledAgregar").prop("disabled", false);
            recargarMenuLateral(["bGuardar", "bCancelar", "bSalir"]);
            cancelar = 1;
        } else
            ErrorCustom(result.Mensaje, "");
    });
}
function Buscar() {
    ajaxLoad("V_TablaBusquedaAmpliacionReduccion", $("#frmB").serialize(), "tabla", "POST", function () {
        ConstruirTabla("tbl", "No hay resultado para mostrar.");
    });
}
function Cancelar() {
    if (cancelar == 1) {
        $("#frm")[0].reset();
        $(".jsEnabledAgregar").prop("disabled", true);
        recargarMenuLateral(["bNuevo", "bBuscar", "bSalir"]);
    }
    else {
        $("#Descrip").prop("disabled", true);
        $("#Descrip").val(tempDesc);
        recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bEditar", "bEliminar", "bSalir"]);
    }
}
function changeRadio() {
    if ($(this).val() == 1) {
        $(".js_porFolio").removeClass("hide");
        $(".js_porDesc").addClass("hide");
        $("#DescripciónBusqueda").val("");
    }
    if ($(this).val() == 2) {
        $(".js_porDesc").removeClass("hide");
        $(".js_porFolio").addClass("hide");
        $("#folioBusqueda").val("");
    }
}
function Detalles() {
    window.location.href = "/Tesoreria/Ingresos/V_DetalleDeTransferencia?IdTransferencia=" + $("#Folio").val() + "&origen=1";
}
function Editar() {
    if ($("#Id_Estatus").val() == 1) {
        $(".jsEnabledEditar").prop("disabled", false);
        tempDesc = $("#Descrip").val();
        recargarMenuLateral(["bCancelar", "bGuardar", "bSalir"]);
        cancelar = 2;
    }
    else
        ErrorCustom("Transferencia afectada, NO se puede editar.", "");
}
function ModalEliminar() {
    ajaxJson("ValidarEliminar", { IdTransferencia: $("#Folio").val() }, "POST", true, function (result) {
        if (result.Exito) {
            customModal("V_ModalEliminar", { Folio: $("#Folio").val() }, "GET", "md", Eliminar, "", "Aceptar", "Cancelar", "Fecha de cancelación", "ModalEliminar");
        }
        else
            ErrorCustom(result.Mensaje, "");
    });
}
function Eliminar() {
    var id = $("#Folio").val();
    var confirmar = function () {
        ajaxJson("EliminarTransferencia", { IdTransferencia: id, FechaCancelacion: $("#FechaCancelacion").val() }, "POST", true, function (result) {
            if (result.Exito) {
                ExitoCustom("Eliminado correctamente.");
                $(".ModalEliminar").modal("hide");
                llenarMaestro(result.Registro);
                $(".jsEnabledAgregar").prop("disabled", true);
                recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bSalir"]);
            } else
                ErrorCustom(result.Mensaje, "");

        });
    };
    ConfirmCustom("¿Seguro que desea eliminar?", confirmar, "", "Aceptar", "Cancelar");
}
function Guardar() {
    if ($("#frm").valid()) {
        /*url, data, metodo, asincrono, callback*/
        ajaxJson("GuardarAmpRed", $("#frm").serialize(), "POST", true, function (result) {
            if (result.Exito) {
                ExitoCustom(result.Mensaje, closeModal);
                $("#Folio").val(result.Registro.Folio);
                $("#Id_Estatus").val("1");
                $(".jsEnabledAgregar").prop("disabled", true);
                recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bEditar", "bEliminar", "bSalir"]);
            }
            else
                ErrorCustom(result.Mensaje, closeModal);
        });
    }
    else
        $("#frm").validate();
}
function ModalAfectar() {
    ajaxJson("ValidarAfectacion", { IdTransferencia: $("#Folio").val() }, "POST", true, function (result) {
        if (result.Exito)
            customModal("V_ModalAfectacion", { IdTransferencia: $("#Folio").val(), IdTipo: 2 }, "GET", "md", Afectar, "", "Si", "No", "Ampliaciones y reducciones", "ModalAfectar");
        else
            ErrorCustom(result.Mensaje, "");
    });
}
function ModalBuscar() {
    customModal("V_BuscarAmpliacionReduccion", { idPpto: 1 }, "GET", "lg", Buscar, "", "Buscar", "Cancelar", "Buscar Ampliación/Reducción", "ModalBuscar");
}
function Seleccionar() {
    ajaxJson("SeleccionarTransferencia", { IdTransferencia: $(this).data("id") }, "POST", true, function (result) {
        if (result.Exito) {
            $(".ModalBuscar").modal("hide");
            llenarMaestro(result.Registro);
            if (result.Registro.Id_TipoT == 1)
                $("#rAmpliacion").attr("checked", true);
            else
                $("#rReduccion").attr("checked", true);
            if (result.Afectar == 1) {
                if (result.Registro.Id_Estatus == 3)
                    recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bAfectar", "bSalir"]);
                else
                    recargarMenuLateral(["bNuevo", "bBuscar", "bEditar", "bDetalles", "bEliminar", "bAfectar", "bSalir"]);
            }
            else {
                if (result.Registro.Id_Estatus == 3)
                    recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bSalir"]);
                else
                    recargarMenuLateral(["bNuevo", "bBuscar", "bDetalles", "bEliminar", "bSalir"]);
            }
        }
        else
            ErrorCustom(result.Mensaje, closeModal);
    });
}