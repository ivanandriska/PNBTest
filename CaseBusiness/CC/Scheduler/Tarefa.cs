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
    public class Tarefa : BusinessBase
    {
        #region Atributos
        #endregion Atributos

        #region Propriedades

        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Tarefa
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="bancoDeDados">Banco de dados a ter a tarefa executada</param>
        public Tarefa(CaseBusiness.Framework.BancoDeDados bancoDeDados, Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(bancoDeDados);
        }

        /// <summary>
        /// Construtor classe Tarefa
        /// </summary>
        /// <param name="bancoDeDados">Banco de dados a ter a tarefa executada</param>
        public Tarefa(CaseBusiness.Framework.BancoDeDados bancoDeDados)
        {
            acessoDadosBase = new AcessoDadosBase(bancoDeDados);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Executa uma tarefa assincronamente
        /// </summary>
        /// <param name="nomeProcedure">Nome da procedure a ser executada</param>
        /// <param name="nomeParametro1">[Opcional] Nome do parametro 1 a ser recebido na procedure a ser executada</param>
        /// <param name="valor1">[Opcional] Valor do parametro 1 a ser recebido na procedure a ser executada</param>
        /// <param name="nomeParametro2">[Opcional] Nome do parametro 2 a ser recebido na procedure a ser executada</param>
        /// <param name="valor2">[Opcional] Valor do parametro 2 a ser recebido na procedure a ser executada</param>
        /// <param name="nomeParametro3">[Opcional] Nome do parametro 3 a ser recebido na procedure a ser executada</param>
        /// <param name="valor3">[Opcional] Valor do parametro 3 a ser recebido na procedure a ser executada</param>
        /// <param name="nomeParametro4">[Opcional] Nome do parametro 4 a ser recebido na procedure a ser executada</param>
        /// <param name="valor4">[Opcional] Valor do parametro 4 a ser recebido na procedure a ser executada</param>
        /// <param name="nomeParametro5">[Opcional] Nome do parametro 5 a ser recebido na procedure a ser executada</param>
        /// <param name="valor5">[Opcional] Valor do parametro 5 a ser recebido na procedure a ser executada</param>
        public void AsyncExecute(String nomeProcedure, 
              String nomeParametro1 = "", Object valor1 = null
            , String nomeParametro2 = "", Object valor2 = null
            , String nomeParametro3 = "", Object valor3 = null
            , String nomeParametro4 = "", Object valor4 = null
            , String nomeParametro5 = "", Object valor5 = null)
        {
            try
            {
                acessoDadosBase.AddParameter("@procedureName", nomeProcedure);
                acessoDadosBase.AddOptionalParameter("@nameParameter1", nomeParametro1);
                acessoDadosBase.AddOptionalParameter("@value1", valor1);
                acessoDadosBase.AddOptionalParameter("@nameParameter2", nomeParametro2);
                acessoDadosBase.AddOptionalParameter("@value2", valor2);
                acessoDadosBase.AddOptionalParameter("@nameParameter3", nomeParametro3);
                acessoDadosBase.AddOptionalParameter("@value3", valor3);
                acessoDadosBase.AddOptionalParameter("@nameParameter4", nomeParametro4);
                acessoDadosBase.AddOptionalParameter("@value4", valor4);
                acessoDadosBase.AddOptionalParameter("@nameParameter5", nomeParametro5);
                acessoDadosBase.AddOptionalParameter("@value5", valor5);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                    "prASYNC_EXE_INVOKE");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }
        
        #endregion
    }
}