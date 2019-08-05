var TabelaParceiroNegocioServicoModulo = /** @class */ (function () {
    function TabelaParceiroNegocioServicoModulo() {
    }
    TabelaParceiroNegocioServicoModulo.prototype.init = function () {
        $(".btn-add-parceiro-servico-usuario").on("click", function () {
            var comboModulo = document.getElementById("comboModulo");
            var comboServico = document.getElementById("comboServico");
            var tabela = document.getElementById("tabela_servico_modulo");
            var modulo = comboModulo.selectedOptions.item(0);
            var servico = comboServico.selectedOptions.item(0);
            var registroEncontrato = false;
            if (tabela.rows.length > 0) {
                for (var i = 0; i < tabela.rows.length; i++) {
                    var row = tabela.rows.item(i);
                    if (row != undefined) {
                        if (row.attributes.getNamedItem("idmodulo").value == modulo.value && row.attributes.getNamedItem("idservico").value == servico.value) {
                            registroEncontrato = true;
                            break;
                        }
                    }
                }
            }
            if (registroEncontrato === false) {
                var row = void 0;
                var r = tabela.insertRow();
                var newRow = void 0;
                row = '<tr idmodulo="' + modulo.value + '" idservico="' + servico.value + '">';
                var cols = "";
                cols += '<td>' + modulo.text + '</td>';
                cols += '<td>' + servico.text + '</td>';
                cols += '<td>';
                cols += '<button onclick="RemoveTableRow(this)" type="button">Remover</button>';
                cols += '</td>';
                row += cols;
                row += "</tr>";
                $("#tabela_servico_modulo").children("tbody").append(row);
            }
        });
    };
    return TabelaParceiroNegocioServicoModulo;
}());
$(function () {
    var obj = new TabelaParceiroNegocioServicoModulo();
    obj.init();
});
//# sourceMappingURL=TabelaParceiroNegocioServicoModulo.js.map