$((function () {
    var url;
    var redirectUrl;
    var target;

    $('body').append(`
 <div class="modal fade" id="cancelModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
	<div class="modal-dialog" role="document">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
			</div>
			<div class="modal-body cancel-modal-body">

			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-default" data-dismiss="modal" id="cancel-cancel">Cancelar</button>
				<button type="button" class="btn btn-danger" id="confirm-cancel">Confirmar</button>
			</div>
		</div>
	</div>
</div>`);

    //Delete Action
    $(".cancel").on('click', (e) => {
        e.preventDefault();

        target = e.target;
        var Id = $(target).data('id');
        var controller = $(target).data('controller');
        var action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        redirectUrl = $(target).data('redirect-url');

        url = "/" + controller + "/" + action + "?Id=" + Id;
        $(".cancel-modal-body").text(bodyMessage);
        $("#cancelModal").modal('show');
    });

    $("#confirm-cancel").on('click', () => {

        window.location.href = url;
    });
}()));