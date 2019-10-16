var MenuUsuario = /** @class */ (function () {
    function MenuUsuario() {
    }
    MenuUsuario.prototype.init = function () {
        this.addClickEvent();
        $(".btn-add-rotina-associada").on("click", function () {
            var comboRotinaAssociada = document.getElementById("comboRotinaAssociada");
            if (comboRotinaAssociada.selectedIndex == 0) {
                return false;
            }
            var tabela = document.getElementById("tabela_rotina_associada");
            var rotinaSelecionada = comboRotinaAssociada.selectedOptions.item(0);
            var registroEncontrato = false;
            $("#tabela_rotina_associada > tbody > tr").each(function () {
                var CD_ROT_ASS = $(this).data("cd-rot-ass");
                if (CD_ROT_ASS == rotinaSelecionada.value) {
                    registroEncontrato = true;
                }
            });
            if (registroEncontrato === false) {
                var row = void 0;
                row = '<tr data-cd-rot-ass="' + rotinaSelecionada.value + '">';
                var cols = "";
                cols += '<td>' + rotinaSelecionada.text + '</td>';
                cols += '<td class="text-center">';
                cols += '<a data-cd-rot-ass="' + rotinaSelecionada.value + '">';
                cols += '<i style="font-weight:bold;cursor:pointer" class="fal fa-minus-circle text-danger bt-excluir"></i>';
                cols += '</a>';
                cols += '</td>';
                row += cols;
                row += "</tr>";
                $("#tabela_rotina_associada").children("tbody").append(row);
                comboRotinaAssociada.selectedIndex = 0;
                $(".bt-excluir").on("click", function () {
                    $(this).closest("tr").remove();
                });
            }
        });
    };
    return MenuUsuario;
}());
$(function () {
    var obj = new MenuUsuario();
    obj.init();
});
//# sourceMappingURL=TabelaRotinasAssociadas.js.map