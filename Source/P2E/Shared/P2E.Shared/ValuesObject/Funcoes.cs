using System;

namespace P2E.Shared.ValuesObject
{
    public class Funcoes
    {
        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool isCpf(string sNumero)
        {
            String Numero = sNumero;
            bool igual = true;

            if (Numero.Length != 11)
            {
                return false;
            }

            for (int i = 1; i < 11 && igual; i++)
            {
                if (Numero[i] != Numero[0])
                {
                    igual = false;
                }
            }

            if (igual || Numero == "12345678909")
            {
                return false;
            }

            int[] Num = new int[11];

            for (int i = 0; i < 11; i++)
            {
                Num[i] = int.Parse(Numero[i].ToString());
            }

            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * Num[i];
            }

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (Num[9] != 0)
                {
                    return false;
                }
            }

            else if (Num[9] != (11 - resultado))
            {
                return false;
            }

            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * Num[i];
            }

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (Num[10] != 0)
                {
                    return false;
                }
            }
            else if (Num[10] != (11 - resultado))
            {
                return false;
            }

            return true;
        }
    }
}
