using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CaseBusiness.ISO.MISO8583.Parsing;
using System.Linq;

namespace CaseBusiness.ISO.MISO8583
{
    /// <summary>
    /// This class can create ISO8583 messages from a template defined in a configuration
    /// file, or from a byte buffer that was read from a stream. For messages created
    /// from templates, it can set the current DateTime in field 4 and also use
    /// an ITraceGenerator to assign the new message a new trace number.
    /// </summary>
    public class MessageFactory
    {
        private ITraceGenerator traceGen;
        private Int32 etx = -1;


        /// <summary>
        /// Creates a MessageFactory using the default configuration
        /// </summary>
        /// <returns></returns>
        public MessageFactory()
        {
        }

        /// <summary>
        /// The ETX character to assign to new messages. The default is -1 which
        /// means not to use an ETX character.
        /// </summary>
        public Int32 Etx
        {
            set { etx = value; }
            get { return etx; }
        }

        /// <summary>
        /// If a TraceGenerator is set then it is used to assign
        /// a new message trace to every message created by the factory.
        /// Default is null. The trace is a 6-digit number in field 11.
        /// </summary>
        public ITraceGenerator TraceGenerator
        {
            set { traceGen = value; }
            get { return traceGen; }
        }



        /// <summary>
        /// Creates a new message of the given type. If there is a template
        /// for the message type, then it is used to set all the values in the
        /// new message (the values will be copied from the original messages,
        /// not referred to directly, to avoid affecting the templates if a value
        /// in a message created this way is modified). If the factory has an
        /// ITraceGenerator set, it uses it to assign a new trace number as a
        /// NUMERIC value of length 6 in field 11; if AssignDate is true,
        /// then the current DateTime is stored in field 7 as a DATE10 type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //public IsoMessage NewTemplateMessage(Int32 type)
        //{
        //    IsoMessage m = (ConfigParser.ISOHeaders.ContainsKey(type)) ? new IsoMessage(ConfigParser.ISOHeaders[type]) : new IsoMessage(null);

        //    m.Type = type;
        //    m.Etx = Etx;

        //    if (ConfigParser.ISOTypeTemplates.ContainsKey(type))
        //    {
        //        IsoMessage templ = ConfigParser.ISOTypeTemplates[type];

        //        for (Int32 i = 2; i < 128; i++)
        //        {
        //            if (templ.HasField(i))
        //                m.SetField(i, (IsoValue)templ.GetField(i).Clone());
        //        }
        //    }

        //    if (TraceGenerator != null)
        //    {
        //        m.SetValue(11, TraceGenerator.NextTrace(), IsoType.NUMERIC, 6, TypeElement.Field);
        //    }

        //    if (ConfigParser.ISOAssignDate)
        //    {
        //        m.SetValue(7, DateTime.Now, IsoType.DATE10, 10, TypeElement.Field);
        //    }

        //    return m;
        //}

        /// <summary>
        /// Creates a new message of the given type. If there is a template
        /// for the message type, then it is used to set all the values in the
        /// new message (the values will be copied from the original messages,
        /// not referred to directly, to avoid affecting the templates if a value
        /// in a message created this way is modified). If the factory has an
        /// ITraceGenerator set, it uses it to assign a new trace number as a
        /// NUMERIC value of length 6 in field 11; if AssignDate is true,
        /// then the current DateTime is stored in field 7 as a DATE10 type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //public IsoMessage NewParseMessage(Int32 type)
        //{
        //    IsoMessage m = (ConfigParser.ISOHeaders.ContainsKey(type)) ? new IsoMessage(ConfigParser.ISOHeaders[type]) : new IsoMessage(null);

        //    m.Type = type;
        //    m.Etx = Etx;

        //    if (ConfigParser.ISOParseMap.ContainsKey(type))
        //    {
        //        Dictionary<Int32, FieldParseInfo> guide = ConfigParser.ISOParseMap[type];
        //        List<Int32> index = ConfigParser.ISOParseOrder[type];

        //        foreach (Int32 i in index)
        //        {
        //            FieldParseInfo fpi = guide[i];

        //            if (fpi.SubData.Count > 0)
        //            {
        //                Dictionary<Int32, IsoValue> subFieldList = new Dictionary<int, IsoValue>();
        //                IsoValue subField = null;

