#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Scheduler
{
    public class StatusAgendamentoProcessoExecucao : BusinessBase
    {
        #region Enums e Constantes
        public enum enmStatusExecucao { Agendado, Execucao, Finalizado, FinalizadoErro, FinalizadoManualmente, EMPTY, ExcluidoDependencia }
        public const string kStatusExecucao_AGEN = "AGEN";
        public const string kStatusExecucao_EXEC = "EXEC";
        public const string kStatusExecucao_FIN = "FIN";
        public const string kStatusExecucao_FINER = "FINER";
        public const string kStatusExecucao_FINMN = "FINMN";
        public const string kStatusExecucao_EXCDP = "EXCDP";

        public enum enmStatusExecucaoTexto { Agendado, Execucao, Finalizado, FinalizadoErro, FinalizadoManualmente, ExcluidoDependencia }
        public const string kkStatusExecucao_AGEN_Texto = "Agendado";
        public const string kStatusExecucao_EXEC_Texto = "Execucao";
        public const string kStatusExecucao_FIN_Texto = "Finalizado";
        public const string kStatusExecucao_FINER_Texto = "Finalizado Erro";
        public const string kStatusExecucao_FINMN_Texto = "Finalizado Manualmente";
        public const string kStatusExecucao_EXCDP_Texto = "Excluido Dependencia";
        #endregion Enums e Constantes

        #region Construtores
        /// <summary>
        /// Construtor classe StatusAgendamentoProcessoExecucao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public StatusAgendamentoProcessoExecucao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe StatusAgendamentoProcessoExecucao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public StatusAgendamentoProcessoExecucao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe StatusAgendamentoProcessoExecucao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public StatusAgendamentoProcessoExecucao(Int32 idUsuarioManutencao,
                                 CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        #endregion Construtores

        #region Suporte Métodos

        static public enmStatusExecucao ObterEnum_StatusExecucao(String statusExecucao)
        {
            enmStatusExecucao enumItem = enmStatusExecucao.EMPTY;

            switch (statusExecucao.Trim())
            {
                case kStatusExecucao_AGEN: enumItem = enmStatusExecucao.Agendado; break;
                case kStatusExecucao_EXEC: enumItem = enmStatusExecucao.Execucao; break;
                case kStatusExecucao_FIN: enumItem = enmStatusExecucao.Finalizado; break;
                case kStatusExecucao_FINER: enumItem = enmStatusExecucao.FinalizadoErro; break;
                case kStatusExecucao_FINMN: enumItem = enmStatusExecucao.FinalizadoManualmente; break;
                case kStatusExecucao_EXCDP: enumItem = enmStatusExecucao.ExcluidoDependencia; break;
            }
            return enumItem;
        }

        static internal String ObterDBValue_StatusExecucao(enmStatusExecucao statusExecucao)
        {
            String dbvalue = String.Empty;

            switch (statusExecucao)
            {
                case enmStatusExecucao.Agendado: dbvalue = kStatusExecucao_AGEN; break;
                case enmStatusExecucao.Execucao: dbvalue = kStatusExecucao_EXEC; break;
                case enmStatusExecucao.Finalizado: dbvalue = kStatusExecucao_FIN; break;
                case enmStatusExecucao.FinalizadoErro: dbvalue = kStatusExecucao_FINER; break;
                case enmStatusExecucao.FinalizadoManualmente: dbvalue = kStatusExecucao_FINMN; break;
                case enmStatusExecucao.ExcluidoDependencia: dbvalue = kStatusExecucao_EXCDP; break;
            }
            return dbvalue;
        }

        public static String ObterTexto(enmStatusExecucao enmStatusExecucao)
        {
            String DBValue = String.Empty;

            switch (enmStatusExecucao)
            {
                case enmStatusExecucao.Agendado: DBValue = kkStatusExecucao_AGEN_Texto; break;
                case enmStatusExecucao.Execucao: DBValue = kStatusExecucao_EXEC_Texto; break;
                case enmStatusExecucao.Finalizado: DBValue = kStatusExecucao_FIN_Texto; break;
                case enmStatusExecucao.FinalizadoErro: DBValue = kStatusExecucao_FINER_Texto; break;
                case enmStatusExecucao.FinalizadoManualmente: DBValue = kStatusExecucao_FINMN_Texto; break;
                case enmStatusExecucao.ExcluidoDependencia: DBValue = kStatusExecucao_EXCDP_Texto; break;
            }

            return DBValue;
        }
        #endregion
    }
}
