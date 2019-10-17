var MenuUsuario = /** @class */ (function () {
    function MenuUsuario() {
    }
    MenuUsuario.prototype.init = function () {
        $(".link_associados").on("click", function (e) {
            var json = $(this).data("associados");
            if (json != "") {
                console.log('Associados:    ' + json);
                var associados = json;
                if (associados != undefined) {
                    for (var i = 0; i < associados.length; i++) {
                        var link = (document.createElement('link'));
                        link.href = associados[i].Href;
                        link.title = associados[i].Title;
                        link.target = associados[i].Title;
                        var janela = window.open(link.href, link.target);
                    }
                }
            }
            else {
                console.log("menu sem associados.");
            }
        });
    };
    return MenuUsuario;
}());
$(function () {
    var obj = new MenuUsuario();
    obj.init();
});
//# sourceMappingURL=MenuUsuario.js.map