        //                foreach (KeyValuePair<Int32, FieldParseInfo> fpiSub in fpi.SubData)
        //                {
        //                    switch (fpiSub.Value.Type)
        //                    {
        //                        case IsoType.NUMERIC:
        //                            subField = new IsoValue(fpiSub.Value.Type, "0".PadRight(fpiSub.Value.Length, '0'), fpiSub.Value.Length, "0".PadRight(fpiSub.Value.Length, '0'), fpiSub.Value.TypeElement);
        //                            break;
        //                        case IsoType.ALPHA:
        //                            subField = new IsoValue(fpiSub.Value.Type, "0".PadRight(fpiSub.Value.Length, '0'), fpiSub.Value.Length, "0".PadRight(fpiSub.Value.Length, '0'), fpiSub.Value.TypeElement);
        //                            break;
        //                        case IsoType.LLVAR:
        //                            subField = new IsoValue(fpiSub.Value.Type, "00", "00");
        //                            break;
        //                        case IsoType.LLLVAR:
        //                            subField = new IsoValue(fpiSub.Value.Type, "000", "000");
        //                            break;
        //                        case IsoType.DATE10:
        //                            subField = new IsoValue(fpiSub.Value.Type, DateTime.Now, DateTime.Now.ToString("MMddHHmmss"));
        //                            break;
        //                        case IsoType.DATE12:
        //                            subField = new IsoValue(fpiSub.Value.Type, DateTime.Now, DateTime.Now.ToString("yyMMddHHmmss"));
        //                            break;
        //                        case IsoType.DATE6:
        //                            subField = new IsoValue(fpiSub.Value.Type, DateTime.Now, DateTime.Now.ToString("yyMMdd"));
        //                            break;
        //                        case IsoType.DATE4:
        //                            subField = new IsoValue(fpiSub.Value.Type, DateTime.Now, DateTime.Now.ToString("MMdd"));
        //                            break;
        //                        case IsoType.DATE_EXP:
        //                            subField = new IsoValue(fpiSub.Value.Type, DateTime.Now, DateTime.Now.ToString("YYmm"));
        //                            break;
        //                        case IsoType.TIME:
        //                            subField = new IsoValue(fpiSub.Value.Type, DateTime.Now, DateTime.Now.ToString("HHmmss"));
        //                            break;
        //                        case IsoType.AMOUNT:
        //                            subField = new IsoValue(fpiSub.Value.Type, "000000000000", "000000000000");
        //                            break;
        //                    }

        //                    subFieldList.Add(fpiSub.Key, subField);
        //                }
        //                m.SetField(i, new IsoValue(fpi.Type, "0".PadRight(fpi.Length, '0'), fpi.Length, "0".PadRight(fpi.Length, '0'), "", subFieldList));
        //            }
        //            else
        //            {
        //                switch (fpi.Type)
        //                {
        //                    case IsoType.NUMERIC:
        //                        m.SetField(i, new IsoValue(fpi.Type, "0".PadRight(fpi.Length, '0'), fpi.Length, "0".PadRight(fpi.Length, '0'), fpi.TypeElement));
        //                        break;
        //                    case IsoType.ALPHA:
        //                        m.SetField(i, new IsoValue(fpi.Type, "0".PadRight(fpi.Length, '0'), fpi.Length, "0".PadRight(fpi.Length, '0'), fpi.TypeElement));
        //                        break;
        //                    case IsoType.LLVAR:
        //                        m.SetField(i, new IsoValue(fpi.Type, "00", "00"));
        //                        break;
        //                    case IsoType.LLLVAR:
        //                        m.SetField(i, new IsoValue(fpi.Type, "000", "000"));
        //                        break;
        //                    case IsoType.DATE10:
        //                        m.SetField(i, new IsoValue(fpi.Type, DateTime.Now, DateTime.Now.ToString("MMddHHmmss")));
        //                        break;
        //                    case IsoType.DATE12:
        //                        m.SetField(i, new IsoValue(fpi.Type, DateTime.Now, DateTime.Now.ToString("yyMMddHHmmss")));
        //                        break;
        //                    case IsoType.DATE6:
        //                        m.SetField(i, new IsoValue(fpi.Type, DateTime.Now, DateTime.Now.ToString("yyMMdd")));
        //                        break;
        //                    case IsoType.DATE4:
        //                        m.SetField(i, new IsoValue(fpi.Type, DateTime.Now, DateTime.Now.ToString("MMdd")));
        //                        break;
        //                    case IsoType.DATE_EXP:
        //                        m.SetField(i, new IsoValue(fpi.Type, DateTime.Now, DateTime.Now.ToString("YYmm")));
        //                        break;
        //                    case IsoType.TIME:
        //                        m.SetField(i, new IsoValue(fpi.Type, DateTime.Now, DateTime.Now.ToString("HHmmss")));
        //                        break;
        //                    case IsoType.AMOUNT:
        //                        m.SetField(i, new IsoValue(fpi.Type, "000000000000", "000000000000"));
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    if (TraceGenerator != null)
        //    {
        //        m.SetValue(11, TraceGenerator.NextTrace(), IsoType.NUMERIC, 6, TypeElement.Field);
        //    }

