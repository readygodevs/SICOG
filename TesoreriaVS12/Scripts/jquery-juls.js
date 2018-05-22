(function($){
	$.fn.extend({
		openModal: function(parameters){
			var URL = typeof parameters.url === "undefined" ? "" : parameters.url;
			var DATA = typeof parameters.data === "undefined" || parameters.data == {} ? DATA = {} : DATA = parameters.data;
			var METHOD = typeof parameters.method === "undefined" ? METHOD = "GET" : METHOD = parameters.method;
			var CALLBACK = parameters.callback;
			var SIZE = typeof parameters.size == "undefined" ? SIZE = "" : SIZE = parameters.size;
			var Elemet = this;
			$.ajax({
				type: METHOD,
				datatype: "html",
				url: URL,
				data: DATA,
				cache: false,
				statusCode:{
					200:function(response){
						if(typeof response === "string"){
							$(Elemet).find(".modal-body").empty();
		                	$(Elemet).find(".modal-body").html(response);
		                	if(SIZE != "")
		                		$(Elemet).find(".modal-dialog").addClass("modal-"+SIZE);
		                	abrirModal(Elemet.selector);						
		                }
						else
						{
							if(response.Exito == false)
								ErrorCustom(response.Mensaje,"");
						}
						if(typeof CALLBACK !== "undefined")
	                	{
		                	var call = $.Callbacks('memory once');
		                    call.add(CALLBACK);
		                    call.fire(CALLBACK, response);
	                	}						
					},
		            401: code400,
		            404: code404,
		            500: code500,
		            409: code409
				}
			});
		},
		toDate: function(){
			Number.prototype.padLeft = function (n, str) {
		        return Array(n - String(this).length + 1).join(str || '0') + this;
		    }
			var myDate = new Date(parseInt(new String($(this).val()).replace(/\/+Date\(([\d+-]+)\)\/+/, '$1')));
	        $(this).val((myDate.getDate().padLeft(2)) + "/" + (myDate.getMonth() + 1).padLeft(2) + "/" + myDate.getFullYear());
		},
		toDateTime: function(){
			Number.prototype.padLeft = function (n, str) {
		        return Array(n - String(this).length + 1).join(str || '0') + this;
		    }
			var myDate = new Date(parseInt(new String($(this).val()).replace(/\/+Date\(([\d+-]+)\)\/+/, '$1')));
	        $(this).val((myDate.getDate().padLeft(2)) + "/" + (myDate.getMonth() + 1).padLeft(2) + "/" + myDate.getFullYear()+" "+myDate.getHours()+":"+myDate.getMinutes());
		},
		ajaxLoad: function(parameters){
			var URL = typeof parameters.url === "undefined" ? "" : parameters.url;
			var DATA = typeof parameters.data === "undefined" || parameters.data == {} ? DATA = {} : DATA = parameters.data;
			var METHOD = typeof parameters.method === "undefined" ? METHOD = "GET" : METHOD = parameters.method;
			var CALLBACK = parameters.callback;
			var Elemet = this;
			$(Elemet).isLoading({ position: "overlay" });
			$.ajax({
				type: METHOD,
				datatype: "html",
				url: URL,
				data: DATA,
				cache: false,
				statusCode:{
					200:function(response){
						$(Elemet).empty();
		                $(Elemet).html(response);
		                if(typeof CALLBACK !== "undefined"){
		                	var call = $.Callbacks('memory once');
		                    call.add(CALLBACK);
		                    call.fire(CALLBACK, response);
		                }
		                $(Elemet).isLoading("hide");
					},
		            401: code400,
		            404: code404,
		            500: code500,
		            409: code409
				}
			});
		},
		focusOut: function(parameters){
			var CAMPOS = typeof parameters.campos === "undefined" ? {} : parameters.campos;
			var URL = typeof parameters.url === "undefined" ? "" : parameters.url;
			var DATA = typeof parameters.data === "undefined" || parameters.data == {} ? DATA = {} : DATA = parameters.data;
			var CALLBACK = parameters.callback;
			ajaxJson(URL,DATA,"POST",true,function(response){
				$.each(CAMPOS, function (key, value) {
			        var valor = "response." + value.Base;
		        	$("#"+value.Campo).val(eval(valor));
		    	});
				if(typeof CALLBACK !== "undefined"){
	            	var call = $.Callbacks('memory once');
	                call.add(CALLBACK(response));
	                call.fire();
	            }
			});
		},
		isSiblingDisabled: function(){
		    return ($(this).siblings().is(":disabled") || $(this).siblings().is("[readonly]"));
		},
		isDisabled: function () {
		    return ($(this).is(":disabled") || $(this).is("[readonly]"));
		}
	});
})(jQuery)