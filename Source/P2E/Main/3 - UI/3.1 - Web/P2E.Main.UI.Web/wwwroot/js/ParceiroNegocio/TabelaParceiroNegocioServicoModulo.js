var TabelaParceiroNegocioServicoModulo = /** @class */ (function () {
    function TabelaParceiroNegocioServicoModulo() {
    }
    TabelaParceiroNegocioServicoModulo.prototype.init = function () {
        $(".bt-excluir").on("click", function () {
            $(this).removeData("fa-minus-circle").removeClass("text-danger");
            var tr = $(this).closest("tr");
            $("#tabela_servico_modulo tbody").append(tr);
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
            $("#tabela_servico_modulo > tbody > tr").each(function () {
                var CD_MOD = $(this).data("cdmod");
                var CD_SRV = $(this).data("cdsrv");
                if (CD_MOD == modulo.value && CD_SRV == servico.value) {
                    registroEncontrato = true;
                }
            });
            if (registroEncontrato === false) {
                var row = void 0;
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
    };
    return TabelaParceiroNegocioServicoModulo;
}());
$(function () {
    var obj = new TabelaParceiroNegocioServicoModulo();
    obj.init();
});
//# sourceMappingURL=TabelaParceiroNegocioServicoModulo.js.map