        //    if (ConfigParser.ISOAssignDate)
        //    {
        //        m.SetValue(7, DateTime.Now, IsoType.DATE10, 10, TypeElement.Field);
        //    }

        //    return m;
        //}

        /// <summary>
        /// Creates a response for the specified request, by creating a new
        /// message with a message type of the original request type plus 16.
        /// If there is a template for the resulting type, its values are copied
        /// onto the new message; after that, all the values from the original
        /// request that are not already in the response are copied to it.
        /// </summary>
        /// <param name="request">An ISO8583 request.</param>
        /// <returns>A new ISO8583 message with the corresponding response
        /// type for the request and with values already copied from its
        /// template (if any) and the request.</returns>
        public IsoMessage CreateResponse(CaseBusiness.CC.Global.MensagemInterfaceDados interfaceDados,  IsoMessage request)
        {
            Int32 responseType = request.Type + 16;

            IsoMessage resp = (ConfigParser.ISOHeaders.ContainsKey(responseType)) ? new IsoMessage(interfaceDados.MensagemInterfaceHeader.TipoCodificacao, ConfigParser.ISOHeaders[responseType]) : new IsoMessage();
            IsoMessage templ = null;

            resp.Type = responseType;
            resp.Etx = etx;

            if (ConfigParser.ISOTypeTemplates.ContainsKey(resp.Type))
                templ = ConfigParser.ISOTypeTemplates[resp.Type];

            if (templ != null)
            {
                for (Int32 i = 2; i < 128; i++)
                {
                    if (templ.Has(i))
                    {
                        resp.SetField(i, (IsoValue)templ.Get(i).Clone());

                        if (request.Has(i))
                        {
                            IsoValue field = request.Get(i);
                            if (field.SubData != null)
                            {
                                if (field.SubData.Count > 0)
                                {
                                    foreach (KeyValuePair<Int32, IsoValue> subValue in field.SubData)
                                    {
                                        if (templ.Has(i, subValue.Key))
                                        {
                                            IsoValue v = (IsoValue)templ.Get(i, subValue.Key).Clone();
                                            resp.SetSubData(i, subValue.Key, new IsoValue(v.Type, field.Base, subValue.Value, subValue.Value.ToString().Length, subValue.Value.ToString(), v.Requirements, v.TypeElement, v.SubElementIDLength, v.LengthOfLengthSubElement));
                                        }
                                    }
                                }
                                else
                                {
                                    resp.SetField(i, field);
                                }
                            }
                            else
                            {
                                resp.SetField(i, field);
                            }
                        }
                        else
                        {
                            if (templ.Get(i).Requirements == "CE")
                                resp.SetField(i, null);
                        }
                    }
                }
            }
            else
                resp = new IsoMessage();

            return resp;
        }

