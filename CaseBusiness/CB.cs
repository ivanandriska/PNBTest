using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using CaseBusiness.CC.Global;
using Microsoft.Extensions.Configuration;

namespace CaseBusiness
{
    public class CB
    {
        #region Atributos
        private static CaseBusiness.CB.ModoRespostaRequisicao _modoRespostaRequisicao = CB.ModoRespostaRequisicao.REG;
        private static ConcurrentDictionary<String, CC.Global.MensagemInterfaceHeader> _listaMensagemInterfacesHeader = new ConcurrentDictionary<String, CC.Global.MensagemInterfaceHeader>();
        private static ConcurrentDictionary<String, Dictionary<String, CC.Global.MensagemInterfaceDetalhe>> _mapaMensagemInterfacePalavraDados = new ConcurrentDictionary<String, Dictionary<String, CC.Global.MensagemInterfaceDetalhe>>();
        private static ConcurrentDictionary<String, Dictionary<String, CC.Global.MensagemInterfaceDetalhe>> _mapaMensagemInterfacePalavraStatus = new ConcurrentDictionary<String, Dictionary<String, CC.Global.MensagemInterfaceDetalhe>>();
        private static ConcurrentDictionary<String, Dictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo>> _mapaMensagemInterfaceCamposISO = new ConcurrentDictionary<String, Dictionary<Int32, ISO.MISO8583.Parsing.FieldParseInfo>>();
        #endregion

        #region Enums
        public enum TransacaoOrigem { SRT, BRM };

        /// <summary>
        /// Indica a fonte de origem dos dados utilizados pela classe
        /// <para>REG: Informação extraida dos REGs (1,2,3, ou 4)</para>
        /// <para>Interface: Informação extraida da mensagem ISO recebida</para>
        /// <para>Database: Informação extraída dos dados gravados no Banco de Dados</para>
        /// <para>REG_Interface: Formato quando é recebido REGs 1,2,3,4 e ISO. Informação extraída dos REGs quando não existe na ISO. Prioridade de extração sempre será da ISO.</para>
        /// </summary>
        public enum OrigemDados { REG, Interface, DataBase, REG_Interface };

        /// <summary>
        /// Indica o modo de resposta utilizado pelo comando RESPOND
        /// </summary>
        public enum ModoRespostaRequisicao { REG, ISO };

        /// <summary>
        /// Indica se os dados da classe serão carregados porDemanda: somente quando alguma informação for utilizada ou carregado Imediato: Será feito requisição imediata a sua fonte para carga dos dados
        /// </summary>
        public enum ModoCarregamentoDados
        {
            porDemanda,
            Imediato
        }

        public enum Canal
        {
            SMS = 1,
            EMAIL = 2,
            PUSH = 3
        }

        public enum Fornecedora
        {
            TwoRP = 1,
            TWW = 2
        }

        #endregion

        #region Propriedades

        public static IConfiguration AppSettings
        {
            get
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig)
                .AddJsonFile("appsettings.json");

                return builder.Build();
            }
        }

        public static ConcurrentDictionary<Int64, System.Data.DataRow> registradoresDefinicao = new ConcurrentDictionary<Int64, System.Data.DataRow>();

        public static ModoRespostaRequisicao modoRespostaRequisicao
        {
            get
            {
                return _modoRespostaRequisicao;
            }

            set => _modoRespostaRequisicao = value;
        }

        /// <summary>
        /// Lista de Interfaces suportadas para recepção de dados
        /// </summary>
        public static ConcurrentDictionary<String, MensagemInterfaceHeader> ListaMensagemInterfacesHeader
        {
            get => _listaMensagemInterfacesHeader;
            set => _listaMensagemInterfacesHeader = value;
        }


        #endregion
    }
}
