class RotinaGrupoOperacao {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {        

        //$("#cancel-delete").click(function (e) {
        //    e.preventDefault();
        //});



        this.btnSalvar.on("click", (e) => {           

            var a = confirm("Tem certeza que deseja salvar as alterações?");
            if (!a) {
                return false;
            }
            e.preventDefault();

            var agrupamento = "RotinaServico";
            var g = 0;

            $("#tabela_rotina_servico > tbody > tr").each(function () {

                var CD_SRV = $(this).data("cdsrv");

                if (CD_SRV != undefined) {
                    $("#form").append("<input type='hidden' name= '" + agrupamento + "[" + g + "].CD_SRV' value= '" + CD_SRV + "' > ");
                }
                g += 1;
            });


            this.form.submit();
        });
    }
}

$(function () {
    var obj = new RotinaGrupoOperacao();
    obj.init();
});
