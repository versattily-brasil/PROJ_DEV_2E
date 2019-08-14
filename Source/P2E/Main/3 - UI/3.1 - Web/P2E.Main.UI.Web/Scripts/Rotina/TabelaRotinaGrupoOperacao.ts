class TabelaRotinaGrupoOperacao {
    addClickEvent(): void {
        $(".btn-delete-srv").on("click", function () {
            $(this).closest("tr").remove();
        });
    }

    init(): void {

        this.addClickEvent();

        $(".btn-add-rotina-servico").on("click", function () {
            let comboServico: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboServico");         
            if (comboServico.selectedIndex == 0)
            {
                return false
            }
            let tabela: HTMLTableElement = <HTMLTableElement>document.getElementById("tabela_rotina_servico");
            let servico: HTMLOptionElement = comboServico.selectedOptions.item(0);
            
            let registroEncontrato: boolean = false
            $("#tabela_rotina_servico > tbody > tr").each(function () {
                var CD_SRV = $(this).data("cdsrv");
               
                if (CD_SRV == servico.value) {
                    registroEncontrato = true;
                }
            });
            if (registroEncontrato === false) {
                let row: string
                row = '<tr data-cdsrv="' + servico.value + '">';
                var cols = "";
                cols += '<td>' + servico.text + '</td>';
                cols += '<td class="text-center">';
                cols += '<a data-srv="' + servico.value + '">';
                cols += '<i style="font-weight:bold;cursor:pointer" class="fal fa-minus-circle text-danger btn-delete-srv"></i>';
                cols += '</a>';
                cols += '</td>';
                row += cols;
                row += "</tr>";
                $("#tabela_rotina_servico").children("tbody").append(row);
                comboServico.selectedIndex = 0;
                
                $(".bt-excluir").on("click", function () {
                    $(this).closest("tr").remove();
                });
            }
        });
    }
}

$(function () {
    var obj = new TabelaRotinaGrupoOperacao();
    obj.init();
});