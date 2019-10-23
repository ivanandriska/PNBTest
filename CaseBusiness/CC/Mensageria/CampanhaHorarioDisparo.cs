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
    public class CampanhaHorarioDisparo : BusinessBase
    {

        #region Atributos
        private Int32 _idCampanha = Int32.MinValue;
        private Int32 _idCanal = Int32.MinValue;
        private DateTime _horarioSegunda = DateTime.MinValue;
        private DateTime _horarioTerca = DateTime.MinValue;
        private DateTime _horarioQuarta = DateTime.MinValue;
        private DateTime _horarioQuinta = DateTime.MinValue;
        private DateTime _horarioSexta = DateTime.MinValue;
        private DateTime _horarioSabado = DateTime.MinValue;
        private DateTime _horarioDomingo = DateTime.MinValue;
        private DateTime _horarioFeriado = DateTime.MinValue;

        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 120;  // 60 minutos * 24 horas
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 30;  // 60 minutos * 24 horas
        #endregion Atributos

        #region Propriedades
        public Int32 IdCampanha
        {
            get { return _idCampanha; }
            set { _idCampanha = value; }
        }
        public Int32 IdCanal
        {
            get { return _idCanal; }
            set { _idCanal = value; }
        }
        public DateTime HorarioSegunda
        {
            get { return _horarioSegunda; }
            set { _horarioSegunda = value; }
        }
        public DateTime HorarioTerca
        {
            get { return _horarioTerca; }
            set { _horarioTerca = value; }
        }
        public DateTime HorarioQuarta
        {
            get { return _horarioQuarta; }
            set { _horarioQuarta = value; }
        }
        public DateTime HorarioQuinta
        {
            get { return _horarioQuinta; }
            set { _horarioQuinta = value; }
        }
        public DateTime HorarioSexta
        {
            get { return _horarioSexta; }
            set { _horarioSexta = value; }
        }
        public DateTime HorarioSabado
        {
            get { return _horarioSabado; }
            set { _horarioSabado = value; }
        }
        public DateTime HorarioDomingo
        {
            get { return _horarioDomingo; }
            set { _horarioDomingo = value; }
        }

        public DateTime HorarioFeriado
        {
            get { return _horarioFeriado; }
            set { _horarioFeriado = value; }
        }

        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Configuracao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public CampanhaHorarioDisparo(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Configuracao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public CampanhaHorarioDisparo()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public CampanhaHorarioDisparo(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public CampanhaHorarioDisparo(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Campanhas e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idCampanhas">ID Restrição</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public CampanhaHorarioDisparo(Int32 idCampanhas, Int32 idUsuarioManutencao)
           : this(idUsuarioManutencao)
        {
            Consultar(idCampanhas);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar Configuração de Envio
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id da Canal</param>
        public DataTable Buscar(Int32 idcampanha, Int32 idCanal)
        {
            DataTable dt = null;
            String kCacheKey = "CampanhaHorarioDisparo_" + idcampanha.ToString() + idCanal.ToString();

            try
            {
                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dt = MemoryCache.Default[kCacheKey] as DataTable;
                    return dt;
                }

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idcampanha);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "PRCMPHRDISP_SEL_BUSCAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                MemoryCache.Default.Set(kCacheKey, dt,
                new CacheItemPolicy()
                {
                    SlidingExpiration = new TimeSpan(DateTime.Now.AddMinutes(kCache_SLIDINGEXPIRATION_MINUTES).Ticks - DateTime.Now.Ticks)
                });
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Incluir Hora Disparo
        /// </summary>
        /// <param name="idCampanha">Id Campanha</param>
        /// <param name="IdConfiguracaoCanal">Id Configuração Canal</param>
        /// <param name="dhSegunda">Data/hora Segunda</param>
        /// <param name="dhTerca">Data/hora Terça</param>
        /// <param name="dhQuarta">Data/hora Quarta</param>
        /// <param name="dhQuinta">Data/hora Quinta</param>
        /// <param name="dhSexta">Data/hora Sexta</param>
        /// <param name="dhSabado">Data/hora Sabado</param>
        /// <param name="dhDomingo">Data/hora Domingo</param>
        public void Incluir(Int32 idCampanha,
                            Int32 idComunicacaoCanal,
                            DateTime dhSegunda,
                            DateTime dhTerca,
                            DateTime dhQuarta,
                            DateTime dhQuinta,
                            DateTime dhSexta,
                            DateTime dhSabado,
                            DateTime dhDomingo,
                            DateTime dhFeriado)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanhas está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@COMCNL_ID", idComunicacaoCanal);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_SEG", dhSegunda);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_TER", dhTerca);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_QUA", dhQuarta);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_QUI", dhQuinta);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_SEX", dhSexta);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_SAB", dhSabado);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_DOM", dhDomingo);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_FER", dhFeriado);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPHRDISP_ins");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Incluir Hora Disparo
        /// </summary>
        /// <param name="idCampanha">Id Campanha</param>
        /// <param name="IdConfiguracaoCanal">Id Configuração Canal</param>
        /// <param name="dhSegunda">Data/hora Segunda</param>
        /// <param name="dhTerca">Data/hora Terça</param>
        /// <param name="dhQuarta">Data/hora Quarta</param>
        /// <param name="dhQuinta">Data/hora Quinta</param>
        /// <param name="dhSexta">Data/hora Sexta</param>
        /// <param name="dhSabado">Data/hora Sabado</param>
        /// <param name="dhDomingo">Data/hora Domingo</param>
        public void Alterar(Int32 idCampanha,
                            Int32 idComunicacaoCanal,
                            DateTime dhSegunda,
                            DateTime dhTerca,
                            DateTime dhQuarta,
                            DateTime dhQuinta,
                            DateTime dhSexta,
                            DateTime dhSabado,
                            DateTime dhDomingo,
                            DateTime dhFeriado)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@COMCNL_ID", idComunicacaoCanal);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_SEG", dhSegunda);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_TER", dhTerca);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_QUA", dhQuarta);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_QUI", dhQuinta);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_SEX", dhSexta);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_SAB", dhSabado);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_DOM", dhDomingo);
                acessoDadosBase.AddParameter("@CMPHRDISP_HR_FER", dhFeriado);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPHRDISP_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        // <summary>
        /// Consultar Horario Disparo
        /// </summary>
        /// <param name="idCampanha">Id Campanha</param>        
        private void Consultar(Int32 idCampanhas)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanhas está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanhas);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMPHRDISP_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdCampanha = Convert.ToInt32(dt.Rows[0]["CMP_ID"]);
                    IdCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"]);
                    HorarioSegunda = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_SEG"]);
                    HorarioTerca = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_TER"]);
                    HorarioQuarta = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_QUA"]);
                    HorarioQuinta = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_QUI"]);
                    HorarioSexta = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_SEX"]);
                    HorarioSabado = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_SAB"]);
                    HorarioDomingo = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_DOM"]);
                    HorarioFeriado = Convert.ToDateTime(dt.Rows[0]["CMPHRDISP_HR_FER"]);

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
        /// Excluir uma Horario Disparo
        /// </summary>        
        /// <param name="idCampanha">Id Campanha</param>
        public void Excluir(Int32 idCampanha, Int32 idCanal)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe QualificadorDefinicao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@comcnl_id", idCanal);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPHRDISP_DEL");
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
            if (dt.Columns.Contains("CMP_ID")) { dt.Columns["CMP_ID"].ColumnName = "idCampanha"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "idCanal"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_SEG")) { dt.Columns["CMPHRDISP_HR_SEG"].ColumnName = "HorarioSegunda"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_TER")) { dt.Columns["CMPHRDISP_HR_TER"].ColumnName = "HorarioTerca"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_QUA")) { dt.Columns["CMPHRDISP_HR_QUA"].ColumnName = "HorarioQuarta"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_QUI")) { dt.Columns["CMPHRDISP_HR_QUI"].ColumnName = "HorarioQuinta"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_SEX")) { dt.Columns["CMPHRDISP_HR_SEX"].ColumnName = "HorarioSexta"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_SAB")) { dt.Columns["CMPHRDISP_HR_SAB"].ColumnName = "HorarioSabado"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_DOM")) { dt.Columns["CMPHRDISP_HR_DOM"].ColumnName = "HorarioDomingo"; }
            if (dt.Columns.Contains("CMPHRDISP_HR_FER")) { dt.Columns["CMPHRDISP_HR_FER"].ColumnName = "HorarioFeriado"; }
        }
        #endregion Métodos
    }
}
