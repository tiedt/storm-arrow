using System;
using System.Threading.Tasks;

namespace SIMP.Models{
    
    public static class ValidatingClass{

        public static bool CPF(string value){            
            if (String.IsNullOrEmpty(value))
                return false;
            try{
                // Fonte: https://www.devmedia.com.br/validando-o-cpf-em-uma-aplicacao-delphi/22180
                // Exemplo: 546.471.429-49
                string cpf = value.Replace(".", ""); // Remove pontos
                cpf = cpf.Replace(" ", ""); // Remove espaços em branco
                cpf = cpf.Replace("-", ""); // Remove traçados

                if (cpf.Length != 11)
                    return false;

                /* a) cada um dos nove primeiros números do CPF é multiplicado por um 
                 *  peso que começa de 10 e que vai sendo diminuido de 1 a cada passo, 
                 *  somando-se as parcelas calculadas: 
                 *  sm = (5*10)+(4*9)+(6*8)+(4*7)+(7*6)+(1*5)+(4*4)+(2*3)+(9*2) = 249;
                 */
                int sumDigitos = 0;
                int maxFor = 10;
                for (int i = maxFor; i > 1; i--){
                    sumDigitos += Convert.ToInt32(Char.ToString(cpf[(i * -1) + maxFor])) * i;
                }

                /* b) calcula-se o dígito através da seguinte expressão:
                 *  11 - (sm % 11) = 11 - (249 % 11) = 11 - 7 = 4
                 */

                int digitoVerificador1 = 11 - (sumDigitos % 11);
                if (digitoVerificador1 >= 10) // Caso o resultado for 10 ou maior, o penúltimo dígito verificador será o 0
                    digitoVerificador1 = 0;

                // Para calcular o 2º dígito verificador:
                /* a) cada um dos dez primeiros números do CPF, considerando-se aqui o primeiro DV, 
                 * é multiplicado por um peso que começa de 11 e que vai sendo diminuido de 1 a cada 
                 * passo, somando-se as parcelas calculadas:
                 * sm = (5*11)+(4*10)+(6*9)+(4*8)+(7*7)+(1*6)+(4*5)+(2*4)+(9*3)+(4*2) = 299;
                 */
                sumDigitos = 0;
                maxFor = 11;
                for (int i = maxFor; i > 1; i--){
                    sumDigitos += Convert.ToInt32(Char.ToString(cpf[(i * -1) + maxFor])) * i;
                }

                int digitoVerificador2 = 11 - (sumDigitos % 11);
                if (digitoVerificador2 >= 10) // Caso o resultado for 10 ou maior, o penúltimo dígito verificador será o 0
                    digitoVerificador2 = 0;

                return Convert.ToInt32(Char.ToString(cpf[^2])) == digitoVerificador1
                    && Convert.ToInt32(Char.ToString(cpf[^1])) == digitoVerificador2;

            }catch(Exception){
                return false;
            }
        }

        public static bool CNPJ(string value){
            if(String.IsNullOrEmpty(value))
                return false;
            try{
                // https://campuscode.com.br/conteudos/o-calculo-do-digito-verificador-do-cpf-e-do-cnpj#:~:text=O%20c%C3%A1lculo%20de%20valida%C3%A7%C3%A3o%20do,2%20e%20somamos%20esse%20resultado.
                // Exemplo: 85.254.134/0001-91
                string cnpj = value.Replace(".", ""); // Remover pontos
                cnpj = cnpj.Replace(" ", ""); // Remover espaços em branco
                cnpj = cnpj.Replace("-", ""); // Remover traços
                cnpj = cnpj.Replace("/", ""); // Remover barras

                if(cnpj.Length != 14)
                    return false;

                int maxFor = 12;
                string cnpjInvertido = new string("");
                for(int i = 0; i <= maxFor; i++)
                    cnpjInvertido = Char.ToString(cnpj[i]) + cnpjInvertido;

                int sumDigitos = 0;
                int[] pesos = new int[12] {2,3,4,5,6,7,8,9,2,3,4,5};
                for(int i = 1; i <= pesos.Length; i++)
                    sumDigitos += Convert.ToInt32(Char.ToString(cnpjInvertido[i])) * pesos[i-1];

                int digitoVerificador1 = 11 - (sumDigitos % 11);
                if(digitoVerificador1 >= 10)
                    digitoVerificador1 = 0;

                pesos = new int[13] {2,3,4,5,6,7,8,9,2,3,4,5,6};
                sumDigitos = 0;
                for(int i = 0; i < pesos.Length; i++)
                    sumDigitos += Convert.ToInt32(Char.ToString(cnpjInvertido[i])) * pesos[i];

                int digitoVerificador2 = 11 - (sumDigitos % 11);
                if(digitoVerificador2 >= 10)
                    digitoVerificador2 = 0;
                return Convert.ToInt32(Char.ToString(cnpj[^2])) == digitoVerificador1
                    && Convert.ToInt32(Char.ToString(cnpj[^1])) == digitoVerificador2;
            }catch(Exception){
                return false;
            }
        }

    }

}
