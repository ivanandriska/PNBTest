using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Data.Common;

namespace CaseBusiness.Framework.Log
{

    //
    // Summary:
    //     Specifies the event type of an event log entry.
    public enum TipoEventoLog
    {
        Erro = 1,
        Aviso = 2,
        Informacao = 4,
        Sucesso = 8,
        Falha = 16
    }

    public static class Log
    {
        private static object locker = new object();

        public static void LogarArquivo(String mensagem, TipoEventoLog tipoEvento, String nomeAplicacao)
        {
            nomeAplicacao = Framework.Configuracao.Configuracao.Aplicacao.ToString();

            try
            {
                lock (locker)
                {
                    StreamWriter arq = new StreamWriter(CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig + @"/" + nomeAplicacao + "_Case_Framework.log", true);
                    arq.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " - " + mensagem);
                    arq.Close();
                }
            }
            catch (System.Exception ex)
            {
                lock (locker)
                {
                    StreamWriter arq2 = new StreamWriter(CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig + @"/" + nomeAplicacao + "Case_Framework_" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_fff") + ".log", true);
                    arq2.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " - Erro principal: " + mensagem);
                    arq2.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " - Erro secundário: " + ex.Message);
                    arq2.Close();
                }
            }
        }

        public static Int32 Logar(TipoLog tipoLog, String mensagem, String stackTrace, String campo, String tipoErroFisico, DateTime dataHora, App aplicacao, Tela tela, Int32 cdLogTransacao)
        {

#if DEBUG
            switch (tipoLog)
            {
                case TipoLog.Sucesso:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case TipoLog.Informacao:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case TipoLog.Aviso:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case TipoLog.Erro:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
            }

            Console.WriteLine(dataHora.ToString("dd/MM/yyyy HH:mm:ss.fff") + " " + tipoLog.ToString() + "-" + mensagem);
            if (!String.IsNullOrEmpty(stackTrace))
                Console.WriteLine(dataHora.ToString("dd/MM/yyyy HH:mm:ss.fff") + " " + tipoLog.ToString() + "-" + stackTrace);
            Console.BackgroundColor = ConsoleColor.Black;
#endif

            Int32 cod = 0;

            try
            {
                lock (locker)
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._erroInicializacao = mensagem;
                    cod = new Processo.Log().Logar(tipoLog, mensagem, stackTrace, campo, tipoErroFisico, dataHora, aplicacao, tela, cdLogTransacao);
                }
            }
            catch (System.Exception ex)
            {
                switch (tipoLog)
                {
                    case TipoLog.Sucesso:
                        LogarArquivo(mensagem, TipoEventoLog.Sucesso, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                    case TipoLog.Informacao:
                        LogarArquivo(mensagem, TipoEventoLog.Informacao, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                    case TipoLog.Aviso:
                        LogarArquivo(mensagem, TipoEventoLog.Aviso, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                    case TipoLog.Erro:
                        LogarArquivo(mensagem, TipoEventoLog.Erro, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                }

                LogarArquivo(ex.Message, TipoEventoLog.Erro, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
            }

            return cod;
        }

        public static Int32 Logar(TipoLog tipoLog, String mensagem, String stackTrace, CaseBusiness.Framework.BancoDados.AcessoDadosBase acessoDados, String campo, String tipoErroFisico, DateTime dataHora, App aplicacao, Tela tela, Int32 cdLogTransacao)
        {
            Int32 cod = 0;
            String parameters = "";

            try
            {
                if (acessoDados != null)
                {
                    if (acessoDados.Parameters != null)
                    {
                        foreach (DbParameter param in acessoDados.Parameters)
                        {
                            String direction = "";
                            if (param.Direction == System.Data.ParameterDirection.Input)
                                direction = "(I)";
                            else if (param.Direction == System.Data.ParameterDirection.Output)
                                direction = "(O)";
                            else if (param.Direction == System.Data.ParameterDirection.InputOutput)
                                direction = "(I/O)";
                            else
                                direction = "(RV)";

                            parameters += param.ParameterName + " " + param.Value.ToString() + " " + direction;
                        }
                    }
                    else
                        parameters = "AcessoDados.Parameters = NULL";
                }
                else
                    parameters = "AcessoDados = NULL";

                CaseBusiness.Framework.Configuracao.Configuracao._erroInicializacao = mensagem + " " + parameters;
                cod = new Processo.Log().Logar(tipoLog, mensagem + " " + parameters, stackTrace, campo, tipoErroFisico, dataHora, aplicacao, tela, cdLogTransacao);
            }
            catch (System.Exception ex)
            {
                switch (tipoLog)
                {
                    case TipoLog.Sucesso:
                        LogarArquivo(mensagem, TipoEventoLog.Sucesso, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                    case TipoLog.Informacao:
                        LogarArquivo(mensagem, TipoEventoLog.Informacao, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                    case TipoLog.Aviso:
                        LogarArquivo(mensagem, TipoEventoLog.Aviso, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                    case TipoLog.Erro:
                        LogarArquivo(mensagem, TipoEventoLog.Erro, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
                        break;
                }

                LogarArquivo(ex.Message, TipoEventoLog.Erro, CaseBusiness.Framework.Configuracao.Configuracao.NomeAplicacao);
            }

            return cod;
        }
    }
}
