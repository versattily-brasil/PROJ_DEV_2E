class MenuUsuario {
    init(): void {
        $(".link_associados").on("click", function (e) {

            var json = $(this).data("associados");

            if (json != "") {
                console.log('Associados:    ' + json);
                let associados = <ItemAssociado[]>json;

                if (associados != undefined) {

                    for (var i = 0; i < associados.length; i++) {
                        var link = <HTMLLinkElement>(document.createElement('link'));

                        link.href = associados[i].Href;
                        link.title = associados[i].Title;
                        link.target = associados[i].Title;
                        
                        var janela = window.open(link.href, link.target);
                    }
                }
            }
            else
            {
                console.log("menu sem associados.");
            }
        });
    }
}

$(function () {
    var obj = new MenuUsuario();
    obj.init();
});

interface ItemAssociado {
    Title: string;
    Text: string;
    Href: string;
}