var RotinaGrupoOperacao = /** @class */ (function () {
    function RotinaGrupoOperacao() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
        this.btnConfirmarSalvar = $("#confirm-delete");
    }
    RotinaGrupoOperacao.prototype.init = function () {
        var _this = this;
        this.btnConfirmarSalvar.on("click", function (e) {
            _this.form.submit();
        });
        this.btnSalvar.on("click", function (e) {
            var agrupamento = "RotinaServico";
            var g = 0;
            $("#tabela_rotina_servico > tbody > tr").each(function () {
                var CD_SRV = $(this).data("cdsrv");
                if (CD_SRV != undefined) {
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_SRV' value= '" + CD_SRV + "' > ");
                }
                g += 1;
            });
            //this.form.submit();
        });
    };
    return RotinaGrupoOperacao;
}());
$(function () {
    var obj = new RotinaGrupoOperacao();
    obj.init();
});
//# sourceMappingURL=RotinaGrupoOperacao.js.map