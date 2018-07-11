var dataTab3 = [
    //{ ingresos: 5000000, descuentos: 500000, descuentos_no_autorizado: 900000},
];

var pathname = window.location.pathname; // Returns path only
if (pathname.indexOf("TableroControl") > -1) {
    $.ajax({
        type: "POST",
        dataType: "json",
        async: true,
        url: urlIngresosDesctos,
        //data: { tiendas: tiendas[indexTiendas], categorias: categorias[indexCategorias] },
        complete: function (e, xhr, settings) {
            llenarTab3();
        },
        success: function (data) {
            console.log("response->");
            console.log(data);

            //for (var i = 0; i < data.length; i++) {

            //}

            dataTab3 = data;


        }
    });
}

