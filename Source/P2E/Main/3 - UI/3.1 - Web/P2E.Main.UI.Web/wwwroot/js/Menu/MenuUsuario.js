var MenuUsuario = /** @class */ (function () {
    function MenuUsuario() {
    }
    MenuUsuario.prototype.init = function () {
        $(".linkMenuUsuario").on("click", function () {
            alert();
            var menu = this;
            console.log(menu);
        });
    };
    return MenuUsuario;
}());
$(function () {
    var obj = new MenuUsuario();
    obj.init();
});
//# sourceMappingURL=MenuUsuario.js.map