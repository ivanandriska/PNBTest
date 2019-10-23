#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Runtime.Caching;
using System.Collections.Concurrent;
using CaseBusiness.Framework.BancoDados;
using Newtonsoft.Json;
#endregion Using

namespace CaseBusiness.CC.Global
{
    public class MensagemInterfaceDetalhe : BusinessBase
    {
        #region Atributos
        private String _idMensagemInterfaceHeader = String.Empty;
        private Decimal _numeroOrdem = Decimal.MinValue;
        private String _tipoCampo = String.Empty;
        private Int32 _numeroCampo = Int32.MinValue;
        private String _tipoDados = String.Empty;
        private String _tipoBase = String.Empty;
        private String _nomeCampo = String.Empty;
        private Int32 _tamanho = Int32.MinValue;
        private Int32 _tamanhoId = Int32.MinValue;
        private Int32 _tamanhoTamanho = Int32.MinValue;
        private Int32[] _track;
        private Boolean _obrigatorio;
        private String _palavraDados = String.Empty;
        private String _palavraStatus = String.Empty;
        private Int32 _qtdCasasDecimais = Int32.MinValue;
        #endregion Atributos

        #region Propriedades
        public String IdMensagemInterfaceHeader
        {
            get { return _idMensagemInterfaceHeader; }
            set { _idMensagemInterfaceHeader = value; }
        }

        public Decimal NumeroOrdem
        {
            get { return _numeroOrdem; }
            set { _numeroOrdem = value; }
        }

        public String TipoCampo
        {
            get { return _tipoCampo; }
            set { _tipoCampo = value; }
        }

        public Int32 NumeroCampo
        {
            get { return _numeroCampo; }
            set { _numeroCampo = value; }
        }

        public String TipoDados
        {
            get { return _tipoDados; }
            set { _tipoDados = value; }
        }

        public String TipoBase
        {
            get { return _tipoBase; }
            set { _tipoBase = value; }
        }

        public String NomeCampo
        {
            get { return _nomeCampo; }
            set { _nomeCampo = value; }
        }

        public Int32 Tamanho
        {
            get { return _tamanho; }
            set { _tamanho = value; }
        }

        public Int32 TamanhoId
        {
            get { return _tamanhoId; }
            set { _tamanhoId = value; }
        }

        public Int32 TamanhoTamanho
        {
            get { return _tamanhoTamanho; }
            set { _tamanhoTamanho = value; }
        }

        public Int32[] Track
        {
            get { return _track; }
            set { _track = value; }
        }

        public Boolean Obrigatorio
        {
            get { return _obrigatorio; }
            set { _obrigatorio = value; }
        }

        public String PalavraDados
        {
            get { return _palavraDados; }
            set { _palavraDados = value; }
        }

        public String PalavraStatus
        {
            get { return _palavraStatus; }
            set { _palavraStatus = value; }
        }