        /// <summary>
        /// Parses a buffer containing a message, considering the specified
        /// length as a prefix for the ISO header, using UTF8 encoding.
        /// </summary>
        /// <param name="buf">The buffer containing the message.</param>
        /// <param name="isoHeaderLength">The length of the ISO header.</param>
        /// <returns>The parsed message.</returns>
        public IsoMessage ParseMessage(CaseBusiness.CC.Global.MensagemInterfaceDados interfaceDados, Int32 isoHeaderLength, ParseMode parseMode)
        {
            Byte[] buf = interfaceDados.Dados;

            if (buf != null)
            {
                String message = "";
                Byte[] messageData = null;
                Boolean secondBitMap = false;
                Int32 fimConversao = 12;

                //MTI
                for (Int32 pos = 0; pos < fimConversao; pos++)
                {
                    if ((pos >= 0 && pos <= 3))
                    {
                        if (interfaceDados.MensagemInterfaceHeader.TipoCodificacao == EncodingType.EBCDIC)
                            message += ParseCharacter.convertEBCDIC2ASCII(new Byte[] { buf[pos] });
                        else
                            message += Encoding.ASCII.GetString(new Byte[] { buf[pos] });
                    }

                    //1º BitMap
                    if (pos > 3 && pos <= 11)
                    {
                        //Verifica se existe um 2º BitMap
                        if (pos == 4)
                        {
                            String caracter = "";
                            caracter = buf[pos].ToString("X").PadLeft(2, '0').Substring(0, 1);
                            Byte[] valor = Encoding.ASCII.GetBytes(caracter);
                            Int32 hex = HexByteValue(valor[0]);
                            secondBitMap = (hex & 8) > 0;

                            if (secondBitMap)
                                fimConversao = 20;
                        }
                        message += buf[pos].ToString("X").PadLeft(2, '0');
                    }

                    //2º BitMap
                    if (pos > 11)
                        message += buf[pos].ToString("X").PadLeft(2, '0');
                }

                messageData = new Byte[buf.Length - fimConversao];
                Array.Copy(buf, fimConversao, messageData, 0, messageData.Length);

                //Converte string em array de bytes
                Byte[] mti_bitmap = Encoding.ASCII.GetBytes(message);
                return ParseMessage(interfaceDados, mti_bitmap, messageData, isoHeaderLength, Encoding.UTF8, parseMode);
            }
            else
                return null;
        }

