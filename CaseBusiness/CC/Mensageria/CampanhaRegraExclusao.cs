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
    public class CampanhaRegraExclusao : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_CampanhaRegraExclusaos_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idCampanhaRegraExclusaos = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private String _numeroCPF_CNPJ = String.Empty;
        private String _destinatario = String.Empty;
        private String _numeroCartaoDescriptografado = String.Empty;
        private String _segmento = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataInclusao = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdCampanhaRegraExclusaos
        {
            get { return _idCampanhaRegraExclusaos; }
            set { _idCampanhaRegraExclusaos = value; }
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
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Campanhas - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public CampanhaRegraExclusao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Campanhas com o DB, gerando um Erro)
        }

        public CampanhaRegraExclusao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Campanhas utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public CampanhaRegraExclusao(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        #endregion Construtores

        #region Métodos

        // <summary>
        /// Listar Regras de Exclusão
        /// </summary>           
        public DataTable Listar()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe CampanhaRegraExclusaos está operando em Modo Entidade Only"); }
                      
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMPRGEXC_SEL_LISTAR").Tables[0];

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

        // <summary>
        /// Buscar Regra de Exclusão por Idcampanha
        /// </summary>
        /// <param name="IdCampanha">Id Campanha</param>       
        public DataTable Buscar( Int32 idCampanha)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe CampanhaRegraExclusaos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMPRGEXC_SEL_BUSCAR").Tables[0];

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
        /// Inclui uma Regra Exclusão
        /// </summary>
        /// <param name="idRegraExclusao">Id REgra Exclusão</param>
        /// <param name="idCampanha">Id Campanha</param>        
        public void Incluir(Int32 idRegraExclusao,
                            Int32 idCampanha)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPRGEXC_ID", idRegraExclusao);
                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPRGEXCCMP_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Inclui uma Regra Exclusão
        /// </summary>
        /// <param name="idRegraExclusao">Id REgra Exclusão</param>
        /// <param name="idCampanha">Id Campanha</param>
        public void Alterar(Int32 idRegraExclusao,
                            Int32 idCampanha)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPRGEXC_ID", idRegraExclusao);
                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPRGEXCCMP_UPD");

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Excluir uma Regra Exclusão
        /// </summary>        
        /// <param name="idCampanha">Id Campanha</param>
        public void Excluir(Int32 idRegraExclusao, Int32 idCampanha)
        {            
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe QualificadorDefinicao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPRGEXC_ID", idRegraExclusao);
                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPRGEXCCMP_DEL");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }            
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
 
            if (dt.Columns.Contains("CMPRGEXC_ID")) { dt.Columns["CMPRGEXC_ID"].ColumnName = "IdRegraExclusao"; }
            if (dt.Columns.Contains("CMPRGEXC_DS")) { dt.Columns["CMPRGEXC_DS"].ColumnName = "DescricaoRegraExclusao"; }           

        }
        #endregion Métodos
    }
}

