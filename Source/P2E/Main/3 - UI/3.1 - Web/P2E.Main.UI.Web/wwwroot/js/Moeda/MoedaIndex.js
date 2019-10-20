var MoedaIndex = /** @class */ (function () {
    function MoedaIndex() {
    }
    MoedaIndex.prototype.init = function () {
        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            var frm = document.getElementById("formLista");
            frm.submit();
        });
    };
    return MoedaIndex;
}());
$(function () {
    var obj = new MoedaIndex();
    obj.init();
});
//# sourceMappingURL=MoedaIndex.js.map