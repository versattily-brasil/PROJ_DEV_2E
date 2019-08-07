var ParceiroNegocioServicoModulo = /** @class */ (function () {
    function ParceiroNegocioServicoModulo() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
    }
    ParceiroNegocioServicoModulo.prototype.init = function () {
        var _this = this;
        this.btnSalvar.on("click", function () {
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
            _this.form.submit();
        });
    };
    return ParceiroNegocioServicoModulo;
}());
$(function () {
    var obj = new ParceiroNegocioServicoModulo();
    obj.init();
});
//# sourceMappingURL=ParceiroNegocioServicoModulo.js.map