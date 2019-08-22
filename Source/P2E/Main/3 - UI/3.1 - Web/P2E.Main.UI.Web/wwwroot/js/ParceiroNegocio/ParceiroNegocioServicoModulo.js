var ParceiroNegocioServicoModulo = /** @class */ (function () {
    function ParceiroNegocioServicoModulo() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
        this.btnConfirmarSalvar = $("#confirm-save");
        this.txCnpj = $("#txt-cnpj");
    }
    ParceiroNegocioServicoModulo.prototype.init = function () {
        var _this = this;
        this.txCnpj.focusout(function () {
            var texto = _this.MascaraCnpj(_this.txCnpj.val().toString());
            _this.txCnpj.val(texto);
        });
        this.btnConfirmarSalvar.on("click", function (e) {
            _this.form.submit();
        });
        this.btnSalvar.on("click", function (e) {
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
    };
    ParceiroNegocioServicoModulo.prototype.MascaraCnpj = function (cnpj) {
        return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, "\$1.\$2.\$3\/\$4\-\$5");
    };
    ParceiroNegocioServicoModulo.prototype.MascaraCpf = function (cpf) {
        return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3\-\$4");
    };
    return ParceiroNegocioServicoModulo;
}());
$(function () {
    var obj = new ParceiroNegocioServicoModulo();
    obj.init();
});
//# sourceMappingURL=ParceiroNegocioServicoModulo.js.map