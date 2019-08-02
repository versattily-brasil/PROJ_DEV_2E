class UsuarioGrupo {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {



        this.btnSalvar.on("click", () => {

            var listName = "UsuarioGrupo";
            var qtd = 0;

            $("#table-destino-grp > tbody > tr").each(function () {

                var CD_GRP = $(this).data("grp");
                $("#form").prepend("<input type='hidden' name= '" + listName + "[" + qtd + "].CD_GRP' value= '" + CD_GRP + "' > ");

                qtd += 1;
            });

            console.log(qtd);
            this.form.submit();
        });


    }
}

$(function () {
    var obj = new UsuarioGrupo();
    obj.init();
});
