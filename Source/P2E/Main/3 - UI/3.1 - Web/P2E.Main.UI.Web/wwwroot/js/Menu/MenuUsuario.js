var MenuUsuario = /** @class */ (function () {
    function MenuUsuario() {
    }
    MenuUsuario.prototype.init = function () {
        $(".link_associados").on("click", function (e) {
            var link = (this);
            console.log(link);
        });
    };
    return MenuUsuario;
}());
$(function () {
    var obj = new MenuUsuario();
    obj.init();
});
//# sourceMappingURL=MenuUsuario.js.map