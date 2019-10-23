#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
#endregion Using


namespace CaseBusiness.CtrlAcesso
{
  public  class SistemaFuncionalidadeAcao_TreeViewNodeAcao
    {
        #region Atributos
        private Int32 _idSistema = Int32.MinValue;
        private Int32 _idSistemaFuncionalidade = Int32.MinValue;
        private String _codigoSistemaFuncionalidadeAcao = String.Empty;
        private String _acaonodevalue = String.Empty;
        private Boolean? _nodechecked;
        #endregion Atributos


        #region Propriedades
        public Int32 IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        public Int32 IdSistemaFuncionalidade
        {
            get { return _idSistemaFuncionalidade; }
            set { _idSistemaFuncionalidade = value; }
        }

        public String CodigoSistemaFuncionalidadeAcao
        {
            get { return _codigoSistemaFuncionalidadeAcao; }
            set { _codigoSistemaFuncionalidadeAcao = value; }
        }

        public String AcaoNodeValue
        {
            get { return _acaonodevalue; }
            set { _acaonodevalue = value; }
        }

        public Boolean? NodeChecked
        {
            get { return _nodechecked; }
            set { _nodechecked = value; }
        }
        #endregion Propriedades


        #region Construtores
        public SistemaFuncionalidadeAcao_TreeViewNodeAcao()
        {
        }

        public SistemaFuncionalidadeAcao_TreeViewNodeAcao(Int32 idSistema,
                                                          Int32 idSistemaFuncionalidade,
                                                          String codigoSistemaFuncionalidadeAcao)
        {
            _idSistema = idSistema;
            _idSistemaFuncionalidade = idSistemaFuncionalidade;
            _codigoSistemaFuncionalidadeAcao = codigoSistemaFuncionalidadeAcao;
            _acaonodevalue = AcaoValue_Montar(idSistema,
                                             idSistemaFuncionalidade,
                                             codigoSistemaFuncionalidadeAcao);
        }

        public SistemaFuncionalidadeAcao_TreeViewNodeAcao(String acaonodevalue)
        {
            _acaonodevalue = acaonodevalue;
            AcaoValue_Desmontar();
        }
        #endregion Construtores


        #region Métodos
        static public String AcaoValue_Montar(Int32 idSistema,
                                              Int32 idSistemaFuncionalidade,
                                              String codigoSistemaFuncionalidadeAcao)
        {
            String value = String.Empty;

            value = "ACAO"
                  + "|" + idSistema.ToString()
                  + "|" + idSistemaFuncionalidade.ToString()
                  + "|" + codigoSistemaFuncionalidadeAcao;
            return value;
        }

        private void AcaoValue_Desmontar()
        {
            String[] values;

            values = _acaonodevalue.Split('|');

            if (_acaonodevalue.StartsWith("ACAO|"))
            {
                _idSistema = Convert.ToInt32(values[1]);
                _idSistemaFuncionalidade = Convert.ToInt32(values[2]);
                _codigoSistemaFuncionalidadeAcao = values[3];
            }
        }
        #endregion Métodos
    }
}
