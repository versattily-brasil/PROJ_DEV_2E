var TabelaRotinaGrupoOperacao = /** @class */ (function () {
    function TabelaRotinaGrupoOperacao() {
    }
    TabelaRotinaGrupoOperacao.prototype.addClickEvent = function () {
        $(".btn-delete-srv").on("click", function () {
            $(this).closest("tr").remove();
        });
    };
    TabelaRotinaGrupoOperacao.prototype.init = function () {
        this.addClickEvent();
        $(".btn-add-rotina-servico").on("click", function () {
            var comboServico = document.getElementById("comboServico");
            if (comboServico.selectedIndex == 0) {
                return false;
            }
            var tabela = document.getElementById("tabela_rotina_servico");
            var servico = comboServico.selectedOptions.item(0);
            var registroEncontrato = false;
            $("#tabela_rotina_servico > tbody > tr").each(function () {
                var CD_SRV = $(this).data("cdsrv");
                if (CD_SRV == servico.value) {
                    registroEncontrato = true;
                }
            });
            if (registroEncontrato === false) {
                var row = void 0;
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
    };
    return TabelaRotinaGrupoOperacao;
}());
$(function () {
    var obj = new TabelaRotinaGrupoOperacao();
    obj.init();
});
//# sourceMappingURL=TabelaRotinaGrupoOperacao.js.map