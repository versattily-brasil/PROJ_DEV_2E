var GrupoIndex = /** @class */ (function () {
    function GrupoIndex() {
    }
    GrupoIndex.prototype.init = function () {
        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            var frm = document.getElementById("formLista");
            frm.submit();
        });
    };
    return GrupoIndex;
}());
$(function () {
    var obj = new GrupoIndex();
    obj.init();
});
//# sourceMappingURL=GrupoIndex.js.map