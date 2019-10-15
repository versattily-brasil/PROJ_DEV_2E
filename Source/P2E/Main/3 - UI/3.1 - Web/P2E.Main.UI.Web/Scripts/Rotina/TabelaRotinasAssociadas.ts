class TabelaRotinasAssociadas {
    addClickEvent(): void {
        $(".excluir-rotina-associada").on("click", function () {
            $(this).closest("tr").remove();
        });
    }

    init(): void {

        this.addClickEvent();

        $(".btn-add-rotina-associada").on("click", function () {
            let comboRotinaAssociada: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboRotinaAssociada");         
            if (comboRotinaAssociada.selectedIndex == 0)
            {
                return false
            }
            let tabela: HTMLTableElement = <HTMLTableElement>document.getElementById("tabela_rotina_associada");
            let rotinaSelecionada: HTMLOptionElement = comboRotinaAssociada.selectedOptions.item(0);
            
            let registroEncontrato: boolean = false
            $("#tabela_rotina_associada > tbody > tr").each(function () {
                var CD_ROT_ASS = $(this).data("cd-rot-ass");

                if (CD_ROT_ASS == rotinaSelecionada.value) {
                    registroEncontrato = true;
                }
            });

            if (registroEncontrato === false) {
                let row: string
                row = '<tr data-rot-ass="' + rotinaSelecionada.value + '">';
                var cols = "";
                cols += '<td>' + rotinaSelecionada.text + '</td>';
                cols += '<td class="text-center">';
                cols += '<a data-cd-rot_ass="' + rotinaSelecionada.value + '">';
                cols += '<i style="font-weight:bold;cursor:pointer" class="fal fa-minus-circle text-danger excluir-rotina-associada"></i>';
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
    }
}

$(function () {
    var obj = new TabelaRotinasAssociadas();
    obj.init();
});