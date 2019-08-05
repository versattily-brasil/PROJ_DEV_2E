using System;
using System.Collections.Generic;
using System.Text;

namespace P2E.Shared.Message
{
    public static class GenericMessages
    {
        public static string SucessSave(string param)
        {
            return $"{param} salvo com sucesso.";
        }

        public static string SucessRemove(string param)
        {
            return $"{param} excluído com sucesso.";
        }


        public static string ListNull()
        {
            return $"Nenhum registro encontrado para a pesquisa realizada.";
        }


        public static string ErrorSave(string param, string messages)
        {
            var msgReturn = new StringBuilder();
            msgReturn.Append($"Erro ao salvar {param}.");
            msgReturn.Append(messages);
            return msgReturn.ToString();
        }
    }
}
