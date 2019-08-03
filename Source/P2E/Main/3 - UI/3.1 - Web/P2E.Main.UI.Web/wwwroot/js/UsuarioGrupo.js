var UsuarioGrupo = /** @class */ (function () {
    function UsuarioGrupo() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
    }
    UsuarioGrupo.prototype.init = function () {
        var _this = this;
        this.btnSalvar.on("click", function () {
            var listaGrupo = "UsuarioGrupo";
            var qtd = 0;
            $("#table-destino-grp > tbody > tr").each(function () {
                var CD_GRP = $(this).data("grp");
                $("#form").prepend("<input type='hidden' name= '" + listaGrupo + "[" + qtd + "].CD_GRP' value= '" + CD_GRP + "' > ");
                qtd += 1;
            });
            var listaModulo = "UsuarioModulos";
            var qtd = 0;
            $("#table-destino > tbody > tr").each(function () {
                var CD_MOD = $(this).data("mod");
                $("#form").prepend("<input type='hidden' name= '" + listaModulo + "[" + qtd + "].CD_MOD' value= '" + CD_MOD + "' > ");
                qtd += 1;
            });
            console.log(qtd);
            _this.form.submit();
            //console.log(qtd);
            //this.form.submit();
        });
    };
    return UsuarioGrupo;
}());
$(function () {
    var obj = new UsuarioGrupo();
    obj.init();
});
//# sourceMappingURL=UsuarioGrupo.js.map