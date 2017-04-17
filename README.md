# AuthNet.WebHooks

This is a c#.Net wrapper around the Authorize.Net WebHook Rest APIs.  This soluition consists of the AuthNet.WebHooks.Dll, and unit tests.  There is a dependency on RestSharp (http://restsharp.org/) to facilitate the Rest communication with Authorize.Net.

## Code Overview

There are 2 main classes in the solution:
1. WebHook:  Allows you to load, create/save/update, and delete a Authorie.Net web hooks.
2. Event: Allows ou to easily parse a web hook response.  Includes SHA512 validation.

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


## Running the Unit Tests
There are numerous unit tests for both the WebHook and Event object. To execute the WebHook unit tests, you must first set your apiLoginID, apiTransactionKey, and notifyUrl (for example, a test url from http://requestb.in)

To execute the Event Unit tests, you need to set your signature:

```csharp
// Note:You must have configured a Signature Key in the Authorize.Net Merchant Interface before you can receive Webhooks 
// notifications. This signature key is used to create a message hash to be sent with each notification that the merchant 
// can then use to verify the notification is genuine. The signature key can be obtained in the Authorize.Net Merchant 
// Interface, at Account > Settings > Security Settings > General Security Settings > API Credentials and Keys

string signatureKey = "<<Enter your signature key here >>";

```

From the response your receive from your notification URL, you will need the JSON Raw Body, and the text from the response header for X-ANET-Signature.

