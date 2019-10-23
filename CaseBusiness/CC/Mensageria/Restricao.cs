#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Runtime.Caching;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class Restricao : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_Restricao_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idRestricao = Int32.MinValue;
        private String _numeroCPF_CNPJ = String.Empty;
        private String _destinatario = String.Empty;
        private String _tipoDestinatario = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataInclusao = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdRestricao
        {
            get { return _idRestricao; }
            set { _idRestricao = value; }
        }

        public String NumeroCPF_CNPJ
        {
            get { return _numeroCPF_CNPJ; }
            set { _numeroCPF_CNPJ = value; }
        }

        public String Destinatario
        {
            get { return _destinatario; }
            set { _destinatario = value; }
        }

        public String TipoDestinatario
        {
            get { return _tipoDestinatario; }
            set { _tipoDestinatario = value; }
        }

        public Int32 IdUsuarioInclusao
        {
            get { return _idUsuarioInclusao; }
            set { _idUsuarioInclusao = value; }
        }

        public DateTime DataInclusao
        {
            get { return _dataInclusao; }
            set { _dataInclusao = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Restricao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Restricao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Restricao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Restricao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Restricao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Restricao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Restricao(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Restricao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idRestricao">ID Restrição</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Restricao(Int32 idRestricao, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idRestricao);
        }
        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Inclui um Grupo / Cliente com Restrição de Envio com Restrição
        /// </summary>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="dataHoraInclusao">Data/Hora de Inclusão</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário (celular, email, etc)</param>
        public Int32 IncluirRestricao(String numeroCPF_CNPJ, 
                                      String destinatario, 
                                      DateTime dataHoraInclusao, 
                                      String tipoDestinatario)
        {
            _idRestricao = Incluir(numeroCPF_CNPJ, destinatario, dataHoraInclusao, tipoDestinatario);

            return _idRestricao;
        }

        /// <summary>
        /// Inclui um Grupo / Cliente com Restrição de Envio com Restrição
        /// </summary>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="dataHoraInclusao">Data/Hora de Inclusão</param>
        /// <returns>Id do Grupo / Cliente com Restrição de Envio de Restrição</returns>
        /// <param name="tipoDestinatario">Tipo de Destinatário (celular, email, etc)</param>
        private Int32 Incluir(String numeroCPF_CNPJ, 
                              String destinatario, 
                              DateTime dataHoraInclusao, 
                              String tipoDestinatario)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Restricao está operando em Modo Entidade Only"); }

                #region Criptografando
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (!String.IsNullOrEmpty(numeroCPF_CNPJ)) { _numeroCPF_CNPJ = pci.Codificar(numeroCPF_CNPJ.Trim()); }

                pci = null;
                #endregion Criptografando

                acessoDadosBase.AddParameter("@MGRESTR_NR_CPF_CNPJ", _numeroCPF_CNPJ);
                acessoDadosBase.AddParameter("@MGRESTR_DS_DESTINATARIO", destinatario.Trim());
                acessoDadosBase.AddParameter("@MGRESTR_TP_DESTINATARIO", tipoDestinatario.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGRESTR_DH_USU_INS", dataHoraInclusao);
                acessoDadosBase.AddParameter("@MGRESTR_ID", _idRestricao, ParameterDirection.Output);

                _idRestricao = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGRESTR_INS")[0]);

                return _idRestricao;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }        

        /// <summary>
        /// Exclui um Grupo / Cliente com Restrição de Envio com Restrição 
        /// </summary>
        /// <param name="idRestricao">Id da Restrição</param>
        public void ExcluirRestricao(Int32 idRestricao)
        {
            Excluir(idRestricao);
        }

        /// <summary>
        /// Exclui um Grupo / Cliente com Restrição de Envio com Restrição 
        /// </summary>
        /// <param name="idRestricao">Id da Restrição</param>
        private void Excluir(Int32 idRestricao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Moeda está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGRESTR_ID", idRestricao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMGRESTR_DEL");

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
        /// Remove do Cache as Moedas
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        /// <summary>
        /// Altera um Grupo / Cliente com Restrição de Envio com Restrição
        /// </summary>
        /// <param name="idRestricao">Id da Restrição</param>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário (celular, email, etc)</param>
        public Boolean AlterarRestricao(Int32 idRestricao, 
                                        String numeroCPF_CNPJ, 
                                        String destinatario,
                                        String tipoDestinatario)
        {
            Boolean _Restricao_ja_existente = false;

            _Restricao_ja_existente = Alterar(idRestricao, numeroCPF_CNPJ, destinatario, tipoDestinatario);

            return _Restricao_ja_existente;
        }

        /// <summary>
        /// Altera um Grupo / Cliente com Restrição de Envio com Restrição
        /// </summary>
        /// <param name="idRestricao">Id da Restrição</param>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário (celular, email, etc)</param>
        private Boolean Alterar(Int32 idRestricao, 
                                String numeroCPF_CNPJ, 
                                String destinatario, 
                                String tipoDestinatario)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Restricao está operando em Modo Entidade Only"); }

                Int16 _nome_Restricao_ja_existente = Int16.MinValue;

                #region Criptografando
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                numeroCPF_CNPJ = pci.Codificar(numeroCPF_CNPJ.Trim());

                pci = null;
                #endregion Criptografando

                acessoDadosBase.AddParameter("@MGRESTR_ID", idRestricao);
                acessoDadosBase.AddParameter("@MGRESTR_NR_CPF_CNPJ", numeroCPF_CNPJ);
                acessoDadosBase.AddParameter("@MGRESTR_DS_DESTINATARIO", destinatario.Trim());
                acessoDadosBase.AddParameter("@MGRESTR_TP_DESTINATARIO", tipoDestinatario.Trim());
                acessoDadosBase.AddParameter("@MGRESTR_RETORNO", _nome_Restricao_ja_existente, ParameterDirection.Output);

                _nome_Restricao_ja_existente = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGRESTR_UPD")[0]);

                if (_nome_Restricao_ja_existente > 0)
                {
                    // Grupo / Cliente com Restrição de Envio com Restrição já existe para outra Organização
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar Grupo / Cliente com Restrição de Envio com Restrição
        /// </summary>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário (celular, email, etc)</param>
        public DataTable Buscar(String numeroCPF_CNPJ, 
                                String destinatario,
                                String tipoDestinatario)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Restricao está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (!String.IsNullOrEmpty(numeroCPF_CNPJ.Trim()))
                    acessoDadosBase.AddParameter("@MGRESTR_NR_CPF_CNPJ", pci.Codificar(numeroCPF_CNPJ.Trim()));
                else
                    acessoDadosBase.AddParameter("@MGRESTR_NR_CPF_CNPJ", "");

                acessoDadosBase.AddParameter("@MGRESTR_DS_DESTINATARIO", destinatario.Trim());
                acessoDadosBase.AddParameter("@MGRESTR_TP_DESTINATARIO", tipoDestinatario.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGRESTR_SEL_BUSCAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region Ajustes
                foreach (DataRow dr in dt.Rows)
                {
                    #region DesCriptografando
                    //Descriptografar o Número do CPF/CNPJ
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF_CNPJ"])))
                    {
                        _numeroCPF_CNPJ = Convert.ToString(dr["NumeroCPF_CNPJ"]);
                        dr["NumeroCPF_CNPJ"] = pci.Decodificar(_numeroCPF_CNPJ);

                        if (String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF_CNPJ"])))
                        {
                            // ERRO DE DECRIPT
                            dr["NumeroCPF_CNPJ"] = "!!ERRO CRYPT!! " + _numeroCPF_CNPJ;
                        }
                    }
                    #endregion DesCriptografando

                    #region Mascara
                    //Colocando máscara no ddd/celular do destinatário
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Destinatario"])))
                    {
                        _destinatario = Convert.ToString(dr["Destinatario"]);
                        dr["Destinatario"] = Util.Telefone_ComMascara(Util.RemoveFormat(_destinatario), Util.enumTipoTelefone.CELULAR);
                    }
                    #endregion Mascara
                }
                #endregion Ajustes

                pci = null;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Verifica as restrições para CPF/CNPJ e destinatário antes do envio de uma mensagem
        /// </summary>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        public DataTable VerificarRestricoes(String numeroCPF_CNPJ, 
                                             String destinatario)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Restricao está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (!String.IsNullOrEmpty(numeroCPF_CNPJ))
                    acessoDadosBase.AddParameter("@MGRESTR_NR_CPF_CNPJ", pci.Codificar(numeroCPF_CNPJ.Trim()));
                else
                    acessoDadosBase.AddParameter("@MGRESTR_NR_CPF_CNPJ", "");

                acessoDadosBase.AddParameter("@MGRESTR_DS_DESTINATARIO", destinatario.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGRESTR_SEL_VERIFICAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region DesCriptografando
                foreach (DataRow dr in dt.Rows)
                {
                    //Descriptografar o Número do CPF/CNPJ
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF_CNPJ"])))
                    {
                        _numeroCPF_CNPJ = Convert.ToString(dr["NumeroCPF_CNPJ"]);
                        dr["NumeroCPF_CNPJ"] = pci.Decodificar(_numeroCPF_CNPJ);

                        if (String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF_CNPJ"])))
                        {
                            // ERRO DE DECRIPT
                            dr["NumeroCPF_CNPJ"] = "!!ERRO CRYPT!! " + _numeroCPF_CNPJ;
                        }
                    }
                }
                #endregion DesCriptografando

                pci = null;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Caso tenha restrição definitiva, retorna mensagem
        /// </summary>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        public String RestricaoDefinitiva(String numeroCPF_CNPJ, 
                                          String destinatario)
        {
            String mensagem = string.Empty;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instãncia da Classe Restricao está operando em Modo Entidade Only"); }

                DataTable dtRestricoes = VerificarRestricoes(numeroCPF_CNPJ, destinatario);

                if (dtRestricoes.Rows.Count > 0)
                {

                    if (dtRestricoes.Rows[0]["NumeroCPF_CNPJ"].ToString().Length > 0)
                    {
                        mensagem = mensagem + "CPF/CNPJ " + dtRestricoes.Rows[0]["NumeroCPF_CNPJ"].ToString() + ", ";
                    }

                    if (dtRestricoes.Rows[0]["Destinatario"].ToString().Length > 0)
                    {
                        mensagem = mensagem + "Destinatário " + dtRestricoes.Rows[0]["Destinatario"].ToString() + ", ";
                    }

                    mensagem = mensagem + "encontrado na lista restritiva. ";
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                mensagem = ex.Message;
                throw;
            }

            return mensagem;
        }

        /// <summary>
        /// Consultar uma Restricao por ID
        /// </summary>
        /// <param name="idRestricao">ID da Restrição</param>
        private void Consultar(Int32 idRestricao)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Restricao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGRESTR_ID", idRestricao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGRESTR_SEL_CONSULTARID").Tables[0];

                // Fill Object
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Listar Conexões
        /// </summary>
        //public DataTable Listar()
        //{
        //    DataTable dt = null;

        //    try
        //    {
        //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Restricao está operando em Modo Entidade Only"); }

        //        dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGRESTR_SEL_LISTAR").Tables[0];

        //        // Renomear Colunas
        //        RenomearColunas(ref dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
        //        throw;
        //    }

        //    return dt;
        //}

        /// <summary>
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                IdRestricao = Convert.ToInt32(dt.Rows[0]["MGRESTR_ID"]);

                if (dt.Rows[0]["MGRESTR_NR_CPF_CNPJ"] != DBNull.Value) { NumeroCPF_CNPJ = pci.Decodificar((String)dt.Rows[0]["MGRESTR_NR_CPF_CNPJ"]); }
                if (dt.Rows[0]["MGRESTR_DS_DESTINATARIO"] != DBNull.Value) { Destinatario = (String)dt.Rows[0]["MGRESTR_DS_DESTINATARIO"]; }
                if (dt.Rows[0]["MGRESTR_TP_DESTINATARIO"] != DBNull.Value) { TipoDestinatario = (String)dt.Rows[0]["MGRESTR_TP_DESTINATARIO"]; }

                IdUsuarioInclusao = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                DataInclusao = Convert.ToDateTime(dt.Rows[0]["MGRESTR_DH_USU_INS"]);
                
                pci = null;

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MGRESTR_ID")) { dt.Columns["MGRESTR_ID"].ColumnName = "IdRestricao"; }
            if (dt.Columns.Contains("MGRESTR_NR_CPF_CNPJ")) { dt.Columns["MGRESTR_NR_CPF_CNPJ"].ColumnName = "NumeroCPF_CNPJ"; }
            if (dt.Columns.Contains("MGRESTR_DS_DESTINATARIO")) { dt.Columns["MGRESTR_DS_DESTINATARIO"].ColumnName = "Destinatario"; }
            if (dt.Columns.Contains("MGRESTR_TP_DESTINATARIO")) { dt.Columns["MGRESTR_TP_DESTINATARIO"].ColumnName = "TipoDestinatario"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioCadastro"; }
            if (dt.Columns.Contains("MGRESTR_DH_USU_INS")) { dt.Columns["MGRESTR_DH_USU_INS"].ColumnName = "DataCadastro"; }
        }
        #endregion Métodos
    }
}

