var RotinaGrupoOperacao = /** @class */ (function () {
    function RotinaGrupoOperacao() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
    }
    RotinaGrupoOperacao.prototype.init = function () {
        var _this = this;
        this.btnSalvar.on("click", function () {
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
            _this.form.submit();
        });
    };
    return RotinaGrupoOperacao;
}());
$(function () {
    var obj = new RotinaGrupoOperacao();
    obj.init();
});
//# sourceMappingURL=RotinaGrupoOperacao.js.map