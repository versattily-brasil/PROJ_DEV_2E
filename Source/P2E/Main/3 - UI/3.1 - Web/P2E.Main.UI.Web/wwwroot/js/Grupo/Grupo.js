var Grupo = /** @class */ (function () {
    function Grupo() {
        this.form = $("#form");
        this.btnSalvar = $("#btnSalvar");
    }
    Grupo.prototype.init = function () {
        var _this = this;
        $(".btn-add-rotina").on("click", function () {
            var rowRotina = '<tr><td>' + $("#comboRotina option:selected").text() + '</td>';
            $('.operacao_hidden').each(function () {
                var CD_OPR = $(this).data("cd_opr");
                var CD_ROT = $('#comboRotina').val();
                //rowRotina += '<td>' + $(this).data('tx_dsc') + '</td>';
                rowRotina += '<td data-cd_rot="' + CD_ROT + '" data-cd_opr="' + CD_OPR + '" class="rotina_selecionada"><input class="rotina_check" type="checkbox"/></td>';
            });
            rowRotina += '<td><i style="font-weight:bold;cursor:pointer" class="excluir-rotina fal fa-minus-circle text-danger bt-selecao-md"> </i></td>';
            rowRotina += '</tr>';
            $("#tabela_grupo_rotina tbody").append(rowRotina);
        });
        this.btnSalvar.on("click", function () {
            var listaRotinas = "RotinaGrupoOperacao";
            var g = 0;
            $(".rotina_selecionada").each(function () {
                var checked = $(this).find('.rotina_check').is(':checked');
                console.log(checked);
                if (checked) {
                    var CD_ROT = $(this).data("cd_rot");
                    var CD_OPR = $(this).data("cd_opr");
                    $("#form").append("<input type='hidden' name= '" + listaRotinas + "[" + g + "].CD_ROT' value= '" + CD_ROT + "' > ");
                    $("#form").append("<input type='hidden' name= '" + listaRotinas + "[" + g + "].CD_OPR' value= '" + CD_OPR + "' > ");
                    g += 1;
                }
            });
            _this.form.submit();
        });
        $(document).on("click", ".excluir-rotina", function () {
            $(this).closest("tr").remove();
        });
    };
    return Grupo;
}());
$(function () {
    var obj = new Grupo();
    obj.init();
});
//# sourceMappingURL=Grupo.js.map