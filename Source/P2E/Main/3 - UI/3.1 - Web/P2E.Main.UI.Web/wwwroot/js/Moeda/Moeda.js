var Moeda = /** @class */ (function () {
    function Moeda() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
    }
    Moeda.prototype.init = function () {
        this.btnSalvar.on("click", function (e) {
            //var a = confirm("Tem certeza que deseja salvar as alterações?");
            //if (!a) {
            //    return false;
            //}
            //e.preventDefault();
            //this.form.submit();
        });
    };
    return Moeda;
}());
$(function () {
    var obj = new Moeda();
    obj.init();
});
//# sourceMappingURL=Moeda.js.map