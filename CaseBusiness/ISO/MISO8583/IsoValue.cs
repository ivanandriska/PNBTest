using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CaseBusiness.ISO.MISO8583
{
    /// <summary>
    /// Stores a value contained in a field inside a Message.
    /// </summary>
    [Serializable]
    public class IsoValue : ICloneable
    {
        #region Atributos

        private IsoType type;
        private Int32 length;
        private Object fval;
        private String originalValue;
        private String requirements;
        private TypeElement typeElement;
        private Int32 subElementIDLength;
        private Int32 lengthLengthSubElement;
        private Dictionary<Int32, IsoValue> subData = new Dictionary<Int32, IsoValue>();
        private Dictionary<Int32, IsoValue> subSubData = new Dictionary<Int32, IsoValue>();
        private Parsing.ConfigParser.Base b = Parsing.ConfigParser.Base.Decimal;
		private Boolean completelyParsed = false;

        #endregion


        #region Construtores

        public IsoValue() { }

        /// <summary>
        /// Creates a new instance to store a value of a fixed-length type.
        /// Fixed-length types are DATE4, DATE_EXP, DATE10, TIME, AMOUNT.
        /// </summary>
        /// <param name="t">The ISO8583 type of the value that is going to be stored.</param>
        /// <param name="value">The value to store.s</param>
        public IsoValue(IsoType t, Parsing.ConfigParser.Base configBase, Object value, String originalValue)
        {
            if (value == null)
            {
                throw new ArgumentException("Value cannot be null");
            }

            if (IsoTypeHelper.NeedsLength(t))
            {
                throw new ArgumentException("Use IsoValue constructor for Fixed-value types");
            }

            type = t;
            fval = value;
            b = configBase;
            this.originalValue = originalValue;

            if (t == IsoType.LLVAR || type == IsoType.LLLVAR)
                length = value.ToString().Length;
            else
                length = IsoTypeHelper.GetLength(t);
        }

        /// <summary>
        /// Creates a new instance to store a value of a fixed-length type.
        /// Fixed-length types are DATE4, DATE_EXP, DATE10, TIME, AMOUNT.
        /// </summary>
        /// <param name="t">The ISO8583 type of the value that is going to be stored.</param>
        /// <param name="value">The value to store.s</param>
        /// <param name="originalValue">The original value of field</param>
        /// <param name="req">Indicates the requiremts (M,C, CE, ME, O )</param>
        public IsoValue(IsoType t, Parsing.ConfigParser.Base configBase, Object value, String originalValue, String req)
        {
            if (value == null)
            {
                throw new ArgumentException("Value cannot be null");
            }

            if (IsoTypeHelper.NeedsLength(t))
            {
                throw new ArgumentException("Use IsoValue constructor for Fixed-value types");
            }

            type = t;
            fval = value;
            b = configBase;
            this.originalValue = originalValue;
            requirements = req;
            if (t == IsoType.LLVAR || type == IsoType.LLLVAR)
            {
                length = value.ToString().Length;
            }
            else
            {
                length = IsoTypeHelper.GetLength(t);
            }
        }

        /// <summary>
        /// Creates a new instance to store a value of a fixed-length type.
        /// Fixed-length types are DATE4, DATE_EXP, DATE10, TIME, AMOUNT.
        /// </summary>
        /// <param name="t">The ISO8583 type of the value that is going to be stored.</param>
        /// <param name="value">The value to store.s</param>
        /// <param name="originalValue">The original value of field</param>
        /// <param name="req">Indicates the requiremts (M,C, CE, ME, O )</param>
        /// <param name="subData">Collection of subDatas (SubField/SubElements) of field</param>
        public IsoValue(IsoType t, Parsing.ConfigParser.Base configBase, Object value, String originalValue, String req, Dictionary<Int32, IsoValue> subData)
        {
            if (value == null)
            {
                throw new ArgumentException("Value cannot be null");
            }

            if (IsoTypeHelper.NeedsLength(t))
            {
                throw new ArgumentException("Use IsoValue constructor for Fixed-value types");
            }

            type = t;
            fval = value;
            b = configBase;
            this.originalValue = originalValue;
            this.subData = subData;
            requirements = req;
            if (t == IsoType.LLVAR || type == IsoType.LLLVAR)
            {
                length = value.ToString().Length;
            }
            else
            {
                length = IsoTypeHelper.GetLength(t);
            }
        }

        /// <summary>
        /// Creates a new instance to store a value of a given type. This constructor
        /// is used for variable-length types (LLVAR, LLLVAR, ALPHA, NUMERIC) -
        /// variable in the sense that that length of the value does not depend
        /// solely on the ISO type.
        /// </summary>
        /// <param name="t">the ISO8583 type of the value to be stored.</param>
        /// <param name="val">The value to be stored.</param>
        /// <param name="len">The length of the field.</param>
        /// <param name="originalValue">The original value of field</param>
		public IsoValue(IsoType t, Parsing.ConfigParser.Base configBase, Object val, Int32 len, String originalValue, TypeElement typeElement)
        {
            if (val == null)
            {
                throw new ArgumentException("Value cannot be null");
            }

            type = t;
            fval = val;
            length = len;
            b = configBase;
            this.originalValue = originalValue;

			if (typeElement == TypeElement.Field || typeElement == TypeElement.SubField)
			{
				if (length == 0 && IsoTypeHelper.NeedsLength(t))
				{
					throw new ArgumentException("Length must be greater than zero");
				}
			}

            if (t == IsoType.LLVAR || t == IsoType.LLLVAR)
            {
                length = val.ToString().Length;
            }
        }

        /// <summary>
        /// Creates a new instance to store a value of a given type. This constructor
        /// is used for variable-length types (LLVAR, LLLVAR, ALPHA, NUMERIC) -
        /// variable in the sense that that length of the value does not depend
        /// solely on the ISO type.
        /// </summary>
        /// <param name="t">the ISO8583 type of the value to be stored.</param>
        /// <param name="val">The value to be stored.</param>
        /// <param name="len">The length of the field.</param>
        /// <param name="originalValue">The original value of field</param>
        /// <param name="req">Indicates the requiremts (M,C, CE, ME, O )</param>
        /// <param name="typeElement">Indicates the Type of Element (Field, SubField, SubElement)</param>
        public IsoValue(IsoType t, Parsing.ConfigParser.Base configBase, Object val, Int32 len, String originalValue, String req, TypeElement typeElement, Int32 subElementIDLength, Int32 lengthLengthSubElement)
        {
            if (val == null)
            {
                throw new ArgumentException("Value cannot be null");
            }

            type = t;
            fval = val;
            length = len;
            this.originalValue = originalValue;
            requirements = req;
            this.typeElement = typeElement;
            this.subElementIDLength = subElementIDLength;
            this.lengthLengthSubElement = lengthLengthSubElement;
            b = configBase;

            if (typeElement == TypeElement.Field || typeElement == TypeElement.SubField)
            {
                if (length == 0 && IsoTypeHelper.NeedsLength(t))
                {
                    throw new ArgumentException("Length must be greater than zero");
                }
            }

            if (t == IsoType.LLVAR || t == IsoType.LLLVAR)
            {
                length = val.ToString().Length;
            }
        }

        /// <summary>
        /// Creates a new instance to store a value of a given type. This constructor
        /// is used for variable-length types (LLVAR, LLLVAR, ALPHA, NUMERIC) -
        /// variable in the sense that that length of the value does not depend
        /// solely on the ISO type.
        /// </summary>
        /// <param name="t">the ISO8583 type of the value to be stored.</param>
        /// <param name="val">The value to be stored.</param>
        /// <param name="len">The length of the field.</param>
        /// <param name="originalValue">The original value of field</param>
        /// <param name="req">Indicates the requiremts (M,C, CE, ME, O )</param>
        /// <param name="typeElement">Indicates the Type of Element (Field, SubField, SubElement)</param>
        public IsoValue(IsoType t, Parsing.ConfigParser.Base configBase, Object val, Int32 len, String originalValue, String req, TypeElement typeElement, Dictionary<Int32, IsoValue> subData, Int32 subElementIDLength, Int32 lengthLengthSubElement)
        {
            if (val == null)
            {
                throw new ArgumentException("Value cannot be null");
            }

            type = t;
            fval = val;
            length = len;
            this.originalValue = originalValue;
            requirements = req;
            this.typeElement = typeElement;
            this.subElementIDLength = subElementIDLength;
            this.lengthLengthSubElement = lengthLengthSubElement;
            this.subData = subData;
            b = configBase;

            if (typeElement == TypeElement.Field || typeElement == TypeElement.SubField)
            {
                if (length == 0 && IsoTypeHelper.NeedsLength(t))
                {
                    throw new ArgumentException("Length must be greater than zero");
                }
            }

            if (t == IsoType.LLVAR || t == IsoType.LLLVAR)
            {
                length = val.ToString().Length;
            }
        }

        /// <summary>
        /// Creates a new instance to store a value of a given type. This constructor
        /// is used for variable-length types (LLVAR, LLLVAR, ALPHA, NUMERIC) -
        /// variable in the sense that that length of the value does not depend
        /// solely on the ISO type.
        /// </summary>
        /// <param name="t">the ISO8583 type of the value to be stored.</param>
        /// <param name="val">The value to be stored.</param>
        /// <param name="len">The length of the field.</param>
        /// <param name="req">Indicates the requiremts (M,C, CE, ME, O )</param>
        /// <param name="subData">Collection of subDatas (SubField/SubElements) of field</param>
        public IsoValue(IsoType t, Parsing.ConfigParser.Base configBase, Object val, Int32 len, String originalValue, String req, Dictionary<Int32, IsoValue> subData)
        {
            if (val == null)
            {
                throw new ArgumentException("Value cannot be null");
            }

            type = t;
            fval = val;
            length = len;
            b = configBase;
            this.subData = subData;
            requirements = req;
			FormatOriginalValue(); 

            if (length == 0 && IsoTypeHelper.NeedsLength(t))
            {
                throw new ArgumentException("Length must be greater than zero");
            }

            if (t == IsoType.LLVAR || t == IsoType.LLLVAR)
            {
                length = val.ToString().Length;
            }
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// The ISO8583 type of the value stored in the receiver.
        /// </summary>
        public IsoType Type
        {
            get { return type; }
        }

		/// <summary>
		/// Indicate if IsoValue was completely Parsed by ISO Parse Component
		/// </summary>
		public Boolean CompletelyParsed
		{
			get
			{
				return completelyParsed;
			}
			set
			{
				completelyParsed = value;
			}

		}

        /// <summary>
        /// Indicates the Type of Element (Field, SubField, SubElement)
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
        /// The ISO8583 original value of field
        /// </summary>
        public String OriginalValue
        {
            get { return originalValue; }
        }

        /// <summary>
        /// The length of the field.
        /// </summary>
        public Int32 Length
        {
            get { return length; }
            set { length = value; }
        }

        /// <summary>
        /// The value stored in the receiver.
        /// </summary>
        public Object Value
        {
            get { return fval; }
            set { fval = value; }
        }

        /// <summary>
        /// Indicates the type of requirements (M, C, O, CE, ME, etc)
        /// </summary>
        public String Requirements
        {
            get { return requirements; }
        }

        /// <summary>
        /// Collection of SubData (SubFields/SubElements)
        /// </summary>
        public Dictionary<Int32, IsoValue> SubData
        {
            get { return subData; }
        }

        /// <summary>
        /// Indicates the base (Binary, Octal, Decimal, Hexadecimal)
        /// </summary>
        public Parsing.ConfigParser.Base Base
        {
            get { return b; }
        }


        #endregion

        #region Metodos

        private void FormatOriginalValue()
		{
			if (subData != null)
			{
				if (subData.Count > 0)
				{
					String orgValue = "";
					foreach (KeyValuePair<Int32, IsoValue> sub in subData)
						orgValue += sub.Value.originalValue;

					originalValue = orgValue;
					return;
				}
			}

			switch (type)
			{
				case IsoType.NUMERIC:
					originalValue = IsoTypeHelper.Format(fval.ToString(), type, length);
					break;
				case IsoType.ALPHA:
					originalValue = IsoTypeHelper.Format(fval.ToString(), type, length);
					break;
				case IsoType.DATE10:
					originalValue = IsoTypeHelper.Format(Convert.ToDateTime(fval), type);
					break;
				case IsoType.DATE12:
					originalValue = IsoTypeHelper.Format(Convert.ToDateTime(fval), type);
					break;
				case IsoType.DATE6:
					originalValue = IsoTypeHelper.Format(Convert.ToDateTime(fval), type);
					break;
				case IsoType.DATE4:
					originalValue = IsoTypeHelper.Format(Convert.ToDateTime(fval), type);
					break;
				case IsoType.DATE_EXP:
					originalValue = IsoTypeHelper.Format(Convert.ToDateTime(fval), type);
					break;
				case IsoType.TIME:
					originalValue = IsoTypeHelper.Format(Convert.ToDateTime(fval), type);
					break;
				case IsoType.AMOUNT:
					originalValue = IsoTypeHelper.Format(Convert.ToDecimal(fval), type, length);
					break;
			}
		}

        public override Boolean Equals(object other)
        {
            if (other == null || !(other is IsoValue)) { return false; }

            IsoValue comp = (IsoValue)other;

            return (comp.Type == type && comp.Value.Equals(fval) && comp.Length == length);
        }

        public override Int32 GetHashCode()
        {
            return fval.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation of the stored value, which
        /// varies a little depending on its type: NUMERIC and ALPHA values
        /// are returned padded to the specified length of the field either
        /// with zeroes or spaces; AMOUNT values are formatted to 12 digits
        /// as cents; date/time values are returned with the specified format
        /// for their type. LLVAR and LLLVAR values are returned as they are.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            if (type == IsoType.NUMERIC || type == IsoType.ALPHA)
            {
                return IsoTypeHelper.Format(fval.ToString(), type, length);
            }

            if (type == IsoType.AMOUNT)
            {
                if (fval is Decimal)
                {
                    return IsoTypeHelper.Format((Decimal)fval, type, 12);
                }
                else
                {
                    return IsoTypeHelper.Format(Convert.ToDecimal(fval), type, 12);
                }
            }

            if (fval is DateTime)
            {
                return IsoTypeHelper.Format((DateTime)fval, type);
            }

            return fval.ToString();
        }

        /// <summary>
        /// Writes the stored value to a stream, preceded by the length header
        /// in case of LLVAR or LLLVAR values.
        /// </summary>
        /// <param name="outs"></param>
        public void Write(Stream outs)
        {
            //TODO binary encoding is pending
            String v = ToString();
            if (type == IsoType.LLVAR || type == IsoType.LLLVAR)
            {
                length = v.Length;
                if (length >= 100)
                {
                    outs.WriteByte((Byte)((length / 100) + 48));
                }
                else if (type == IsoType.LLLVAR)
                {
                    outs.WriteByte(48);
                }

                if (length >= 10)
                {
                    outs.WriteByte((Byte)(((length % 100) / 10) + 48));
                }
                else
                {
                    outs.WriteByte(48);
                }

                outs.WriteByte((Byte)((length % 10) + 48));
            }

            Byte[] buf = System.Text.Encoding.ASCII.GetBytes(v);
            outs.Write(buf, 0, buf.Length);
        }

        public object Clone()
        {
			IsoValue isoClone = (IsoValue)MemberwiseClone();
			if (isoClone.SubData != null)
			{
				Dictionary<Int32, IsoValue> sub = new Dictionary<Int32, IsoValue>();
				foreach (KeyValuePair<Int32, IsoValue> subClone in isoClone.SubData)
					sub.Add(subClone.Key, (IsoValue)subClone.Value.Clone());

				isoClone.subData = sub;
			}

			return (Object)isoClone;
        }
        #endregion
    }
}