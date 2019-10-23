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
    public class Feriado : BusinessBase
    {

        #region Atributos
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 120;  // 60 minutos * 24 horas
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 30;  // 60 minutos * 24 horas
        #endregion Atributos

        #region Propriedades

        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Configuracao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Feriado(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Configuracao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public Feriado()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Feriado(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Feriado(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar Configuração de Envio
        /// </summary>
        public DataTable Listar()
        {
            DataTable dt = null;
            String kCacheKey = "Feriado";

            try
            {
                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dt = MemoryCache.Default[kCacheKey] as DataTable;
                    return dt;
                }

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "PRFER_SEL_LISTAR").Tables[0];

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
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("FER_DT")) { dt.Columns["FER_DT"].ColumnName = "DataFeriado"; }
        }
        #endregion Métodos
    }
}
