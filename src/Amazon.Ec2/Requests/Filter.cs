using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.Ec2
{
    public struct Filter
    {
        public Filter(string name, string value)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            #endregion

            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
