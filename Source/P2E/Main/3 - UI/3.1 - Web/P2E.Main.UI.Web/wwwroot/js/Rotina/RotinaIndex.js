var RotinaIndex = /** @class */ (function () {
    function RotinaIndex() {
    }
    RotinaIndex.prototype.init = function () {
        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            var frm = document.getElementById("formLista");
            frm.submit();
        });
    };
    return RotinaIndex;
}());
$(function () {
    var obj = new RotinaIndex();
    obj.init();
});
//# sourceMappingURL=RotinaIndex.js.map