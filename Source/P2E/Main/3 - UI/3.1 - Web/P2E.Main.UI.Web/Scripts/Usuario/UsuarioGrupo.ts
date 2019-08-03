class UsuarioGrupo {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {



        this.btnSalvar.on("click", () => {

            var listaGrupo = "UsuarioGrupo";
            var qtd = 0;

            $("#table-destino-grp > tbody > tr").each(function () {

                var CD_GRP = $(this).data("grp");
                $("#form").prepend("<input type='hidden' name= '" + listaGrupo + "[" + qtd + "].CD_GRP' value= '" + CD_GRP + "' > ");

                qtd += 1;
            });

            var listaModulo = "UsuarioModulos";
            var qtd = 0;

            $("#table-destino > tbody > tr").each(function () {

                var CD_MOD = $(this).data("mod");
                $("#form").prepend("<input type='hidden' name= '" + listaModulo + "[" + qtd + "].CD_MOD' value= '" + CD_MOD + "' > ");

                qtd += 1;
            });

            console.log(qtd);
            this.form.submit();



            //console.log(qtd);
            //this.form.submit();
        });



    }
}

$(function () {
    var obj = new UsuarioGrupo();
    obj.init();
});
