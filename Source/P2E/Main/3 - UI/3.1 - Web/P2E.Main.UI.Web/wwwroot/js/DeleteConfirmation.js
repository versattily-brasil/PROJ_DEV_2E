$((function () {
    var url;
    var redirectUrl;
    var target;

    $('body').append(`
 <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
	<div class="modal-dialog" role="document">
		<div class="modal-content">
			<div class="modal-header">
				<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
			</div>
			<div class="modal-body delete-modal-body">

			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-default" data-dismiss="modal" id="cancel-delete">Cancelar</button>
				<button type="button" class="btn btn-danger" id="confirm-delete">Confirmar</button>
			</div>
		</div>
	</div>
</div>

<div class="modal fade" id="deleteModalConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
	<div class="modal-dialog" role="document">
		<div class="modal-content alert alert-danger">
			<div class="modal-header">
				<h5 class="modal-title font-weight-bold align-middle">Confirma a exclusão do Registro?</h5>
				<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
			</div>
			<div class="modal-body font-weight-bold align-middle">
				<p>Se confirmar a exclusão, o registro será definitivamente removido do banco de dados!</p>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-default" data-dismiss="modal" id="cancel-delete">Cancelar</button>
				<button type="button" class="btn btn-danger" id="confirm-delete-modal">Confirmar</button>
			</div>
		</div>
	</div>
</div>`);

    //Delete Action
    $(".delete").on('click', (e) => {
        e.preventDefault();

        target = e.target;
        var Id = $(target).data('id');
        var controller = $(target).data('controller');
        var action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        redirectUrl = $(target).data('redirect-url');

        url = "/" + controller + "/" + action + "?Id=" + Id;
        $(".delete-modal-body").text(bodyMessage);
        $("#deleteModal").modal('show');
    });

    $("#confirm-delete").on('click', () => {
        
        $("#deleteModal").modal('hide');
       
        $("#deleteModalConfirm").modal('show');
    });

    $("#confirm-delete-modal").on('click', () => {
        window.location.href = url;

    });

}()));