        /// <summary>
        /// Parses a byte buffer containing an ISO8583 message. The buffer must
        /// not include the length header. If it includes the ISO message header,
        /// then its length must be specified so the message type can be found.
        /// </summary>
        /// <param name="mti_bitmap">The byte buffer containing the message, starting
        /// at the ISO header or the message type.</param>
        /// <param name="isoHeaderLength">Specifies the position at which the message
        /// type is located, which is algo the length of the ISO header.</param>
        /// <param name="encoder">The encoder to use for reading string values.</param>
        /// <returns>The parsed message.</returns>
        private IsoMessage ParseMessage(CaseBusiness.CC.Global.MensagemInterfaceDados interfaceDados, Byte[] mti_bitmap, Byte[] messageData, Int32 isoHeaderLength, Encoding encoder, ParseMode parseMode)
        {
            Int32 pos = 0;

            IsoMessage m = (isoHeaderLength > 0) ? new IsoMessage(interfaceDados.MensagemInterfaceHeader.TipoCodificacao, encoder.GetString(mti_bitmap, 0, isoHeaderLength)) : new IsoMessage(EncodingType.NotDefined, null);
            m.isValid = true;
            m.MapFields = interfaceDados.MensagemInterfaceHeader.MapaCamposVAR;
            m.IdInterface = interfaceDados.MensagemInterfaceHeader.IdMensagemInterfaceHeader;

            Int32 type = ((mti_bitmap[isoHeaderLength] - 48) << 12) | ((mti_bitmap[isoHeaderLength + 1] - 48) << 8) | ((mti_bitmap[isoHeaderLength + 2] - 48) << 4) | (mti_bitmap[isoHeaderLength + 3] - 48);

            m.Type = type;

            //Parse the bitmap
            BitArray bs = ((HexByteValue(mti_bitmap[isoHeaderLength + ConfigParser.ISOLengthMTI]) & 8) > 0) ? new BitArray(128) : new BitArray(64);

            Int32 tamanhoMensagemAtePrimeiroMapaDeBits = ConfigParser.ISOLengthMTI + ConfigParser.ISOLengthBitMap;

            for (Int32 i = isoHeaderLength + ConfigParser.ISOLengthMTI; i < isoHeaderLength + tamanhoMensagemAtePrimeiroMapaDeBits; i++)
            {
                Int32 hex = HexByteValue(mti_bitmap[i]);
                bs.Set(pos++, (hex & 8) > 0);
                bs.Set(pos++, (hex & 4) > 0);
                bs.Set(pos++, (hex & 2) > 0);
                bs.Set(pos++, (hex & 1) > 0);
            }

            //Extended bitmap
            if (bs.Get(0))
            {
                Int32 tamanhoMensagemAteSegundoMapaDeBits = tamanhoMensagemAtePrimeiroMapaDeBits + ConfigParser.ISOLengthBitMap;

                for (Int32 i = isoHeaderLength + tamanhoMensagemAtePrimeiroMapaDeBits; i < isoHeaderLength + tamanhoMensagemAteSegundoMapaDeBits; i++)
                {
                    Int32 hex = HexByteValue(mti_bitmap[i]);
                    bs.Set(pos++, (hex & 8) > 0);
                    bs.Set(pos++, (hex & 4) > 0);
                    bs.Set(pos++, (hex & 2) > 0);
                    bs.Set(pos++, (hex & 1) > 0);
                }
            }

            pos = 0;
            m.BitArray = bs;

            foreach (KeyValuePair<Int32, FieldParseInfo> i in interfaceDados.MensagemInterfaceHeader.MapaCamposVAR)
            {
                try
                {
                    if (!bs.Get(i.Key - 1)) //Só efetua checagem se o field do mapa existe na mensagem, caso não tenha, pula pro proximo field ou gera exception e pula pro proximo. Class BitArray não tem opção para checar se um bit está ligado/desligado
                        continue;
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }

                FieldParseInfo fpi = i.Value;
                IsoValue val = fpi.Parse(interfaceDados.MensagemInterfaceHeader.TipoCodificacao, messageData, pos, encoder, false);

                if (val.OriginalValue != null)
                {
                    if (parseMode == ParseMode.Complete)
                    {
                        if (fpi.SubDataElements_Fields != null)
                        {
                            if (fpi.SubDataElements_Fields.Count > 0)
                                val = parseDataElement_Field(interfaceDados.MensagemInterfaceHeader.TipoCodificacao, fpi.Base, fpi.SubDataElements_Fields, val, encoder, ref pos);

                            if (val == null)
                            {
                                m.isValid = false;
                                return m;
                            }
                        }
                    }

                    if (interfaceDados.MensagemInterfaceHeader.TipoCodificacao == EncodingType.EBCDIC && fpi.Base == ConfigParser.Base.Hexadecimal)
                        pos += val.Length / 2;
                    else
                        pos += val.Length;

                    if (val.Type == IsoType.LLVAR)
                        pos += 2;
                    else if (val.Type == IsoType.LLLVAR)
                        pos += 3;

                    m.SetField(i.Key, val);

                    if (fpi.Required && !bs.Get(i.Key - 1))
                        m.isValid = false;
                }
            }

            return m;
        }

