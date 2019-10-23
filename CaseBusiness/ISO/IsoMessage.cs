using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using CaseBusiness.ISO.MISO8583;
using CaseBusiness.ISO.MISO8583.Parsing;
using System.Collections.Concurrent;

namespace CaseBusiness.ISO
{
    /// <summary>
    /// This class represents an ISO8583 Message. It contains up to 127 fields
    /// numbered from 2 to 128; field 1 is reserved for the secondary bitmap
    /// and the bitmaps are calculated automatically by the message when it is
    /// going to be written to a stream. It can be a binary or an ASCII message;
    /// ASCII messages write themselves as their string representations and
    /// binary messages can write binary values to the stream, encoding numeric
    /// values using BCD.
    /// </summary>
    [Serializable]
    public class IsoMessage
    {
        #region Atributes
        const String HEX = "0123456789ABCDEF";

        Int32 type;
        Boolean binary;
        String isoHeader;
        Boolean isvalid;
        String messageError = null;
        BitArray bitBinary;

        Int32 etx = -1;
        Dictionary<Int32, IsoValue> fields = new Dictionary<Int32, IsoValue>();
        ConcurrentDictionary<Int32, FieldParseInfo> mapFields = new ConcurrentDictionary<Int32, FieldParseInfo>();
        String idInterface = "";
        private ISO.EncodingType _encodingType = EncodingType.NotDefined;

        #endregion

        #region Constructor
        public IsoMessage()
        {
        }

        #endregion

        #region Properties
        /// <summary>
        /// Creates a new instance with the specified header. This header
        /// will be written after the length and before the message type.
        /// </summary>
        /// <param name="header"></param>
        internal IsoMessage(ISO.EncodingType encodingType, String header)
        {
            isoHeader = header;
            _encodingType = encodingType;
        }

        /// <summary>
        /// The ISO8583 message header, which goes after the length header
        /// and before the message type.
        /// </summary>
        public String IsoHeader
        {
            get
            {
                return isoHeader;
            }
        }

        /// <summary>
        /// Messagem error
        /// </summary>
        public String MessageError
        {
            set
            {
                messageError = value;
            }
            get
            {
                return messageError;
            }
        }

