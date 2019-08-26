var ServicoIndex = /** @class */ (function () {
    function ServicoIndex() {
    }
    ServicoIndex.prototype.init = function () {
        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            var frm = document.getElementById("formLista");
            frm.submit();
        });
    };
    return ServicoIndex;
}());
$(function () {
    var obj = new ServicoIndex();
    obj.init();
});
//# sourceMappingURL=ServicoIndex.js.map