#region Using
using CaseBusiness.Framework.BancoDados;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web;
#endregion Using

namespace CaseBusiness.CtrlAcesso
{
    public class UsuarioLoginPolitica : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "POLITICA_LOGIN";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int16 _carac_minimo = 0;
        private Int16 _carac_especial = 0;
        private Int16 _carac_numerico = 0;
        private Int16 _letra_maiuscula = 0;
        private Int16 _senha_anteriores = 0;
        private Int16 _dias_login_inativo = 0;
        private Int16 _dias_senha_expiracao = 0;
        private Int16 _senha_errada_bloqueio = 0;
        #endregion Atributos


        #region Propriedades
        public Int16 QtdeMinimaCaracteres_Minima { get { return 3; } }
        public Int16 QtdeMinimaCaracteres_Maxima { get { return 12; } }
        public Int16 QtdeMinimaCaracteres
        {
            get { return _carac_minimo; }
            set { _carac_minimo = value; }
        }

        public Int16 QtdeMinimaCaracteresEspeciais_Minima { get { return 0; } }
        public Int16 QtdeMinimaCaracteresEspeciais_Maxima { get { return 10; } }
        public Int16 QtdeMinimaCaracteresEspeciais
        {
            get { return _carac_especial; }
            set { _carac_especial = value; }
        }


        public Int16 QtdeMinimaCaracteresNumericos_Minima { get { return 0; } }
        public Int16 QtdeMinimaCaracteresNumericos_Maxima { get { return 10; } }
        public Int16 QtdeMinimaCaracteresNumericos
        {
            get { return _carac_numerico; }
            set { _carac_numerico = value; }
        }

        public Int16 QtdeMinimaLetrasMaiusculas_Minima { get { return 0; } }
        public Int16 QtdeMinimaLetrasMaiusculas_Maxima { get { return 10; } }
        public Int16 QtdeMinimaLetrasMaiusculas
        {
            get { return _letra_maiuscula; }
            set { _letra_maiuscula = value; }
        }

        public Int16 QtdeSenhaAnteriores_Minima { get { return 0; } }
        public Int16 QtdeSenhaAnteriores_Maxima { get { return 10; } }
        public Int16 QtdeSenhaAnteriores
        {
            get { return _senha_anteriores; }
            set { _senha_anteriores = value; }
        }

        public Int16 QtdeDiasBloqueioLoginInativo_Minima { get { return 0; } }
        public Int16 QtdeDiasBloqueioLoginInativo_Maxima { get { return 360; } }
        public Int16 QtdeDiasBloqueioLoginInativo
        {
            get { return _dias_login_inativo; }
            set { _dias_login_inativo = value; }
        }

        public Int16 QtdeDiasSenhaExpiracao_Minima { get { return 0; } }
        public Int16 QtdeDiasSenhaExpiracao_Maxima { get { return 360; } }
        public Int16 QtdeDiasSenhaExpiracao
        {
            get { return _dias_senha_expiracao; }
            set { _dias_senha_expiracao = value; }
        }

        public Int16 QtdeSenhaErradaBloqueio_Minima { get { return 0; } }
        public Int16 QtdeSenhaErradaBloqueio_Maxima { get { return 10; } }
        public Int16 QtdeSenhaErradaBloqueio
        {
            get { return _senha_errada_bloqueio; }
            set { _senha_errada_bloqueio = value; }
        }
        #endregion Propriedades


