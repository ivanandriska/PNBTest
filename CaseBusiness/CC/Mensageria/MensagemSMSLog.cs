using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;

namespace CaseBusiness.CC.Mensageria
{
    public class MensagemSMSLog : BusinessBase
    {
        #region Atributos
        private Int32 _idFornecedora = Int32.MaxValue;
        private String _idMensagemSMS = String.Empty;
        private DateTime _dataHoraInclusao = DateTime.MinValue;
        private String _descricao = String.Empty;
        private String _respostaCliente = String.Empty;
        private Int32 _idMensagemSMSlog = Int32.MinValue;
        private String _processo = String.Empty;
        private String _statusMensagem = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private String _codigoRetorno = String.Empty;
        private DateTime _dataHoraEventoRetorno = DateTime.MinValue;
        private String _descricaoOperadora = String.Empty;
        #endregion

        #region Propriedades
        public Int32 IdFornecedora
        {
            get { return _idFornecedora; }
            set { _idFornecedora = value; }
        }

        public String IdMensagemSMS
        {
            get { return _idMensagemSMS; }
            set { _idMensagemSMS = value; }
        }

        public DateTime DataHoraInclusao
        {
            get { return _dataHoraInclusao; }
            set { _dataHoraInclusao = value; }
        }

        public String Descricao
        {
            get { return _descricao; }
            set { _descricao = value; }
        }

        public String RespostaCliente
        {
            get { return _respostaCliente; }
            set { _respostaCliente = value; }
        }

        public Int32 IdMensagemSMSlog
        {
            get { return _idMensagemSMSlog; }
            set { _idMensagemSMSlog = value; }
        }

        public String Processo
        {
            get { return _processo; }
            set { _processo = value; }
        }

        public String StatusMensagem
        {
            get { return _statusMensagem; }
            set { _statusMensagem = value; }
        }

        public Int32 IdUsuarioInclusao
        {
            get { return _idUsuarioInclusao; }
            set { _idUsuarioInclusao = value; }
        }

        public String CodigoRetorno
        {
            get { return _codigoRetorno; }
            set { _codigoRetorno = value; }
        }

        public DateTime DataHoraEventoRetorno
        {
            get { return _dataHoraEventoRetorno; }
            set { _dataHoraEventoRetorno = value; }
        }

        public String DescricaoOperadora
        {
            get { return _descricaoOperadora; }
            set { _descricaoOperadora = value; }
        }
        #endregion

        #region Construtores
        /// <summary>
        /// Construtor classe MensagemSMSLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public MensagemSMSLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe MensagemSMSLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public MensagemSMSLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe MensagemSMSLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public MensagemSMSLog(Int32 idUsuarioManutencao,
                              CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion


