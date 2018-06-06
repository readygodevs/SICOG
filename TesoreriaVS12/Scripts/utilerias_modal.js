$(document).ready(function () {
    console.log("utilerias modal");
    $(document).on('shown.bs.modal', function () {
		setTimeout(function(){
		    $(".modal-body :input:enabled:visible:first").focus().select();
			
			$(this).keydown(function(e){
				console.log(e.keyCode);
				if(e.keyCode==13){
					$(".modal-footer").find(".btn-success").trigger("click");
				}
				else if (e.keyCode == 27) {
				    $(".modal").modal("hide");
				}
			});
		},500);
	});


});
	