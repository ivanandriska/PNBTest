using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.Configuracao.Entidade
{
    public class Configuracao
    {
        internal static Aplicacao _aplicacao = new Aplicacao();
        internal static String _nomeAplicacao = "";
        internal static String _templateEmail = "";
        internal static List<String> _arquivosInicializacao = new List<String>();

        public static Aplicacao Aplicacao
        {
            get { return _aplicacao; }
        }

        public static String NomeAplicacao
        {
            get { return _nomeAplicacao; }
        }

        public static String TemplateEmail
        {
            get { return _templateEmail; }
        }

        public static List<String> ArquivosInicializacao
        {
            get { return _arquivosInicializacao; }
            set { _arquivosInicializacao = value; }
        }
    }
}
