class MenuUsuario {
    init(): void {
        $(".linkMenuUsuario").on("click", function () {
            alert();
            let menu: HTMLSelectElement = <HTMLSelectElement>this;
            console.log(menu);
        });

    }
}

$(function () {
    var obj = new MenuUsuario();
    obj.init();
});