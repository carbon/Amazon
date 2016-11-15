using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.Ses
{
    public class SesEmail
    {
        public string Source { get; set; }

        public string[] ReplyTo { get; set; }

        public string[] To { get; set; }

        public string[] BCC { get; set; }

        public string[] CC { get; set; }

        public SesContent Subject { get; set; }

        public SesContent Html { get; set; }

        public SesContent Text { get; set; }

        public Dictionary<string, string> ToParams()
        {
            var dic = new Dictionary<string, string>();

            dic.Add("Source", Source);

            SetContent("Message.Subject", Subject, dic);
            SetContent("Message.Body.Html", Html, dic);
            SetContent("Message.Body.Text", Text, dic);

            AddList(RecipientType.ReplyTo, ReplyTo, dic);
            AddList(RecipientType.To, To, dic);
            AddList(RecipientType.Cc, CC, dic);
            AddList(RecipientType.Bcc, BCC, dic);

            return dic;
        }

        public static SesEmail FromMailMessage(MailMessage message)
        {
            #region Preconditions

            if (message == null) throw new ArgumentNullException(nameof(message));

            #endregion

            var doc = new SesEmail
            {
                Source = SesHelper.EncodeEmail(message.From),
                To = message.To.Select(r => SesHelper.EncodeEmail(r)).ToArray(),
                Subject = new SesContent(message.Subject, CharsetType.UTF8)
            };

            // "Carbonmade" <hello@carbonmade.com>
            if (message.ReplyTo != null)
            {
                doc.ReplyTo = new[] { SesHelper.EncodeEmail(message.ReplyTo) };
            }
            else if (message.ReplyToList.Count > 0)
            {
                doc.ReplyTo = message.ReplyToList.Select(r => SesHelper.EncodeEmail(r)).ToArray();
            }

            if (message.IsBodyHtml)
            {
                doc.Html = new SesContent(message.Body, CharsetType.UTF8);
            }
            else
            {
                doc.Html = new SesContent(message.Body, CharsetType.UTF8);
            }

            // Alternate view support
            if (message.AlternateViews != null)
            {
                foreach (var view in message.AlternateViews)
                {
                    using (var streamReader = new StreamReader(view.ContentStream))
                    {
                        var text = streamReader.ReadToEnd();

                        switch (view.ContentType.MediaType)
                        {
                            case "text/plain": doc.Text = new SesContent(text, CharsetType.UTF8); break;
                            case "text/html": doc.Html = new SesContent(text, CharsetType.UTF8); break;
                        }
                    }
                }
            }

            return doc;
        }

        public enum RecipientType
        {
            ReplyTo,
            To,
            Cc,
            Bcc
        }

        public void SetContent(string prefix, SesContent content, Dictionary<string, string> dic)
        {
            if (content == null) return;

            dic.Add(prefix + ".Data", content.Data);

            if (content.Charset != null) dic.Add(prefix + ".Charset", content.Charset);
        }

        public void AddList(RecipientType type, string[] list, Dictionary<string, string> dic)
        {
            // http://www.ietf.org/rfc/rfc0822.txt

            if (list == null || list.Length == 0) return;

            var i = 1;

            // By default, the string must be 7-bit ASCII.
            // If the text must contain any other characters, then you must use MIME encoded-word syntax (RFC 2047) instead of a literal string. 
            // MIME encoded-word syntax uses the following form: =?charset?encoding?encoded-text?=. For more information, see RFC 2047.


            foreach (var address in To)
            {
                // &Destination.ToAddresses.member.1=allan%40example.com

                dic.Add($"Destination.{type}Addresses.member.{i}", address);

                i++;
            }
        }

        /*
		AWSAccessKeyId=AKIAIOSFODNN7EXAMPLE
		&Action=SendEmail
		&Destination.ToAddresses.member.1=allan%40example.com
		&Message.Body.Text.Data=body
		&Message.Subject.Data=Example&Source=user%40example.com
		&Timestamp=2011-08-18T22%3A25%3A27.000Z
		*/

        public XDocument ToXml()
        {
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(SesEmail));

            var namespaces = new XmlSerializerNamespaces();

            //  Add lib namespace with empty prefix 
            namespaces.Add("", "http://cloudfront.amazonaws.com/doc/2010-11-01/");

            var doc = new XDocument();

            using (var xw = doc.CreateWriter())
            {
                serializer.Serialize(xw, this, namespaces);
            }

            return doc;
        }
    }
}