        #region Construtores
        /// <summary>
        /// Construtor classe UsuarioLoginPolitica
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public UsuarioLoginPolitica()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);

            Consultar();
        }

        /// <summary>
        /// Construtor classe UsuarioLoginPolitica
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public UsuarioLoginPolitica(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);

            Consultar();
        }
        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Alterar a Politica de Login de Usuário
        /// </summary>
        public void Alterar(Int16 qtdeMinimaCaracteres,
                            Int16 qtdeMinimaCaracteresEspeciais,
                            Int16 qtdeMinimaCaracteresNumericos,
                            Int16 qtdeMinimaLetrasMaiusculas,
                            Int16 qtdeSenhaAnteriores,
                            Int16 qtdeDiasBloqueioLoginInativo,
                            Int16 qtdeDiasSenhaExpiracao,
                            Int16 qtdeSenhaErradaBloqueio)
        {
            try
            {
                acessoDadosBase.AddParameter("@USULGPOL_CARAC_MINIMO", qtdeMinimaCaracteres);
                acessoDadosBase.AddParameter("@USULGPOL_CARAC_ESPECIAL", qtdeMinimaCaracteresEspeciais);
                acessoDadosBase.AddParameter("@USULGPOL_CARAC_NUMERICO", qtdeMinimaCaracteresNumericos);
                acessoDadosBase.AddParameter("@USULGPOL_LETRA_MAIUSCULA", qtdeMinimaLetrasMaiusculas);
                acessoDadosBase.AddParameter("@USULGPOL_SENHA_ANTERIORES", qtdeSenhaAnteriores);
                acessoDadosBase.AddParameter("@USULGPOL_DIAS_LOGIN_INATIVO", qtdeDiasBloqueioLoginInativo);
                acessoDadosBase.AddParameter("@USULGPOL_DIAS_SENHA_EXPIR", qtdeDiasSenhaExpiracao);
                acessoDadosBase.AddParameter("@USULGPOL_SENHA_ERRADA_BLOQ", qtdeSenhaErradaBloqueio);
                acessoDadosBase.AddParameter("@USU_ID_UPD", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prUSULGPOL_UPD");

                // MemoryCache Clean
                RemoverCache();
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Consultar a Politica de Login de Usuário
        /// </summary>
        private void Consultar()
        {
            try
            {
                DataTable dt;

                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dt = MemoryCache.Default[kCacheKey] as DataTable;
                }
                else
                {
                    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                       "prUSULGPOL_SEL_CONSULTAR").Tables[0];

                    MemoryCache.Default.Set(kCacheKey,
                                            dt,
                                            new CacheItemPolicy()
                                            {
                                                AbsoluteExpiration = DateTime.Now.AddMinutes(kCache_ABSOLUTEEXPIRATION_MINUTES)
                                            });
                }

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    QtdeMinimaCaracteres = Convert.ToInt16(dt.Rows[0]["USULGPOL_CARAC_MINIMO"]);
                    QtdeMinimaCaracteresEspeciais = Convert.ToInt16(dt.Rows[0]["USULGPOL_CARAC_ESPECIAL"]);
                    QtdeMinimaCaracteresNumericos = Convert.ToInt16(dt.Rows[0]["USULGPOL_CARAC_NUMERICO"]);
                    QtdeMinimaLetrasMaiusculas = Convert.ToInt16(dt.Rows[0]["USULGPOL_LETRA_MAIUSCULA"]);
                    QtdeSenhaAnteriores = Convert.ToInt16(dt.Rows[0]["USULGPOL_SENHA_ANTERIORES"]);
                    QtdeDiasBloqueioLoginInativo = Convert.ToInt16(dt.Rows[0]["USULGPOL_DIAS_LOGIN_INATIVO"]);
                    QtdeDiasSenhaExpiracao = Convert.ToInt16(dt.Rows[0]["USULGPOL_DIAS_SENHA_EXPIRAC"]);
                    QtdeSenhaErradaBloqueio = Convert.ToInt16(dt.Rows[0]["USULGPOL_SENHA_ERRADA_BLOQ"]);

                    __blnIsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Remove do Cache a Politica
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        public Boolean ValidarSenha(String senha, ref String erromsg)
        {
            String caracteres_permitidos = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM 1234567890 çÇáéíóúýÁÉÍÓÚÝàèìòùÀÈÌÒÙãõñäëïöüÿÄËÏÖÜÃÕÑâêîôûÂÊÎÔÛ !@#$%¨&*() -=_+  \\ | ,.;/<>:? ~]^} ´[`{ °ªº§¬¢£";
            //Boolean eh_senha_valida = false;
            Char caracter;
            //String cnet_feedback = String.Empty;

            Int16 countLetrasMaiusculas = 0;
            Int16 countLetrasMinusculas = 0;
            Int16 countCarateresEspeciais = 0;
            Int16 countCarateresNumericos = 0;

            Int16 countCharRepetido = 0;
            Char ultimoCaracter = '\0';

            erromsg = String.Empty;

            // Checa Tamanho Minimo
            if (senha.Length < QtdeMinimaCaracteres)
            {
                ValidacaoSenhaErroMsg(ref erromsg, "Senha deve conter, no mínimo, " + QtdeMinimaCaracteres.ToString() + " caracteres");
                return false;
            }


            for (Int16 pointer = 0; pointer <= (senha.Length - 1); pointer++)
            {
                caracter = Convert.ToChar(senha.Substring(pointer, 1));

                //cnet_feedback += "\n\ncaracter: '" + caracter.ToString() + "'";
                //if (Char.IsSymbol(caracter)) { cnet_feedback += "\n  IsSymbol = " + Char.IsSymbol(caracter).ToString(); }
                //if (Char.IsPunctuation(caracter)) { cnet_feedback += "\n  IsPunctuation = " + Char.IsPunctuation(caracter).ToString(); }

                //if (Char.IsDigit(caracter)) { cnet_feedback += "\n  IsDigit = " + Char.IsDigit(caracter).ToString(); }
                //if (Char.IsUpper(caracter)) { cnet_feedback += "\n  IsUpper = " + Char.IsUpper(caracter).ToString(); }
                //if (Char.IsLower(caracter)) { cnet_feedback += "\n  IsLower = " + Char.IsLower(caracter).ToString(); }
                //if (Char.IsLetter(caracter)) { cnet_feedback += "\n  IsLetter = " + Char.IsLetter(caracter).ToString(); }

                //if (Char.IsWhiteSpace(caracter)) { cnet_feedback += "\n  IsWhiteSpace = " + Char.IsWhiteSpace(caracter).ToString(); }
                ////if (Char.IsSeparator(caracter)) { cnet_feedback += "\n  IsSeparator = " + Char.IsSeparator(caracter).ToString(); }

                //if (Char.IsSurrogate(caracter)) { cnet_feedback += "\n  IsSurrogate = " + Char.IsSurrogate(caracter).ToString(); }
                //if (Char.IsHighSurrogate(caracter)) { cnet_feedback += "\n  IsHighSurrogate = " + Char.IsHighSurrogate(caracter).ToString(); }
                //if (Char.IsLowSurrogate(caracter)) { cnet_feedback += "\n  IsLowSurrogate = " + Char.IsLowSurrogate(caracter).ToString(); }
                //if (Char.IsControl(caracter)) { cnet_feedback += "\n  IsControl = " + Char.IsControl(caracter).ToString(); }
                ////continue;


                if (caracteres_permitidos.IndexOf(caracter.ToString()) < 0)
                {
                    ValidacaoSenhaErroMsg(ref erromsg, "Caracter '" + caracter.ToString() + "' não é permitido nas senhas");
                    //return eh_senha_valida;
                }

                // Space permitido não é permitido
                if (Char.IsWhiteSpace(caracter))
                {
                    ValidacaoSenhaErroMsg(ref erromsg, "Sua nova senha não pode conter Espaços");
                    //return eh_senha_valida;
                }


                if (Char.IsUpper(caracter)) { countLetrasMaiusculas++; }
                if (Char.IsLower(caracter)) { countLetrasMinusculas++; }
                if (Char.IsDigit(caracter)) { countCarateresNumericos++; }
                if (Char.IsSymbol(caracter) || Char.IsPunctuation(caracter)) { countCarateresEspeciais++; }



                //
                // Caracter repetido
                //
                if (caracter == ultimoCaracter)
                { countCharRepetido++; }
                else
                { countCharRepetido = 0; }

                if (countCharRepetido >= 2)
                {
                    ValidacaoSenhaErroMsg(ref erromsg, "Sua nova senha não pode conter caracteres '" + new String(caracter, 3) + "' repetidos em sequência");
                }

                ultimoCaracter = caracter;
            }


            //
            // Checando Letras Maiusculas
            //
            if (QtdeMinimaLetrasMaiusculas > 0)
            {
                if (countLetrasMaiusculas < QtdeMinimaLetrasMaiusculas)
                {
                    ValidacaoSenhaErroMsg(ref erromsg, "Sua nova senha precisa conter, no mínimo, " + QtdeMinimaLetrasMaiusculas.ToString() + " Letra" + (String)(QtdeMinimaLetrasMaiusculas > 1 ? "s" : "") + " Maiúscula" + (String)(QtdeMinimaLetrasMaiusculas > 1 ? "s" : ""));
                    //return eh_senha_valida;
                }

                if (countLetrasMinusculas == 0)
                {
                    ValidacaoSenhaErroMsg(ref erromsg, "Sua nova senha precisa conter também Letras Minúsculas");
                    //return eh_senha_valida;
                }
            }

            //
            // Checando Caracteres Numéricos
            //
            if (QtdeMinimaCaracteresNumericos > 0)
            {
                if (countCarateresNumericos < QtdeMinimaCaracteresNumericos)
                {
                    ValidacaoSenhaErroMsg(ref erromsg, "Sua nova senha precisa conter, no mínimo, " + QtdeMinimaCaracteresNumericos.ToString() + " Número" + (String)(QtdeMinimaCaracteresNumericos > 1 ? "s" : ""));
                    //return eh_senha_valida;
                }
            }

            //
            //  Checando Caracteres Especiais
            //
            if (QtdeMinimaCaracteresEspeciais > 0)
            {
                if (countCarateresEspeciais < QtdeMinimaCaracteresEspeciais)
                {
                    ValidacaoSenhaErroMsg(ref erromsg, "Sua nova senha precisa conter, no mínimo, " + QtdeMinimaCaracteresEspeciais.ToString() + " Caracter" + (String)(QtdeMinimaCaracteresEspeciais > 1 ? "es" : "") + " Especia" + (String)(QtdeMinimaCaracteresEspeciais > 1 ? "is" : "l"));
                    //return eh_senha_valida;
                }
            }


            if (String.IsNullOrEmpty(erromsg))
            { return true; }
            else
            { return false; }

            //return eh_senha_valida;
        }


        private void ValidacaoSenhaErroMsg(ref String erromsg, String msg_toappend)
        {
            if (erromsg.IndexOf(msg_toappend) >= 0)
            {
                // A Mensagem já foi adicionada, desnecessário novamente
                return;
            }

            if (erromsg.Length > 0) { erromsg += "\n"; }
            erromsg += msg_toappend;
        }


        public String RegrasSenhaExplicativo()
        {
            String _explicativo = String.Empty;

            _explicativo += "Deve conter, no mínimo, " + QtdeMinimaCaracteres.ToString() + " caracteres";
            _explicativo += "<br/>Não pode conter Espaços";
            _explicativo += "<br/>Não pode conter 3 caracteres repetidos em sequência";

            if (QtdeMinimaLetrasMaiusculas > 0)
            {
                _explicativo += "<br/>Precisa conter, no mínimo, " + QtdeMinimaLetrasMaiusculas.ToString() + " Letra" + (String)(QtdeMinimaLetrasMaiusculas > 1 ? "s" : "") + " Maiúscula" + (String)(QtdeMinimaLetrasMaiusculas > 1 ? "s" : "");
                _explicativo += "<br/>Sua nova senha precisa conter também Letras Minúsculas";
            }

            if (QtdeMinimaCaracteresNumericos > 0)
            {
                _explicativo += "<br/>Precisa conter, no mínimo, " + QtdeMinimaCaracteresNumericos.ToString() + " Número" + (String)(QtdeMinimaCaracteresNumericos > 1 ? "s" : "");
            }

            if (QtdeMinimaCaracteresEspeciais > 0)
            {
                _explicativo += "<br/>Precisa conter, no mínimo, " + QtdeMinimaCaracteresEspeciais.ToString() + " Caracter" + (String)(QtdeMinimaCaracteresEspeciais > 1 ? "es" : "") + " Especia" + (String)(QtdeMinimaCaracteresEspeciais > 1 ? "is" : "l");
            }

            if (QtdeSenhaAnteriores > 0)
            {
                _explicativo += "<br/>Sua nova senha não pode ter sido utilizada na" + (String)(QtdeSenhaAnteriores > 1 ? "s" : "") + " sua" + (String)(QtdeSenhaAnteriores > 1 ? "s" : "") + " última" + (String)(QtdeSenhaAnteriores > 1 ? "s" : "") + " " + QtdeSenhaAnteriores.ToString() + " Senha" + (String)(QtdeSenhaAnteriores > 1 ? "s" : "") + " Anterior" + (String)(QtdeSenhaAnteriores > 1 ? "es" : "") + "";
            }


            return _explicativo;
        }
        #endregion Métodos
    }
}
//“ç”, “Ç”, “á”, “é”, “í”, “ó”, “ú”, “ý”, “Á”, “É”, “Í”, “Ó”, “Ú”, “Ý”, “à”, “è”, “ì”, “ò”, “ù”, “À”, “È”, “Ì”, “Ò”, “Ù”, “ã”, “õ”, “ñ”, “ä”, “ë”, “ï”, “ö”, “ü”, “ÿ”, “Ä”, “Ë”, “Ï”, “Ö”, “Ü”, “Ã”, “Õ”, “Ñ”, “â”, “ê”, “î”, “ô”, “û”, “Â”, “Ê”, “Î”, “Ô”, “Û”
//çÇáéíóúýÁÉÍÓÚÝàèìòùÀÈÌÒÙãõñäëïöüÿÄËÏÖÜÃÕÑâêîôûÂÊÎÔÛ

//caracter: 'q'
//  IsLower = True
//  IsLetter = True

//caracter: 'w'
//  IsLower = True
//  IsLetter = True

//caracter: 'e'
//  IsLower = True
//  IsLetter = True

//caracter: 'r'
//  IsLower = True
//  IsLetter = True

//caracter: 't'
//  IsLower = True
//  IsLetter = True

//caracter: 'y'
//  IsLower = True
//  IsLetter = True

//caracter: 'u'
//  IsLower = True
//  IsLetter = True

//caracter: 'i'
//  IsLower = True
//  IsLetter = True

//caracter: 'o'
//  IsLower = True
//  IsLetter = True

//caracter: 'p'
//  IsLower = True
//  IsLetter = True

//caracter: 'a'
//  IsLower = True
//  IsLetter = True

//caracter: 's'
//  IsLower = True
//  IsLetter = True

//caracter: 'd'
//  IsLower = True
//  IsLetter = True

//caracter: 'f'
//  IsLower = True
//  IsLetter = True

//caracter: 'g'
//  IsLower = True
//  IsLetter = True

//caracter: 'h'
//  IsLower = True
//  IsLetter = True

//caracter: 'j'
//  IsLower = True
//  IsLetter = True

//caracter: 'k'
//  IsLower = True
//  IsLetter = True

//caracter: 'l'
//  IsLower = True
//  IsLetter = True

//caracter: 'z'
//  IsLower = True
//  IsLetter = True

//caracter: 'x'
//  IsLower = True
//  IsLetter = True

//caracter: 'c'
//  IsLower = True
//  IsLetter = True

//caracter: 'v'
//  IsLower = True
//  IsLetter = True

//caracter: 'b'
//  IsLower = True
//  IsLetter = True

//caracter: 'n'
//  IsLower = True
//  IsLetter = True

//caracter: 'm'
//  IsLower = True
//  IsLetter = True

//caracter: 'Q'
//  IsUpper = True
//  IsLetter = True

//caracter: 'W'
//  IsUpper = True
//  IsLetter = True

//caracter: 'E'
//  IsUpper = True
//  IsLetter = True

//caracter: 'R'
//  IsUpper = True
//  IsLetter = True

//caracter: 'T'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Y'
//  IsUpper = True
//  IsLetter = True

//caracter: 'U'
//  IsUpper = True
//  IsLetter = True

//caracter: 'I'
//  IsUpper = True
//  IsLetter = True

//caracter: 'O'
//  IsUpper = True
//  IsLetter = True

//caracter: 'P'
//  IsUpper = True
//  IsLetter = True

//caracter: 'A'
//  IsUpper = True
//  IsLetter = True

//caracter: 'S'
//  IsUpper = True
//  IsLetter = True

//caracter: 'D'
//  IsUpper = True
//  IsLetter = True

//caracter: 'F'
//  IsUpper = True
//  IsLetter = True

//caracter: 'G'
//  IsUpper = True
//  IsLetter = True

//caracter: 'H'
//  IsUpper = True
//  IsLetter = True

//caracter: 'J'
//  IsUpper = True
//  IsLetter = True

//caracter: 'K'
//  IsUpper = True
//  IsLetter = True

//caracter: 'L'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Z'
//  IsUpper = True
//  IsLetter = True

//caracter: 'X'
//  IsUpper = True
//  IsLetter = True

//caracter: 'C'
//  IsUpper = True
//  IsLetter = True

//caracter: 'V'
//  IsUpper = True
//  IsLetter = True

//caracter: 'B'
//  IsUpper = True
//  IsLetter = True

//caracter: 'N'
//  IsUpper = True
//  IsLetter = True

//caracter: 'M'
//  IsUpper = True
//  IsLetter = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '1'
//  IsDigit = True

//caracter: '2'
//  IsDigit = True

//caracter: '3'
//  IsDigit = True

//caracter: '4'
//  IsDigit = True

//caracter: '5'
//  IsDigit = True

//caracter: '6'
//  IsDigit = True

//caracter: '7'
//  IsDigit = True

//caracter: '8'
//  IsDigit = True

//caracter: '9'
//  IsDigit = True

//caracter: '0'
//  IsDigit = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: 'ç'
//  IsLower = True
//  IsLetter = True

//caracter: 'Ç'
//  IsUpper = True
//  IsLetter = True

//caracter: 'á'
//  IsLower = True
//  IsLetter = True

//caracter: 'é'
//  IsLower = True
//  IsLetter = True

//caracter: 'í'
//  IsLower = True
//  IsLetter = True

//caracter: 'ó'
//  IsLower = True
//  IsLetter = True

//caracter: 'ú'
//  IsLower = True
//  IsLetter = True

//caracter: 'ý'
//  IsLower = True
//  IsLetter = True

//caracter: 'Á'
//  IsUpper = True
//  IsLetter = True

//caracter: 'É'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Í'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ó'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ú'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ý'
//  IsUpper = True
//  IsLetter = True

//caracter: 'à'
//  IsLower = True
//  IsLetter = True

//caracter: 'è'
//  IsLower = True
//  IsLetter = True

//caracter: 'ì'
//  IsLower = True
//  IsLetter = True

//caracter: 'ò'
//  IsLower = True
//  IsLetter = True

//caracter: 'ù'
//  IsLower = True
//  IsLetter = True

//caracter: 'À'
//  IsUpper = True
//  IsLetter = True

//caracter: 'È'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ì'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ò'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ù'
//  IsUpper = True
//  IsLetter = True

//caracter: 'ã'
//  IsLower = True
//  IsLetter = True

//caracter: 'õ'
//  IsLower = True
//  IsLetter = True

//caracter: 'ñ'
//  IsLower = True
//  IsLetter = True

//caracter: 'ä'
//  IsLower = True
//  IsLetter = True

//caracter: 'ë'
//  IsLower = True
//  IsLetter = True

//caracter: 'ï'
//  IsLower = True
//  IsLetter = True

//caracter: 'ö'
//  IsLower = True
//  IsLetter = True

//caracter: 'ü'
//  IsLower = True
//  IsLetter = True

//caracter: 'ÿ'
//  IsLower = True
//  IsLetter = True

//caracter: 'Ä'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ë'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ï'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ö'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ü'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ã'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Õ'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ñ'
//  IsUpper = True
//  IsLetter = True

//caracter: 'â'
//  IsLower = True
//  IsLetter = True

//caracter: 'ê'
//  IsLower = True
//  IsLetter = True

//caracter: 'î'
//  IsLower = True
//  IsLetter = True

//caracter: 'ô'
//  IsLower = True
//  IsLetter = True

//caracter: 'û'
//  IsLower = True
//  IsLetter = True

//caracter: 'Â'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ê'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Î'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Ô'
//  IsUpper = True
//  IsLetter = True

//caracter: 'Û'
//  IsUpper = True
//  IsLetter = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '!'
//  IsPunctuation = True

//caracter: '@'
//  IsPunctuation = True

//caracter: '#'
//  IsPunctuation = True

//caracter: '$'
//  IsSymbol = True

//caracter: '%'
//  IsPunctuation = True

//caracter: '¨'
//  IsSymbol = True

//caracter: '&'
//  IsPunctuation = True

//caracter: '*'
//  IsPunctuation = True

//caracter: '('
//  IsPunctuation = True

//caracter: ')'
//  IsPunctuation = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '-'
//  IsPunctuation = True

//caracter: '='
//  IsSymbol = True

//caracter: '_'
//  IsPunctuation = True

//caracter: '+'
//  IsSymbol = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '\'
//  IsPunctuation = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '|'
//  IsSymbol = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: ','
//  IsPunctuation = True

//caracter: '.'
//  IsPunctuation = True

//caracter: ';'
//  IsPunctuation = True

//caracter: '/'
//  IsPunctuation = True

//caracter: '<'
//  IsSymbol = True

//caracter: '>'
//  IsSymbol = True

//caracter: ':'
//  IsPunctuation = True

//caracter: '?'
//  IsPunctuation = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '~'
//  IsSymbol = True

//caracter: ']'
//  IsPunctuation = True

//caracter: '^'
//  IsSymbol = True

//caracter: '}'
//  IsPunctuation = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '´'
//  IsSymbol = True

//caracter: '['
//  IsPunctuation = True

//caracter: '`'
//  IsSymbol = True

//caracter: '{'
//  IsPunctuation = True

//caracter: ' '
//  IsWhiteSpace = True

//caracter: '°'
//  IsSymbol = True

//caracter: 'ª'
//  IsLower = True
//  IsLetter = True

//caracter: 'º'
//  IsLower = True
//  IsLetter = True

//caracter: '§'
//  IsSymbol = True

//caracter: '¬'
//  IsSymbol = True

//caracter: '¢'
//  IsSymbol = True

//caracter: '£'
//  IsSymbol = True

//caracter: '³'

//caracter: '²'

//caracter: '¹'

//caracter: ' '
//  IsWhiteSpace = True
