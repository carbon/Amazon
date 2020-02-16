using System.Collections.Generic;
using System.Text;

namespace Amazon.Helpers
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendJoin(this StringBuilder sb, char separator, params string[] values)
        {
            int i = 0;

            foreach (string value in values)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(value);
                i++;
            }

            return sb;
        }
      
        public static StringBuilder AppendJoin(this StringBuilder sb, char separator, IEnumerable<string> values)
        {
            int i = 0;

            foreach (string value in values)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }

                sb.Append(value);
                i++;
            }

            return sb;
        }
    }
}