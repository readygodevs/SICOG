var dataTab4 = [
    //{ columna: 'Columna 1', valor: 500000},
    //{ columna: 'Columna 2', valor: 500000},
    //{ columna: 'Columna 3', valor: 500000},
    //{ columna: 'Columna 4', valor: 500000},
    //{ columna: 'Columna 5', valor: 500000},
    //{ columna: 'Columna 6', valor: 500000},
    //{ columna: 'Columna 7', valor: 500000},
    //{ columna: 'Columna 7', valor: 500000},
];
var pathname = window.location.pathname; // Returns path only
if (pathname.indexOf("TableroControl") > -1) {
    $.ajax({
        type: "POST",
        dataType: "json",
        async: true,
        url: urlEstadoActividades,
        //data: { tiendas: tiendas[indexTiendas], categorias: categorias[indexCategorias] },
        complete: function (e, xhr, settings) {
            estadoActividades();
        },
        success: function (data) {
            //console.log("response edo actividades->");
            //console.log(data);
            dataTab4 = data;
            //for (var i = 0; i < data.length; i++) {

                //dataTab4.push(
                //    {
                //        columna: data[i].Ingreso,
                //        valor: ""
                //    });

            //}

          


        }
    });
}

