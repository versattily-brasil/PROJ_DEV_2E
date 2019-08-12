class RotinaGrupoOperacao {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {
        this.btnSalvar.on("click", () => {

            var agrupamento = "RotinaGrupoOperacao";
            var g = 0;

            $("#tabela_rotina_grupo_operacao > tbody > tr").each(function () {

                var CD_GRP = $(this).data("cdgrp");
                var CD_OPR = $(this).data("cdopr");

                if (CD_GRP != undefined && CD_OPR != undefined) {
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_GRP' value= '" + CD_GRP + "' > ");
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_OPR' value= '" + CD_OPR + "' > ");
                }
                g += 1;
            });


            this.form.submit();
        });
    }
}

$(function () {
    var obj = new RotinaGrupoOperacao();
    obj.init();
});
