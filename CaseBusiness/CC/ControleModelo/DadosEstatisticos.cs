#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.ControleModelo
{
    public class DadosEstatisticos : BusinessBase
    {
        #region Atributos
        private Int32 _idPreProcessamento = Int32.MinValue;
        private Int32 _cdOrg = Int32.MinValue;
        private DateTime _dataBase = DateTime.MinValue;
        private Int32 _cdRedeNeural = Int32.MinValue;
        private Double _vlScore = Double.MinValue;
        private Int32 _qtVP = Int32.MinValue;
        private Int32 _qtFP = Int32.MinValue;
        private Int32 _qtVN = Int32.MinValue;
        private Int32 _qtFN = Int32.MinValue;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dhUsarioIns = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdPreProcessamento
        {
            get { return _idPreProcessamento; }
            set { _idPreProcessamento = value; }
        }

        public Int32 CdOrg
        {
            get { return _cdOrg; }
            set { _cdOrg = value; }
        }

        public DateTime DataBase
        {
            get { return _dataBase; }
            set { _dataBase = value; }
        }

        public Int32 CdRedeNeural
        {
            get { return _cdRedeNeural; }
            set { _cdRedeNeural = value; }
        }

        public Double VlScore
        {
            get { return _vlScore; }
            set { _vlScore = value; }
        }

        public Int32 QtVP
        {
            get { return _qtVP; }
            set { _qtVP = value; }
        }

        public Int32 QtFP
        {
            get { return _qtFP; }
            set { _qtFP = value; }
        }

        public Int32 QtVN
        {
            get { return _qtVN; }
            set { _qtVN = value; }
        }

        public Int32 QtFN
        {
            get { return _qtFN; }
            set { _qtFN = value; }
        }

        public Int32 IdUsuarioIns
        {
            get { return _idUsuarioIns; }
            set { _idUsuarioIns = value; }
        }

        public DateTime DhUsarioIns
        {
            get { return _dhUsarioIns; }
            set { _dhUsarioIns = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe DadosEstatisticos - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public DadosEstatisticos(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe DadosEstatisticos
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public DadosEstatisticos(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe DadosEstatisticos utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public DadosEstatisticos(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe DadosEstatisticos e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código do DadosEstatisticos</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        //public DadosEstatisticos(Int32 idDadosEstatisticos,
        //                 Int32 idUsuarioManutencao)
        //    : this(idUsuarioManutencao)
        //{
        //    Consultar(idDadosEstatisticos);
        //}

        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar Faixa Ideal de Score
        /// </summary>
        /// <param name="idPacote">Id Pacote</param>
        /// <param name="dataDe">Data Início</param>
        /// <param name="dataAte">Data Fim</param>
        /// <param name="cdOrganizacao">Código da Organização</param>
        /// <returns></returns>
        public DataTable ListarIndicadores(Int32 idPacote, 
                                           DateTime dataDe, 
                                           DateTime dataAte, 
                                           Int32 cdOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@SONSPAC_ID", idPacote);
                acessoDadosBase.AddParameter("@CMDESTAT_DT_BASE_DE", dataDe);
                acessoDadosBase.AddParameter("@CMDESTAT_DT_BASE_ATE", dataAte);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF11").Tables[0];

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

        public DataTable ListarIndicadoresSub(Int32 idPacote,
                                              DateTime dataDe,
                                              DateTime dataAte,
                                              Int32 cdOrganizacao, 
                                              Int32 idSubRede)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@SONSPAC_ID", idPacote);
                acessoDadosBase.AddParameter("@CMDESTAT_DT_BASE_DE", dataDe);
                acessoDadosBase.AddParameter("@CMDESTAT_DT_BASE_ATE", dataAte);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORT_CD", idSubRede);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF12").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Resumo
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinamento da rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarResumo(String codigoModeloEstatistico,
                                      String codigoOrigem,
                                      Int32 cdOrganizacao,
                                      Int32 cdRedeNeural,
                                      Int32 cdRedeNeuralTreinamento,
                                      Int32 idSimulacao,
                                      DateTime dataDe,
                                      DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF0").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Volume Elegível
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural </param>
        /// <param name="cdRedeNeuralTreinamento">Código do treinamento rede neural </param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarVolumeElegivel(String codigoModeloEstatistico,
                                              String codigoOrigem,
                                              Int32 cdOrganizacao,
                                              Int32 cdRedeNeural,
                                              Int32 cdRedeNeuralTreinamento,
                                              Int32 idSimulacao,
                                              DateTime dataDe,
                                              DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF1").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Volume Elegível
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do treinamento  rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarTransacoesScore(String codigoModeloEstatistico,
                                               String codigoOrigem,
                                               Int32 cdOrganizacao,
                                               Int32 cdRedeNeural,
                                               Int32 cdRedeNeuralTreinamento,
                                               Int32 idSimulacao,
                                               DateTime dataDe,
                                               DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF2").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Distribuicao de Scores
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinamento rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarDistribuicaoScore(String codigoModeloEstatistico,
                                              String codigoOrigem,
                                              Int32 cdOrganizacao,
                                              Int32 cdRedeNeural,
                                              Int32 cdRedeNeuralTreinamento,
                                              Int32 idSimulacao,
                                              DateTime dataDe,
                                              DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF3").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Volume Elegível
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código rede neural</param>        
        /// <param name="cdRedeNeuralTreinamento">Código do Treinamento rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarVolumeDiario(String codigoModeloEstatistico,
                                            String codigoOrigem,
                                            Int32 cdOrganizacao,
                                            Int32 cdRedeNeural,
                                            Int32 cdRedeNeuralTreinamento,
                                            Int32 idSimulacao,
                                            DateTime dataDe,
                                            DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF4").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Tabular
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinamento da rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarTabular(String codigoModeloEstatistico,
                                       String codigoOrigem,
                                       Int32 cdOrganizacao,
                                       Int32 cdRedeNeural,
                                       Int32 cdRedeNeuralTreinamento,
                                       Int32 idSimulacao,
                                       DateTime dataDe,
                                       DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF5").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Volume Elegível
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinametno da rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarKS(String codigoModeloEstatistico,
                                  String codigoOrigem,
                                  Int32 cdOrganizacao,
                                  Int32 cdRedeNeural,
                                  Int32 cdRedeNeuralTreinamento,
                                  Int32 idSimulacao,
                                  DateTime dataDe,
                                  DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF6").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Volume Elegível
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinamento da rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarCurvaROC(String codigoModeloEstatistico,
                                        String codigoOrigem,
                                        Int32 cdOrganizacao,
                                        Int32 cdRedeNeural,
                                        Int32 cdRedeNeuralTreinamento,
                                        Int32 idSimulacao,
                                        DateTime dataDe,
                                        DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF7").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Gain Chart
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinametno da rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarGainChart(String codigoModeloEstatistico,
                                         String codigoOrigem,
                                         Int32 cdOrganizacao,
                                         Int32 cdRedeNeural,
                                         Int32 cdRedeNeuralTreinamento,
                                         Int32 idSimulacao,
                                         DateTime dataDe,
                                         DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF8").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Lift Chart
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinametno da rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarLiftChart(String codigoModeloEstatistico,
                                         String codigoOrigem,
                                         Int32 cdOrganizacao,
                                         Int32 cdRedeNeural,
                                         Int32 cdRedeNeuralTreinamento,
                                         Int32 idSimulacao,
                                         DateTime dataDe,
                                         DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF9").Tables[0];

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
        /// Buscar DadosEstatisticos para gráfico de Volume Elegível
        /// </summary>
        /// <param name="codigoModeloEstatistico">Modelo estatístico</param>
        /// <param name="codigoOrigem">Origem dos dados</param>
        /// <param name="cdOrganizacao">Org</param>
        /// <param name="cdRedeNeural">Código da rede neural</param>
        /// <param name="cdRedeNeuralTreinamento">Código do Treinamento da rede neural</param>
        /// <param name="dataDe">Data início</param>
        /// <param name="dataAte">Data fim</param>
        /// <returns></returns>
        public DataTable ListarPontoCorte(String codigoModeloEstatistico,
                                          String codigoOrigem,
                                          Int32 cdOrganizacao,
                                          Int32 cdRedeNeural,
                                          Int32 cdRedeNeuralTreinamento,
                                          Int32 idSimulacao,
                                          DateTime dataDe,
                                          DateTime dataAte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe DadosEstatisticos está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_TP_MDL_ESTATISTIC", codigoModeloEstatistico);
                acessoDadosBase.AddParameter("@CMPPROC_FL_ORIGEM", codigoOrigem);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrganizacao);
                acessoDadosBase.AddParameter("@SORD_CD", cdRedeNeural);
                acessoDadosBase.AddParameter("@SORT_CD", cdRedeNeuralTreinamento);
                acessoDadosBase.AddParameter("@SONSPAC_ID", idSimulacao);
                acessoDadosBase.AddParameter("@DATA_DE", dataDe);
                acessoDadosBase.AddParameter("@DATA_ATE", dataAte);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMDESTAT_SEL_LISTAR_GRF10").Tables[0];

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
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("CMPPROC_ID")) { dt.Columns["CMPPROC_ID"].ColumnName = "IdPreProcessamento"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "Org"; }
            if (dt.Columns.Contains("CMDESTAT_DT_BASE")) { dt.Columns["CMDESTAT_DT_BASE"].ColumnName = "DataBase"; }
            if (dt.Columns.Contains("SORT_CD")) { dt.Columns["SORT_CD"].ColumnName = "CodigoRedeNeuralTreinamento"; }
            if (dt.Columns.Contains("SORD_CD")) { dt.Columns["SORD_CD"].ColumnName = "CodigoRedeNeural"; }
            if (dt.Columns.Contains("CMDESTAT_VL_SCORE")) { dt.Columns["CMDESTAT_VL_SCORE"].ColumnName = "ValorScore"; }
		    if (dt.Columns.Contains("CMDESTAT_QT_VP")) { dt.Columns["CMDESTAT_QT_VP"].ColumnName = "QtVerdadeiroPositivo"; }
            if (dt.Columns.Contains("CMDESTAT_QT_FP")) { dt.Columns["CMDESTAT_QT_FP"].ColumnName = "QtFalsoPositivo"; }
            if (dt.Columns.Contains("CMDESTAT_QT_VN")) { dt.Columns["CMDESTAT_QT_VN"].ColumnName = "QtVerdadeiroNegativo"; }
            if (dt.Columns.Contains("CMDESTAT_QT_FN")) { dt.Columns["CMDESTAT_QT_FN"].ColumnName = "QtFalsoNegativo"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "UsuarioResponsavel"; }
            if (dt.Columns.Contains("CMDESTAT_DH_USUARIO_INS")) { dt.Columns["CMDESTAT_DH_USUARIO_INS"].ColumnName = "DataHoraInclusao"; }

            //// Utilizado nos indicadores (prCMDESTAT_SEL_LISTAR_GRF11 e prCMDESTAT_SEL_LISTAR_GRF12) - [ R E D E ]
            if (dt.Columns.Contains("SORT_CD_REDE")) { dt.Columns["SORT_CD_REDE"].ColumnName = "CodigoRede"; }
            if (dt.Columns.Contains("QT_TRANSACOES_SUB_REDE_ARRAY")) { dt.Columns["QT_TRANSACOES_SUB_REDE_ARRAY"].ColumnName = "AlertasSubRedeArray"; }
            if (dt.Columns.Contains("QT_FRAUDES_SUB_REDE_ARRAY")) { dt.Columns["QT_FRAUDES_SUB_REDE_ARRAY"].ColumnName = "FraudesSubRedeArray"; }
            if (dt.Columns.Contains("CMDESTAT_QT_TRANSACOES_REDE")) { dt.Columns["CMDESTAT_QT_TRANSACOES_REDE"].ColumnName = "AlertasRede"; }
            if (dt.Columns.Contains("CMDESTAT_QT_FRAUDES_REDE")) { dt.Columns["CMDESTAT_QT_FRAUDES_REDE"].ColumnName = "FraudesRede"; }
            if (dt.Columns.Contains("CMDESTAT_QT_IFP_REDE")) { dt.Columns["CMDESTAT_QT_IFP_REDE"].ColumnName = "IFPRede"; }

            // Utilizado nos indicadores (prCMDESTAT_SEL_LISTAR_GRF11 e prCMDESTAT_SEL_LISTAR_GRF12) - [ S U B - R E D E ]            
            if (dt.Columns.Contains("CMDESTAT_VL_SCORE_SUB_REDE")) { dt.Columns["CMDESTAT_VL_SCORE_SUB_REDE"].ColumnName = "ValorScoreSubRede"; }
            if (dt.Columns.Contains("CMDESTAT_QT_TRANSACOES_SUB_RED")) { dt.Columns["CMDESTAT_QT_TRANSACOES_SUB_RED"].ColumnName = "AlertasSubRede"; }
            if (dt.Columns.Contains("CMDESTAT_QT_FRAUDES_SUB_REDE")) { dt.Columns["CMDESTAT_QT_FRAUDES_SUB_REDE"].ColumnName = "FraudesSubRede"; }
            if (dt.Columns.Contains("CMDESTAT_QT_IFP_SUB_REDE")) { dt.Columns["CMDESTAT_QT_IFP_SUB_REDE"].ColumnName = "IFPSubRede"; }
            if (dt.Columns.Contains("SORT_NM_SUB_REDE")) { dt.Columns["SORT_NM_SUB_REDE"].ColumnName = "NomeSubRede"; }
            if (dt.Columns.Contains("SORT_CD_SUB_REDE")) { dt.Columns["SORT_CD_SUB_REDE"].ColumnName = "CodigoSubRede"; }            
        }
        #endregion Métodos
    }
}
