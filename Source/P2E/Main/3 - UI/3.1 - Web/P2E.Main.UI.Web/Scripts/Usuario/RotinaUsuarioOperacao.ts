class RotinaUsuarioOperacao {



    init(): void {
        

        $("#comboServico").on("change", function () {

            var encontrou = false;
            $("#comboRotina > option").each(function () {
                if ($(this).data("cd_srv") == $("#comboServico").val()) {
                    $(this).show();
                    encontrou = true;
                } else {
                    $(this).hide();
                }
            });

            if (($("#comboServico").val() != '') && encontrou) {
                $('#comboRotina').prop('disabled', false);
            } else {
                $('#comboRotina').prop('disabled', true);
            }

            $('#rotina-selecao-validacao').text("");
        });

        $("#btn-add-rotina").on("click", function () {
            let comboRotina: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboRotina");
            let comboServico: HTMLSelectElement = <HTMLSelectElement>document.getElementById("comboServico");
            if (comboRotina.selectedIndex == 0 || comboServico.selectedIndex == 0) {
                return false
            }

            $('#rotina-selecao-validacao').text("");
            var existe = false;
            $(".rotina_selecionada").each(function () {

                if ($(this).data('cd_rot') == $('#comboRotina').val()) {
                    existe = true;
                }

            });

            if (!existe) {
                var rowRotina = '<tr><td>' + $("#comboRotina option:selected").text() + '</td>';
                $('.operacao_hidden').each(function () {

                    var CD_OPR = $(this).data("cd_opr");
                    var CD_ROT = $('#comboRotina').val();

                    rowRotina += '<td data-cd_rot="' + CD_ROT + '" data-cd_opr="' + CD_OPR + '" class="text-center rotina_selecionada"><input class="rotina_check" type="checkbox"/></td>';
                })
                rowRotina += '<td><i style="font-weight:bold;cursor:pointer" class="excluir-rotina fal fa-minus-circle text-danger bt-selecao-md"> </i></td>'
                rowRotina += '</tr>';

                $("#tabela_grupo_rotina tbody").append(rowRotina);
            } else {
                $('#rotina-selecao-validacao').text("A Rotina selecionada já foi adicionada.");
            }



        });

        $(document).on("click", ".excluir-rotina", function () {
            $(this).closest("tr").remove();
        });

        $("#comboRotina").on("change", function () {
            $('#rotina-selecao-validacao').text("");
        });
    }
}

$(function () {
    var obj = new RotinaUsuarioOperacao();
    obj.init();
});
