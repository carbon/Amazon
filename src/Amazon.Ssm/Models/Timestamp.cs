using System;

namespace Amazon.Ssm
{
    public readonly struct Timestamp
    {
        private readonly double value;

        public Timestamp(double value)
        {
            this.value = value;
        }

        public double Value => value;
        
        public static implicit operator DateTime(Timestamp timestamp)
        {
            long unixTimeMillseconds = (long)(timestamp.Value * 1000d);

            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMillseconds).UtcDateTime;
        }

        public static implicit operator DateTimeOffset(Timestamp timestamp)
        {
            long unixTimeMillseconds = (long)(timestamp.Value * 1000d);

            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMillseconds);
        }
    }
}

// scientific notation: 1.494825472676E9 
