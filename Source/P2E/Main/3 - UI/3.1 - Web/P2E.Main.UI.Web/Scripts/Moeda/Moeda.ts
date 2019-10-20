class Moeda {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {

        this.btnSalvar.on("click", (e) => {

            //var a = confirm("Tem certeza que deseja salvar as alterações?");
            //if (!a) {
            //    return false;
            //}
            //e.preventDefault();

            //this.form.submit();
        });
    }
}

$(function () {
    var obj = new Moeda();
    obj.init();
});