        /// <summary>
        /// Inclui o log de execução de processo para uma mensagem
        /// </summary>
        /// <param name="idMensagemSMS">Id da mensagem</param>
        /// <param name="descricao">Descrição do status</param>
        /// <param name="respostaCliente">Texto da resposta do cliente</param>
        /// <param name="processo">Processo que está registrando o status</param>
        /// <param name="statusMensagem">Status interno da mensagem</param>
        /// <param name="idFornecedora">ID da fornecedora utilizada para enviar a mensagem</param>
        /// <param name="dataHoraInclusao">Data/Hora inclusão registro</param>
        /// <param name="dataHoraEvento">Data/Hora que ocorreu o evento (Data/Hora interna)</param>
        /// <param name="DataHoraRetorno">Data/Hora que ocorreu evento na entidade externa</param>
        /// <param name="codigoRetorno">Código de retorno na entidade externa</param>
        /// <param name="Operadora">Codigo/Nome operadora do celular</param>
        public void Incluir(String idMensagemSMS,
                            String descricao,
                            String respostaCliente,
                            String processo,
                            String statusMensagem,
                            Int32 idFornecedora,
                            DateTime dataHoraInclusao,
                            DateTime dataHoraEvento,
                            DateTime DataHoraRetorno,
                            String codigoRetorno,
                            String Operadora)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemSMSLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagemSMS);
                acessoDadosBase.AddParameter("@MGSMSLOG_DS", descricao.Trim());
                acessoDadosBase.AddParameter("@MGSMSLOG_DS_RESPOSTA", respostaCliente.Trim());
                acessoDadosBase.AddParameter("@MGMSMPROC_PROC", processo.Trim());
                acessoDadosBase.AddParameter("@MGMSGST_ST", statusMensagem.Trim());
                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGSMSLOG_DH", dataHoraInclusao);
                acessoDadosBase.AddParameter("@MGSMSLOG_DS_OPERADORA", Operadora);
                acessoDadosBase.AddParameter("@MGSMSLOG_CD_RETORNO", codigoRetorno);
                acessoDadosBase.AddParameter("@MGSMSLOG_DH_EVENTO", dataHoraEvento);
                acessoDadosBase.AddParameter("@MGSMSLOG_DH_RETORNO", DataHoraRetorno);



                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMGSMSLOG_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        //TODO: verificar se terá essa funcionalidade
        /// <summary>
        /// Verifica se tem mensagem enviada no intervalo para envio de mensagem
        /// </summary>
        /// <param name="CPF_CNPJ_Descriptografado">CPF/CNPJ Descriptografado</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário</param>
        /// <param name="quantidadeMinutos">Quantidade em Minutos</param>
        /// <returns>Quantidade de mensagem enviada no intervalo mencionado</returns>
        public Int32 IntervaloMensagem(String CPF_CNPJ_Descriptografado,
                                       String destinatario,
                                       String tipoDestinatario,
                                       Int32 quantidadeMinutos)
        {
            Int32 qtdeMensagens = 0;
            String CPF_CNPJ_Criptografado = String.Empty;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemSMSLog está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (!String.IsNullOrEmpty(CPF_CNPJ_Descriptografado))
                {
                    CPF_CNPJ_Criptografado = pci.Codificar(CPF_CNPJ_Descriptografado.Trim());
                }

                acessoDadosBase.AddParameter("@MGSMS_NR_CPF", CPF_CNPJ_Criptografado);
                acessoDadosBase.AddParameter("@MGSMS_DS_DEST", destinatario.Trim());
                acessoDadosBase.AddParameter("@MGSMS_TP_DEST", tipoDestinatario.Trim());
                acessoDadosBase.AddParameter("@QTD_MINUTOS", quantidadeMinutos);
                acessoDadosBase.AddParameter("@QTD_MENSAGEM", qtdeMensagens, ParameterDirection.InputOutput);

                qtdeMensagens = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGSMSLOG_SEL_BUSCAR_INT_MSG")[0]);

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return qtdeMensagens;
        }

