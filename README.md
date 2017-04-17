# AuthNet.WebHooks

This is a c#.Net wrapper around the Authorize.Net WebHook Rest APIs.  This soluition consists of the AuthNet.WebHooks.Dll, and unit tests.  There is a dependency on RestSharp (http://restsharp.org/) to facilitate the Rest communication with Authorize.Net.

## Code Overview

There are 2 main classes in the solution:
1. WebHook:  Allows you to load, create/save/update, and delete a Authorie.Net web hooks.
2. Event: Allows ou to easily parse a web hook response.  Includes SHA512 validation.

Here's an example of creating a WebHook:

```csharp
var s = "JavaScript syntax highlighting";
alert(s);
```


## Running the Unit Tests
There are numerous unit tests for both the WebHook and Event object. To execute the WebHook


