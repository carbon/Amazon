namespace Amazon.Ses
{
    public class SesContent
    {
        public SesContent(string data, CharsetType charset = CharsetType.SevenBitASCII)
        {
            Data = data;

            switch (charset)
            {
                case CharsetType.UTF8: Charset = "UTF-8"; break;
            }

            // UTF-8
        }

        public string Charset { get; }

        public string Data { get; }
    }

    public enum CharsetType
    {
        SevenBitASCII,
        UTF8,
    }
}


/*
Description

Represents textual data, plus an optional character set specification.

By default, the text must be 7-bit ASCII, due to the constraints of the SMTP protocol. If the text must contain any other characters, then you must also specify a character set. Examples include UTF-8, ISO-8859-1, and Shift_JIS.

Contents

Charset
The character set of the content.

Type: String

Required: No

Data
The textual data of the content.

Type: String

Required: Yes
*/
