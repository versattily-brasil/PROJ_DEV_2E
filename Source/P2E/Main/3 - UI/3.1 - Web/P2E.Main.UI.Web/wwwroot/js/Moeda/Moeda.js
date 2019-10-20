var Moeda = /** @class */ (function () {
    function Moeda() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
        this.btnConfirmarSalvar = $("#confirm-save");
        this.btnConfirmarImport = $("#confirm-import");
    }
    Moeda.prototype.init = function () {
        var _this = this;
        this.btnConfirmarSalvar.on("click", function (e) {
            _this.form.submit();
        });
        this.btnConfirmarImport.on("click", function (e) {
            _this.form.submit();
        });
        this.btnSalvar.on("click", function (e) {
            //var a = confirm("Tem certeza que deseja salvar as alterações?");
            //if (!a) {
            //    return false;
            //}
            //e.preventDefault();
            // this.form.submit();
        });
    };
    return Moeda;
}());
$(function () {
    var obj = new Moeda();
    obj.init();
});
//# sourceMappingURL=Moeda.js.map