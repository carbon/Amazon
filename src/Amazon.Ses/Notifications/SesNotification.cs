namespace Amazon.Ses
{
    public class SesNotification
    {
        public SesNotificationType NotificationType { get; set; }

        public SesComplaint Complaint { get; set; }

        public SesBounce Bounce { get; set; }

        public SesMail Mail { get; set; }
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