        public Int32 QtdCasasDecimais
        {
            get { return _qtdCasasDecimais; }
            set { _qtdCasasDecimais = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe InterfaceDetalhe - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public MensagemInterfaceDetalhe(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe InterfaceDetalhe
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        [JsonConstructor]
        public MensagemInterfaceDetalhe(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe InterfaceDetalhe utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public MensagemInterfaceDetalhe(Int32 idUsuarioManutencao,
                             CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Consulta Palavra Status e Palavra Dados pelo Id.
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public MensagemInterfaceDetalhe Consultar(String interfaceHeader, String track)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemInterfaceDetalhe está operando em Modo Entidade Only"); }

                // Fill Object
                __blnIsLoaded = false;

                acessoDadosBase.AddParameter("@MSGITFHEAD_ID", interfaceHeader.Trim());
                acessoDadosBase.AddParameter("@MSGITFDET_DS_TRACK", track.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMSGITFDET_SEL_CONSULTAR").Tables[0];

                if (dt.Rows.Count > 0)
                {
                    PalavraStatus = dt.Rows[0]["MSGITFDET_DS_PALAVRA_STATUS"].ToString();
                    PalavraDados = dt.Rows[0]["MSGITFDET_DS_PALAVRA_DADOS"].ToString();

                    __blnIsLoaded = true;
                }

                dt = null;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
            return this;
        }

        public ConcurrentDictionary<string, MensagemInterfaceDetalhe> ListarPalavraDados(String interfaceHeader)
        {
            DataTable dtInterfaceDetalhe = null;
            ConcurrentDictionary<string, MensagemInterfaceDetalhe> retorno = new ConcurrentDictionary<string, MensagemInterfaceDetalhe>();

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemInterfaceDetalhe está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MSGITFHEAD_ID", interfaceHeader.Trim());

                dtInterfaceDetalhe = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                    "prMSGITFDET_SEL_LISTAR_DADOS").Tables[0];
                // Renomear Colunas
                RenomearColunas(ref dtInterfaceDetalhe);

                //Montar retono
                for (int i = 0; i < dtInterfaceDetalhe.Rows.Count; i++)
                {
                    MensagemInterfaceDetalhe interfaceDetalhe = new MensagemInterfaceDetalhe(UsuarioManutencao.ID);

                    interfaceDetalhe.IdMensagemInterfaceHeader = dtInterfaceDetalhe.Rows[i]["IdMensagemInterfaceHeader"].ToString();

                    interfaceDetalhe.NumeroOrdem = Convert.ToDecimal(dtInterfaceDetalhe.Rows[i]["NumeroOrdem"]);

                    interfaceDetalhe.TipoCampo = dtInterfaceDetalhe.Rows[i]["TipoCampo"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["NumeroCampo"] != DBNull.Value)
                        interfaceDetalhe.NumeroCampo = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["NumeroCampo"]);
                    interfaceDetalhe.TipoDados = dtInterfaceDetalhe.Rows[i]["TipoDados"].ToString();

                    if (dtInterfaceDetalhe.Rows[i]["TipoBase"] != DBNull.Value)
                        interfaceDetalhe.TipoBase = dtInterfaceDetalhe.Rows[i]["TipoBase"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["NomeCampo"] != DBNull.Value)
                        interfaceDetalhe.NomeCampo = dtInterfaceDetalhe.Rows[i]["NomeCampo"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["Tamanho"] != DBNull.Value)
                        interfaceDetalhe.Tamanho = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["Tamanho"]);

                    if (dtInterfaceDetalhe.Rows[i]["TamanhoId"] != DBNull.Value)
                        interfaceDetalhe.TamanhoId = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["TamanhoId"]);

                    if (dtInterfaceDetalhe.Rows[i]["TamanhoTamanho"] != DBNull.Value)
                        interfaceDetalhe.TamanhoTamanho = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["TamanhoTamanho"]);

                    if (dtInterfaceDetalhe.Rows[i]["Track"].ToString().Length > 0)
                    {
                        String[] dadosString = dtInterfaceDetalhe.Rows[i]["Track"].ToString().Split(',');
                        Int32[] dadosInt32 = new int[dadosString.Length];
                        for (int j = 0; j < dadosString.Length; j++)
                        {
                            dadosInt32[j] = Int32.Parse(dadosString[j]);
                        }
                        interfaceDetalhe.Track = dadosInt32;
                    }

                    interfaceDetalhe.Obrigatorio = Convert.ToBoolean(dtInterfaceDetalhe.Rows[i]["Obrigatorio"]);

                    interfaceDetalhe.PalavraDados = dtInterfaceDetalhe.Rows[i]["PalavraDados"].ToString();

                    interfaceDetalhe.PalavraStatus = dtInterfaceDetalhe.Rows[i]["PalavraStatus"].ToString();

                    if (dtInterfaceDetalhe.Rows[i]["QtdCasasDecimais"] != DBNull.Value)
                        interfaceDetalhe.QtdCasasDecimais = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["QtdCasasDecimais"]);

                    retorno.TryAdd(dtInterfaceDetalhe.Rows[i]["PalavraDados"].ToString(), interfaceDetalhe);
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return retorno;
        }

