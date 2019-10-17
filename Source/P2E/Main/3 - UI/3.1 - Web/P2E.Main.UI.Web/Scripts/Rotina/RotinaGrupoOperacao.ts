class RotinaGrupoOperacao {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    btnConfirmarSalvar = $("#confirm-save");

    init(): void {        

        this.btnConfirmarSalvar.on("click", (e) => {
            this.form.submit();
        });

        this.btnSalvar.on("click", (e) => {           

            var agrupamento = "RotinasAssociadas";
            var g = 0;

            $("#tabela_rotina_associada > tbody > tr").each(function () {
                    console.log(this);
                var ROT_ASS = $(this).data("cd-rot-ass");

                if (ROT_ASS != undefined) {
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_ROT_ASS' value= '" + ROT_ASS + "' > ");
                }
                g += 1;
            });


            //this.form.submit();
        });
    }
}

$(function () {
    var obj = new RotinaGrupoOperacao();
    obj.init();
});
