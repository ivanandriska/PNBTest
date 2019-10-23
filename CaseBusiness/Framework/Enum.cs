using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework
{
    public class BancoDadosExecucao : BusinessBase
    {
        #region Enums e Constantes
        public enum enumStatus { EMPTY, CASEMANAGER, CASE, CASEMANAGERNEURAL }
        public const string kStatus_EMPTY_DBValue = "";
        public const string kStatus_CASEMANAGER_DBValue = "CaseManager";
        public const string kStatus_CASE_DBValue = "Case";
        public const string kStatus_CASEMANAGERNEURAL_DBValue = "CaseManagerNeural";

        public enum enumStatusTexto { CaseManager, Case, CaseManagerNeural }
        public const string kStatus_CASEMANAGER_Texto = "Case Manager";
        public const string kStatus_CASE_Texto = "Case";
        public const string kStatus_CASEMANAGERNEURAL_Texto = "Case Manager Neural";
        #endregion Enums e Constantes

        public static String ObterDBValue(enumStatus enmstatus)
        {
            String _dbvalue = String.Empty;

            switch (enmstatus)
            {
                case enumStatus.CASEMANAGER: _dbvalue = kStatus_CASEMANAGER_DBValue; break;
                case enumStatus.CASE: _dbvalue = kStatus_CASE_DBValue; break;
                case enumStatus.CASEMANAGERNEURAL: _dbvalue = kStatus_CASEMANAGERNEURAL_DBValue; break;
            }

            return _dbvalue;
        }

        public static enumStatus ObterEnum(String status)
        {
            enumStatus _enmstatus = enumStatus.EMPTY;
            switch (status)
            {
                case kStatus_CASEMANAGER_DBValue: _enmstatus = enumStatus.CASEMANAGER; break;
                case kStatus_CASE_DBValue: _enmstatus = enumStatus.CASE; break;
                case kStatus_CASEMANAGERNEURAL_DBValue: _enmstatus = enumStatus.CASEMANAGERNEURAL; break;
            }

            return _enmstatus;
        }
    }

    public enum Cliente
    {
        Nenhum = 0,
        CSU = 1,
        Fidelity = 2,
        Bradescard = 3,
        Bradesco = 4,
        HSBC = 5,
        CredSystem = 6,
        Avista = 7
        //TODO: AFS CLIENTE 6 é CredSystem, como faz para ser Pernanmbucana?
        // vai ter todos esses clientes? não esquecer do arquivo "C:\PNB\CasePortal\Web.config" e "C:\PNB\CaseBusiness\Comunication\Endpoint.cs" que também esta informando
    }

    public enum SGDB
    {
        SQLServer = 1,
        Oracle = 2
    }

    public enum TipoDados
    {
        Int = 1,
        String = 2,
        Date = 3,
        Time = 4,
        BigInt = 5
    }

    public enum Tipo
    {
        Envio = 1,
        Retorno = 2
    }

    public enum TipoMensagem
    {
        Imprevisto = 0,
        Sucesso = 1,
        Aviso = 2,
        Erro = 3,
        Informacao = 4
    }

    public enum RepostaPaginacao
    {
        NaoHaRegistros = 9800,
        NaoHaAnterior = 9801,
        NaoHaSeguinte = 9802,
        NaoHaAnteriorESeguinte = 9803
    }

    public enum Autenticacao
    {
        Windows = 1,
        SQL = 2
    }

    public enum App : int
    {
        Nenhum = 0,
        RetencaoApi = 1,
        RetencaoServico = 2,
        SMSApi = 3,
        SMSServico = 4,
        PacoteTarifasApi = 5,
        PacoteTarifasServico = 6,
        SMSServicoDispatcher = 7,
        CasePortal = 21,
        CaseServicoScheduler = 50,
    }

    public enum BancoDeDados : int
    {
        CaseManager = 1,
        Case = 2,
        CaseManagerNeural = 3,
        TransactionView = 4
    }

    public enum TipoAplicacao : int
    {
        Desktop = 1,
        Web = 2,
        Servico = 3
    }

    public enum TipoComunicacao : int
    {
        Local = 1,
        Remota = 2,
        Indefinido = 3
    }

    public enum Tela : int
    {
        Nenhum = 0
    }

    public enum TipoLog : int
    {
        Sucesso = 1,
        Informacao = 2,
        Aviso = 3,
        Erro = 4,
        ErroLogico = 5
    }

    public enum ComplementaryCryptType
    {
        TripleDES = 1,
        Voltage = 2
    }

    public enum VoltageCryptAlgorithm
    {
        AES = 1,
        FPE = 2
    }

    public enum VoltageCryptAuthentication
    {
        SharedSecret = 1,
        UsernamePassword = 2,
        Ambos = 3
    }
}
