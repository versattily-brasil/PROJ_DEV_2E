var TabelaRotinaGrupoOperacao = /** @class */ (function () {
    function TabelaRotinaGrupoOperacao() {
    }
    TabelaRotinaGrupoOperacao.prototype.init = function () {
        $(".bt-excluir").on("click", function () {
            $(this).removeData("fa-minus-circle").removeClass("text-danger");
            var tr = $(this).closest("tr");
            $("#tabela_rotina_grupo_operacao tbody").append(tr);
        });
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
                cols += '<td>';
                cols += '<a class="btn btn-success btn-m-l btn-icon bt-excluir" data-grp="' + grupo.value + '" data-opr="' + operacao.value + '">';
                cols += '<i class="fal fa-minus-circle"></i>';
                cols += '</td>';
                row += cols;
                row += "</tr>";
                $("#tabela_rotina_grupo_operacao").children("tbody").append(row);
                comboGrupo.selectedIndex = 0;
                comboOperacao.selectedIndex = 0;
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