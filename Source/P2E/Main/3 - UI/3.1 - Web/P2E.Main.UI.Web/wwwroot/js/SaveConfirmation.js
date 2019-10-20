$((function () {
    var url;
    var redirectUrl;
    var target;

    $('body').append(`
                    <div class="modal fade" id="saveModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body save-modal-body">
                            
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal" id="cancel-save">Cancelar</button>
                            <button type="button" class="btn btn-danger" id="confirm-save">Confirmar</button>
                            <button type="button" class="btn btn-secondary" id="confirm-import">Confirmar</button>
                        </div>
                        </div>
                    </div>
                    </div>`);

    //save Action
    $(".save").on('click', (e) => {
        e.preventDefault();

        target = e.target;
        var Id = $(target).data('id');
        var controller = $(target).data('controller');
        var action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        redirectUrl = $(target).data('redirect-url');

        url = "/" + controller + "/" + action + "?Id=" + Id;
        $(".save-modal-body").text(bodyMessage);
        $("#saveModal").modal('show');
    });

    $("#confirm-save").on('click', () => {
        window.location.href = url;
    });

}()));