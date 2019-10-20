class Moeda {

    form = $("#form");
    btnSalvar = $("#btnSalvar");
    btnConfirmarSalvar = $("#confirm-save");
    btnConfirmarImport = $("#confirm-import");

    init(): void {

        this.btnConfirmarSalvar.on("click", (e) => {
            this.form.submit();
        });

        this.btnConfirmarImport.on("click", (e) => {
            this.form.submit();
        });

        this.btnSalvar.on("click", (e) => {

            //var a = confirm("Tem certeza que deseja salvar as alterações?");
            //if (!a) {
            //    return false;
            //}
            //e.preventDefault();

           // this.form.submit();
        });
    }
}

$(function () {
    var obj = new Moeda();
    obj.init();
});
