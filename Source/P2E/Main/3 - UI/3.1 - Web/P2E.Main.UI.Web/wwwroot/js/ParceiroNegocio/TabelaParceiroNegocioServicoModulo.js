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
            //let tabela: HTMLTableElement = <HTMLTableElement>document.getElementById("tabela_servico_modulo");
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
    };
    TabelaParceiroNegocioServicoModulo.sortTable = function (name) {
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
    };
    return TabelaParceiroNegocioServicoModulo;
}());
$(function () {
    var obj = new TabelaParceiroNegocioServicoModulo();
    obj.init();
});
//# sourceMappingURL=TabelaParceiroNegocioServicoModulo.js.map