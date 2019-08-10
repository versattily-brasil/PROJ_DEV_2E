var TabelaParceiroNegocioServicoModulo = /** @class */ (function () {
    function TabelaParceiroNegocioServicoModulo() {
    }
    TabelaParceiroNegocioServicoModulo.prototype.init = function () {
        $(".bt-excluir").on("click", function () {
            $(this).closest("tr").remove();
        });
        $(".btn-add-parceiro-servico-usuario").on("click", function () {
            var comboModulo = document.getElementById("comboModulo");
            var comboServico = document.getElementById("comboServico");
            if (comboModulo.selectedIndex == 0 || comboServico.selectedIndex == 0) {
                return false;
            }
            var tabela = document.getElementById("tabela_servico_modulo");
            var modulo = comboModulo.selectedOptions.item(0);
            var servico = comboServico.selectedOptions.item(0);
            var registroEncontrato = false;
            var totalLinhas = 0;
            $("#tabela_servico_modulo > tbody > tr").each(function () {
                var CD_MOD = $(this).data("cdmod");
                var CD_SRV = $(this).data("cdsrv");
                if (CD_MOD == modulo.value && CD_SRV == servico.value) {
                    registroEncontrato = true;
                }
                totalLinhas++;
            });
            if (registroEncontrato === false) {
                ///let row: string
                var linha = tabela.insertRow(totalLinhas + 1);
                var colunaModulo = linha.insertCell(0);
                var colunaServico = linha.insertCell(1);
                var colunaBotao = linha.insertCell(2);
                colunaModulo.innerHTML = modulo.text;
                colunaServico.innerHTML = servico.text;
                var botao = '<a data-mod="' + modulo.value + '" data-srv="' + servico.value + '" class="bt-excluir">';
                botao += '<i style="font-weight:bold;cursor:pointer" class="fal fa-minus-circle text-danger bt-excluir"></i>';
                botao += '</a>';
                colunaBotao.innerHTML = botao;
                //row = '<tr data-cdmod="' + modulo.value + '" data-cdsrv="' + servico.value + '">';
                //var cols = "";
                //cols += '<td>' + modulo.text + '</td>';
                //cols += '<td>' + servico.text + '</td>';
                //cols += '<td class="text-center">';
                //cols += '<a data-mod="' + modulo.value + '" data-srv="' + servico.value + '">';
                //cols += '<i style="font-weight:bold;cursor:pointer" class="fal fa-minus-circle text-danger bt-excluir"></i>';
                //cols += '</a>';
                //cols += '</td>';
                //row += cols;
                //row += "</tr>";
                //$("#tabela_servico_modulo").children("tbody").append(row);
                comboModulo.selectedIndex = 0;
                comboServico.selectedIndex = 0;
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