        /// <summary>
        /// The message type (0x200, 0x400, etc).
        /// </summary>
        public Int32 Type
        {
            set
            {
                type = value;
            }
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Indicates if the message is binary (true) or ASCII (false).
        /// </summary>
        public Boolean Binary
        {
            set
            {
                binary = value;
            }
            get
            {
                return binary;
            }
        }

        /// <summary>
        /// The optional END TEXT character, which goes at the end of the message.
        /// </summary>
        public Int32 Etx
        {
            set
            {
                etx = value;
            }
            get
            {
                return etx;
            }
        }

        /// <summary>
        /// Indicates if message is Valid or not
        /// </summary>
        public Boolean isValid
        {
            set
            {
                isvalid = value;
            }
            get
            {
                return isvalid;
            }
        }

        /// <summary>
        /// Store Bit Binary of message 
        /// </summary>
        public BitArray BitArray
        {
            set
            {
                bitBinary = value;
            }
            get
            {
                return bitBinary;
            }
        }


        /// <summary>
        /// Map of fields used to parse the message
        /// </summary>
        public ConcurrentDictionary<Int32, FieldParseInfo> MapFields
        {
            set
            {
                mapFields = value;
            }
            get
            {
                return mapFields;
            }
        }

        /// <summary>
        /// Interface's ID used to parse the message
        /// </summary>
        public String IdInterface
        {
            set
            {
                idInterface = value;
            }
            get
            {
                return idInterface;
            }
        }
        #endregion

        #region SetMethod

        /// <summary>
        /// Stores the given IsoValue in the specified field index.
        /// </summary>
        /// <param name="index">The field index (2 to 128)</param>
        /// <param name="field">The IsoValue to store under that index.</param>
        public void SetField(Int32 index, IsoValue field)
        {
            if (index < 2 || index > 128) { throw new ArgumentOutOfRangeException("Index must be between 2 and 128"); }

            if (field == null)
            {
                fields.Remove(index);
                return;
            }

            fields[index] = field;
        }

        /// <summary>
        /// Stores the given sub IsoValue in the specified field index.
        /// </summary>
        /// <param name="index">The field index (2 to 128)</param>
        /// <param name="indexSub">The subField index </param>
        /// <param name="field">The sub IsoValue to store under that index.</param>
        public void SetSubData(Int32 index, Int32 indexSub, IsoValue subData)
        {
            if (subData == null)
            {
                fields[index].SubData.Remove(indexSub);
                return;
            }

            fields[index].SubData[indexSub] = subData;
        }


        /// <summary>
        /// Creates an IsoValue with the given values and stores it in the specified index.
        /// </summary>
        /// <param name="index">The field index (2 to 128)</param>
        /// <param name="value">An object value to store inside an IsoValue.</param>
        /// <param name="t">The ISO8583 for this value.</param>
        /// <param name="length">The length of the value (useful only for NUMERIC and ALPHA types).</param>
        public void SetValue(MISO8583.Parsing.ConfigParser.Base configBase, Int32 index, Object value, IsoType t, Int32 length, String req, Dictionary<Int32, IsoValue> sub, TypeElement typeElement)
        {
            if (index < 2 || index > 128) { throw new ArgumentOutOfRangeException("Index must be between 2 and 128"); }

            if (value == null)
            {
                fields.Remove(index);
                return;
            }

            if (typeElement == TypeElement.Field || typeElement == TypeElement.SubField)
            {
                if (IsoTypeHelper.NeedsLength(t))
                {
                    fields[index] = new IsoValue(t, configBase, value, length, value.ToString(), req, sub);
                    return;
                }
            }

            fields[index] = new IsoValue(t, configBase, value, value.ToString(), req, sub);
        }

        #endregion

        #region HasMethod

        public Boolean Has(params Int32[] TrackKeyValue)
        {
            if (TrackKeyValue.Length == 0)
                throw new ArgumentOutOfRangeException("At least one TrackKeyValue must be informed");

            if (TrackKeyValue[0] < 2 || TrackKeyValue[0] > 128)
                throw new ArgumentOutOfRangeException("First value of TrackKeyValue must be between 2 and 128");

            return Has(0, fields, TrackKeyValue);

        }

        private Boolean Has(Int32 level, Dictionary<Int32, IsoValue> fields, params Int32[] TrackKeyValue)
        {
            Int32 pos = 0;

            if (level == 0)
            {
                if (!fields.ContainsKey(TrackKeyValue[0]))
                    return false;
            }

            if (fields.ContainsKey(TrackKeyValue[level]))
            {
                if (level == (TrackKeyValue.Length - 1))
                    return true;
                else
                {
                    if (!fields[TrackKeyValue[0]].CompletelyParsed)
                    {
                        if (MapFields[TrackKeyValue[0]].SubDataElements_Fields != null && MapFields[TrackKeyValue[0]].SubDataElements_Fields.Count > 0)
                            fields[TrackKeyValue[0]] = new MessageFactory().parseDataElement_Field(_encodingType, MapFields[TrackKeyValue[0]].Base, MapFields[TrackKeyValue[0]].SubDataElements_Fields, fields[TrackKeyValue[0]], Encoding.UTF8, ref pos);
                    }
                }

                if (fields[TrackKeyValue[level]].SubData != null && fields[TrackKeyValue[level]].SubData.Count > 0)
                    return Has(level + 1, fields[TrackKeyValue[level]].SubData, TrackKeyValue);
            }

            return false;
        }
        #endregion

        #region GetMethod

        /// <summary>
        /// Get the value inside an ISO Message
        /// </summary>
        /// <param name="TrackKeyValue">Supply the absolute way of the value to get. I.e.: supplying the parameters {48,42,1}, will get the value of the: Field 48, Subelement 42, Subfield 1</param>
        /// <returns>Return the IsoValue of the way supplied</returns>
        public IsoValue Get(params Int32[] TrackKeyValue)
        {
            if (TrackKeyValue.Length == 0)
                throw new ArgumentOutOfRangeException("At least one TrackKeyValue must be informed");

            if (TrackKeyValue[0] < 2 || TrackKeyValue[0] > 128)
                throw new ArgumentOutOfRangeException("First value of TrackKeyValue must be between 2 and 128");

            return Get(0, fields, TrackKeyValue);
        }

        private IsoValue Get(Int32 level, Dictionary<Int32, IsoValue> fields, params Int32[] TrackKeyValue)
        {
            Int32 pos = 0;
            if (fields.ContainsKey(TrackKeyValue[level]))
            {
                if (level == (TrackKeyValue.Length - 1))
                    return fields[TrackKeyValue[level]];

                if (fields[TrackKeyValue[level]].SubData != null && fields[TrackKeyValue[level]].SubData.Count > 0)
                    return Get(level + 1, fields[TrackKeyValue[level]].SubData, TrackKeyValue);
                else
                {
                    if (MapFields[TrackKeyValue[level]].SubDataElements_Fields.Count > 0)
                    {
                        fields[TrackKeyValue[level]] = new MessageFactory().parseDataElement_Field(_encodingType, MapFields[TrackKeyValue[level]].Base, MapFields[TrackKeyValue[level]].SubDataElements_Fields, fields[TrackKeyValue[level]], Encoding.UTF8, ref pos);
                        return Get(level + 1, fields[TrackKeyValue[level]].SubData, TrackKeyValue);
                    }
                }
            }

            return null;
        }

        #endregion

        #region OthersMethod
        /// <summary>
        /// Writes the entire message to a stream, using the specified number
        /// of bytes to write a length header first.
        /// </summary>
        /// <param name="outs">The stream to write the message to.</param>
        /// <param name="lenBytes">The number of bytes to write the length of the message in. Can be anything from 1 to 4.</param>
        /// <param name="countEtx">Indicates if the ETX character (if present) should be counted as part of the message, for the length header.</param>
        public void Write(Stream outs, Int32 lenBytes, Boolean countEtx)
        {
            if (lenBytes > 4) { throw new ArgumentException("Length header can have at most 4 bytes"); }

            Byte[] data = WriteInternal();

            if (lenBytes > 0)
            {
                Int32 l = (etx > -1 && countEtx) ? data.Length + 1 : data.Length;

                Byte[] buf = new Byte[lenBytes];

                if (binary)
                {
                    Int32 pos = 0;

                    if (lenBytes == 4)
                    {
                        buf[pos] = (Byte)((l & 0xff000000) >> 24);
                        pos++;
                    }

                    if (lenBytes > 2)
                    {
                        buf[pos] = (Byte)((l & 0xff0000) >> 16);
                        pos++;
                    }

                    if (lenBytes > 1)
                    {
                        buf[pos] = (Byte)((l & 0xff00) >> 8);
                        pos++;
                    }

                    buf[pos] = (Byte)(l & 0xff);
                }
                else
                {
                    Encoding.ASCII.GetBytes(l.ToString("0000"), 0, lenBytes, buf, 0);
                }

                outs.Write(buf, 0, buf.Length);
            }

            outs.Write(data, 0, data.Length);

            //ETX
            if (etx > -1) { outs.WriteByte((Byte)etx); }

            outs.Flush();
        }

        protected Byte[] WriteInternal()
        {
            MemoryStream ms = new MemoryStream(16);

            Byte[] buf;

            if (isoHeader != null)
            {
                buf = Encoding.ASCII.GetBytes(isoHeader);
                ms.Write(buf, 0, buf.Length);
            }

            if (binary)
            {
                ms.WriteByte((Byte)((type & 0xff00) >> 8));
                ms.WriteByte((Byte)(type & 0xff));
            }
            else
            {
                String x = type.ToString("x4");
                ms.Write(Encoding.ASCII.GetBytes(x), 0, 4);
            }

            //TODO write the bitmap
            Dictionary<Int32, IsoValue>.KeyCollection keys = fields.Keys;

            BitArray bits = new BitArray(64);

            foreach (Int32 i in keys)
            {
                if (i > 64)
                {
                    bits.Length = 128;
                    bits.Set(0, true);
                }

                bits.Set(i - 1, true);
            }

            if (binary)
            {
                buf = new Byte[bits.Length / 8];
                bits.CopyTo(buf, 0);
            }
            else
            {
                buf = new Byte[bits.Length / 4];
                Int32 pos = 0;
                Int32 lim = bits.Length / 4;
                for (Int32 i = 0; i < lim; i++)
                {
                    Int32 nibble = 0;
                    if (bits.Get(pos++)) nibble += 8;
                    if (bits.Get(pos++)) nibble += 4;
                    if (bits.Get(pos++)) nibble += 2;
                    if (bits.Get(pos++)) nibble++;

                    Encoding.ASCII.GetBytes(HEX, nibble, 1, buf, i);
                }
            }

            ms.Write(buf, 0, buf.Length);

            //Write each field/subField
            for (Int32 i = 1; i < bits.Length; i++)
            {
                if (fields.ContainsKey(i))
                {
                    if (fields[i].SubData != null)
                    {
                        if (fields[i].SubData.Count > 0)
                        {
                            String dataField = "";

                            foreach (KeyValuePair<Int32, IsoValue> subValue in fields[i].SubData)
                            {
                                String dataSubField = "";
                                if (subValue.Value.SubData != null)
                                {
                                    if (subValue.Value.SubData.Count > 0)
                                    {
                                        foreach (KeyValuePair<Int32, IsoValue> subSubValue in subValue.Value.SubData)
                                        {
                                            if (subSubValue.Value.TypeElement == TypeElement.SubElement)
                                            {
                                                if (subSubValue.Value.Value.ToString().Length > 0)
                                                {
                                                    dataSubField += subSubValue.Key.ToString().PadLeft(subSubValue.Value.SubElementIDLength, '0');
                                                    dataSubField += subSubValue.Value.ToString().Length.ToString().PadLeft(subSubValue.Value.LengthOfLengthSubElement, '0');
                                                }
                                            }
                                            else
                                            {
                                                if (subSubValue.Value.Type == IsoType.LLVAR)
                                                    dataSubField += subSubValue.Value.ToString().Length.ToString().PadLeft(2, '0');

                                                if (subSubValue.Value.Type == IsoType.LLLVAR)
                                                    dataSubField += subSubValue.Value.ToString().Length.ToString().PadLeft(3, '0');
                                            }
                                            dataSubField += subSubValue.Value.ToString();
                                        }

                                        if (fields[i].SubData[subValue.Key].Type == IsoType.ALPHA || fields[i].SubData[subValue.Key].Type == IsoType.NUMERIC)
                                            fields[i].SubData[subValue.Key].Length = dataSubField.Length;

                                        fields[i].SubData[subValue.Key].Value = Format(fields[i].SubData[subValue.Key].Type, dataSubField);

                                        if (subValue.Value.TypeElement == TypeElement.SubElement)
                                        {
                                            if (dataSubField.Length > 0)
                                                dataSubField = subValue.Key.ToString().PadLeft(subValue.Value.SubElementIDLength, '0') + dataSubField.Length.ToString().PadLeft(subValue.Value.LengthOfLengthSubElement, '0') + dataSubField;
                                        }
                                        else
                                        {
                                            if (subValue.Value.Type == IsoType.LLVAR)
                                                dataSubField += dataSubField.Length.ToString().PadLeft(2, '0');

                                            if (subValue.Value.Type == IsoType.LLLVAR)
                                                dataSubField += dataSubField.Length.ToString().PadLeft(3, '0');
                                        }

                                        dataField += dataSubField;
                                    }
                                    else
                                    {
                                        if (subValue.Value.TypeElement == TypeElement.SubElement)
                                        {
                                            if (subValue.Value.ToString().Length > 0)
                                                dataSubField = subValue.Key.ToString().PadLeft(subValue.Value.SubElementIDLength, '0') + subValue.Value.Length.ToString().PadLeft(subValue.Value.LengthOfLengthSubElement, '0');
                                        }
                                        else
                                        {
                                            if (subValue.Value.Type == IsoType.LLVAR)
                                                dataSubField += subValue.Value.ToString().Length.ToString().PadLeft(2, '0');

                                            if (subValue.Value.Type == IsoType.LLLVAR)
                                                dataSubField += subValue.Value.ToString().Length.ToString().PadLeft(3, '0');
                                        }

                                        dataSubField += subValue.Value.ToString();
                                        dataField += dataSubField;
                                    }
                                }
                                else
                                {
                                    if (subValue.Value.TypeElement == TypeElement.SubElement)
                                    {
                                        if (subValue.Value.ToString().Length > 0)
                                            dataSubField = subValue.Key.ToString().PadLeft(subValue.Value.SubElementIDLength, '0') + subValue.Value.Length.ToString().PadLeft(subValue.Value.LengthOfLengthSubElement, '0');
                                    }
                                    else
                                    {
                                        if (subValue.Value.Type == IsoType.LLVAR)
                                            dataSubField += subValue.Value.ToString().Length.ToString().PadLeft(2, '0');

                                        if (subValue.Value.Type == IsoType.LLLVAR)
                                            dataSubField += subValue.Value.ToString().Length.ToString().PadLeft(3, '0');
                                    }

                                    dataSubField += subValue.Value.ToString();
                                    dataField += dataSubField;
                                }
                            }

                            if (fields[i].Type == IsoType.ALPHA || fields[i].Type == IsoType.NUMERIC)
                                fields[i].Length = dataField.Length;

                            fields[i].Value = Format(fields[i].Type, dataField);
                            fields[i].Write(ms);
                        }
                        else
                        {
                            fields[i].Write(ms);
                        }
                    }
                    else
                    {
                        fields[i].Write(ms);
                    }
                }
            }

            return ms.ToArray();
        }

        private Object Format(IsoType type, Object value)
        {
            switch (type)
            {
                case IsoType.NUMERIC:
                    return value;
                case IsoType.ALPHA:
                    return value;
                case IsoType.LLVAR:
                    return value;
                case IsoType.LLLVAR:
                    return value;
                case IsoType.DATE10:
                    return new DateTime(DateTime.Now.Year, Convert.ToInt32(value.ToString().Substring(0, 2)), Convert.ToInt32(value.ToString().Substring(2, 2)), Convert.ToInt32(value.ToString().Substring(4, 2)), Convert.ToInt32(value.ToString().Substring(6, 2)), Convert.ToInt32(value.ToString().Substring(8, 2)));
                case IsoType.DATE12:
                    return new DateTime(Convert.ToInt32(value.ToString().Substring(0, 2)), Convert.ToInt32(value.ToString().Substring(2, 2)), Convert.ToInt32(value.ToString().Substring(4, 2)), Convert.ToInt32(value.ToString().Substring(6, 2)), Convert.ToInt32(value.ToString().Substring(8, 2)), Convert.ToInt32(value.ToString().Substring(10, 2)));
                case IsoType.DATE4:
                    return new DateTime(DateTime.Now.Year, Convert.ToInt32(value.ToString().Substring(0, 2)), Convert.ToInt32(value.ToString().Substring(2, 2)));
                case IsoType.DATE6:
                    return new DateTime(Convert.ToInt32(value.ToString().Substring(0, 2)), Convert.ToInt32(value.ToString().Substring(2, 2)), Convert.ToInt32(value.ToString().Substring(4, 2)));
                case IsoType.DATE_EXP:
                    DateTime dt = new DateTime(Convert.ToInt32(value.ToString().Substring(0, 2)), Convert.ToInt32(value.ToString().Substring(2, 2)), 1);
                    dt = dt.AddMonths(1);
                    dt = dt.AddDays(-1);
                    return dt;
                case IsoType.TIME:
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(value.ToString().Substring(6, 2)), Convert.ToInt32(value.ToString().Substring(8, 2)), Convert.ToInt32(value.ToString().Substring(10, 2)));
                case IsoType.AMOUNT:
                    return new Decimal(Convert.ToInt32(value));
                default:
                    return "";
            }
        }

        public override Boolean Equals(object obj)
        {
            IsoMessage msg = obj as IsoMessage;

            if (msg == null) return false;

            return (msg.ToString() == this.ToString());
        }

        public override int GetHashCode()
        {
            Int32 hash = base.GetHashCode();

            foreach (IsoValue value in this.fields.Values)
            {
                hash += value.GetHashCode();
            }

            return hash;
        }

        public override String ToString()
        {
            using (MemoryStream mem = new MemoryStream())
            {
                //Write(mem, TAMANHO_USADO_PARA_ESCREVER_LENGTH_DA_MENSAGEM, false);
                Write(mem, 2, false);

                return Encoding.ASCII.GetString(mem.ToArray());
            }
        }

        #endregion
    }
}