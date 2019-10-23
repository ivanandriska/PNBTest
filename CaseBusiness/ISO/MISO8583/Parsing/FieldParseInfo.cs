using System;
using System.Text;
using System.Collections.Generic;

namespace CaseBusiness.ISO.MISO8583.Parsing
{
    /// <summary>
    /// This class stores the necessary information for parsing an ISO8583 field
    /// inside a message.
    /// </summary>
    public class FieldParseInfo
    {
        private IsoType type;
        private Int32 length;
        private Int32 subElementIDLength;
        private Int32 lengthLengthSubElement;
        private Boolean required;
        private TypeElement typeElement;
        private Dictionary<Int32, FieldParseInfo> subDataElements_Fields;
        private ConfigParser.Base b;

        /// <summary>
        /// Creates a new instance that knows how to parse a value of the given
        /// type and the given length (the length is necessary for ALPHA and NUMERIC
        /// values only).
        /// </summary>
        /// <param name="t">The ISO8583 type.</param>
        /// <param name="len">The length of the value to parse (for ALPHA and NUMERIC values).</param>
        /// <param name="req">Define mandatory xml item</param>
        public FieldParseInfo(IsoType t, Int32 len, Boolean req, TypeElement st, Dictionary<Int32, FieldParseInfo> sub, Int32 subElementIDLength, Int32 lengthLengthSubElement)
        {
            type = t;
            length = len;
            required = req;
            typeElement = st;
            this.subElementIDLength = subElementIDLength;
            this.lengthLengthSubElement = lengthLengthSubElement;
            subDataElements_Fields = sub;
        }

        /// <summary>
        /// Creates a new instance that knows how to parse a value of the given
        /// type and the given length (the length is necessary for ALPHA and NUMERIC
        /// values only).
        /// </summary>
        /// <param name="t">The ISO8583 type.</param>
        /// <param name="len">The length of the value to parse (for ALPHA and NUMERIC values).</param>
        /// <param name="req">Define mandatory xml item</param>
        public FieldParseInfo(IsoType t, Int32 len, Boolean req, TypeElement st, Dictionary<Int32, FieldParseInfo> sub, ConfigParser.Base b)
        {
            type = t;
            length = len;
            required = req;
            subDataElements_Fields = sub;
            typeElement = st;
            this.b = b;
        }

        /// <summary>
        /// The field length to parse.
        /// </summary>
        public Int32 Length
        {
            get { return length; }
        }

        /// <summary>
        /// The SubField/SubElements of Field
        /// </summary>
        public Dictionary<Int32, FieldParseInfo> SubDataElements_Fields
        {
            get { return subDataElements_Fields; }
        }

        /// <summary>
        /// Type of Element (Field/SubField/SubElement)
        /// </summary>
        public TypeElement TypeElement
        {
            get { return typeElement; }
        }

        /// <summary>
        /// Length of SubElement ID
        /// </summary>
        public Int32 SubElementIDLength
        {
            get { return subElementIDLength; }
        }

        /// <summary>
        /// Indicates the Length of fields presents length of SubElement
        /// </summary>
        public Int32 LengthOfLengthSubElement
        {
            get { return lengthLengthSubElement; }
        }

        /// <summary>
        /// The type of the value that will be parsed.
        /// </summary>
        public IsoType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Indicates if field is required
        /// </summary>
        public Boolean Required
        {
            get { return required; }
        }

        /// <summary>
        /// Indicates the base (Binary, Octal, Decimal, Hexadecimal)
        /// </summary>
        public ConfigParser.Base Base
        {
            get { return b; }
        }

