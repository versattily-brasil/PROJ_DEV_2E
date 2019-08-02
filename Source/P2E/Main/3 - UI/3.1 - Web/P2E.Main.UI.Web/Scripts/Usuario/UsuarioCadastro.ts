class UsuarioCadastro {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {



        this.btnSalvar.on("click", () => {

            var listName = "UsuarioModulos";
            var qtd = 0;

            $("#table-destino > tbody > tr").each(function () {

                var CD_MOD = $(this).data("mod");
                $("#form").prepend("<input type='hidden' name= '" + listName + "[" + qtd + "].CD_MOD' value= '" + CD_MOD + "' > ");
                
                qtd += 1;
            });

            console.log(qtd);
            this.form.submit();
        });


    }
}

$(function () {
    var obj = new UsuarioCadastro();
    obj.init();
});
