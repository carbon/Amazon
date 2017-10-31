using System;

namespace Amazon.Kinesis.Firehose
{
    public struct Record // readonly
    {
        public Record(byte[] data)
        {
            #region Preconditions

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length > 1024000)
            {
                throw new ArgumentException(nameof(data), "Must be less than 1MB");
            }

            #endregion


            Data = Convert.ToBase64String(data);
        }

        public string Data { get; }
    }
}

// The data blob, which is base64-encoded when the blob is serialized. The maximum size of the data blob, before base64-encoding, is 1,000 KB.