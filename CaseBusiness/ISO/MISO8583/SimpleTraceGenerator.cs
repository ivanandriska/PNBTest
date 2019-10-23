using System;

namespace CaseBusiness.ISO.MISO8583
{
    public class SimpleTraceGenerator : ITraceGenerator
    {
        Int32 value = 0;

        public SimpleTraceGenerator(Int32 initialValue)
        {
            if (initialValue < 1 || initialValue > 999999)
            {
                throw new ArgumentException("initialValue must be between 1 and 999999", "initialValue");
            }

            value = initialValue;
        }

        public Int32 LastTrace
        {
            get { return value; }
        }

        public Int32 NextTrace()
        {
            lock (this)
            {
                value++;

                if (value > 999999)
                {
                    value = 1;
                }

                return value;
            }
        }
    }
}