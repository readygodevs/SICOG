
function obtenerGrafica() {
    Highcharts.chart('tab1container', {
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: catTab1
        },
        yAxis: {
            min: 0,
            title: {
                text: ''
            },
            reversedStacks: false
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
            shared: true
        },
        plotOptions: {
            column: {
                stacking: 'quantity'
            }
        },
        series: dataTab1
    });
}

var total_ejecutado = 0, total_por_ejecutar = 0, total = 0;

function llenarTab2() {
    $.each(dataTab2, function (i, item) {
        total_ejecutado += item.ejecutado;
        total_por_ejecutar += item.por_ejecutar;
        total += item.total;

        $('.table2 tbody').append('<tr><td>' + item.mes + '</td><td>' + currency(item.ejecutado).format() + '</td><td>' + currency(item.por_ejecutar).format() + '</td><td>' + currency(item.total).format() + '</td></tr>');
    })
    $('.table2 tbody').append('<tr><td class="dark">Total</td><td class="dark">' + currency(total_ejecutado).format() + '</td><td class="dark">' + currency(total_por_ejecutar).format() + '</td><td class="dark">' + currency(total).format() + '</td></tr>');

}


function llenarTab3() {
    //$.each(dataTab3, function (i, item) {
    $('.table3 tbody').append('<tr><td style="border:solid 1px lightgray">' + currency(dataTab3.ingresos).format() + '</td><td style="border:solid 1px lightgray">' + currency(dataTab3.descuentos).format() + '</td><td style="border:solid 1px lightgray">' + currency(dataTab3.descuentos_no_autorizado).format() + '</td></tr>');
    //})
}

function llenarTab01() {
    $.each(dataTab01, function (i, item) {
        $('.table01 tbody').append('<tr><td style="border:solid 1px lightgray">' + item.nombre + '</td><td style="border:solid 1px lightgray">' + item.pendientes + '</td><td  style="border:solid 1px lightgray">' + item.pagados + '</td><td style="border:solid 1px lightgray">' + currency(item.total).format() + '</td></tr>');
    })
}
function llenarTab02() {
    $.each(dataTab02, function (i, item) {
        $('.table02 tbody').append('<tr><td style="border:solid 1px lightgray">' + item.tipo + '</td><td style="border:solid 1px lightgray">' + item.pendientes + '</td><td style="border:solid 1px lightgray">' + item.pagados + '</td><td style="border:solid 1px lightgray">' + currency(item.total).format() + '</td></tr>');
    })
}

function estadoActividades() {
    $('.table4 thead').append('<tr><th>DESCRIPCION</th><th>' + dataTab4[0].ejercicio1 + '</th><th>' + dataTab4[0].ejercicio2 + '</th></tr>');
    

    $.each(dataTab4, function (i, item) {
        if (i > 0) {
            if (i >= Math.floor((dataTab4.length / 2))) {
                //$('.table4 tbody').append('<tr class="oculto hidden"><td>' + item.columna + '</td><td>' + currency(item.valor).format() + '</td></tr>');
                if (item.agrupador == "1") {
                    $('.table4 tbody').append('<tr style="background-color: lightgray;" class="oculto hidden"><td colspan="3">' + item.texto + '</td></tr>');
                } else {
                    $('.table4 tbody').append('<tr class="oculto hidden"><td>' + item.texto + '</td><td>' + currency(item.valor1).format() + '</td><td>' + currency(item.valor2).format() + '</td></tr>');
                }
            } else {
                if (item.agrupador == "1") {
                    $('.table4 tbody').append('<tr style="background-color: lightgray;"><td colspan="3">' + item.texto + '</td></tr>');
                } else {
                    $('.table4 tbody').append('<tr><td>' + item.texto + '</td><td>' + currency(item.valor1).format() + '</td><td>' + currency(item.valor2).format() + '</td></tr>');
                }
            }
        }

    })
}

$.each(dataTab5, function (i, item){
	if( i >= Math.floor((dataTab5.length / 2))){
      $('.table5 tbody').append('<tr class="oculto hidden"><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
	}else{
      $('.table5 tbody').append('<tr><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
	}
	
})

$.each(dataTab5a, function (i, item){
	if( i >= Math.floor((dataTab5a.length / 2))){
      $('.table5a tbody').append('<tr class="oculto hidden"><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
	}else{
      $('.table5a tbody').append('<tr><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
	}
	
})

$.each(dataTab5b, function (i, item){
    if( i >= Math.floor((dataTab5a.length / 2))){
    	$('.table5b tbody').append('<tr class="oculto hidden"><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
    }else{
        $('.table5b tbody').append('<tr><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
    }
	
})

$.each(dataTab6, function(i,item){
	if( i >= Math.floor((dataTab6.length / 2))){
       $('.table6 tbody').append('<tr class="oculto hidden"><td><b>'+item.nombre+'</b></td><td></td><td></td></tr>');
       $.each(item.data, function(i,itex){
       	$('.table6 tbody').append('<tr class="oculto hidden"><td>'+itex.nombre+'</td><td>'+itex.x+'</td><td>'+itex.y+'</td></tr>');
       })
	}else{
       $('.table6 tbody').append('<tr><td><b>'+item.nombre+'</b></td><td></td><td></td></tr>');
       $.each(item.data, function(i,itex){
       	$('.table6 tbody').append('<tr><td>'+itex.nombre+'</td><td>'+itex.x+'</td><td>'+itex.y+'</td></tr>');
       })
	}
	
})

$.each(dataTab7a, function(i,item){
	if( i >= Math.floor((dataTab7a.length / 2))){
       $('.table7a tbody').append('<tr class="oculto hidden"><td><b>'+item.columna+'</b></td><td></td></tr>');
	}else{
       $('.table7a tbody').append('<tr><td><b>'+item.columna+'</b></td><td></td></tr>');
	}
	
})

$.each(dataTab7b, function(i,item){
	if( i >= Math.floor((dataTab7b.length / 2))){
       $('.table7b tbody').append('<tr class="oculto hidden"><td><b>'+item.columna+'</b></td><td></td></tr>');
	}else{
       $('.table7b tbody').append('<tr><td><b>'+item.columna+'</b></td><td></td></tr>');
	}
	
})

$.each(dataTab8, function (i, item){
	if( i >= Math.floor((dataTab8.length / 2))){
    	$('.table8 tbody').append('<tr class="oculto hidden"><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
    }else{
        $('.table8 tbody').append('<tr><td>'+item.columna+'</td><td>'+currency(item.valor).format()+'</td></tr>');
    }
	
})

$(document).on('click', '.first-tabs ul li img, .second-tabs ul li img', function(){
	window.location = $(this).data('link');
})

$(document).on('click', '.shRow', function(){
	$(this).closest('.tab-pane').find('.table tbody tr.oculto').toggleClass('hidden');
})