var RotinaGrupoOperacao = /** @class */ (function () {
    function RotinaGrupoOperacao() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
        this.btnConfirmarSalvar = $("#confirm-save");
    }
    RotinaGrupoOperacao.prototype.init = function () {
        var _this = this;
        this.btnConfirmarSalvar.on("click", function (e) {
            _this.form.submit();
        });
        this.btnSalvar.on("click", function (e) {
            var agrupamento = "RotinasAssociadas";
            var g = 0;
            $("#tabela_rotina_associada > tbody > tr").each(function () {
                alert();
                console.log(this);
                var ROT_ASS = $(this).data("cd-rot-ass");
                if (ROT_ASS != undefined) {
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_ROT_ASS' value= '" + ROT_ASS + "' > ");
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