        /// <summary>
        /// Parse and store the data in its respective subelement/subfield 
        /// </summary>
        /// <param name="configBase"></param>
        /// <param name="subDataElements_Fields"></param>
        /// <param name="val"></param>
        /// <param name="encoder"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        internal IsoValue parseDataElement_Field(CaseBusiness.ISO.EncodingType encoding, ConfigParser.Base configBase, Dictionary<Int32, FieldParseInfo> subDataElements_Fields, IsoValue val, Encoding encoder, ref Int32 pos)
        {
            Byte[] bufSub = null;

            if (configBase == ConfigParser.Base.B64)
                bufSub = Convert.FromBase64String(val.OriginalValue);
            else
                bufSub = Encoding.ASCII.GetBytes(val.OriginalValue);

            Int32 posLeitura = 0;
            Int32 multFieldHex = 1; //variavel para controle de campos do tipo Binary (recebido no formato Hex)
            Int32 multHex = 1; //variavel para controle de campos do tipo Binary (recebido no formato Hex)
            Int32 DE_FieldsChecked = 0;

            for (Int32 i = 0; DE_FieldsChecked < subDataElements_Fields.Count; i++)
            {
                Int32 posSub = 0;
                KeyValuePair<Int32, FieldParseInfo> subData = subDataElements_Fields.ElementAt(i);

                if (val.SubData.Keys.Contains(subData.Key))
                    continue;

                if (posLeitura > 0)
                    posSub = posLeitura;

                if (subData.Value.TypeElement == TypeElement.SubElement)
                {
                    #region SubElement

                    Int32 numSubElementArray = 0;

                    if (configBase == ConfigParser.Base.Hexadecimal) //Quando campo Binario, é tratado como campo Hexadecimal, logo a extração de bytes é dobrada
                    {
                        multFieldHex = 2;
                        multHex = 2;
                        numSubElementArray = Convert.ToInt32(Encoding.ASCII.GetString(bufSub, posSub, subData.Value.SubElementIDLength * multFieldHex), 16);
                    }
                    else
                    {
                        multFieldHex = 1;
                        multHex = 1;
                        numSubElementArray = Convert.ToInt32(Encoding.ASCII.GetString(bufSub, posSub, subData.Value.SubElementIDLength));
                    }

                    if (numSubElementArray == subData.Key)
                    {
                        posSub += subData.Value.SubElementIDLength * multFieldHex;


                        Int32 tamSubElement = (configBase == ConfigParser.Base.Hexadecimal ? Convert.ToInt32(Encoding.ASCII.GetString(bufSub, posSub, subData.Value.LengthOfLengthSubElement * multFieldHex), 16) : Convert.ToInt32(Encoding.ASCII.GetString(bufSub, posSub, subData.Value.LengthOfLengthSubElement * multFieldHex))) * multHex;
                        posSub += subData.Value.LengthOfLengthSubElement * multFieldHex;
                        String value = "";
                        IsoValue valSub = null;
                        //Byte[] bufSubSub = null;

                        if (configBase == ConfigParser.Base.B64)
                        {
                            value = Convert.ToBase64String(bufSub, posSub, tamSubElement);
                            //bufSubSub = Encoding.ASCII.GetBytes(valSub.OriginalValue);
                        }
                        else
                        {
                            value = Encoding.ASCII.GetString(bufSub, posSub, tamSubElement);
                           // bufSubSub = Encoding.ASCII.GetBytes(valSub.OriginalValue);
                        }

                        valSub = new IsoValue(subData.Value.Type, configBase, value, value.Length, value, "", subData.Value.TypeElement, subData.Value.SubElementIDLength, subData.Value.LengthOfLengthSubElement);

                        if (subData.Value.SubDataElements_Fields == null || subData.Value.SubDataElements_Fields.Count == 0)
                            val.SubData.Add(subData.Key, valSub);
                        else  //Quando: subData.Value.SubDataElements_Fields.Count > 0
                            val.SubData.Add(subData.Key, parseDataElement_Field(encoding, configBase, subData.Value.SubDataElements_Fields, valSub, encoder, ref pos));

                        posSub += tamSubElement;
                        posLeitura = posSub;
                        DE_FieldsChecked++;
                        i = -1;
                    }
                    else if (i ==  (subDataElements_Fields.Count - 1))
                    {
                        DE_FieldsChecked++;
                        i = -1;
                    }

                    #endregion SubElement
                }
                else
                {
                    #region SubField
                    if (posSub < bufSub.Length)
                    {
                        IsoValue valSub = subData.Value.Parse(encoding, bufSub, posSub, encoder, true);

                        if (subData.Value.SubDataElements_Fields == null || subData.Value.SubDataElements_Fields.Count == 0)
                        {
                            if (valSub.OriginalValue != null)
                            {
                                val.SubData.Add(subData.Key, valSub);
                                posSub += valSub.Length;
                                posLeitura += valSub.Length;

                                if (valSub.Type == IsoType.LLVAR)
                                    pos += 2;
                                else if (valSub.Type == IsoType.LLLVAR)
                                    pos += 3;
                            }
                            else
                                return null;
                        }
                        else
                            val.SubData.Add(subData.Key, parseDataElement_Field(encoding, configBase, subData.Value.SubDataElements_Fields, valSub, encoder, ref pos));
                    }

                    DE_FieldsChecked++;
                    #endregion
                }

                if (posSub >= val.Length)
                    break;
            }

            val.CompletelyParsed = true;
            return val;
        }

