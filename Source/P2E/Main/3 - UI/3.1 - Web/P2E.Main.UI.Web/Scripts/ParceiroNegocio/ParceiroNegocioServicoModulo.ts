class ParceiroNegocioServicoModulo {

    form = $("#form");
    btnSalvar = $("#btnSalvar");
    btnConfirmarSalvar = $("#confirm-save");
    txCnpj = $("#txt-cnpj");

    init(): void {

        this.txCnpj.focusout(() => {

            var texto: string = this.MascaraCnpj(this.txCnpj.val().toString());

            this.txCnpj.val(texto);

        });

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

    MascaraCnpj(cnpj: string): string {
        return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, "\$1.\$2.\$3\/\$4\-\$5");
    }

    MascaraCpf(cpf: string): string {
        return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3\-\$4");
    }
}

$(function () {
    var obj = new ParceiroNegocioServicoModulo();
    obj.init();
});
