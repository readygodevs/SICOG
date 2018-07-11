var dataTab01 = [];
var dataTab02 = [];

var pathname = window.location.pathname; // Returns path only
if (pathname.indexOf("TableroControl") > -1) {
    $.ajax({
        type: "POST",
        dataType: "json",
        async: true,
        url: urlTop10Ben,
        //data: { tiendas: tiendas[indexTiendas], categorias: categorias[indexCategorias] },
        complete: function (e, xhr, settings) {
            llenarTab01();
            obtenerTab02();
        },
        success: function (data) {
            console.log("response top 10->");
            console.log(data);

            dataTab01 = data;

            
        }
    });
}

function obtenerTab02() {
    $.ajax({
        type: "POST",
        dataType: "json",
        async: true,
        url: urlTop10TipoBen,
        //data: { tiendas: tiendas[indexTiendas], categorias: categorias[indexCategorias] },
        complete: function (e, xhr, settings) {
            llenarTab02();
        },
        success: function (data) {
            console.log("response top 10 tipos->");
            console.log(data);

            dataTab02 = data;


        }
    });
}