        /// <summary>
        /// Parses a value of the type and length specified in the constructor
        /// and returns the IsoValue.
        /// </summary>
        /// <param name="buf">The byte buffer containing the value to parse.</param>
        /// <param name="pos">The position inside the byte buffer where the parsing must start.</param>
        /// <param name="encoder">The encoder to use for converting bytes to strings.</param>
        /// <returns>The resulting IsoValue with the given types and length, and the stored value.</returns>
        public IsoValue Parse(CaseBusiness.ISO.EncodingType encoding, Byte[] buf, Int32 pos, Encoding encoder, Boolean isSub)
        {
            try
            {
                if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC && Base == ConfigParser.Base.Hexadecimal)
                {
                    Byte[] bufTemp = null;
                    String fieldASCII = "";
                    String result = "";
                    Int32 lengthLenght = 0;

                    if (type == IsoType.LLVAR)
                    {
                        bufTemp = new Byte[2];
                        Array.Copy(buf, pos, bufTemp, 0, 2);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);
                        bufTemp = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                        length = ((bufTemp[0] - 48) * 10) + (bufTemp[1] - 48);
                        lengthLenght = 2;
                    }

                    if (type == IsoType.LLLVAR)
                    {
                        bufTemp = new Byte[3];
                        Array.Copy(buf, pos, bufTemp, 0, 3);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);
                        bufTemp = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                        length = ((bufTemp[0] - 48) * 100) + ((bufTemp[1] - 48) * 10) + (bufTemp[2] - 48);
                        lengthLenght = 3;
                    }

                    for (int i = 0; i < length; i++)
                        result += buf[pos + lengthLenght + i].ToString("X").PadLeft(2, '0');

                    return new IsoValue(type, Base, result, result);
                }

                if (Base == ConfigParser.Base.B64)
                    buf = Encoding.ASCII.GetBytes(Convert.ToBase64String(buf));


