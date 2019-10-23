using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseBusiness
{
    public class UsuarioManutencao
    {
        #region Constantes
        /// <summary>
        /// Esse ID de Usuário é utilizado pelo Serviço 
        /// e na criação de instancias de classes que se localizam em classes 
        /// onde não ha a UsuarioManutencao
        /// </summary>
        //public const Int32 IDUSUARIO_USOINTERNO = 9999;

        public const String kAPPKEY_USUARIOMANUTENCAO = "_usuarioManutencao";
        #endregion Constantes


        #region Atributos
        private Int32 _id = Int32.MinValue;
        private String _nome = String.Empty;
        private String _email = String.Empty;
        //private String _ddd= String.Empty;
        //private String _fone = String.Empty;
        private String _codigologin = String.Empty;
        private String _codigologinAD = String.Empty;
        private DateTime _dataHoraLogin = DateTime.MinValue;
        #endregion Atributos


        #region Propriedades
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public String EMail
        {
            get { return _email; }
            set { _email = value; }
        }

        //public String DDD
        //{
        //    get { return _ddd; }
        //    set { _ddd= value; }
        //}

        //public String Fone
        //{
        //    get { return _fone; }
        //    set { _fone = value; }
        //}

        public String CodigoLogin
        {
            get { return _codigologin; }
            set { _codigologin = value; }
        }

        public String CodigoLoginAD
        {
            get { return _codigologinAD; }
            set { _codigologinAD = value; }
        }

        public DateTime DataHoraLogin
        {
            get { return _dataHoraLogin; }
            set { _dataHoraLogin = value; }
        }
        #endregion Propriedades
    }
}
