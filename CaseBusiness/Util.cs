using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace CaseBusiness
{
    public class Util
    {
        #region Enums e Constantes
        public enum enumTipoTelefone { ZERO, FIXO, CELULAR }

        public const SByte kTipoTelefone_ZERO = 0;
        public const SByte kTipoTelefone_FIXO = 8;
        public const SByte kTipoTelefone_CELULAR = 9;
        #endregion Enums e Constantes

        public static SByte ObterDBValue(enumTipoTelefone enmTipoTelefone)
        {
            SByte _dbvalue = 0;

            switch (enmTipoTelefone)
            {
                case enumTipoTelefone.ZERO: _dbvalue = kTipoTelefone_ZERO; break;
                case enumTipoTelefone.FIXO: _dbvalue = kTipoTelefone_FIXO; break;
                case enumTipoTelefone.CELULAR: _dbvalue = kTipoTelefone_CELULAR; break;
            }

            return _dbvalue;
        }

        public static enumTipoTelefone ObterEnum(SByte tipoTelefone)
        {
            enumTipoTelefone _enmTipoTelefone = enumTipoTelefone.ZERO;

            switch (tipoTelefone)
            {
                case kTipoTelefone_ZERO: _enmTipoTelefone = enumTipoTelefone.ZERO; break;
                case kTipoTelefone_FIXO: _enmTipoTelefone = enumTipoTelefone.FIXO; break;
                case kTipoTelefone_CELULAR: _enmTipoTelefone = enumTipoTelefone.CELULAR; break;
            }

            return _enmTipoTelefone;
        }

        /// <summary>
        /// Retirar Caracter a Esquerda de um Texto
        /// </summary>
        /// <param name="texto">Texto para retirar caracteres a esquerda</param>
        /// <param name="inicio">Inícia da string de retorno</param>
        /// <param name="tamanho">Tota de caracters a excluir no final da string</param>
        /// <returns>Texto a direita sem a parte final a esquerda</returns>
        public static String RetirarCaracterEsquerda(string texto, int tamanho)
        {
            return texto.Substring(0, texto.Length - tamanho);
        }

        /// <summary>
        /// Retirar Caracter a Direita de um Texto 
        /// </summary>
        /// <param name="texto">Texto para retirar caracteres a direita</param>
        /// <param name="tamanho"></param>
        /// <returns>Texto a esquerda sem a parte inicial a direita</returns>
        public static string RetirarCaracterDireita(string texto, int tamanho)
        {
            return texto.Remove(0, texto.Length - tamanho);
        }

        /// <summary>
        /// Concatena Zeros a Esquerda do número informado
        /// </summary>
        /// <param name="numero">número a receber os Zeros a esquerda</param>
        /// <param name="qtdebytes">Qtde de Zeros a concatenar</param>
        public static String ZerosAEsquerda(Int16 numero, Int16 qtdebytes)
        {
            return ZerosAEsquerda(Convert.ToDecimal(numero), qtdebytes);
        }

        /// <summary>
        /// Concatena Zeros a Esquerda do número informado
        /// </summary>
        /// <param name="numero">número a receber os Zeros a esquerda</param>
        /// <param name="qtdebytes">Qtde de Zeros a concatenar</param>
        public static String ZerosAEsquerda(Int32 numero, Int16 qtdebytes)
        {
            return ZerosAEsquerda(Convert.ToDecimal(numero), qtdebytes);
        }

        /// <summary>
        /// Concatena Zeros a Esquerda do número informado
        /// </summary>
        /// <param name="numero">número a receber os Zeros a esquerda</param>
        /// <param name="qtdebytes">Qtde de Zeros a concatenar</param>
        public static String ZerosAEsquerda(Int64 numero, Int16 qtdebytes)
        {
            return ZerosAEsquerda(Convert.ToDecimal(numero), qtdebytes);
        }

        /// <summary>
        /// Concatena Zeros a Esquerda do número informado
        /// </summary>
        /// <param name="numero">número a receber os Zeros a esquerda</param>
        /// <param name="qtdebytes">Qtde de Zeros a concatenar</param>
        public static String ZerosAEsquerda(Decimal numero, Int16 qtdebytes)
        {
            String textoComZeros = String.Empty;

            // Preenche a ESQUERDA com Zeros até o Limite de 'n' bytes
            textoComZeros = textoComZeros.PadLeft(qtdebytes, '0') + numero.ToString();

            // Obtem os 'n' bytes a DIREITA
            textoComZeros = textoComZeros.Substring(textoComZeros.Length - qtdebytes);  // = Right(qtdebytes)

            return textoComZeros;
        }

        /// <summary>
        /// Retorna a Idade já em texto formatado
        /// </summary>
        /// <param name="datanascimento">Data de Nascimento</param>
        public static String Idade_Texto(DateTime datanascimento)
        {
            Int32 idade = Int32.MinValue;

            idade = Idade(datanascimento);

            if (idade.Equals(Int32.MinValue))
            {
                return "---";
            }

            return idade.ToString() + " anos";
        }

        /// <summary>
        /// Retorna a Idade
        /// </summary>
        /// <param name="datanascimento">Data de Nascimento</param>
        public static Int32 Idade(DateTime datanascimento)
        {
            return IdadeFutura(DateTime.Now, datanascimento);
        }

        /// <summary>
        /// Retorna uma Idade Futura
        /// </summary>
        /// <param name="referencia_futura">Data Futura de Referência</param>
        /// <param name="datanascimento">Data de Nascimento</param>
        public static Int32 IdadeFutura(DateTime referencia_futura, DateTime datanascimento)
        {
            if (datanascimento.Year <= 1900) { return Int32.MinValue; }

            Int32 idade = referencia_futura.Year - datanascimento.Year;
            if (referencia_futura < datanascimento.AddYears(idade)) idade--;

            return idade;
        }

        public static String RemoveFormat(String numero)
        {
            String retorno = String.Empty;
            Int16 caracter_numerico = Int16.MinValue;

            //For intInd = 0 To _strNumero.Length - 1
            for (Int32 intInd = 0; intInd <= numero.Length - 1; intInd++)
            {
                if (Int16.TryParse(numero.Substring(intInd, 1), out caracter_numerico))
                {
                    retorno += caracter_numerico.ToString();
                }
            }

            return retorno;
        }

        /// <summary>
        /// Retorna se um CPF é válido
        /// </summary>
        /// <param name="vrCPF"></param>
        /// <returns>true or false</returns>
        public static bool ValidaCPF(String vrCPF)
        {
            String valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else
                if (numeros[9] != 11 - resultado)
                    return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        /// <summary>
        /// Retorna se um CNPJ é válido
        /// </summary>
        /// <param name="vrCNPJ"></param>
        /// <returns>true or false</returns>
        public static bool ValidaCNPJ(String vrCNPJ)
        {
            String CNPJ = vrCNPJ.Replace(".", "");

            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");

            int[] digitos, soma, resultado;
            int nrDig;
            String ftmt;
            bool[] CNPJOk; ftmt = "6543298765432";

            digitos = new int[14];

            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;

            resultado = new int[2]; resultado[0] = 0;
            resultado[1] = 0;

            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));

                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));

                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                }

                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retira máscara do CPF ou CNPJ
        /// </summary>
        /// <param name="vrCPF_CNPJ"></param>
        /// <returns>Retorna CPF ou CNPJ sem máscara</returns>
        public static String CPF_CNPJ_SemMascara(String vrCPF_CNPJ)
        {
            if (String.IsNullOrEmpty(vrCPF_CNPJ.Trim()))
            {
                return "";
            }
            else
            {
                return Convert.ToInt64(vrCPF_CNPJ.Trim().Replace(".", "").Replace("/", "").Replace("-", "")).ToString();
            }
        }

        /// <summary>
        /// Incluí máscara no CPF ou CNPJ
        /// </summary>
        /// <param name="vrCPF_CNPJ"></param>
        /// <returns>Retorna CPF ou CNPJ com máscara</returns>
        public static String CPF_CNPJ_ComMascara(String vrCPF_CNPJ)
        {
            if (String.IsNullOrEmpty(vrCPF_CNPJ.Trim()))
            {
                return "";
            }
            else
            {
                // Retira possível formatação errada
                String valor = vrCPF_CNPJ.Replace(".", "");
                valor = valor.Replace("/", "");
                valor = valor.Replace("-", "");

                if (valor.Trim().Length < 12)
                {
                    long CPF = Convert.ToInt64(valor);
                    String CPF_CNPJ_ComMascara = String.Format(@"{0:000\.000\.000\-00}", CPF);

                    return CPF_CNPJ_ComMascara;
                }
                else
                {
                    // Formata Corretamente
                    long CNPJ = Convert.ToInt64(valor);
                    String CPF_CNPJ_ComMascara = String.Format(@"{0:00\.000\.000\/0000\-00}", CNPJ);

                    return CPF_CNPJ_ComMascara;
                }
            }
        }

        /// <summary>
        /// Valida se CPF ou CNPJ é verdadeiro
        /// </summary>
        /// <param name="vrCPF_CNPJ"></param>
        /// <param name="requerido"></param>
        /// <param name="valida"></param>
        /// <param name="numeroSemMascara"></param>
        /// <param name="mensagemErro"></param>
        /// <returns>true or false</returns>
        public static Boolean ValidarCPF_CNPJ(String vrCPF_CNPJ, Boolean requerido, Boolean valida, ref String numeroSemMascara, ref String mensagemErro)
        {
            String numeroCPF_CNPJ;

            if (!String.IsNullOrEmpty(vrCPF_CNPJ.Trim()))
            {
                numeroCPF_CNPJ = CPF_CNPJ_ComMascara(vrCPF_CNPJ.Trim());

                numeroSemMascara = CPF_CNPJ_SemMascara(numeroCPF_CNPJ);

                if (valida)
                {
                    // Verifica se o CPF ou CNPJ é válido
                    if (numeroCPF_CNPJ.Trim().Length < 15)
                    {
                        if (!ValidaCPF(numeroCPF_CNPJ.Trim()))
                        {
                            mensagemErro = "CPF '" + numeroCPF_CNPJ + "' inválido";

                            return false;
                        }
                    }
                    else
                    {
                        if (!ValidaCNPJ(numeroCPF_CNPJ.Trim()))
                        {
                            mensagemErro = "CNPJ '" + numeroCPF_CNPJ + "' inválido";

                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                numeroSemMascara = "";

                if (!requerido)
                {
                    mensagemErro = "";

                    return true;
                }
                else
                {
                    mensagemErro = "CPF/CNPJ é obrigatório";

                    return false;
                }
            }
        }


        /// <summary>
        /// Remove os caracteres especiais informado
        /// </summary>
        /// <param name="texto">Texto que deseja remover caracter especial</param>
        /// <param name="comCaracter">String do caracter especial.                 Ex: "!#$%¨&*()-?:{}][ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç "</param>
        /// <param name="semCaracter">String da substituição do caracter especial. Ex: "________________AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc_"</param>
        /// <returns>Retorna texto sem os caracteres especiais</returns>
        public static String RemoverCaracterEspeciais(String texto, String comCaracter, String semCaracter)
        {
            String retorno = texto;

            for (int i = 0; i <= comCaracter.Length - 1; i++)
            {
                retorno = retorno.Replace(comCaracter[i].ToString(), semCaracter[i].ToString()).Trim();

            }
            return retorno;
        }

        /// <summary>
        /// Utilizado para formatar telefone celular ou telefone fixo
        /// Formata mascara do ddd + número com 8 e 9 digitos
        /// </summary>
        /// <param name="telefone">DDD + Número</param>
        /// <param name="tipoTelefone">8 = FIXO, 9 = CELULAR</param>
        /// <returns></returns>
        public static String Telefone_ComMascara(String telefone, enumTipoTelefone tipoTelefone)
        {
            String retorno = String.Empty;
            String valor = String.Empty;
            SByte quantidadeNumeroRetorno = 0;
            long numero = long.MinValue;

            if ((tipoTelefone == enumTipoTelefone.FIXO) || (tipoTelefone == enumTipoTelefone.CELULAR))
            {
                valor = Telefone_Montar(telefone.Trim(), ObterDBValue(tipoTelefone), ref quantidadeNumeroRetorno);
            }

            if (!String.IsNullOrEmpty(valor))
            {
                numero = Convert.ToInt64(valor);


                switch (quantidadeNumeroRetorno)
                {
                    case 8: retorno = String.Format(@"{0:\(00\)\ 0000\-0000}", numero); break;
                    case 9: retorno = String.Format(@"{0:\(00\)\ 00000\-0000}", numero); break;
                }
            }

            return retorno;
        }


        /// <summary>
        /// Devolve ddd + numero (telefone = DDNNNNNNNN ou celular = DDNNNNNNNNN)
        /// </summary>
        /// <param name="telefone">DDD + Número</param>
        /// <param name="quantidadeNumero">8 = FIXO, 9 = CELULAR</param>
        /// <param name="quantidadeNumeroRetorno">8 = FIXO, 9 = CELULAR</param>
        /// <returns></returns>
        public static String Telefone_Montar(String telefone, SByte quantidadeNumero, ref SByte quantidadeNumeroRetorno)
        {
            String retorno = String.Empty;

            try
            {
                String ddd = String.Empty;
                String numero = String.Empty;
                String[] split = telefone.Split(' ');

                if (split.Length == 1)
                {
                    ddd = String.Empty;
                    numero = Convert.ToDecimal(split[0]).ToString();
                }
                else if (split.Length == 2)
                {
                    ddd = Convert.ToDecimal(split[0]).ToString();
                    numero = Convert.ToDecimal(split[1]).ToString();
                }


                if (((ddd + numero).Length > 14) ||
                    ((ddd + numero).Length > 11 && split.Length == 1) ||
                    (split.Length > 2))
                {
                    // Telefone/Celular inválido
                    retorno = String.Empty;
                }
                else
                {
                    if (split.Length == 1)
                    {
                        if (quantidadeNumero == 8)
                        {
                            numero = Convert.ToInt64(split[0]).ToString().PadLeft(2 + quantidadeNumero, '0');
                        }
                        else if (quantidadeNumero == 9)
                        {
                            numero = Convert.ToInt64(split[0]).ToString().PadLeft(2 + quantidadeNumero, '0');
                        }

                        quantidadeNumeroRetorno = quantidadeNumero;
                        retorno = numero;
                    }
                    else
                    {
                        quantidadeNumeroRetorno = Convert.ToSByte(split[1].Length);

                        if (quantidadeNumeroRetorno > 9)
                        {
                            quantidadeNumeroRetorno = 9;
                        }

                        if ((Convert.ToInt64(split[1]).ToString().Length < 9) && (quantidadeNumero == 8))
                        {
                            quantidadeNumeroRetorno = 8;
                        }
                        else if ((Convert.ToInt64(split[1]).ToString().Length < 9) && (quantidadeNumero == 9))
                        {
                            quantidadeNumeroRetorno = 9;
                        }

                        ddd = Convert.ToInt64(split[0]).ToString().PadLeft(12, '0').Substring(10, 2);
                        numero = Convert.ToInt64(split[1]).ToString().PadLeft(12, '0').Substring(12 - quantidadeNumeroRetorno, quantidadeNumeroRetorno);

                        retorno = ddd + numero;
                    }
                }

                if (retorno.Trim().Length > 11)
                {
                    // Telefone/Celular inválido
                    retorno = String.Empty;
                }
            }
            catch (Exception)
            {
                // Ocorreu um erro
                retorno = String.Empty;
            }

            return retorno;
        }

        /// <summary>
        /// Encode to Base64 format
        /// </summary>
        /// <param name="plainText">Texto para codificar</param>
        /// <returns>Texto codificado na Base64</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decode from Base64 format
        /// </summary>
        /// <param name="base64EncodedData">Texto codificado na Base64</param>
        /// <returns>Texto descodificado</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
