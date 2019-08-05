
class TabelaParceiroNegocioServicoModulo {

    init(): void {

        $(".btn-add-parceiro-servico-usuario").on("click", function () {

            let comboModulo: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboModulo");
            let comboServico: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboServico");

            let tabela: HTMLTableElement = <HTMLTableElement>document.getElementById("tabela_servico_modulo");

            let modulo: HTMLOptionElement = comboModulo.selectedOptions.item(0);

            let servico: HTMLOptionElement = comboServico.selectedOptions.item(0);

            let registroEncontrato: boolean = false

            if (tabela.rows.length > 0) {
                for (var i = 0; i < tabela.rows.length; i++) {
                    let row: HTMLTableRowElement = tabela.rows.item(i)

                    if (row != undefined) {
                        if (row.attributes.getNamedItem("idmodulo").value == modulo.value && row.attributes.getNamedItem("idservico").value == servico.value) {
                            registroEncontrato = true
                            break
                        }
                    }
                }
            }

            if (registroEncontrato === false) {
                let row: string

                let r = tabela.insertRow()

                let newRow: HTMLTableRowElement

                row = '<tr idmodulo="' + modulo.value + '" idservico="' + servico.value + '">';
                var cols = "";
                cols += '<td>' + modulo.text + '</td>';
                cols += '<td>' + servico.text + '</td>';
                cols += '<td>'; cols += '<button onclick="RemoveTableRow(this)" type="button">Remover</button>'; cols += '</td>';

                row += cols;
                row += "</tr>";
                
                $("#tabela_servico_modulo").children("tbody").append(row);
            }
        });
    }
}

$(function () {
    var obj = new TabelaParceiroNegocioServicoModulo();
    obj.init();
});