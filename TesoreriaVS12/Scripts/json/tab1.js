var dataTab1 = [];
var catTab1 = [];
var dataTab2 = [];

var pathname = window.location.pathname; // Returns path only
if (pathname.indexOf("TableroControl") > -1) {
    $.ajax({
        type: "POST",
        dataType: "json",
        async: true,
        url: urlJsonGrafica,
        //data: { tiendas: tiendas[indexTiendas], categorias: categorias[indexCategorias] },
        complete: function (e, xhr, settings) {
            obtenerGrafica();
            llenarTab2();
        },
        success: function (data) {
            //console.log("response->");
            //console.log(data);

            var Ejecutado = [];
            var PorEjecutar = [];
            for (var i = 0; i < data.length; i++) {
                catTab1.push(data[i].Mes);

                Ejecutado.push(data[i].Ejecutado);

                PorEjecutar.push(data[i].PorEjecutar);

                dataTab2.push(
                    { mes: data[i].Mes, ejecutado: data[i].Ejecutado, por_ejecutar: data[i].PorEjecutar, total: data[i].Total });

            }

            dataTab1 = [{
                name: 'Ejercido',
                color: "#FFEC17",
                data: Ejecutado,
                index: 1
            }, {
                name: 'Disponible',
                color: "green",
                data: PorEjecutar,
                index: 2
            }];


        }
    });
}

