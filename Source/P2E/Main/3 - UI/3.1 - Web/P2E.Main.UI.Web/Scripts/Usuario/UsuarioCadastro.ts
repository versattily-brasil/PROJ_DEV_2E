class UsuarioCadastro {

    form = $("#form");
    btnSalvar = $("#btnSalvar");

    init(): void {
        this.btnSalvar.on("click", () => {

            //ASSOCIÇÃO DE GRUPOS
            var listaGrupos = "UsuarioGrupo";
            var g = 0;

            $("#table-destino-grp > tbody > tr").each(function () {

                var CD_GRP = $(this).data("grp");
                $("#form").append("<input type='hidden' name= '" + listaGrupos + "[" + g + "].CD_GRP' value= '" + CD_GRP + "' > ");
                
                g += 1;
            });

            //ASSOCIÇÃO DE MÓDULOS
            var listaModulos = "UsuarioModulo";
            var m = 0;

            $("#table-destino-md > tbody > tr").each(function () {

                var CD_MOD = $(this).data("mod");
                $("#form").append("<input type='hidden' name= '" + listaModulos + "[" + m + "].CD_MOD' value= '" + CD_MOD + "' > ");

                m += 1;
            });


            //ASSOCIÇÃO DE ROTINAS
            var listaRotinas = "RotinaUsuarioOperacao";
            var indiceRotinas = 0;
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

                    $("#form").append("<input type='hidden' name= '" + listaRotinas + "[" + indiceRotinas + "].CD_ROT' value= '" + CD_ROT + "' > ");
                    $("#form").append("<input type='hidden' name= '" + listaRotinas + "[" + indiceRotinas + "].CD_OPR' value= '" + CD_OPR + "' > ");

                    indiceRotinas += 1;
                }
            });


            if (rotinasInvalidas > 0) {
                $('#rotina-validacao').text("É necessário permitir pelo menos uma operação para cada Rotina.");
            } else {
                this.form.submit();
            }

        });
    }
}

$(function () {
    var obj = new UsuarioCadastro();
    obj.init();
});
