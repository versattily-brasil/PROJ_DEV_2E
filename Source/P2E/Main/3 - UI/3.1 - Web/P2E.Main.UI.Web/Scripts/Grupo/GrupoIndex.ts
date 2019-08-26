class GrupoIndex {

    init(): void {

        $("#comboQtdRegistro").on("change", function () {
            //$(this).submit();
            let frm: HTMLFormElement = <HTMLFormElement>document.getElementById("formLista");
            frm.submit();
            
        });
    }
}

$(function () {
    var obj = new GrupoIndex();
    obj.init();
});
