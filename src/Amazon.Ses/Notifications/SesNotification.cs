namespace Amazon.Ses
{
    public class SesNotification
    {
        public SesNotificationType NotificationType { get; set; }

        public SesComplaint Complaint { get; set; }

        public SesBounce Bounce { get; set; }

        public SesMail Mail { get; set; }
    }

    public class SesRecipient
    {
        public string EmailAddress { get; set; }
    }

    /*
	* {
	   "userAgent":"Comcast Feedback Loop (V0.01)",
	   "complainedRecipients":[{
			 "emailAddress":"recipient1@example.com"
		  }
	   ],
	   "complaintFeedbackType":"abuse",
	   "arrivalDate":"2009-12-03T04:24:21.000-05:00",
	   "timestamp":"2012-05-25T14:59:38.623-07:00",
	   "feedbackId":"000001378603177f-18c07c78-fa81-4a58-9dd1-fedc3cb8f49a-000000"
	 */


    public class SesMail
    {
        public string Source { get; set; }

        public string[] Destination { get; set; }

        public string MessageId { get; set; }
    }
}



/*
{
	"notificationType":"Bounce",
	"bounce":{
		"bounceSubType":"Suppressed",
		"bounceType":"Permanent",
		"bouncedRecipients":[ {
			"status":"5.1.1",
			"action":"failed",
			"diagnosticCode":"Amazon SES has suppressed sending to this address because it has a recent history of bouncing as an invalid address. For more information about how to remove an address from the suppression list, see the Amazon SES Developer Guide: http://docs.aws.amazon.com/ses/latest/DeveloperGuide/remove-from-suppressionlist.html",
			"emailAddress":"hi@simulator.amazonses.com"
			}
		],
		"reportingMTA":"dns; amazonses.com",
		"timestamp":"2014-01-14T07:24:37.621Z",
		"feedbackId":"000001438fa3411b-eca1a734-7cec-11e3-8dd2-7bd5ede3b8f3-000000"},
		"mail":{
			"timestamp":"2014-01-14T07:24:37.000Z",
			"source":"hello@carbonmade.com",
			"messageId":"000001438fa3405c-0320ffd1-faa9-4b29-9305-c53432279b7c-000000",
			"destination":[ "hi@simulator.amazonses.com" ]
		}}
*/
