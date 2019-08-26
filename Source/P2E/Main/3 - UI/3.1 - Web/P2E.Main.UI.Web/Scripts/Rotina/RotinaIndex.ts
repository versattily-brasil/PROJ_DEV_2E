class RotinaIndex {

    init(): void {

        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            let frm: HTMLFormElement = <HTMLFormElement>document.getElementById("formLista");
            frm.submit();

        });
    }
}

$(function () {
    var obj = new RotinaIndex();
    obj.init();
});
