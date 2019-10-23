#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Global
{
    public class MensagemInterfaceDados
    {
        #region Atributos
        private Byte[] _dados = null;
        private String _dadosString = String.Empty;
        private MensagemInterfaceHeader _mensagemInterfaceHeader = null;
        #endregion Atributos

        #region Construtores
        /// <summary>
        /// Construtor classe InterfaceDados
        /// </summary>
        public MensagemInterfaceDados()
        {
        }

        #endregion

        #region Propriedades

        public Byte[] Dados
        {
            get
            {
                if (_dados == null && MensagemInterfaceHeader != null && !String.IsNullOrEmpty(_dadosString))
                {
                    if (MensagemInterfaceHeader.TipoFormato == "VAR")
                        _dados = generateBytesFromString(_dadosString);
                    else
                        _dados = Encoding.ASCII.GetBytes(_dadosString);
                }

                return _dados;
            }
            set { _dados = value; }
        }

        public String DadosString
        {
            get
            {
                if (String.IsNullOrEmpty(_dadosString) && MensagemInterfaceHeader != null && _dados != null)
                {
                    if (MensagemInterfaceHeader.TipoFormato == "VAR")
                        _dadosString = generateStringFromBytes(_dados);
                    else
                        _dadosString = Encoding.ASCII.GetString(_dados);
                }
                return _dadosString;
            }
            set => _dadosString = value;
        }
        public MensagemInterfaceHeader MensagemInterfaceHeader
        {
            get => _mensagemInterfaceHeader;
            set => _mensagemInterfaceHeader = value;
        }
        #endregion Propriedades

        #region Metodos

        private String generateStringFromBytes(Byte[] dados)
        {
            String msg = "";
            Boolean secondBitMap = false;
            Int32 fimConversao = 12;

            for (Int32 pos = 0; pos < dados.Length; pos++)
            {
                if (pos >= 0 && pos <= 3)
                {
                    msg += Encoding.ASCII.GetString(dados, pos, 1);
                }
                else if (pos > 3 && pos < fimConversao)
                {
                    if (pos == 4)
                    {
                        msg += Convert.ToInt32(dados[pos]).ToString("X");
                        String caracter = Convert.ToInt32(dados[pos]).ToString("X").PadLeft(2, '0').Substring(0, 1);
                        Byte[] valor = Encoding.ASCII.GetBytes(caracter);

                        Int32 hex = HexByteValue(valor[0]);
                        secondBitMap = (hex & 8) > 0;

                        if (secondBitMap)
                            fimConversao = 20;
                    }
                    else
                        msg += Convert.ToInt32(dados[pos]).ToString("X").PadLeft(2, '0');
                }
                else
                    msg += Encoding.ASCII.GetString(dados, pos, 1);
            }


            return msg;
        }

        private Byte[] generateBytesFromString(String stringDados)
        {
            Byte[] bytesMessage = new Byte[0];
            Boolean secondBitMap = false;
            Int32 fimConversao = 20;

            for (Int32 pos = 0; pos < stringDados.Length; pos++)
            {
                Array.Resize(ref bytesMessage, bytesMessage.Length + 1);

                if (pos >= 0 && pos <= 3)
                {
                    bytesMessage.SetValue(Encoding.ASCII.GetBytes(stringDados.Substring(pos, 1))[0], bytesMessage.Length - 1);
                }
                else if (pos > 3 && pos < fimConversao)
                {
                    if (pos == 4)
                    {
                        String caracter = Convert.ToByte(Convert.ToInt32(stringDados.Substring(pos, 2), 16)).ToString("X").PadLeft(2, '0').Substring(0, 1);
                        Byte[] valor = Encoding.ASCII.GetBytes(caracter);
                        Int32 hex = HexByteValue(valor[0]);
                        secondBitMap = (hex & 8) > 0;

                        if (secondBitMap)
                            fimConversao = 36;
                    }

                    bytesMessage.SetValue(Convert.ToByte(Convert.ToInt32(stringDados.Substring(pos, 2), 16)), bytesMessage.Length - 1);
                    pos++;
                }
                else
                {
                    bytesMessage.SetValue(Encoding.ASCII.GetBytes(stringDados.Substring(pos, 1))[0], bytesMessage.Length - 1);
                }
            }

            return bytesMessage;
        }

        private Int32 HexByteValue(Byte val)
        {
            Int16 what = Convert.ToInt16(val);

            if (val > 47 && val < 58)
            {
                return val - 48;
            }

            if ((val > 64 && val <= 70) || (val > 96 && val < 102))
            {
                return val - 55;
            }

            throw new ArgumentException("The byte is not a valid hex nibble rep", "val");
        }
        #endregion

    }
}
