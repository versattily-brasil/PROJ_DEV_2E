class TabelaParceiroNegocioServicoModulo {
    init(): void {

        $(".bt-excluir").on("click", function () {
            $(this).removeData("fa-minus-circle").removeClass("text-danger");
            var tr = $(this).closest("tr");
            $("#tabela_servico_modulo tbody").append(tr);
        });

        $(".btn-add-parceiro-servico-usuario").on("click", function () {
            let comboModulo: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboModulo");
            let comboServico: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboServico");
            if (comboModulo.selectedIndex == 0 || comboServico.selectedIndex == 0) {
                return false
            }
            let tabela: HTMLTableElement = <HTMLTableElement>document.getElementById("tabela_servico_modulo");
            let modulo: HTMLOptionElement = comboModulo.selectedOptions.item(0);
            let servico: HTMLOptionElement = comboServico.selectedOptions.item(0);
            let registroEncontrato: boolean = false
            $("#tabela_servico_modulo > tbody > tr").each(function () {
                var CD_MOD = $(this).data("cdmod");
                var CD_SRV = $(this).data("cdsrv");
                if (CD_MOD == modulo.value && CD_SRV == servico.value) {
                    registroEncontrato = true;
                }
            });
            if (registroEncontrato === false) {
                let row: string
                row = '<tr data-cdmod="' + modulo.value + '" data-cdsrv="' + servico.value + '">';
                var cols = "";
                cols += '<td>' + modulo.text + '</td>';
                cols += '<td>' + servico.text + '</td>';
                cols += '<td>';
                cols += '<a class="btn btn-success btn-m-l btn-icon bt-excluir" data-mod="' + modulo.value + '" data-srv="' + servico.value + '">';
                cols += '<i class="fal fa-minus-circle"></i>';
                cols += '</td>';
                row += cols;
                row += "</tr>";
                $("#tabela_servico_modulo").children("tbody").append(row);
                comboModulo.selectedIndex = 0;
                comboServico.selectedIndex = 0;
            }
        });
    }
}

$(function () {
    var obj = new TabelaParceiroNegocioServicoModulo();
    obj.init();
});