                if (type == IsoType.NUMERIC || type == IsoType.ALPHA)
                {
                    Int32 Llength = length;

                    if (isSub)
                    {
                        if ((pos + length) > buf.Length || length == 0)
                            Llength = buf.Length - pos;
                    }

                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[length];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, length);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }
                    else if (Base == ConfigParser.Base.Hexadecimal)
                    {
                        String result = "";
                        for (int i = 0; i < Llength / 2; i++)
                            result += buf[pos + i].ToString("X").PadLeft(2, '0');

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(result);
                    }

                    return new IsoValue(type, Base, encoder.GetString(buf, pos, Llength), Llength, encoder.GetString(buf, pos, Llength), typeElement);
                }

                if (type == IsoType.LLVAR)
                {
                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[2];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 2);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);
                        bufTemp = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                        length = ((bufTemp[0] - 48) * 10) + (bufTemp[1] - 48);

                        Array.Resize(ref bufTemp, length);
                        Array.Copy(buf, pos + 2, bufTemp, 0, length);
                        fieldASCII += ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    length = ((buf[pos] - 48) * 10) + (buf[pos + 1] - 48);

                    if (length < 1 || length > 99)
                    {
                        throw new ArgumentException("LLVAR field with invalid length");
                    }

                    return new IsoValue(type, Base, encoder.GetString(buf, pos + 2, length), encoder.GetString(buf, pos + 2, length));
                }

                if (type == IsoType.LLLVAR)
                {
                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[3];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 3);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);
                        bufTemp = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                        length = ((bufTemp[0] - 48) * 100) + ((bufTemp[1] - 48) * 10) + (bufTemp[2] - 48);

                        Array.Resize(ref bufTemp, length);
                        Array.Copy(buf, pos + 3, bufTemp, 0, length);
                        fieldASCII += ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    length = ((buf[pos] - 48) * 100) + ((buf[pos + 1] - 48) * 10) + (buf[pos + 2] - 48);

                    if (length < 1 || length > 999)
                    {
                        throw new ArgumentException("LLLVAR field with invalid length");
                    }

                    return new IsoValue(type, Base, encoder.GetString(buf, pos + 3, length), encoder.GetString(buf, pos + 3, length));
                }

                if (type == IsoType.AMOUNT)
                {
                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[12];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 12);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    byte[] c = new Byte[13];

                    Array.Copy(buf, pos, c, 0, 10);
                    Array.Copy(buf, pos + 10, c, 11, 2);

                    c[10] = (Byte)'.';

                    return new IsoValue(type, Base, Decimal.Parse(encoder.GetString(c)), encoder.GetString(buf, pos, 12));
                }

                if (type == IsoType.DATE10)
                {
                    DateTime dt = DateTime.Now;

                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[10];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 10);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    dt = new DateTime(dt.Year,
                       ((buf[pos] - 48) * 10) + buf[pos + 1] - 48,
                       ((buf[pos + 2] - 48) * 10) + buf[pos + 3] - 48,
                       ((buf[pos + 4] - 48) * 10) + buf[pos + 5] - 48,
                       ((buf[pos + 6] - 48) * 10) + buf[pos + 7] - 48,
                       ((buf[pos + 8] - 48) * 10) + buf[pos + 9] - 48);

                    if (dt.Month > DateTime.Now.Month)
                        dt = dt.AddYears(-1);

                    return new IsoValue(type, Base, dt, encoder.GetString(buf, pos, 10));
                }

                if (type == IsoType.DATE12)
                {
                    DateTime dt = DateTime.Now;

                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[12];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 12);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    dt = new DateTime(
                       (((buf[pos + 2] - 48) * 10) + buf[pos + 3] - 48) <= DateTime.Now.Month ? DateTime.Now.Year : DateTime.Now.Year - 1,
                       ((buf[pos + 2] - 48) * 10) + buf[pos + 3] - 48,
                       ((buf[pos + 4] - 48) * 10) + buf[pos + 5] - 48,
                       ((buf[pos + 6] - 48) * 10) + buf[pos + 7] - 48,
                       ((buf[pos + 8] - 48) * 10) + buf[pos + 9] - 48,
                       ((buf[pos + 10] - 48) * 10) + buf[pos + 11] - 48,
                       0);

                    return new IsoValue(type, Base, dt, encoder.GetString(buf, pos, 12));
                }

                if (type == IsoType.DATE6)
                {
                    DateTime dt = DateTime.Now;

                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[6];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 6);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    dt = new DateTime(
                       ((buf[pos] - 48) * 10) + buf[pos + 1] - 48,
                       ((buf[pos + 2] - 48) * 10) + buf[pos + 3] - 48,
                       ((buf[pos + 4] - 48) * 10) + buf[pos + 5] - 48);

                    return new IsoValue(type, Base, dt, encoder.GetString(buf, pos, 6));
                }

                if (type == IsoType.DATE4)
                {
                    DateTime dt = DateTime.Now;

                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[4];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 4);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    dt = new DateTime(dt.Year,
                       ((buf[pos] - 48) * 10) + buf[pos + 1] - 48,
                       ((buf[pos + 2] - 48) * 10) + buf[pos + 3] - 48);

                    if (dt.Month > DateTime.Now.Month)
                        dt = dt.AddYears(-1);

                    return new IsoValue(type, Base, dt, encoder.GetString(buf, pos, 4));
                }

                if (type == IsoType.DATE_EXP)
                {
                    DateTime dt = DateTime.Now;

                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[4];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 4);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    dt = new DateTime(dt.Year - (dt.Year % 100) + ((buf[pos] - 48) * 10) + buf[pos + 1] - 48,
                       ((buf[pos + 2] - 48) * 10) + buf[pos + 3] - 48, 1);

                    dt = dt.AddMonths(1);
                    dt = dt.AddDays(-1);

                    return new IsoValue(type, Base, dt, encoder.GetString(buf, pos, 4));
                }

                if (type == IsoType.TIME)
                {
                    DateTime dt = DateTime.Now;

                    if (encoding == CaseBusiness.ISO.EncodingType.EBCDIC)
                    {
                        Byte[] bufTemp = new Byte[6];
                        String fieldASCII = "";

                        Array.Copy(buf, pos, bufTemp, 0, 6);
                        fieldASCII = ParseCharacter.convertEBCDIC2ASCII(bufTemp);

                        pos = 0;
                        buf = System.Text.Encoding.ASCII.GetBytes(fieldASCII);
                    }

                    dt = new DateTime(dt.Year, dt.Month, dt.Day,
                       ((buf[pos] - 48) * 10) + buf[pos + 1] - 48,
                       ((buf[pos + 2] - 48) * 10) + buf[pos + 3] - 48,
                       ((buf[pos + 4] - 48) * 10) + buf[pos + 5] - 48);

                    return new IsoValue(type, Base, dt, encoder.GetString(buf, pos, 6));
                }

                return new IsoValue();
            }
            catch
            {
                return new IsoValue();
            }
        }
    }
}