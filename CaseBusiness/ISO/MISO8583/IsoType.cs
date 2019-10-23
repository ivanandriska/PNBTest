using System;

namespace CaseBusiness.ISO.MISO8583
{
    /// <summary>
    /// Lists the available ISO8583 datatypes.
    /// </summary>
    public enum IsoType
    {
        /// <summary>
        /// A numeric value of a certain length, zero-padded to the left.
        /// </summary>
        NUMERIC = 0,
        
		/// <summary>
        /// An alphanumeric value of a certain length, space-padded to the right.
        /// </summary>
        ALPHA,
        
		/// <summary>
        /// A variable-length alphanumeric value, from 1 to 99 characters.
        /// </summary>
        LLVAR,
        
		/// <summary>
        /// A variable-length alphanumeric value, from 1 to 999 characters.
        /// </summary>
        LLLVAR,
        
		/// <summary>
        /// A date and time in format MMddHHmmss
        /// </summary>
        DATE10,
        
		/// <summary>
        /// A date and time in format YYmmddHHmmss
        /// </summary>
        DATE12,
        
		/// <summary>
        /// A date in format MMdd
        /// </summary>
        DATE4,

        /// <summary>
        /// A date in format yyMMdd
        /// </summary>
        DATE6,
        
		/// <summary>
        /// An expiration date (for a credit card, for example), in format YYmm
        /// </summary>
        DATE_EXP,
        
		/// <summary>
        /// The time of day, in format HHmmss
        /// </summary>
        TIME,
        
		/// <summary>
        /// A monetary amount, expressed in cents, with a fixed length of 12 positions, zero-padded to the left.
        /// </summary>
        AMOUNT
    }

    /// <summary>
    /// This convenience class implements some methods to ease the handling
    /// of IsoTypes.
    /// </summary>
    public abstract class IsoTypeHelper
    {
        /// <summary>
        /// Returns true if the IsoType is NUMERIC or ALPHA, since these two
        /// types need a length to be specified as part of a field.
        /// </summary>
        /// <param name="t">The type in question.</param>
        /// <returns>true if the IsoType needs a length specification to be stored as an IsoValue.</returns>
        public static Boolean NeedsLength(IsoType t)
        {
            return (t == IsoType.NUMERIC || t == IsoType.ALPHA);
        }

        /// <summary>
        /// Returns the length of a type, for types that always have the same
        /// length, of 0 in case of types that can have a variable length.
        /// </summary>
        /// <param name="t">The type in question.</param>
        /// <returns>The length of the type if it's a fixed-length type,
        /// or 0 if it's a variable-length type.</returns>
        public static Int32 GetLength(IsoType t)
        {
            if (t == IsoType.DATE10) { return 10; }
            if (t == IsoType.TIME) { return 6; }
            if (t == IsoType.AMOUNT || t == IsoType.DATE12) { return 12; }
            if (t == IsoType.DATE4 || t == IsoType.DATE_EXP) { return 4; }
            if (t == IsoType.DATE6 || t == IsoType.DATE_EXP) { return 6; }

            return 0;
        }

        /// <summary>
        /// Formats a DateTime of the specified IsoType as a string.
        /// </summary>
        /// <param name="d">A DateTime object.</param>
        /// <param name="t">An IsoType (must be DATE10, DATE4, DATE_EXP or TIME).</param>
        /// <returns>The date formatted as a string according to the specified IsoType.</returns>
        public static String Format(DateTime d, IsoType t)
        {
            if (t == IsoType.DATE10) { return d.ToString("MMddHHmmss"); }
            if (t == IsoType.DATE12) { return d.ToString("yyMMddHHmmss"); }
            if (t == IsoType.DATE4) { return d.ToString("MMdd"); }
            if (t == IsoType.DATE6) { return d.ToString("yyMMdd"); }
            if (t == IsoType.DATE_EXP) { return d.ToString("yyMM"); }
            if (t == IsoType.TIME) { return d.ToString("HHmmss"); }

            throw new ArgumentException("IsoType must be DATE10, DATE12, DATE4, DATE6, DATE_EXP or TIME");
        }

