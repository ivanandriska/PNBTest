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
    public class RestricaoLog : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_RestricaoLog_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idRestricaoLog = Int32.MinValue;
        private Int32 _idRestricao = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private String _numeroCPF_CNPJ = String.Empty;
        private String _destinatario = String.Empty;
        private String _numeroCartaoDescriptografado = String.Empty;
        private String _segmento = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataInclusao = DateTime.MinValue;
        private String _operacao = String.Empty;
        #endregion Atributos

        #region Propriedades
        public Int32 IdRestricaoLog
        {
            get { return _idRestricaoLog; }
            set { _idRestricaoLog = value; }
        }

        public Int32 IdRestricao
        {
            get { return _idRestricao; }
            set { _idRestricao = value; }
        }

        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
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

        public String NumeroCartaoDescriptografado
        {
            get { return _numeroCartaoDescriptografado; }
            set { _numeroCartaoDescriptografado = value; }
        }

        public String Segmento
        {
            get { return _segmento; }
            set { _segmento = value; }
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

        public String Operacao
        {
            get { return _operacao; }
            set { _operacao = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe RestricaoLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public RestricaoLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Configuracao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe RestricaoLog
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public RestricaoLog()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe RestricaoLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public RestricaoLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe RestricaoLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public RestricaoLog(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Conexao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idRestricaoLog">ID do Log da Configuração</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public RestricaoLog(Int32 idRestricaoLog, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idRestricaoLog);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Remove do Cache as Organizações
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        /// <summary>
        /// Inclui um Log de Grupo / Cliente com Restrição de Envio
        /// </summary>
        /// <param name="idRestricao">Id da Restrição</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="numeroCartaoDescriptografado">Número Descritografado do Cartão</param>
        /// <param name="segmento">Segmento</param>
        /// <param name="dataHoraInclusao">Data/Hora Inclusão do Log</param>
        /// <param name="operacao">Operação</param>
        public void IncluirRestricaoLog(Int32 idRestricao,
                                        Int32 codigoOrganizacao,
                                        String numeroCPF_CNPJ,
                                        String destinatario,
                                        String numeroCartaoDescriptografado,
                                        String segmento,
                                        DateTime dataHoraInclusao,
                                        String operacao)
        {
            Incluir(idRestricao, codigoOrganizacao, numeroCPF_CNPJ, destinatario, numeroCartaoDescriptografado, segmento, dataHoraInclusao, operacao);
        }

        /// <summary>
        /// Inclui um Log de Grupo / Cliente com Restrição de Envio
        /// </summary>
        /// <param name="idRestricao">Id da Restrição</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="numeroCartaoDescriptografado">Número Descritografado do Cartão</param>
        /// <param name="segmento">Segmento</param>
        /// <param name="dataHoraInclusao">Data/Hora Inclusão do Log</param>
        /// <param name="operacao">Operação</param>
        private void Incluir(Int32 idRestricao,
                             Int32 codigoOrganizacao,
                             String numeroCPF_CNPJ,
                             String destinatario,
                             String numeroCartaoDescriptografado,
                             String segmento,
                             DateTime dataHoraInclusao,
                             String operacao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe RestricaoLog está operando em Modo Entidade Only"); }

                #region Criptografando
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (!String.IsNullOrEmpty(numeroCPF_CNPJ)) { _numeroCPF_CNPJ = pci.Codificar(numeroCPF_CNPJ.Trim()); }
                if (!String.IsNullOrEmpty(numeroCartaoDescriptografado)) { _numeroCartaoDescriptografado = pci.Codificar(numeroCartaoDescriptografado.Trim()); }

                pci = null;
                #endregion Criptografando

                acessoDadosBase.AddParameter("@MGRESTR_ID", idRestricao);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGRESTRLOG_NR_CPF_CNPJ", _numeroCPF_CNPJ);
                acessoDadosBase.AddParameter("@MGRESTRLOG_DS_DESTINATARIO", destinatario.Trim());
                acessoDadosBase.AddParameter("@MGRESTRLOG_NR_CARTAO", _numeroCartaoDescriptografado);
                acessoDadosBase.AddParameter("@MGRESTRLOG_CD_SEGMENTO", segmento.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGRESTRLOG_DH_USU_INS", dataHoraInclusao);
                acessoDadosBase.AddParameter("@MGRESTRLOG_OPERACAO", operacao);

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGRESTRLOG_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar Grupo / Cliente com Restrição de Envio
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="numeroCPF_CNPJ">Número do CPF/CNPJ</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="numeroCartaoDescriptografado">Número Descritografado do Cartão</param>
        /// <param name="segmento">Segmento</param>
        public DataTable Buscar(Int32 codigoOrganizacao, 
                                String numeroCPF_CNPJ, 
                                String destinatario, 
                                String numeroCartaoDescriptografado, 
                                String segmento)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe RestricaoLog está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                if (!String.IsNullOrEmpty(numeroCPF_CNPJ))
                    acessoDadosBase.AddParameter("@MGRESTRLOG_NR_CPF_CNPJ", pci.Codificar(numeroCPF_CNPJ.Trim()));
                else
                    acessoDadosBase.AddParameter("@MGRESTRLOG_NR_CPF_CNPJ", "");

                acessoDadosBase.AddParameter("@MGRESTRLOG_DS_DESTINATARIO", destinatario.Trim());

                if (!String.IsNullOrEmpty(numeroCartaoDescriptografado))
                    acessoDadosBase.AddParameter("@MGRESTRLOG_NR_CARTAO", pci.Codificar(numeroCartaoDescriptografado.Trim()));
                else
                    acessoDadosBase.AddParameter("@MGRESTRLOG_NR_CARTAO", "");

                acessoDadosBase.AddParameter("@MGRESTRLOG_CD_SEGMENTO", segmento.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGRESTRLOG_SEL_BUSCAR").Tables[0];

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

                    //Descriptografar o Número do Cartão
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["NumeroCartaoDescriptografado"])))
                    {
                        _numeroCartaoDescriptografado = Convert.ToString(dr["NumeroCartaoDescriptografado"]);
                        dr["NumeroCartaoDescriptografado"] = pci.Decodificar(_numeroCartaoDescriptografado);

                        if (String.IsNullOrEmpty(Convert.ToString(dr["NumeroCartaoDescriptografado"])))
                        {
                            // ERRO DE DECRIPT
                            dr["NumeroCartaoDescriptografado"] = "!!ERRO CRYPT!! " + _numeroCartaoDescriptografado;
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
        /// Consultar uma Restricao por ID
        /// </summary>
        /// <param name="idRestricao">ID da Restrição</param>
        private void Consultar(Int32 idRestricao)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Restricao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGRESTRLOG_ID", idRestricao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGRESTRLOG_SEL_CONSULTARID").Tables[0];

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
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                IdRestricaoLog = Convert.ToInt32(dt.Rows[0]["MGRESTRLOG_ID"]);
                IdRestricao = Convert.ToInt32(dt.Rows[0]["MGRESTR_ID"]);
                if (dt.Rows[0]["ORG_CD"] != DBNull.Value) { CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]); }
                if (dt.Rows[0]["MGRESTRLOG_NR_CPF_CNPJ"] != DBNull.Value) { NumeroCPF_CNPJ = pci.Decodificar((String)dt.Rows[0]["MGRESTRLOG_NR_CPF_CNPJ"]); }
                if (dt.Rows[0]["MGRESTRLOG_DS_DESTINATARIO"] != DBNull.Value) { Destinatario = (String)dt.Rows[0]["MGRESTRLOG_DS_DESTINATARIO"]; }
                if (dt.Rows[0]["MGRESTRLOG_NR_CARTAO"] != DBNull.Value) { NumeroCartaoDescriptografado = pci.Decodificar((String)dt.Rows[0]["MGRESTRLOG_NR_CARTAO"]); }
                if (dt.Rows[0]["MGRESTRLOG_CD_SEGMENTO"] != DBNull.Value) { Segmento = (String)dt.Rows[0]["MGRESTRLOG_CD_SEGMENTO"]; }
                IdUsuarioInclusao = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                DataInclusao = Convert.ToDateTime(dt.Rows[0]["MGRESTRLOG_DH_USU_INS"]);
                Operacao = (String)dt.Rows[0]["MGRESTRLOG_OPERACAO"];

                pci = null;

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MGRESTRLOG_ID")) { dt.Columns["MGRESTRLOG_ID"].ColumnName = "IdRestricaoLog"; }
            if (dt.Columns.Contains("MGRESTR_ID")) { dt.Columns["MGRESTR_ID"].ColumnName = "IdRestricao"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "Organizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("MGRESTRLOG_NR_CPF_CNPJ")) { dt.Columns["MGRESTRLOG_NR_CPF_CNPJ"].ColumnName = "NumeroCPF_CNPJ"; }
            if (dt.Columns.Contains("MGRESTRLOG_DS_DESTINATARIO")) { dt.Columns["MGRESTRLOG_DS_DESTINATARIO"].ColumnName = "Destinatario"; }
            if (dt.Columns.Contains("MGRESTRLOG_NR_CARTAO")) { dt.Columns["MGRESTRLOG_NR_CARTAO"].ColumnName = "NumeroCartaoDescriptografado"; }
            if (dt.Columns.Contains("MGRESTRLOG_CD_SEGMENTO")) { dt.Columns["MGRESTRLOG_CD_SEGMENTO"].ColumnName = "Segmento"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioInclusao"; }
            if (dt.Columns.Contains("MGRESTRLOG_DH_USU_INS")) { dt.Columns["MGRESTRLOG_DH_USU_INS"].ColumnName = "DataInclusao"; }
            if (dt.Columns.Contains("MGRESTRLOG_OPERACAO")) { dt.Columns["MGRESTRLOG_OPERACAO"].ColumnName = "Operacao"; }
        }
        #endregion Métodos
    }
}
