using Carbon.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public sealed class UpdateExpression
    {
        private readonly Dictionary<string, string> attributeNames;
        private readonly AttributeCollection attributeValues;

        private readonly StringBuilder? set = null;
        private readonly StringBuilder? remove = null;
        private readonly StringBuilder? add = null;
        private readonly StringBuilder? delete = null;

        public UpdateExpression(
            Change[] changes,
            Dictionary<string, string> attributeNames,
            AttributeCollection attributeValues)
        {
            this.attributeNames  = attributeNames;
            this.attributeValues = attributeValues;

            for (int i = 0; i < changes.Length; i++)
            {
                ref Change change = ref changes[i];

                /*
                An update expression consists of sections. 
                Each section begins with a SET, REMOVE, ADD or DELETE keyword. 
                You can include any of these sections in an update expression in any order. 
                However, each section keyword can appear only once. 
                */

                if (change.Operation == DataOperation.Remove)
                {
                    // REMOVE (attributes)
                    // e.g. REMOVE Title, RelatedItems[2], Pictures.RearView
                    // DELETE (elements in set)
                    // e.g. Color :c (deleted :c from color set)

                    if (change.Value is null)
                    {
                        // Remove attribute
                        remove ??= new StringBuilder("REMOVE ");
                        
                        WriteChange(change, remove);
                    }
                    else
                    {
                        // Delete element
                        delete ??= new StringBuilder("DELETE ");

                        WriteChange(change, delete);
                    }

                }
                else if (change.Operation == DataOperation.Add)
                {
                    add ??= new StringBuilder("ADD ");

                    WriteChange(change, add);

                }
                else if (change.Operation == DataOperation.Replace)
                {
                    set ??= new StringBuilder("SET ");
                    
                    WriteChange(change, set);
                }
                else
                {
                    throw new Exception("Invalid change operation: " + change.Operation);
                }
            }
        }

        internal void WriteChange(in Change change, StringBuilder sb)
        {
            if (sb[sb.Length - 1] != ' ')
            {
                sb.Append(", ");
            }

            WriteName(change.Name, sb);

            if (change.Value != null)
            {
                if (change.Operation == DataOperation.Replace)
                {
                    sb.Append(" = ");
                }
                else
                {
                    sb.Append(' ');
                }

                WriteValue(change.Value, sb);
            }
        }

        private void WriteValue(object value, StringBuilder sb)
        {
            sb.WriteValue(value, attributeValues);
        }

        private void WriteName(string name, StringBuilder sb)
        {
            sb.WriteName(name, attributeNames);
        }

        public override string ToString()
        {
            /*
             SET set-action , ... 
           | REMOVE remove-action , ...  
           | ADD add-action , ... 
           | DELETE delete-action , ...  
           */

            var sb = StringBuilderCache.Aquire();

            AppendSet(sb, set);
            AppendSet(sb, remove);
            AppendSet(sb, add);
            AppendSet(sb, delete);

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        private static void AppendSet(StringBuilder sb, StringBuilder? segment)
        {
            if (segment is null) return;

            if (sb.Length > 0)
            {
                sb.AppendLine(); // \n ?
            }

#if NETSTANDARD2_0
            sb.Append(segment.ToString());
#else
            sb.Append(segment);
#endif
        }
    }
}