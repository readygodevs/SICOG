$(document).ready(function () {
    //console.log("utilerias modal");
    $(document).on('shown.bs.modal', function (event) {
        setTimeout(function () {

            $(".modal-body :input:enabled:visible:first").focus().select();

            $(this).on("keydown", keydownfunc);
        }, 500);
    });

    $(document).on('hidden.bs.modal', function () {
        console.log("limpiar shown");
        $(document).off('shown.bs.modal');

        $(document).on('shown.bs.modal', function (event) {
            setTimeout(function () {
                $(".modal-body :input:enabled:visible:first").focus().select();
            }, 500);
        });
    });

});

function keydownfunc(e) {
    //console.log(e.keyCode);
    if (e.keyCode == 13) {
        console.log("clicik");
        $(".modal-footer").find(".btn-success").trigger("click");


    }
    else if (e.keyCode == 27) {
        $(".modal").modal("hide");
    }
}