        /// <summary>
        /// Formats a string to the given length, padding it if necessary.
        /// </summary>
        /// <param name="value">The string to format.</param>
        /// <param name="t">The ISO8583 type to format the string as.</param>
        /// <param name="length">The length (in case of ALPHA or NUMERIC).</param>
        /// <returns>The formatted string.</returns>
        public static String Format(String value, IsoType t, Int32 length)
        {
            if (t == IsoType.ALPHA)
            {
                Char[] c = new Char[length];

                if (value == null)
                {
                    Array.Copy(value.ToCharArray(), c, value.Length);

                    for (Int32 i = value.Length; i < length; i++)
                    {
                        c[i] = ' ';
                    }

                    return new String(c);
                }

                if (value.Length == length)
                {
                    return value;
                }

                if (value.Length > length)
                {
                    return value.Substring(0, length);
                }
            }

            if (t == IsoType.LLVAR || t == IsoType.LLLVAR)
            {
                return value;
            }

            if (t == IsoType.NUMERIC)
            {
                if (value.Length == length) { return value; }

                Char[] c = new Char[length];
                Char[] x = value.ToCharArray();

                if (x.Length > length)
                {
                    throw new ArgumentOutOfRangeException("Numeric value is larger than intended length");
                }

                Int32 lim = c.Length - x.Length;
                for (Int32 i = 0; i < lim; i++)
                {
                    c[i] = '0';
                }

                Array.Copy(x, 0, c, lim, x.Length);

                return new String(c);
            }

            throw new ArgumentException("IsoType must be ALPHA, LLVAR, LLLVAR or NUMERIC");
        }

        /// <summary>
        /// Formats a number as an ISO8583 type.
        /// </summary>
        /// <param name="value">The number to format.</param>
        /// <param name="t">The ISO8583 type to format the value as.</param>
        /// <param name="length">The length to format to (in case of ALPHA or NUMERIC)</param>
        /// <returns>The formatted string representation of the value.</returns>
        public static String Format(Int64 value, IsoType t, Int32 length)
        {
            if (t == IsoType.NUMERIC || t == IsoType.ALPHA)
            {
                return Format(value.ToString(), t, length);
            }

            if (t == IsoType.LLLVAR || t == IsoType.LLVAR)
            {
                return value.ToString();
            }

            if (t == IsoType.AMOUNT)
            {
                return value.ToString("0000000000") + "00";
            }

            throw new ArgumentException("IsoType must be AMOUNT, NUMERIC, ALPHA, LLLVAR or LLVAR");
        }

        /// <summary>
        /// Formats a decimal value as an ISO8583 type.
        /// </summary>
        /// <param name="value">The decimal value to format.</param>
        /// <param name="t">The ISO8583 type to format the value as.</param>
        /// <param name="length">The length for the formatting, useful if the type is NUMERIC or ALPHA.</param>
        /// <returns>The formatted string representation of the value.</returns>
        public static String Format(Decimal value, IsoType t, Int32 length)
        {
            if (t == IsoType.AMOUNT)
            {
                Char[] x = value.ToString("0000000000.00").ToCharArray();
                Char[] digits = new Char[12];
                
                Array.Copy(x, digits, 10);
                Array.Copy(x, 11, digits, 10, 2);

                return new String(digits);
            }

            if (t == IsoType.NUMERIC || t == IsoType.ALPHA)
            {
                return Format(value.ToString(), t, length);
            }

            if (t == IsoType.LLVAR || t == IsoType.LLLVAR)
            {
                return value.ToString();
            }

            throw new ArgumentException("IsoType must be AMOUNT, NUMERIC, ALPHA, LLLVAR or LLVAR");
        }
    }
}