        //TODO: verificar se terá essa funcionalidade
        /// <summary>
        /// Quantidade de mensagens que foram enviadas de uma mesma transação nas últimas 24h
        /// </summary>
        /// <param name="tipoDestinatario">Tipo de Destinatário (CLIEN = Cliente ou MONIT = Monitoria</param>
        /// <param name="cpfCnpj">CPF ou CNPJ do Cliente</param>
        /// <param name="tipoPesquisa">Tipo de Pesquisa (TRANSACAO ou CLIENTE</param>
        /// <param name="idGrupoTeste">Id do Grupo de Teste</param>
        /// <param name="quantidadeMinutos">Quantidade em Minutos (60 (minutos) * 24 (horas) = 1440 minutos)</param>
        /// <returns>Quantidade de mensagem enviada</returns>
        public Int32 QuantidadeMensagensEnviada(String tipoDestinatario,
                                                String CPF_CNPJ_Descriptografado,
                                                String tipoPesquisa,
                                                Int32 idGrupoTeste,
                                                Int32 quantidadeMinutos)
        {
            Int32 qtdeMensagens = 0;
            String CPF_CNPJ_Criptografado = String.Empty;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemSMSLog está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (!String.IsNullOrEmpty(CPF_CNPJ_Descriptografado))
                {
                    CPF_CNPJ_Criptografado = pci.Codificar(CPF_CNPJ_Descriptografado.Trim());
                }

                acessoDadosBase.AddParameter("@MGCONFMSG_TP_DEST", tipoDestinatario.Trim());
                acessoDadosBase.AddParameter("@MGSMS_NR_CPF", CPF_CNPJ_Criptografado);
                acessoDadosBase.AddParameter("@TIPO_PESQUISA", tipoPesquisa.Trim());
                acessoDadosBase.AddParameter("@MGGRPTST_ID", idGrupoTeste);
                acessoDadosBase.AddParameter("@QTD_MINUTOS", quantidadeMinutos);
                acessoDadosBase.AddParameter("@QTD_MENSAGEM", qtdeMensagens, ParameterDirection.InputOutput);

                qtdeMensagens = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGSMSLOG_SEL_BUSCAR_QTD_MSG")[0]);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return qtdeMensagens;
        }

        /// <summary>
        /// Listar Log de Mensagem de SMS
        /// </summary>
        /// <param name="idMensagemSMS">Id Mensagem</param>
        public DataTable Listar(String idMensagemSMS)
        {
            DataTable dt;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemSMSLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagemSMS);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGSMSLOG_SEL_LISTAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {            
            //if (dt.Columns.Contains("COMFORN_ID")) { dt.Columns["COMFORN_ID"].ColumnName = "IdFornecedora"; }
            if (dt.Columns.Contains("MGSMS_ID")) { dt.Columns["MGSMS_ID"].ColumnName = "IdMensagemSMS"; }
            if (dt.Columns.Contains("MGSMSLOG_DH")) { dt.Columns["MGSMSLOG_DH"].ColumnName = "DataHoraInclusao"; }
            if (dt.Columns.Contains("MGSMSLOG_DS")) { dt.Columns["MGSMSLOG_DS"].ColumnName = "Descricao"; }
            if (dt.Columns.Contains("MGSMSLOG_DS_RESPOSTA")) { dt.Columns["MGSMSLOG_DS_RESPOSTA"].ColumnName = "RespostaCliente"; }
            if (dt.Columns.Contains("MGSMSLOG_ID")) { dt.Columns["MGSMSLOG_ID"].ColumnName = "IdMensagemSMSlog"; }
            if (dt.Columns.Contains("MGMSMPROC_PROC")) { dt.Columns["MGMSMPROC_PROC"].ColumnName = "Processo"; }
            //if (dt.Columns.Contains("MGMSGST_ST")) { dt.Columns["MGMSGST_ST"].ColumnName = "StatusMensagem"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            //if (dt.Columns.Contains("MGSMSLOG_CD_RETORNO")) { dt.Columns["MGSMSLOG_CD_RETORNO"].ColumnName = "CodigoRetorno"; }
            //if (dt.Columns.Contains("MGSMSLOG_DH_EVENTO_RET")) { dt.Columns["MGSMSLOG_DH_EVENTO_RET"].ColumnName = "DataHoraEventoRetorno"; }
            //if (dt.Columns.Contains("MGSMSLOG_DS_OPERADORA")) { dt.Columns["MGSMSLOG_DS_OPERADORA"].ColumnName = "DescricaoOperadora"; }
            if (dt.Columns.Contains("MGMSGPROC_DS")) { dt.Columns["MGMSGPROC_DS"].ColumnName = "DescricaoProcesso"; }
            if (dt.Columns.Contains("MGMSGST_DS")) { dt.Columns["MGMSGST_DS"].ColumnName = "DescricaoStatus"; }
            //if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioResponsavel"; }
        }
    }
}
