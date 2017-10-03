# AuthNet.WebHooks

This is a c#.Net wrapper around the Authorize.Net WebHook Rest APIs.  This solution consists of the AuthNet.WebHooks.Dll, and unit tests.  There is a dependency on RestSharp (http://restsharp.org/) to facilitate the Rest communication with Authorize.Net.

## Code Overview

There are 2 main classes in the solution:
1. WebHook:  Allows you to load, create/save/update, delete, and list a Authorie.Net web hooks.
2. Event: Allows you to easily parse a web hook response.  Includes SHA512 validation.

Here's an example of creating a WebHook:

```csharp
// Your API Login ID and Transaction Key are unique pieces of information specifically associated with your payment gateway account. 
// However, the API login ID and Transaction Key are NOT used for logging into the Merchant Interface.  
// Available from the Authorize.net Admin Portal in Settings | Security Settings | General Security Settings | API Credentials & Keys
string apiLoginID = "<< Enter your apiLoginID >>";
string apiTransactionKey = "<< Enter your apiTransactionKey >>";

// The URL where Authorize.Net WebHooks will post notifications.
// For testing purposes, create a Free endpoint at http://requestb.in
string notifyUrl = "<< Enter your notify URL >>";

bool sandbox = true;
        
var webHook = new WebHook(apiLoginID, apiTransactionKey, sandbox);

webHook.name = "My new Webhook";
webHook.url = notifyUrl;
webHook.status = "active";
webHook.eventTypes = new List<string>();
webHook.eventTypes.Add("net.authorize.payment.authcapture.created");
webHook.eventTypes.Add("net.authorize.customer.created");
webHook.eventTypes.Add("net.authorize.customer.paymentProfile.created");

var success = webHook.Save();
if (!success) throw webHook.Exception;
    

```
Parsing the event data received from your notification URL can be accomplished like this:

```csharp
// Note:You must have configured a Signature Key in the Authorize.Net Merchant Interface before you can receive Webhooks 
// notifications. This signature key is used to create a message hash to be sent with each notification that the merchant 
// can then use to verify the notification is genuine. The signature key can be obtained in the Authorize.Net Merchant 
// Interface, at Account > Settings > Security Settings > General Security Settings > API Credentials and Keys
string signatureKey = "<<Enter your signature key here >>";

// from your response data sent to your notification url, you will need the Raw Body (JSON) and the X-ANET-Signature
// from the response header.  This class will use your signature key and json body to create a SHA512 token, and
// compare it with the X-ANET-Signature value to make sure the message has not been modified.
string Raw_JSON_Body = ""{\"notificationId\":\"ff8acc67-a473-4550-8e13-da30ed54ab7d\",\"eventType\":\"net.authorize.customer.deleted\",\"eventDate\":\"2017-04-15T20:39:48.68994Z\",\"webhookId\":\"5eca3570-70a4-4293-aec1-5fa7bdf0183b\",\"payload\":{\"entityName\":\"customerProfile\",\"id\":\"1811525753\"}}"";
string X_Anet_Signature = "0BB9379EB6F2FFC6DD53AEE5F30899303D4A49784B2268B4C93788ED7F89242E1261C390853166C9791160755910E0CB2AB3971799F0A45E25BD16EC1E12E603";

var evt = new Event(signatureKey);

// if you happen to know what type of event you are receiving, you can use this method.
var payEvent = evt.Evaluate<paymentEvent>(Raw_JSON_Body, X_Anet_Signature);

// if you don't know the type of event you are receiving because you subscribed to multiple events in a single webhook
// you can use this overload:
object eventObject = null;
var jsonEventType = evt.Evaluate(Raw_JSON_Body, X_Anet_Signature, out eventObject);                
switch (jsonEventType)
{
    // the common static class can be would in the unit test project.  It simply
    // displayt the object in the output console using System.Diagnostics.Debug.WriteLine();
    case WebhookEventType.CustomerEvent:
        common.DisplayItem((customerEvent)eventObject);
        break;

    case WebhookEventType.FraudEvent:
        common.DisplayItem((fraudEvent)eventObject);
        break;

    case WebhookEventType.PaymentEvent:
        common.DisplayItem((paymentEvent)eventObject);
        break;

    case WebhookEventType.PaymentProfileEvent:
        common.DisplayItem((paymentProfileEvent)eventObject);
        break;

    case WebhookEventType.SubscriptionEvent:
        common.DisplayItem((subscriptionEvent)eventObject);
        break;
}

```


## Running the Unit Tests
There are numerous unit tests for both the WebHook and Event object. To execute the WebHook unit tests, you must first set your apiLoginID, apiTransactionKey, and notifyUrl (for example, a test url from http://requestb.in)

To execute the Event Unit tests, you need to set your signature key.  From the response you receive from your notification URL, you will need the JSON Raw Body, and the text from the response header for X-ANET-Signature.

