﻿class ParceiroNegocioServicoModulo {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {
        this.btnSalvar.on("click", () => {

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


            this.form.submit();
        });
    }
}

$(function () {
    var obj = new ParceiroNegocioServicoModulo();
    obj.init();
});