        public ConcurrentDictionary<string, MensagemInterfaceDetalhe> ListarPalavraStatus(String interfaceHeader)
        {
            DataTable dtInterfaceDetalhe = null;
            ConcurrentDictionary<string, MensagemInterfaceDetalhe> retorno = new ConcurrentDictionary<string, MensagemInterfaceDetalhe>();

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemInterfaceDetalhe está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MSGITFHEAD_ID", interfaceHeader.Trim());

                dtInterfaceDetalhe = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                    "prMSGITFDET_SEL_LISTAR_STATUS").Tables[0];
                // Renomear Colunas
                RenomearColunas(ref dtInterfaceDetalhe);

                //Montar retono
                for (int i = 0; i < dtInterfaceDetalhe.Rows.Count; i++)
                {
                    MensagemInterfaceDetalhe interfaceDetalhe = new MensagemInterfaceDetalhe(UsuarioManutencao.ID);
                    interfaceDetalhe.IdMensagemInterfaceHeader = dtInterfaceDetalhe.Rows[i]["IdMensagemInterfaceHeader"].ToString();
                    interfaceDetalhe.NumeroOrdem = Convert.ToDecimal(dtInterfaceDetalhe.Rows[i]["NumeroOrdem"]);
                    interfaceDetalhe.TipoCampo = dtInterfaceDetalhe.Rows[i]["TipoCampo"].ToString();
                    interfaceDetalhe.NumeroCampo = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["NumeroCampo"]);
                    interfaceDetalhe.TipoDados = dtInterfaceDetalhe.Rows[i]["TipoDados"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["TipoBase"] != DBNull.Value)
                        interfaceDetalhe.TipoBase = dtInterfaceDetalhe.Rows[i]["TipoBase"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["Tamanho"] != DBNull.Value)
                        interfaceDetalhe.Tamanho = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["Tamanho"]);
                    if (dtInterfaceDetalhe.Rows[i]["TamanhoId"] != DBNull.Value)
                        interfaceDetalhe.TamanhoId = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["TamanhoId"]);
                    if (dtInterfaceDetalhe.Rows[i]["TamanhoTamanho"] != DBNull.Value)
                        interfaceDetalhe.TamanhoTamanho = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["TamanhoTamanho"]);
                    if (dtInterfaceDetalhe.Rows[i]["Track"].ToString().Trim().Length > 0)
                    {
                        String[] dadosString = dtInterfaceDetalhe.Rows[i]["Track"].ToString().Split(',');
                        Int32[] dadosInt32 = new int[dadosString.Length];
                        for (int j = 0; j < dadosString.Length; j++)
                        {
                            dadosInt32[j] = Int32.Parse(dadosString[j]);
                        }
                        interfaceDetalhe.Track = dadosInt32;
                    }
                    interfaceDetalhe.Obrigatorio = Convert.ToBoolean(dtInterfaceDetalhe.Rows[i]["Obrigatorio"]);
                    interfaceDetalhe.PalavraDados = dtInterfaceDetalhe.Rows[i]["PalavraDados"].ToString();
                    interfaceDetalhe.PalavraStatus = dtInterfaceDetalhe.Rows[i]["PalavraStatus"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["QtdCasasDecimais"] != DBNull.Value)
                        interfaceDetalhe.QtdCasasDecimais = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["QtdCasasDecimais"]);

                    retorno.TryAdd(dtInterfaceDetalhe.Rows[i]["PalavraStatus"].ToString(), interfaceDetalhe);
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return retorno;
        }

        public ConcurrentDictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo> ListarMapaVar(String interfaceHeader, String tipoFormato)
        {
            DataTable dtInterfaceDetalhe = null;
            ConcurrentDictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo> retorno = new ConcurrentDictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo>();

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemInterfaceDetalhe está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MSGITFHEAD_ID", interfaceHeader.Trim());

                dtInterfaceDetalhe = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                    "prMSGITFDET_SEL_LISTAR_MAPA").Tables[0];
                // Renomear Colunas
                RenomearColunas(ref dtInterfaceDetalhe);

                //Montar retono
                for (int i=0; i<dtInterfaceDetalhe.Rows.Count; i++)
                {
                    if (dtInterfaceDetalhe.Rows[i]["Track"].ToString().Split(',').Length == 1) //Se campo não tiver campos com separação com virgula (,) no track significa que ele é um campo no 1º nível
                    {
                        CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo fpi = null;
                        CaseBusiness.ISO.MISO8583.Parsing.ConfigParser.Base baseCodificacao = ISO.MISO8583.Parsing.ConfigParser.Base.Decimal;

                        switch (dtInterfaceDetalhe.Rows[i]["TipoBase"].ToString())
                        {
                            case "BIN":
                                baseCodificacao = ISO.MISO8583.Parsing.ConfigParser.Base.Binary;
                                break;
                            case "OCT":
                                baseCodificacao = ISO.MISO8583.Parsing.ConfigParser.Base.Octal;
                                break;

                            case "DEC":
                                baseCodificacao = ISO.MISO8583.Parsing.ConfigParser.Base.Decimal;
                                break;
                            case "HEX":
                                baseCodificacao = ISO.MISO8583.Parsing.ConfigParser.Base.Hexadecimal;
                                break;
                            case "B64":
                                baseCodificacao = ISO.MISO8583.Parsing.ConfigParser.Base.B64;
                                break;
                        }

                        fpi = new CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo(
                            (CaseBusiness.ISO.MISO8583.IsoType)Enum.Parse(typeof(CaseBusiness.ISO.MISO8583.IsoType), dtInterfaceDetalhe.Rows[i]["TipoDados"].ToString()),
                            dtInterfaceDetalhe.Rows[i]["Tamanho"] == DBNull.Value ? 0 : Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["Tamanho"]),
                            Convert.ToBoolean(dtInterfaceDetalhe.Rows[i]["Obrigatorio"]),
                            (CaseBusiness.ISO.MISO8583.TypeElement)Enum.Parse(typeof(CaseBusiness.ISO.MISO8583.TypeElement), dtInterfaceDetalhe.Rows[i]["TipoCampo"].ToString().ToUpper().Equals("FIELD") ? "1" : "3"),
                            MontaListaVar(dtInterfaceDetalhe, dtInterfaceDetalhe.Rows[i]["Track"].ToString().Trim()),
                            baseCodificacao
                        );

                        retorno.TryAdd(Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["NumeroCampo"]), fpi);
                    }
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return retorno;
        }

        public ConcurrentDictionary<Int32, MensagemInterfaceDetalhe> ListarMapaFixo(String interfaceHeader, String tipoFormato)
        {
            DataTable dtInterfaceDetalhe = null;
            ConcurrentDictionary<Int32, MensagemInterfaceDetalhe> retorno = new ConcurrentDictionary<Int32, MensagemInterfaceDetalhe>();

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemInterfaceDetalhe está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MSGITFHEAD_ID", interfaceHeader.Trim());

                dtInterfaceDetalhe = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                    "prMSGITFDET_SEL_LISTAR_DADOS").Tables[0];
                // Renomear Colunas
                RenomearColunas(ref dtInterfaceDetalhe);

                //Montar retono
                for (int i = 0; i < dtInterfaceDetalhe.Rows.Count; i++)
                {
                    MensagemInterfaceDetalhe interfaceDetalhe = new MensagemInterfaceDetalhe(UsuarioManutencao.ID);

                    interfaceDetalhe.IdMensagemInterfaceHeader = dtInterfaceDetalhe.Rows[i]["IdMensagemInterfaceHeader"].ToString();

                    interfaceDetalhe.NumeroOrdem = Convert.ToDecimal(dtInterfaceDetalhe.Rows[i]["NumeroOrdem"]);


                    interfaceDetalhe.TipoCampo = dtInterfaceDetalhe.Rows[i]["TipoCampo"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["NumeroCampo"] != DBNull.Value)
                        interfaceDetalhe.NumeroCampo = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["NumeroCampo"]);
                    interfaceDetalhe.TipoDados = dtInterfaceDetalhe.Rows[i]["TipoDados"].ToString();

                    if (dtInterfaceDetalhe.Rows[i]["TipoBase"] != DBNull.Value)
                        interfaceDetalhe.TipoBase = dtInterfaceDetalhe.Rows[i]["TipoBase"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["NomeCampo"] != DBNull.Value)
                        interfaceDetalhe.NomeCampo = dtInterfaceDetalhe.Rows[i]["NomeCampo"].ToString();
                    if (dtInterfaceDetalhe.Rows[i]["Tamanho"] != DBNull.Value)
                        interfaceDetalhe.Tamanho = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["Tamanho"]);

                    if (dtInterfaceDetalhe.Rows[i]["TamanhoId"] != DBNull.Value)
                        interfaceDetalhe.TamanhoId = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["TamanhoId"]);

                    if (dtInterfaceDetalhe.Rows[i]["TamanhoTamanho"] != DBNull.Value)
                        interfaceDetalhe.TamanhoTamanho = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["TamanhoTamanho"]);

                    if (dtInterfaceDetalhe.Rows[i]["Track"].ToString().Length > 0)
                    {
                        String[] dadosString = dtInterfaceDetalhe.Rows[i]["Track"].ToString().Split(',');
                        Int32[] dadosInt32 = new int[dadosString.Length];
                        for (int j = 0; j < dadosString.Length; j++)
                        {
                            dadosInt32[j] = Int32.Parse(dadosString[j]);
                        }
                        interfaceDetalhe.Track = dadosInt32;
                    }

                    interfaceDetalhe.Obrigatorio = Convert.ToBoolean(dtInterfaceDetalhe.Rows[i]["Obrigatorio"]);

                    interfaceDetalhe.PalavraDados = dtInterfaceDetalhe.Rows[i]["PalavraDados"].ToString();

                    interfaceDetalhe.PalavraStatus = dtInterfaceDetalhe.Rows[i]["PalavraStatus"].ToString();

                    if (dtInterfaceDetalhe.Rows[i]["QtdCasasDecimais"] != DBNull.Value)
                        interfaceDetalhe.QtdCasasDecimais = Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["QtdCasasDecimais"]);

                    retorno.TryAdd(Convert.ToInt32(dtInterfaceDetalhe.Rows[i]["NumeroCampo"].ToString()), interfaceDetalhe);
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return retorno;
        }

        private Dictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo> MontaListaVar(DataTable dt, String track)
        {
            Dictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo> lista = new Dictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo>();

            for (int i=0; i<dt.Rows.Count; i++)
            {
                Int32 existe = 0;
                String trackBusca = dt.Rows[i]["Track"].ToString().Trim();
                Boolean inicio = trackBusca.StartsWith(track + ',');

                if (inicio)
                {
                    existe = trackBusca.IndexOf(trackBusca, 0);
                }

                if (inicio)
                {
                    CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo fpi = null;

                    fpi = new CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo(
                        (CaseBusiness.ISO.MISO8583.IsoType)Enum.Parse(typeof(CaseBusiness.ISO.MISO8583.IsoType), dt.Rows[i]["TipoDados"].ToString()),
                        dt.Rows[i]["Tamanho"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["Tamanho"]),
                        Convert.ToBoolean(dt.Rows[i]["Obrigatorio"]),
                        (CaseBusiness.ISO.MISO8583.TypeElement)Enum.Parse(typeof(CaseBusiness.ISO.MISO8583.TypeElement), dt.Rows[i]["TipoCampo"].ToString().ToUpper().Equals("FIELD") ? "1" : "3"),
                        MontaListaVar(dt, trackBusca),
                        dt.Rows[i]["TamanhoId"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["TamanhoId"]),
                        dt.Rows[i]["TamanhoTamanho"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["TamanhoTamanho"])
                    );

                    //lista.
                    lista.Add(Convert.ToInt32(dt.Rows[i]["NumeroCampo"]), fpi);

                    if (fpi.SubDataElements_Fields.Count > 0)
                    {
                        i = i + QuantidadeItens(fpi.SubDataElements_Fields);
                    }
                }
            }

            return lista;
        }

        private Int32 QuantidadeItens(Dictionary<Int32, CaseBusiness.ISO.MISO8583.Parsing.FieldParseInfo> dic)
        {
            Int32 retorno = 0;

            retorno = dic.Count;

            for (int i = 0; i < dic.Count; i++)
            {
                if (dic.ElementAt(i).Value.SubDataElements_Fields.Count > 0)
                {
                    retorno += QuantidadeItens(dic.ElementAt(i).Value.SubDataElements_Fields);
                }
            }

            return retorno;
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MSGITFHEAD_ID")) { dt.Columns["MSGITFHEAD_ID"].ColumnName = "IdMensagemInterfaceHeader"; }
            if (dt.Columns.Contains("MSGITFDET_NR_ORDEM")) { dt.Columns["MSGITFDET_NR_ORDEM"].ColumnName = "NumeroOrdem"; }
            if (dt.Columns.Contains("MSGITFDET_TP_CAMPO")) { dt.Columns["MSGITFDET_TP_CAMPO"].ColumnName = "TipoCampo"; }
            if (dt.Columns.Contains("MSGITFDET_NR")) { dt.Columns["MSGITFDET_NR"].ColumnName = "NumeroCampo"; }
            if (dt.Columns.Contains("MSGITFDET_TP_DADOS")) { dt.Columns["MSGITFDET_TP_DADOS"].ColumnName = "TipoDados"; }
            if (dt.Columns.Contains("MSGITFDET_TP_BASE")) { dt.Columns["MSGITFDET_TP_BASE"].ColumnName = "TipoBase"; }
            if (dt.Columns.Contains("MSGITFDET_NR_TAMANHO")) { dt.Columns["MSGITFDET_NR_TAMANHO"].ColumnName = "Tamanho"; }
            if (dt.Columns.Contains("MSGITFDET_NR_TAMANHO_ID")) { dt.Columns["MSGITFDET_NR_TAMANHO_ID"].ColumnName = "TamanhoId"; }
            if (dt.Columns.Contains("MSGITFDET_NR_TAMANHO_TAMANHO")) { dt.Columns["MSGITFDET_NR_TAMANHO_TAMANHO"].ColumnName = "TamanhoTamanho"; }
            if (dt.Columns.Contains("MSGITFDET_DS_TRACK")) { dt.Columns["MSGITFDET_DS_TRACK"].ColumnName = "Track"; }
            if (dt.Columns.Contains("MSGITFDET_FL_OBRIGATORIO")) { dt.Columns["MSGITFDET_FL_OBRIGATORIO"].ColumnName = "Obrigatorio"; }
            if (dt.Columns.Contains("MSGITFDET_DS_PALAVRA_DADOS")) { dt.Columns["MSGITFDET_DS_PALAVRA_DADOS"].ColumnName = "PalavraDados"; }
            if (dt.Columns.Contains("MSGITFDET_DS_PALAVRA_STATUS")) { dt.Columns["MSGITFDET_DS_PALAVRA_STATUS"].ColumnName = "PalavraStatus"; }
            if (dt.Columns.Contains("MSGITFDET_NR_CASAS_DECIMAIS")) { dt.Columns["MSGITFDET_NR_CASAS_DECIMAIS"].ColumnName = "QtdCasasDecimais"; }

            if (dt.Columns.Contains("MSGITFDET_NM_CAMPO")) { dt.Columns["MSGITFDET_NM_CAMPO"].ColumnName = "NomeCampo"; }
        }
        #endregion Métodos
    }
}
