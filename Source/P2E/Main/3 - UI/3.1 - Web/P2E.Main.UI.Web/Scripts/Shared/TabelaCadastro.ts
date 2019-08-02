﻿
class TabelaCadastro {

    init(): void {

        $(".bt-selecao").on("click", function () {
            if ($(this).hasClass("fa-plus-circle")) {
                $(this).removeClass("fa-plus-circle").removeClass("text-success");
                $(this).addClass("fa-minus-circle").addClass("text-danger");
                var tr = $(this).closest("tr");
                $("#table-destino tbody").append(tr);
                TabelaCadastro.sortTable("table-destino");
            } else {
                $(this).removeClass("fa-minus-circle").removeClass("text-danger");
                $(this).addClass("fa-plus-circle").addClass("text-success");
                var tr = $(this).closest("tr");
                $("#table-origem tbody").append(tr);
                TabelaCadastro.sortTable("table-origem");
            }
        });
    }

    static sortTable(name) {

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
    }
}

$(function () {
    var obj = new TabelaCadastro();
    obj.init();
});