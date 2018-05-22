var sumar = function (Nombre) {
    var total = parseFloat($("#"+Nombre+"01").autoNumeric("get"));
    var o = 0;
    for (i = 2; i < 13; i++) {
        if (i < 10) {
            o = i;
            total += parseFloat($("#"+Nombre+"0" + o).autoNumeric("get"));
        } else {
            total += parseFloat($("#"+Nombre+"" + i).autoNumeric("get"));
        }
    }
    if (Nombre == "Aprobado")
        $("#Total").autoNumeric("set", total);
    else
        $("#Importe").autoNumeric("set", total);
};
var InicializarImportes = function (Nombre, Importe) {
    $(".js_previsualizar").on("click", function () {
        if (seleccionado > 0) {
            if (seleccionado == 2) {
                var total = $("#importeMeses").autoNumeric("get");
                var importe = (total / 12).toFixed(2);
                var resto = (total - (importe * 11)).toFixed(2);
                for (i = 0; i < 12; i++) {
                    if (i < 10)
                        $("#"+Nombre+"0" + i).val(importe);
                    else
                        $("#"+Nombre+ i).val(importe);
                }
                $("#"+Nombre+"12").val(resto);
                $("#" + Importe).autoNumeric("set", total);
                $(".js_mes").attr("readonly", "readonly");
                $("#" + Importe).attr("readonly", "readonly");
            } else if (seleccionado == 3) {
                var importe = $("#importe").autoNumeric("get");
                var porcentaje = $("#porcentaje").val().trim();
                porcentaje = (importe * porcentaje) / 100;
                var calculo = parseInt(importe) + porcentaje;
                $("#"+Nombre+"01").val(importe);
                var o = 0;
                for (i = 2; i < 13; i++) {
                    if (i < 10) {
                        o = i;
                        $("#"+Nombre+"0" + o).val(calculo);
                        var valor = parseInt($("#"+Nombre+"0" + o).val());
                        calculo = valor + porcentaje;
                    } else {
                        $("#"+Nombre+"" + i).val(calculo);
                        var valor = parseInt($("#"+Nombre+"" + i).val());
                        calculo = valor + porcentaje;
                    }
                }
                sumar(Nombre);
                $(".js_mes").attr("readonly", "readonly");
                $("#" + Importe).attr("readonly", "readonly");
            }
            $(".js_mes").focusout();
        }
    });
}