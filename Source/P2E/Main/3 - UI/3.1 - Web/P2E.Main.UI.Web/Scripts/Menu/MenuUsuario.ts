class MenuUsuario {
    init(): void {
        $(".link_associados").on("click", function (e) {
            let link: HTMLElement = <HTMLElement>(this);
            console.log(link);
        });
    }
}

$(function () {
    var obj = new MenuUsuario();
    obj.init();
});

