class TabelaParceiroNegocioServicoModulo {
    init(): void {

        $(".bt-excluir").on("click", function () {
            $(this).closest("tr").remove();
        });

        $(".btn-add-parceiro-servico-usuario").on("click", function () {
            let comboModulo: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboModulo");
            let comboServico: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboServico");
            if (comboModulo.selectedIndex == 0 || comboServico.selectedIndex == 0) {
                return false
            }
            //let tabela: HTMLTableElement = <HTMLTableElement>document.getElementById("tabela_servico_modulo");
            let modulo: HTMLOptionElement = comboModulo.selectedOptions.item(0);
            let servico: HTMLOptionElement = comboServico.selectedOptions.item(0);
            let registroEncontrato: boolean = false;

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
                cols += '<a data-mod="' + modulo.value + '" data-srv="' + servico.value + '">';
                cols += '<i style="font-weight:bold;cursor:pointer" class="fal fa-minus-circle text-danger bt-excluir"></i>';
                cols += '</td>';
                row += cols;
                row += "</tr>";
                $("#tabela_servico_modulo").children("tbody").append(row);
                comboModulo.selectedIndex = 0;
                comboServico.selectedIndex = 0;
            }
        });
    }

    static sortTable(name) {

        var seletor = '#' + name + " tbody  tr";
        var rows = $(seletor).get();
        console.log(rows);
        rows.sort(function (a, b) {

            var A = $(a).children('td').eq(0).text().toUpperCase();
            var B = $(b).children('td').eq(0).text().toUpperCase();

            if (A < B) {
                return -1;
            }

            if (A > B) {
                return 1;
            }

            return 0;

        });

        $.each(rows, function (index, row) {
            $('#' + name).children('tbody').append(row);
        });
    }
}

$(function () {
    var obj = new TabelaParceiroNegocioServicoModulo();
    obj.init();
});