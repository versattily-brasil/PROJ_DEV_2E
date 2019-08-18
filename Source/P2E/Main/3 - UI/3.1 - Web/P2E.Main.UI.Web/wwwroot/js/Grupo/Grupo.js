var Grupo = /** @class */ (function () {
    function Grupo() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
        this.btnConfirmarSalvar = $("#confirm-delete");
    }
    Grupo.prototype.init = function () {
        var _this = this;
        this.btnConfirmarSalvar.on("click", function (e) {
            _this.form.submit();
        });
        $("#comboServico").on("change", function () {
            var encontrou = false;
            $("#comboRotina > option").each(function () {
                if ($(this).data("cd_srv") == $("#comboServico").val()) {
                    $(this).show();
                    encontrou = true;
                }
                else {
                    $(this).hide();
                }
            });
            if (($("#comboServico").val() != '') && encontrou) {
                $('#comboRotina').prop('disabled', false);
            }
            else {
                $('#comboRotina').prop('disabled', true);
            }
            $('#rotina-selecao-validacao').text("");
        });
        $("#btn-add-rotina").on("click", function () {
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
                });
                rowRotina += '<td><i style="font-weight:bold;cursor:pointer" class="excluir-rotina fal fa-minus-circle text-danger bt-selecao-md"> </i></td>';
                rowRotina += '</tr>';
                $("#tabela_grupo_rotina tbody").append(rowRotina);
            }
            else {
                $('#rotina-selecao-validacao').text("A Rotina selecionada já foi adicionada.");
            }
        });
        this.btnSalvar.on("click", function (e) {
            var listaRotinas = "RotinaGrupoOperacao";
            var g = 0;
            $('#rotina-validacao').text("");
            var rotinasInvalidas = 0;
            $("#tabela_grupo_rotina > tbody > tr").each(function () {
                var checked = $(this).find('.rotina_check').is(':checked');
                if (!checked) {
                    rotinasInvalidas++;
                }
            });
            $(".rotina_selecionada").each(function () {
                var checked = $(this).find('.rotina_check').is(':checked');
                if (checked) {
                    var CD_ROT = $(this).data("cd_rot");
                    var CD_OPR = $(this).data("cd_opr");
                    $("#form").append("<input type='hidden' name= '" + listaRotinas + "[" + g + "].CD_ROT' value= '" + CD_ROT + "' > ");
                    $("#form").append("<input type='hidden' name= '" + listaRotinas + "[" + g + "].CD_OPR' value= '" + CD_OPR + "' > ");
                    g += 1;
                }
            });
            if (rotinasInvalidas > 0) {
                $('#rotina-validacao').text("É necessário permitir pelo menos uma operação para cada Rotina.");
            }
            else {
                //this.form.submit();
            }
        });
        $(document).on("click", ".excluir-rotina", function () {
            $(this).closest("tr").remove();
        });
        $("#comboRotina").on("change", function () {
            $('#rotina-selecao-validacao').text("");
        });
    };
    return Grupo;
}());
$(function () {
    var obj = new Grupo();
    obj.init();
});
//# sourceMappingURL=Grupo.js.map