/************************************************************
 * Desenvolvido por: Marco Sá                               *
 * Data: 26/02/2013                                         *
 * Descrição: Criado para iniciar a aplicação com as confi- *
 *            gurações necessárias para utilização do frame-*
 *            work de acesso a base de dados                *
 * *********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Collections;

namespace SMS.Api
{
    public class AplicacaoInicializacao
    {
        public bool iniciar()
        {
            CaseBusiness.Framework.Aplicacao app = new CaseBusiness.Framework.Aplicacao();
            bool Iniciar = false;
            List<string> arqsApp = new List<string> { };
           
            arqsApp.Add("configuracao.xml");

            CaseBusiness.Framework.Configuracao.Configuracao.BancoPrincipal = CaseBusiness.Framework.BancoDeDados.Case;

            if (app.Iniciar(CaseBusiness.Framework.TipoAplicacao.Web, CaseBusiness.Framework.App.SMSApi, arqsApp, "2rpnet."))
            {
                Iniciar = true;
             
            }
            else
            {
                if (CaseBusiness.Framework.Configuracao.Configuracao.ErroInicializacao != "")
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Erro ao inicializar: " + CaseBusiness.Framework.Configuracao.Configuracao.ErroInicializacao, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "CASE PORTAL");
                    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, CaseBusiness.Framework.Configuracao.Configuracao.ErroInicializacao, "", "", "", DateTime.Now, CaseBusiness.Framework.App.RetencaoApi, CaseBusiness.Framework.Tela.Nenhum, 0);
                }
            }
            return Iniciar;

        }
    }
}
