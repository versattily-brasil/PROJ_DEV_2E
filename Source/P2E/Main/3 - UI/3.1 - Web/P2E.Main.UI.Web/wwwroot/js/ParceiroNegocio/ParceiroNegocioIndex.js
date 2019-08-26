var ParceiroNegocioIndex = /** @class */ (function () {
    function ParceiroNegocioIndex() {
    }
    ParceiroNegocioIndex.prototype.init = function () {
        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            var frm = document.getElementById("formLista");
            frm.submit();
        });
    };
    return ParceiroNegocioIndex;
}());
$(function () {
    var obj = new ParceiroNegocioIndex();
    obj.init();
});
//# sourceMappingURL=ParceiroNegocioIndex.js.map