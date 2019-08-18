class ParceiroNegocioServicoModulo {

    form = $("#form");
    btnSalvar = $("#btnSalvar");
    btnConfirmarSalvar = $("#confirm-delete");

    init(): void {

        this.btnConfirmarSalvar.on("click", (e) => {
            this.form.submit();
        });

        this.btnSalvar.on("click", (e) => {

            //var a = confirm("Tem certeza que deseja salvar as alterações?");
            //if (!a) {
            //    return false;
            //}
            //e.preventDefault();

            var agrupamento = "ParceiroNegocioServicoModulo";
            var g = 0;

            $("#tabela_servico_modulo > tbody > tr").each(function () {

                var CD_MOD = $(this).data("cdmod");
                var CD_SRV = $(this).data("cdsrv");

                if (CD_MOD != undefined && CD_SRV != undefined) {
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_MOD' value= '" + CD_MOD + "' > ");
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_SRV' value= '" + CD_SRV + "' > ");
                }
                g += 1;
            });


            //this.form.submit();
        });
    }
}

$(function () {
    var obj = new ParceiroNegocioServicoModulo();
    obj.init();
});
