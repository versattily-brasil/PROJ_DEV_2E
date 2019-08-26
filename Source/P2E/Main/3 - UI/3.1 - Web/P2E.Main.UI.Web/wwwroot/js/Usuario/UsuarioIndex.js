var UsuarioIndex = /** @class */ (function () {
    function UsuarioIndex() {
    }
    UsuarioIndex.prototype.init = function () {
        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            var frm = document.getElementById("formLista");
            frm.submit();
        });
    };
    return UsuarioIndex;
}());
$(function () {
    var obj = new UsuarioIndex();
    obj.init();
});
//# sourceMappingURL=UsuarioIndex.js.map