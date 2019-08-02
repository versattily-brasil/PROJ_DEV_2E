var UsuarioCadastro = /** @class */ (function () {
    function UsuarioCadastro() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
    }
    UsuarioCadastro.prototype.init = function () {
        var _this = this;
        this.btnSalvar.on("click", function () {
            var listName = "UsuarioModulos";
            var qtd = 0;
            $("#table-destino > tbody > tr").each(function () {
                var CD_MOD = $(this).data("mod");
                $("#form").prepend("<input type='hidden' name= '" + listName + "[" + qtd + "].CD_MOD' value= '" + CD_MOD + "' > ");
                qtd += 1;
            });
            console.log(qtd);
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