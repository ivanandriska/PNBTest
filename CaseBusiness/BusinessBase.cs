using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CaseBusiness
{
    public class BusinessBase
    {
        #region Atributos
        protected AcessoDadosBase acessoDadosBase = null;
        private UsuarioManutencao _usuarioManutencao = new UsuarioManutencao();
        protected Boolean __modoEntidadeOnly = false;
        protected Boolean __blnIsLoaded = false;
        protected Boolean __blnIsNewValueSetted = false;
        protected Boolean __blnIsSearched = false;

        #endregion Atributos

        #region Propriedades
        [JsonIgnore]
        public UsuarioManutencao UsuarioManutencao
        {
            get { return _usuarioManutencao; }
            set { _usuarioManutencao = value; }
        }

        [JsonIgnore]
        public Boolean ModoEntidadeOnly
        {
            get { return __modoEntidadeOnly; }
        }

        [JsonIgnore]
        public Boolean IsLoaded
        {
            get { return __blnIsLoaded; }
        }

        [JsonIgnore]
        public Boolean IsNewValueSetted
        {
            get { return __blnIsNewValueSetted; }
        }
        [JsonIgnore]
        public Boolean IsSearched
        {
            get { return __blnIsSearched; }
        }

        [JsonIgnore]
        public String PCI_ConfigPath
        {
            get
            {
                switch (CaseBusiness.Framework.Configuracao.Configuracao.TipoAplicacao)
                {
                    case CaseBusiness.Framework.TipoAplicacao.Servico:
                        return AppDomain.CurrentDomain.BaseDirectory + @"configuracao_pci.xml";
#if NET472
                    case CaseBusiness.Framework.TipoAplicacao.Web:
                        return System.Web.HttpContext.Current.Server.MapPath("~/configuracao_pci.xml");
#endif
                    default:
                        return "configuracao_pci.xml";
                }

            }
        }

        [JsonIgnore]
        public AcessoDadosBase AcessoDadosBase
        {
            get
            {
                if (acessoDadosBase == null)
                    acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.Configuracao.Configuracao.BancoPrincipal);

                return acessoDadosBase;
            }
            set
            {
                acessoDadosBase = value;
            }
        }

        #endregion Propriedades

        #region MetodosAsync
        /// <summary>
        /// Cria uma execução assíncrona com retorno de dados do método chamado
        /// </summary>
        /// <typeparam name="T">Tipo do dado a ser retornado conforme return do método a ser chamado</typeparam>
        /// <param name="metodo">Nome do método a ser executado</param>
        /// <param name="opcaoCriacaoTask">Opções de criação da Task a ser executada</param>
        /// <returns>Retorna a Task executada assincronamente</returns>
        public async Task<T> AsAsync<T>(Func<T> metodo, TaskCreationOptions opcaoCriacaoTask = TaskCreationOptions.None)
        {
            return await Task.Factory.StartNew(metodo, opcaoCriacaoTask);
        }

        /// <summary>
        /// Cria uma execução assíncrona com retorno de dados do método chamado com parâmetro(s) de entrada
        /// </summary>
        /// <typeparam name="T">Tipo do dado a ser retornado conforme return do método a ser chamado</typeparam>
        /// <param name="metodo">Nome do método a ser executado</param>
        /// <param name="parametros">Parametros de entrada do metodo a ser chamado</param>
        /// <param name="opcaoCriacaoTask">Opções de criação da Task a ser executada</param>
        /// <returns>Retorna a Task executada assincronamente</returns>
        public async Task<T> AsAsync<T>(Func<Object, T> metodo, Object parametros, TaskCreationOptions opcaoCriacaoTask = TaskCreationOptions.None)
        {
            return await Task.Factory.StartNew(metodo, parametros, opcaoCriacaoTask);
        }

        /// <summary>
        /// Cria uma execução assíncrona sem retorno de dados
        /// </summary>
        /// <param name="metodo">Nome do método a ser executado</param>
        /// <param name="opcaoCriacaoTask">Opções de criação da Task a ser executada</param>
        /// <returns>Retorna a Task executada assincronamente</returns>
        public async Task AsAsync(Action metodo, TaskCreationOptions opcaoCriacaoTask = TaskCreationOptions.None)
        {
            await Task.Factory.StartNew(metodo, opcaoCriacaoTask);
        }

        /// <summary>
        /// Cria uma execução assíncrona sem retorno de dados do método chamado com parâmetro(s) de entrada
        /// </summary>
        /// <param name="metodo">Nome do método a ser executado</param>
        /// <param name="parametros">Parametros de entrada do metodo a ser chamado</param>
        /// <param name="opcaoCriacaoTask">Opções de criação da Task a ser executada</param>
        /// <returns>Retorna a Task executada assincronamente</returns>
        public async Task AsAsync(Action<Object> metodo, Object parametros, TaskCreationOptions opcaoCriacaoTask = TaskCreationOptions.None)
        {
            await Task.Factory.StartNew(metodo, parametros, opcaoCriacaoTask);
        }
        #endregion MetodosAsync
    }
}