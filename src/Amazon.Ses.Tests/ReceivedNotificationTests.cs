﻿using System.Text.Json;

using Amazon.Ses.Serialization;

namespace Amazon.Ses.Tests;

public class ReceivedNotificationTests
{
    [Fact]
    public void CanDeserialize()
    {
        var notification = JsonSerializer.Deserialize(
            """
            {
              "notificationType":"Received",
              "receipt":{
                "timestamp":"2015-09-11T20:32:33.936Z",
                "processingTimeMillis":222,
                "recipients":[
                  "recipient@example.com"
                ],
                "spamVerdict":{
                  "status":"PASS"
                },
                "virusVerdict":{
                  "status":"PASS"
                },
                "spfVerdict":{
                  "status":"PASS"
                },
                "dkimVerdict":{
                  "status":"PASS"
                },
                "action":{
                  "type":"SNS",
                  "topicArn":"arn:aws:sns:us-east-1:012345678912:example-topic"
                }
              },
              "mail":{
                "timestamp":"2015-09-11T20:32:33.936Z",
                "source":"61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com",
                "messageId":"d6iitobk75ur44p8kdnnp7g2n800",
                "destination":[
                  "recipient@example.com"
                ],
                "headersTruncated":false,
                "headers":[
                  {
                    "name":"Return-Path",
                    "value":"<0000014fbe1c09cf-7cb9f704-7531-4e53-89a1-5fa9744f5eb6-000000@amazonses.com>"
                  },
                  {
                    "name":"Received",
                    "value":"from a9-183.smtp-out.amazonses.com (a9-183.smtp-out.amazonses.com [54.240.9.183]) by inbound-smtp.us-east-1.amazonaws.com with SMTP id d6iitobk75ur44p8kdnnp7g2n800 for recipient@example.com; Fri, 11 Sep 2015 20:32:33 +0000 (UTC)"
                  },
                  {
                    "name":"DKIM-Signature",
                    "value":"v=1; a=rsa-sha256; q=dns/txt; c=relaxed/simple; s=ug7nbtf4gccmlpwj322ax3p6ow6yfsug; d=amazonses.com; t=1442003552; h=From:To:Subject:MIME-Version:Content-Type:Content-Transfer-Encoding:Date:Message-ID:Feedback-ID; bh=DWr3IOmYWoXCA9ARqGC/UaODfghffiwFNRIb2Mckyt4=; b=p4ukUDSFqhqiub+zPR0DW1kp7oJZakrzupr6LBe6sUuvqpBkig56UzUwc29rFbJF hlX3Ov7DeYVNoN38stqwsF8ivcajXpQsXRC1cW9z8x875J041rClAjV7EGbLmudVpPX 4hHst1XPyX5wmgdHIhmUuh8oZKpVqGi6bHGzzf7g="
                  },
                  {
                    "name":"From",
                    "value":"sender@example.com"
                  },
                  {
                    "name":"To",
                    "value":"recipient@example.com"
                  },
                  {
                    "name":"Subject",
                    "value":"Example subject"
                  },
                  {
                    "name":"MIME-Version",
                    "value":"1.0"
                  },
                  {
                    "name":"Content-Type",
                    "value":"text/plain; charset=UTF-8"
                  },
                  {
                    "name":"Content-Transfer-Encoding",
                    "value":"7bit"
                  },
                  {
                    "name":"Date",
                    "value":"Fri, 11 Sep 2015 20:32:32 +0000"
                  },
                  {
                    "name":"Message-ID",
                    "value":"<61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com>"
                  },
                  {
                    "name":"X-SES-Outgoing",
                    "value":"2015.09.11-54.240.9.183"
                  },
                  {
                    "name":"Feedback-ID",
                    "value":"1.us-east-1.Krv2FKpFdWV+KUYw3Qd6wcpPJ4Sv/pOPpEPSHn2u2o4=:AmazonSES"
                  }
                ],
                "commonHeaders":{
                  "returnPath":"0000014fbe1c09cf-7cb9f704-7531-4e53-89a1-5fa9744f5eb6-000000@amazonses.com",
                  "from":[
                    "sender@example.com"
                  ],
                  "date":"Fri, 11 Sep 2015 20:32:32 +0000",
                  "to":[
                    "recipient@example.com"
                  ],
                  "messageId":"<61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com>",
                  "subject":"Example subject"
                }
              },
              "content":"Return-Path: <61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com>\r\nReceived: from a9-183.smtp-out.amazonses.com (a9-183.smtp-out.amazonses.com [54.240.9.183])\r\n by inbound-smtp.us-east-1.amazonaws.com with SMTP id d6iitobk75ur44p8kdnnp7g2n800\r\n for recipient@example.com;\r\n Fri, 11 Sep 2015 20:32:33 +0000 (UTC)\r\nDKIM-Signature: v=1; a=rsa-sha256; q=dns/txt; c=relaxed/simple;\r\n\ts=ug7nbtf4gccmlpwj322ax3p6ow6yfsug; d=amazonses.com; t=1442003552;\r\n\th=From:To:Subject:MIME-Version:Content-Type:Content-Transfer-Encoding:Date:Message-ID:Feedback-ID;\r\n\tbh=DWr3IOmYWoXCA9ARqGC/UaODfghffiwFNRIb2Mckyt4=;\r\n\tb=p4ukUDSFqhqiub+zPR0DW1kp7oJZakrzupr6LBe6sUuvqpBkig56UzUwc29rFbJF\r\n\thlX3Ov7DeYVNoN38stqwsF8ivcajXpQsXRC1cW9z8x875J041rClAjV7EGbLmudVpPX\r\n\t4hHst1XPyX5wmgdHIhmUuh8oZKpVqGi6bHGzzf7g=\r\nFrom: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Example subject\r\nMIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 7bit\r\nDate: Fri, 11 Sep 2015 20:32:32 +0000\r\nMessage-ID: <61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com>\r\nX-SES-Outgoing: 2015.09.11-54.240.9.183\r\nFeedback-ID: 1.us-east-1.Krv2FKpFdWV+KUYw3Qd6wcpPJ4Sv/pOPpEPSHn2u2o4=:AmazonSES\r\n\r\nExample content\r\n"
            }
            """u8, SesSerializerContext.Default.SesNotification);

        Assert.NotNull(notification);
        Assert.Equal(SesNotificationType.Received, notification.NotificationType);

        Assert.NotNull(notification.Receipt);
        Assert.Equal(222, notification.Receipt.ProcessingTimeMillis);

        Assert.Equal("PASS", notification.Receipt.SpamVerdict.Status);
        Assert.Equal("PASS", notification.Receipt.VirusVerdict.Status);
        Assert.Equal("PASS", notification.Receipt.SpfVerdict.Status);
        Assert.Equal("PASS", notification.Receipt.DkimVerdict.Status);

        Assert.Equal(SesActionType.SNS, notification.Receipt.Action.Type);
        Assert.Equal("arn:aws:sns:us-east-1:012345678912:example-topic", notification.Receipt.Action.TopicArn);

        Assert.False(notification.Mail.HeadersTruncated.Value);

        Assert.Equal("61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com", notification.Mail.Source);
        Assert.Equal(13, notification.Mail.Headers.Length);

        Assert.Equal("Return-Path", notification.Mail.Headers[0].Name);
        Assert.Equal("<0000014fbe1c09cf-7cb9f704-7531-4e53-89a1-5fa9744f5eb6-000000@amazonses.com>", notification.Mail.Headers[0].Value);

        Assert.Equal("Return-Path: <61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com>\r\nReceived: from a9-183.smtp-out.amazonses.com (a9-183.smtp-out.amazonses.com [54.240.9.183])\r\n by inbound-smtp.us-east-1.amazonaws.com with SMTP id d6iitobk75ur44p8kdnnp7g2n800\r\n for recipient@example.com;\r\n Fri, 11 Sep 2015 20:32:33 +0000 (UTC)\r\nDKIM-Signature: v=1; a=rsa-sha256; q=dns/txt; c=relaxed/simple;\r\n\ts=ug7nbtf4gccmlpwj322ax3p6ow6yfsug; d=amazonses.com; t=1442003552;\r\n\th=From:To:Subject:MIME-Version:Content-Type:Content-Transfer-Encoding:Date:Message-ID:Feedback-ID;\r\n\tbh=DWr3IOmYWoXCA9ARqGC/UaODfghffiwFNRIb2Mckyt4=;\r\n\tb=p4ukUDSFqhqiub+zPR0DW1kp7oJZakrzupr6LBe6sUuvqpBkig56UzUwc29rFbJF\r\n\thlX3Ov7DeYVNoN38stqwsF8ivcajXpQsXRC1cW9z8x875J041rClAjV7EGbLmudVpPX\r\n\t4hHst1XPyX5wmgdHIhmUuh8oZKpVqGi6bHGzzf7g=\r\nFrom: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Example subject\r\nMIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 7bit\r\nDate: Fri, 11 Sep 2015 20:32:32 +0000\r\nMessage-ID: <61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com>\r\nX-SES-Outgoing: 2015.09.11-54.240.9.183\r\nFeedback-ID: 1.us-east-1.Krv2FKpFdWV+KUYw3Qd6wcpPJ4Sv/pOPpEPSHn2u2o4=:AmazonSES\r\n\r\nExample content\r\n", notification.Content);

        Assert.Equal("0000014fbe1c09cf-7cb9f704-7531-4e53-89a1-5fa9744f5eb6-000000@amazonses.com", notification.Mail.CommonHeaders.ReturnPath);
        Assert.Equal("sender@example.com", notification.Mail.CommonHeaders.From[0]);
        Assert.Equal("Fri, 11 Sep 2015 20:32:32 +0000", notification.Mail.CommonHeaders.Date);
        Assert.Equal("recipient@example.com", notification.Mail.CommonHeaders.To[0]);
        Assert.Equal("<61967230-7A45-4A9D-BEC9-87CBCF2211C9@example.com>", notification.Mail.CommonHeaders.MessageId);
        Assert.Equal("Example subject", notification.Mail.CommonHeaders.Subject);
    }
}