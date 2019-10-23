using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseBusiness.Framework.BancoDados;
using System.Data;


namespace CaseBusiness.CC.Admin
{
    public class ExecutarSQL: BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Admin_ExecutarSQL_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idObjeto = Int32.MinValue;
        private String _tipoObjeto = String.Empty;
        private String _nomeObjeto = String.Empty;
        private DateTime _dataInclusao = DateTime.MinValue;
        private DateTime _dataAlteracao = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdObjeto
        {
            get { return _idObjeto; }
            set { _idObjeto = value; }
        }
        public String TipoObjeto
        {
            get { return _nomeObjeto; }
            set { _nomeObjeto = value; }
        }

        public String NomeObjeto
        {
            get { return _nomeObjeto; }
            set { _nomeObjeto = value; }
        }

        public DateTime DataInclusao
        {
            get { return _dataInclusao; }
            set { _dataInclusao = value; }
        }

        public DateTime DataAlteracao
        {
            get { return _dataAlteracao; }
            set { _dataAlteracao = value; }
        }        
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe ExecutarSQL - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public ExecutarSQL(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe ExecutarSQL
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ExecutarSQL(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe ExecutarSQL utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public ExecutarSQL(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe ExecutarSQL e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idObjeto">Id do Objeto</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ExecutarSQL(Int32 idObjeto, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idObjeto);
        }

        /// <summary>
        /// Construtor classe ExecutarSQL e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="tipoObjeto">Tipo do Objeto  (PROCEURE, TABLE, ETC)</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ExecutarSQL(String tipoObjeto, Int32 idUsuarioManutencao) 
            : this(idUsuarioManutencao)
        {
            Consultar(tipoObjeto);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Consulta uma ExecutarSQL por ID
        /// </summary>
        /// <param name="idObjeto">Id do Objeto</param>
        private void Consultar(Int32 idObjeto)
        {
            //try
            //{
            //    DataTable dt = null;

            //    if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ExecutarSQL está operando em Modo Entidade Only"); }

            //    acessoDadosBase.AddParameter("@OBJECT_ID", idObjeto);

            //    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prADM_SEL_CONSULTARID").Tables[0];

            //    // Fill objetct
            //    PreencherAtributos(ref dt);
            //}
            //catch (Exception ex)
            //{
            //    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            //    throw;
            //}
        }

        /// <summary>
        /// Consulta uma ExecutarSQL por ID
        /// </summary>
        /// <param name="tipoObjeto">Tipo de Objeto (PROCEURE, TABLE, ETC)</param>
        private void Consultar(String tipoObjeto)
        {
            //try
            //{
            //    DataTable dt = null;

            //    if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ExecutarSQL está operando em Modo Entidade Only"); }

            //    acessoDadosBase.AddParameter("@OBJECT_TYPE", tipoObjeto);

            //    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prADM_SEL_CONSULTARTP").Tables[0];

            //    // Fill objetct
            //    PreencherAtributos(ref dt);
            //}
            //catch (Exception ex)
            //{
            //    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            //    throw;
            //}
        }

        /// <summary>
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["OBJECT_ID"] != DBNull.Value) { IdObjeto = Convert.ToInt32(dt.Rows[0]["OBJECT_ID"]); }
                if (dt.Rows[0]["OBJECT_TYPE"] != DBNull.Value) { TipoObjeto = (String)dt.Rows[0]["OBJECT_TYPE"]; }
                if (dt.Rows[0]["OBJECT_NAME"] != DBNull.Value) { NomeObjeto = (String)dt.Rows[0]["OBJECT_NAME"]; }
                if (dt.Rows[0]["CREATE_DATE"] != DBNull.Value) { DataInclusao = Convert.ToDateTime(dt.Rows[0]["CREATE_DATE"]); }
                if (dt.Rows[0]["MODIFY_DATE"] != DBNull.Value) { DataAlteracao = Convert.ToDateTime(dt.Rows[0]["MODIFY_DATE"]); }

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Buscar Ojbetos (Procedure e Table) do Banco de Dados
        /// </summary>
        /// <param name="tipoObjeto">Tipo de Ojbeto (Ex.: TODOS, PROCEDURE, TABLE, ETC</param>
        /// <param name="nomeObjeto">Nome do Objeto</param>
        /// <returns>Retorna o Nome das Procedures e/ou Table</returns>
        public DataTable BuscarObjetos(String tipoObjeto, 
                                       String nomeObjeto)
        {
            DataTable dt = null;
            String owner = String.Empty;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ExecutarSQL está operando em Modo Entidade Only"); }

                owner = CaseBusiness.Framework.Configuracao.Configuracao.BancosDisponiveis[Framework.BancoDeDados.Case].Usuario.Trim().ToUpper();

                acessoDadosBase.AddParameter("@OBJECT_TYPE", tipoObjeto.Trim());
                acessoDadosBase.AddParameter("@OBJECT_NAME", nomeObjeto.Trim().ToUpper());
                acessoDadosBase.AddParameter("@OWNER", owner);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prADM_SEL_BUSCAR_OBJETOS").Tables[0];

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
        /// Executa a Instrução SQL informada na tela
        /// </summary>
        /// <param name="stringSQL">String SQL</param>
        /// <returns>Retorna uma consulta do banco de dados</returns>
        public DataTable BuscarSQL(String stringSQL)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instãncia da classe ExecutarSQL está operando em Modo Entidade Only"); }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.Text,
                                                    stringSQL.Trim()).Tables[0];

                // Renomear Colunas
                // Obs.: As colunas são geradas automaticamente conforme a instrução SQL informada
            } 
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Listar os Objetos
        /// </summary>
        /// <param name="tipoObjeto">Tipo de Ojbeto (Ex.: TODOS, PROCEDURE, TABLE, ETC</param>
        /// <param name="nomeObjeto">Nome do Objeto</param>
        /// <returns></returns>
        public DataTable Listar(String tipoObjeto,
                                String nomeObjeto)
        {
            DataTable dt = null;

            //try
            //{
            //    if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ExecutarSQL está operando em Modo Entidade Only"); }
                
            //    acessoDadosBase.AddParameter("@OBJECT_TYPE", tipoObjeto);
            //    acessoDadosBase.AddParameter("@OBJECT_NAME", nomeObjeto);

            //    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
            //                                        "prADM_SEL_LISTAR").Tables[0];

            //    // Renomear Colunas
            //    RenomearColunas(ref dt);
            //}
            //catch (Exception ex)
            //{
            //    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            //    throw;
            //}

            return dt;
        }


        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("OBJECT_ID")) { dt.Columns["OBJECT_ID"].ColumnName = "IdObjeto"; }
            if (dt.Columns.Contains("OBJECT_TYPE")) { dt.Columns["OBJECT_TYPE"].ColumnName = "TipoObjeto"; }
            if (dt.Columns.Contains("OBJECT_NAME")) { dt.Columns["OBJECT_NAME"].ColumnName = "NomeObjeto"; }
            if (dt.Columns.Contains("CREATE_DATE")) { dt.Columns["CREATE_DATE"].ColumnName = "DataInclusao"; }
            if (dt.Columns.Contains("MODIFY_DATE")) { dt.Columns["MODIFY_DATE"].ColumnName = "DataAlteracao"; }
        }
        #endregion Métodos
    }
}
