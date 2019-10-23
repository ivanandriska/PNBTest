#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Concurrent;
using CaseBusiness.Framework.BancoDados;
using CaseBusiness.ISO.MISO8583.Parsing;
#endregion Using

namespace CaseBusiness.CC.Global
{
    public class MensagemInterfaceHeader : BusinessBase
    {
        #region Atributos
        private String _idMensagemInterfaceHeader = String.Empty;
        private String _tipoFormato = String.Empty;
        private CaseBusiness.ISO.EncodingType _tipoCodificacao = ISO.EncodingType.NotDefined;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataHoraInclusao = DateTime.MinValue;
        private Int32 _idUsuarioAlteracao = Int32.MinValue;
        private DateTime _dataHoraAlteracao = DateTime.MinValue;
        private ConcurrentDictionary<String, MensagemInterfaceDetalhe> _mapaPalavraDados = null;
        private ConcurrentDictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo> _mapaCamposVAR = null;
        private ConcurrentDictionary<Int32, MensagemInterfaceDetalhe> _mapaCamposFIXO = null;
        private ConcurrentDictionary<string, MensagemInterfaceDetalhe> _mapaPalavraStatus = null;
        #endregion Atributos

        #region Propriedades
        public String IdMensagemInterfaceHeader
        {
            get { return _idMensagemInterfaceHeader; }
            set { _idMensagemInterfaceHeader = value; }
        }

        public String TipoFormato
        {
            get { return _tipoFormato; }
            set { _tipoFormato = value; }
        }

        public CaseBusiness.ISO.EncodingType TipoCodificacao
        {
            get { return _tipoCodificacao; }
            set { _tipoCodificacao = value; }
        }

        public Int32 IdUsuarioInclusao
        {
            get { return _idUsuarioInclusao; }
            set { _idUsuarioInclusao = value; }
        }

        public DateTime DataHoraInclusao
        {
            get { return _dataHoraInclusao; }
            set { _dataHoraInclusao = value; }
        }

        public Int32 IdUsuarioAlteracao
        {
            get { return _idUsuarioAlteracao; }
            set { _idUsuarioAlteracao = value; }
        }

        public DateTime DataHoraAlteracao
        {
            get { return _dataHoraAlteracao; }
            set { _dataHoraAlteracao = value; }
        }

        public ConcurrentDictionary<string, MensagemInterfaceDetalhe> MapaPalavraDados
        {
            get { return _mapaPalavraDados; }
            set { _mapaPalavraDados = value; }
        }

        public ConcurrentDictionary<Int32, FieldParseInfo> MapaCamposVAR
        {
            get { return _mapaCamposVAR; }
            set { _mapaCamposVAR = value; }
        }

        public ConcurrentDictionary<Int32, MensagemInterfaceDetalhe> MapaCamposFIXO
        {
            get { return _mapaCamposFIXO; }
            set { _mapaCamposFIXO = value; }
        }

        public ConcurrentDictionary<string, MensagemInterfaceDetalhe> MapaPalavraStatus
        {
            get { return _mapaPalavraStatus; }
            set { _mapaPalavraStatus = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe InterfaceHeader - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public MensagemInterfaceHeader(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe InterfaceHeader
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public MensagemInterfaceHeader(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe InterfaceHeader utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public MensagemInterfaceHeader(Int32 idUsuarioManutencao,
                             CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Métodos
        public ConcurrentDictionary<string, MensagemInterfaceHeader> Listar()
        {
            DataTable dtInterfaceHeader = null;
            ConcurrentDictionary<string, MensagemInterfaceHeader> retorno = new ConcurrentDictionary<string, MensagemInterfaceHeader>();

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemInterfaceHeader está operando em Modo Entidade Only"); }

                dtInterfaceHeader = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                   "prMSGITFHEAD_SEL_LISTAR").Tables[0];
                // Renomear Colunas
                RenomearColunas(ref dtInterfaceHeader);

                //Montar retorno
                for (int i=0; i< dtInterfaceHeader.Rows.Count; i++)
                {
                    MensagemInterfaceHeader interfaceHeader = new MensagemInterfaceHeader(UsuarioManutencao.ID);
                    interfaceHeader.IdMensagemInterfaceHeader = dtInterfaceHeader.Rows[i]["IdMensagemInterfaceHeader"].ToString();
                    interfaceHeader.TipoFormato = dtInterfaceHeader.Rows[i]["TipoFormato"].ToString();
                    interfaceHeader.TipoCodificacao = (dtInterfaceHeader.Rows[i]["TipoCodificacao"].ToString() == "EBCDIC" ? ISO.EncodingType.EBCDIC : ISO.EncodingType.ASCII);
                    interfaceHeader.MapaPalavraDados = new CaseBusiness.CC.Global.MensagemInterfaceDetalhe(UsuarioManutencao.ID).ListarPalavraDados(interfaceHeader.IdMensagemInterfaceHeader);
                    interfaceHeader.MapaPalavraStatus = new CaseBusiness.CC.Global.MensagemInterfaceDetalhe(UsuarioManutencao.ID).ListarPalavraStatus(interfaceHeader.IdMensagemInterfaceHeader);
                    interfaceHeader.MapaCamposVAR = interfaceHeader.TipoFormato.Equals("VAR") ? new CaseBusiness.CC.Global.MensagemInterfaceDetalhe(UsuarioManutencao.ID).ListarMapaVar(interfaceHeader.IdMensagemInterfaceHeader, interfaceHeader.TipoFormato) : null;
                    interfaceHeader.MapaCamposFIXO = interfaceHeader.TipoFormato.Equals("FIXO") ? new CaseBusiness.CC.Global.MensagemInterfaceDetalhe(UsuarioManutencao.ID).ListarMapaFixo(interfaceHeader.IdMensagemInterfaceHeader, interfaceHeader.TipoFormato) : null;

                    retorno.TryAdd(dtInterfaceHeader.Rows[i]["IdMensagemInterfaceHeader"].ToString(), interfaceHeader);
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return retorno;
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MSGITFHEAD_ID")) { dt.Columns["MSGITFHEAD_ID"].ColumnName = "IdMensagemInterfaceHeader"; }
            if (dt.Columns.Contains("MSGITFHEAD_TP_FORMATO")) { dt.Columns["MSGITFHEAD_TP_FORMATO"].ColumnName = "TipoFormato"; }
            if (dt.Columns.Contains("MSGITFHEAD_TP_CODIFICACAO")) { dt.Columns["MSGITFHEAD_TP_CODIFICACAO"].ColumnName = "TipoCodificacao"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "CodigoUsuarioInclusao"; }
            if (dt.Columns.Contains("MSGITFHEAD_DH_USUARIO_INS")) { dt.Columns["MSGITFHEAD_DH_USUARIO_INS"].ColumnName = "DataHoraInclusao"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "CodigoUsuarioManutencao"; }
            if (dt.Columns.Contains("MSGITFHEAD_DH_USUARIO_UPD")) { dt.Columns["MSGITFHEAD_DH_USUARIO_UPD"].ColumnName = "DataHoraManutencao"; }
        }
        #endregion Métodos
    }
}
