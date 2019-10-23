using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseBusiness.CtrlAcesso
{
    public class UsuarioLogadoItemLista : IEquatable<UsuarioLogadoItemLista>
    {
        #region Atributos
        private Int32 _id = Int32.MinValue;
        private String _nome = String.Empty;
        private String _sessionId = String.Empty;
        //private String _email = String.Empty;
        ////private String _ddd= String.Empty;
        ////private String _fone = String.Empty;
        //private String _codigologin = String.Empty;
        //private String _codigologinAD = String.Empty;
        //private DateTime _dataHoraLogin = DateTime.MinValue;
        #endregion Atributos


        #region Propriedades
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            UsuarioLogadoItemLista objAsUsuarioLogadoLista = obj as UsuarioLogadoItemLista;
            if (objAsUsuarioLogadoLista == null) return false;
            else return Equals(objAsUsuarioLogadoLista);
        }

        public override int GetHashCode()
        {
            return IdUsuario;
        }

        public bool Equals(UsuarioLogadoItemLista usuario)
        {
            if (usuario == null) return false;

            return (this.IdUsuario.Equals(usuario.IdUsuario));
        }



        public Int32 IdUsuario
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public String SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }        
        
        //public String EMail
        //{
        //    get { return _email; }
        //    set { _email = value; }
        //}

        ////public String DDD
        ////{
        ////    get { return _ddd; }
        ////    set { _ddd= value; }
        ////}

        ////public String Fone
        ////{
        ////    get { return _fone; }
        ////    set { _fone = value; }
        ////}

        //public String CodigoLogin
        //{
        //    get { return _codigologin; }
        //    set { _codigologin = value; }
        //}

        //public String CodigoLoginAD
        //{
        //    get { return _codigologinAD; }
        //    set { _codigologinAD = value; }
        //}

        //public DateTime DataHoraLogin
        //{
        //    get { return _dataHoraLogin; }
        //    set { _dataHoraLogin = value; }
        //}
        #endregion Propriedades

        public UsuarioLogadoItemLista(Int32 idUsuario, String nome, String sessionID)
        {
            IdUsuario = idUsuario;
            Nome = nome;
            SessionId = sessionID;
        }
    }


    public class UsuarioLogadoItemListaComparer : IComparer<   UsuarioLogadoItemLista>
    {
        public int Compare(UsuarioLogadoItemLista x, UsuarioLogadoItemLista y)
        {
            if (x== null)
            {
                if (y== null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    //int retval = x.UsuarioNome.Length.CompareTo(y.UsuarioNome.Length);

                    //if (retval != 0)
                    //{
                    //    // If the strings are not of equal length,
                    //    // the longer string is greater.
                    //    //
                    //    return retval;
                    //}
                    //else
                    //{
                    // If the strings are of equal length,
                    // sort them with ordinary string comparison.
                    //
                    return x.Nome.CompareTo(y.Nome);
                    //}
                }
            }
        }
    }
}
