var UsuarioCadastro = /** @class */ (function () {
    function UsuarioCadastro() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
    }
    UsuarioCadastro.prototype.init = function () {
        var _this = this;
        this.btnSalvar.on("click", function () {
            var listaGrupos = "UsuarioGrupo";
            var g = 0;
            $("#table-destino-grp > tbody > tr").each(function () {
                var CD_GRP = $(this).data("grp");
                $("#form").append("<input type='hidden' name= '" + listaGrupos + "[" + g + "].CD_GRP' value= '" + CD_GRP + "' > ");
                g += 1;
            });
            var listaModulos = "UsuarioModulo";
            var m = 0;
            $("#table-destino-md > tbody > tr").each(function () {
                var CD_MOD = $(this).data("mod");
                $("#form").append("<input type='hidden' name= '" + listaModulos + "[" + m + "].CD_MOD' value= '" + CD_MOD + "' > ");
                m += 1;
            });
            _this.form.submit();
        });
    };
    return UsuarioCadastro;
}());
$(function () {
    var obj = new UsuarioCadastro();
    obj.init();
});
//# sourceMappingURL=UsuarioCadastro.js.map