        /// <summary>
        /// Format the message for to send to the source
        /// </summary>
        /// <param name="mensagem">String to format</param>
        /// <returns>Array bytes</returns>
        public Byte[] FormatMessage(CaseBusiness.CC.Global.MensagemInterfaceDados interfaceDados, String mensagem)
        {
            Byte[] bytesMessage = new Byte[2];
            //Byte[] bytesTemp = null;
            Boolean secondBitMap = false;

            bytesMessage[0] = 0;
            bytesMessage[1] = 0;

            for (Int32 p = 0; p < mensagem.Length; p++)
            {
                //MTI
                if (p >= 0 && p < 4)
                {
                    Array.Resize(ref bytesMessage, bytesMessage.Length + 1);

                    if (interfaceDados.MensagemInterfaceHeader.TipoCodificacao == EncodingType.EBCDIC)
                        bytesMessage.SetValue(Convert.ToByte(ParseCharacter.convertASCII2EBCDIC(mensagem.Substring(p, 1))[0]), bytesMessage.Length - 1);
                    else
                        bytesMessage.SetValue(Convert.ToByte(mensagem.Substring(p, 1)[0]), bytesMessage.Length - 1);
                }

                //1º BitMap
                if (p >= 4 && p < 20)
                {
                    //Verifica se existe um 2º BitMap
                    if (p == 4)
                    {
                        Byte[] valor = Encoding.ASCII.GetBytes(mensagem.Substring(p, 1));
                        Int32 hex = HexByteValue(valor[0]);
                        secondBitMap = (hex & 8) > 0;
                    }
                    Array.Resize(ref bytesMessage, bytesMessage.Length + 1);
                    bytesMessage.SetValue(Convert.ToByte(Convert.ToInt32(mensagem.Substring(p, 2), 16)), bytesMessage.Length - 1);
                    p++;
                }

                //2º BitMap ou DEs
                if (p >= 20)
                {
                    //Leitura 2º BitMap
                    if (secondBitMap && p <= 35)
                    {
                        Array.Resize(ref bytesMessage, bytesMessage.Length + 1);
                        bytesMessage.SetValue(Convert.ToByte(Convert.ToInt32(mensagem.Substring(p, 2), 16)), bytesMessage.Length - 1);
                        p++;
                    }

                    //Leitura DEs
                    if (!secondBitMap || p > 35)
                    {
                        Array.Resize(ref bytesMessage, bytesMessage.Length + 1);
                        if (interfaceDados.MensagemInterfaceHeader.TipoCodificacao == EncodingType.EBCDIC)
                            bytesMessage.SetValue(Convert.ToByte(ParseCharacter.convertASCII2EBCDIC(mensagem.Substring(p, 1))[0]), bytesMessage.Length - 1);
                        else
                            bytesMessage.SetValue(Convert.ToByte(mensagem.Substring(p, 1)[0]), bytesMessage.Length - 1);
                    }
                }
            }

            //Seta o tamanho da mensagem
            Int32 l = bytesMessage.Length - 2;
            String preMessageLength = l.ToString("X").PadLeft(4, '0');

            bytesMessage[0] = Convert.ToByte(Convert.ToInt32(preMessageLength.Substring(0, 2), 16));
            bytesMessage[1] = Convert.ToByte(Convert.ToInt32(preMessageLength.Substring(2, 2), 16));

            return bytesMessage;
        }

        /// <summary>
        /// Parses a byte containing a Hex representation (a digit from 0 to 9
        /// or a letter from A to F upper or lower case), and returns the value
        /// which can be from 0 to 15 inclusive.
        /// </summary>
        /// <param name="val">The byte containing the Hex representation of a nibble.</param>
        /// <returns>The real value of the nibble, between 0 and 15 inclusive.</returns>
        /// <exception cref="ArgumentException">When the value is not a valid Hex rep of a nibble.</exception>
        private static Int32 HexByteValue(Byte val)
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

       
    }
}