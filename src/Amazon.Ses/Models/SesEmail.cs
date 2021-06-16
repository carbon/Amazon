#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.Ses
{
    public sealed class SesEmail
    {
        public string Source { get; set; }

        public string[] ReplyTo { get; set; }

        public string[] To { get; set; }

#nullable enable
        public string[]? BCC { get; set; }

        public string[]? CC { get; set; }

#nullable disable

        public SesContent Subject { get; set; }

#nullable enable

        public SesContent? Html { get; set; }

        public SesContent? Text { get; set; }

        public Dictionary<string, string> ToParams()
        {
            var dic = new Dictionary<string, string> {
                { "Source", Source }
            };

            SetContent("Message.Subject",   Subject, dic);

            if (Html is not null)
            {
                SetContent("Message.Body.Html", Html, dic);
            }

            if (Text is not null)
            {
                SetContent("Message.Body.Text", Text, dic);
            }

            AddList(RecipientType.ReplyTo,  ReplyTo, dic);
            AddList(RecipientType.To,       To, dic);

            if (CC is not null)
            {
                AddList(RecipientType.Cc, CC, dic);
            }

            if (BCC is not null)
            {
                AddList(RecipientType.Bcc, BCC, dic);
            }

            return dic;
        }

        public static SesEmail FromMailMessage(MailMessage message)
        {
            if (message is null) 
                throw new ArgumentNullException(nameof(message));

            var doc = new SesEmail {
                Source = SesHelper.EncodeMailAddress(message.From!),
                To = message.To.Select(r => SesHelper.EncodeMailAddress(r)).ToArray(),
                Subject = new SesContent(message.Subject, CharsetType.UTF8)
            };

            // "Carbonmade" <hello@carbonmade.com>


            if (message.ReplyToList.Count > 0)
            {
                doc.ReplyTo = EncodeMailAddressCollection(message.ReplyToList);
            }
#pragma warning disable CS0618 // Type or member is obsolete
            else if (message.ReplyTo != null)
            {
                doc.ReplyTo = new[] { SesHelper.EncodeMailAddress(message.ReplyTo) };
            }
#pragma warning restore CS0618


            if (message.IsBodyHtml)
            {
                doc.Html = new SesContent(message.Body, CharsetType.UTF8);
            }
            else
            {
                doc.Text = new SesContent(message.Body, CharsetType.UTF8);
            }


            if (message.CC.Count > 0)
            {
                doc.CC = EncodeMailAddressCollection(message.CC);
            }

            // Alternate view support
            if (message.AlternateViews != null)
            {
                foreach (var view in message.AlternateViews)
                {
                    using var streamReader = new StreamReader(view.ContentStream);

                    string text = streamReader.ReadToEnd();

                    switch (view.ContentType.MediaType)
                    {
                        case "text/plain" : doc.Text = new SesContent(text, CharsetType.UTF8); break;
                        case "text/html"  : doc.Html = new SesContent(text, CharsetType.UTF8); break;
                    }
                }
            }

            return doc;
        }

        private static string[] EncodeMailAddressCollection(MailAddressCollection collection)
        {
            if (collection.Count == 0) return Array.Empty<string>();

            string[] addresses = new string[collection.Count];

            for (int i = 0; i < collection.Count; i++)
            {
                addresses[i] = SesHelper.EncodeMailAddress(collection[i]);
            }

            return addresses;
        }

        private enum RecipientType
        {
            ReplyTo = 1,
            To      = 2,
            Cc      = 3,
            Bcc     = 4
        }

        private static void SetContent(string prefix, SesContent content, Dictionary<string, string> dic)
        {
            if (content is null) return;

            dic.Add(prefix + ".Data", content.Data);

            if (content.Charset is not null)
            {
                dic.Add(prefix + ".Charset", content.Charset);
            }
        }

        private void AddList(RecipientType type, string[] list, Dictionary<string, string> dic)
        {
            // http://www.ietf.org/rfc/rfc0822.txt

            if (list is null || list.Length == 0) return;

            int i = 1;

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

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(SesEmail));

        public XDocument ToXml()
        {
            var namespaces = new XmlSerializerNamespaces();

            // Add lib namespace with empty prefix 
            namespaces.Add(string.Empty, "http://cloudfront.amazonaws.com/doc/2010-11-01/");

            var doc = new XDocument();

            using var xw = doc.CreateWriter();

            serializer.Serialize(xw, this, namespaces);

            return doc;
        }
    }
}
