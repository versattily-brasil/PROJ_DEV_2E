var TabelaRotinaGrupoOperacao = /** @class */ (function () {
    function TabelaRotinaGrupoOperacao() {
    }
    TabelaRotinaGrupoOperacao.prototype.addClickEvent = function () {
        $(".bt-excluir").on("click", function () {
            $(this).closest("tr").remove();
        });
    };
    TabelaRotinaGrupoOperacao.prototype.init = function () {
        this.addClickEvent();
        $(".btn-add-rotina-grupo-operacao").on("click", function () {
            var comboGrupo = document.getElementById("comboGrupo");
            var comboOperacao = document.getElementById("comboOperacao");
            if (comboGrupo.selectedIndex == 0 || comboOperacao.selectedIndex == 0) {
                return false;
            }
            var tabela = document.getElementById("tabela_rotina_grupo_operacao");
            var grupo = comboGrupo.selectedOptions.item(0);
            var operacao = comboOperacao.selectedOptions.item(0);
            var registroEncontrato = false;
            $("#tabela_rotina_grupo_operacao > tbody > tr").each(function () {
                var CD_GRP = $(this).data("cdgrp");
                var CD_OPR = $(this).data("cdopr");
                if (CD_GRP == grupo.value && CD_OPR == operacao.value) {
                    registroEncontrato = true;
                }
            });
            if (registroEncontrato === false) {
                var row = void 0;
                row = '<tr data-cdgrp="' + grupo.value + '" data-cdopr="' + operacao.value + '">';
                var cols = "";
                cols += '<td>' + grupo.text + '</td>';
                cols += '<td>' + operacao.text + '</td>';
                cols += '<td class="text-center">';
                cols += '<a data-grp="' + grupo.value + '" data-opr="' + operacao.value + '">';
                cols += '<i style="font-weight:bold;cursor:pointer" class="fal fa-minus-circle text-danger bt-excluir"></i>';
                cols += '</a>';
                cols += '</td>';
                row += cols;
                row += "</tr>";
                $("#tabela_rotina_grupo_operacao").children("tbody").append(row);
                comboGrupo.selectedIndex = 0;
                comboOperacao.selectedIndex = 0;
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