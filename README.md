# BACnet Client Example C#

A basic BACnet IP Client example written in CSharp using the [CAS BACnet Stack](https://store.chipkin.com/services/stacks/bacnet-stack). Client example is meant to be used with the [BACnet IP C# Server Example](https://github.com/chipkin/BACnetServerExampleCSharp). Supports WhoIs, ReadProperty, and WriteProperty services.

## Releases

Build versions of this example can be downloaded from the [Releases](https://github.com/chipkin/BACnetClientExampleCSharp/releases) page.

## Installation

Download the latest release zip file on the [Releases](https://github.com/chipkin/BACnetClientExampleCSharp/releases) page.
Copy CASBACnetStack_x64_Release.dll from the CAS BACnet Stack into the release folder. Please contact Chipkin Automation Systems for access to the CAS BACnet Stack. Launch client by using the following command:
```
dotnet BACnetClientExampleCSharp.dll 
```
Requires [.NET Core 3.0+](https://dotnet.microsoft.com/download)


## Usage

#### Main Menu
- D - WhoIs Menu - Send various WhoIs messages
- F - RegisterForeignDevice message
- C - Send SubscribeCOV message
- R - Send ReadProperty message
- A - Send ReadProperty All message
- W - Send WriteProperty message
- M - Send ReadProperty Multiple Asynch message
- E - Send WriteProperty Multiple Asynch message

#### WhoIs Menu
- L - Send a Local Broadcast WhoIs message
- W - Send a Local Broadcast WhoIs message with Limits
- R - Send a Remote Broadcast WhoIs message
- G - Send a Global Broadcast WhoIs message
- Q - Exit WhoIs menu

Client expects a device with the following objects:
- Device: 389001  (Device name Rainbow)
  - analog_input: 0  (AnalogInput Amber)
  - analog_input: 1  (AnalogInput Bronze)
  - analog_input: 2  (AnalogInput Chartreuse)
  - analog_output: 0  (AnalogOutput Diamond)
  - analog_output: 1  (AnalogOutput Emerald)
  - analog_output: 2  (AnalogOutput Fuchsia)
  - analog_value: 0  (AnalogValue Gold)
  - analog_value: 1  (AnalogValue Hot Pink)
  - analog_value: 2  (AnalogValue Indigo)
  - binary_input: 0  (BinaryInput Kiwi)
  - binary_input: 1  (BinaryInput Lilac)
  - binary_input: 2  (BinaryInput Magenta)
  - binary_value: 0  (BinaryValue Nickel)
  - binary_value: 1  (BinaryValue Onyx)
  - binary_value: 2  (BinaryValue Purple)
  - multi_state_input: 0  (MultiStateInput Quartz)
  - multi_state_input: 1  (MultiStateInput Red)
  - multi_state_input: 2  (MultiStateInput Silver)
  - multi_state_value: 0  (MultiStateValue Turquoise)
  - multi_state_value: 1  (MultiStateValue Umber)
  - multi_state_value: 2  (MultiStateValue Vermilion)

## Build

A [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) project is included with this project. This project also auto built using [Gitlab CI](https://docs.gitlab.com/ee/ci/) on every commit.

1. Copy *CASBACnetStack_x64_Debug.dll*, *CASBACnetStack_x64_Debug.lib*, *CASBACnetStack_x64_Release.dll*, and *CASBACnetStack_x64_Release.lib* from the [CAS BACnet Stack](https://store.chipkin.com/services/stacks/bacnet-stack) project into the /bin/netcoreapp2.1/ folder.
2. Use [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) to build the project. The solution can be found in the */BACnetClientExampleCSharp/* folder.
  
## Example output

```txt
Starting BACnet Client Example CSharp version 0.0.1.0
https://github.com/chipkin/BACnetClientExampleCSharp
FYI: BACnet Stack version: 3.8.1.0
FYI: CAS BACnet Stack Setup, successfuly
FYI: Starting main loop
You pressed the 'Spacebar' key. Not assigned

Help:
  D - Send Whois message
  F - RegisterForeignDevice message
  C - Send SubscribeCOV message
  R - Send ReadProperty message
  A - Send ReadProperty All message
  W - Send WriteProperty message
  M - Send ReadProperty Multiple Asynch message
  E - Send WriteProperty Multiple Asynch message
  Q - Quit the server
  
FYI: Sending Whois message
FYI: Sending 12 bytes to 192.168.1.26:47808
FYI: Recving 25 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[0] is not supported by this device
FYI: Sending RegisterForeignDevice message
FYI: Sending 6 bytes to 192.168.1.26:47808
FYI: Sending SubscribeCOV message
FYI: Sending 23 bytes to 192.168.1.26:47808
FYI: Recving 9 bytes from 192.168.1.26:47808
CallbackHookSimpleAck originalInvokeId=[0], serverAckChoice=[5]
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
You pressed the 'Spacebar' key. Not assigned

Help:
  D - Send Whois message
  F - RegisterForeignDevice message
  C - Send SubscribeCOV message
  R - Send ReadProperty message
  A - Send ReadProperty All message
  W - Send WriteProperty message
  M - Send ReadProperty Multiple Asynch message
  E - Send WriteProperty Multiple Asynch message
  Q - Quit the server
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Sending ReadProperty message
FYI: Sending 17 bytes to 192.168.1.26:47808
FYI: Recving 23 bytes from 192.168.1.26:47808
CallbackHookPropertyReal id=[1], objectType=[0], objectInstance=[0], propertyIdentifier=[85], value=[3996.914]
Send this value to Azure
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Sending ReadProperty message
FYI: Sending 17 bytes to 192.168.1.26:47808
FYI: Recving 23 bytes from 192.168.1.26:47808
CallbackHookPropertyReal id=[2], objectType=[0], objectInstance=[0], propertyIdentifier=[85], value=[3997.915]
Send this value to Azure
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Sending ReadProperty All message
FYI: Sending 19 bytes to 192.168.1.26:47808
FYI: Recving 432 bytes from 192.168.1.26:47808
CallbackHookPropertyUInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[11], value=[3000]
CallbackHookPropertyCharString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyBool id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[24], value=[True]
CallbackHookPropertyCharString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyCharString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyDate id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[56], year=[119], month=[119], day=[119], weekday=[119]
CallbackHookPropertyTime id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[57], hour=[16], minute=[16], second=[16], hundrethSecond=[16]
CallbackHookPropertyUInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[62], value=[1476]
CallbackHookPropertyCharString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyUInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[73], value=[0]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[75], objectTypeValue=[8], objectInstanceValue=[8]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[0], objectInstanceValue=[0]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[1], objectInstanceValue=[1]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[2], objectInstanceValue=[2]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[3], objectInstanceValue=[3]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[4], objectInstanceValue=[4]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[5], objectInstanceValue=[5]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[8], objectInstanceValue=[8]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[13], objectInstanceValue=[13]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[14], objectInstanceValue=[14]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[19], objectInstanceValue=[19]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[20], objectInstanceValue=[20]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[39], objectInstanceValue=[39]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[40], objectInstanceValue=[40]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[42], objectInstanceValue=[42]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[45], objectInstanceValue=[45]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[46], objectInstanceValue=[46]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[47], objectInstanceValue=[47]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[48], objectInstanceValue=[48]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[50], objectInstanceValue=[50]
CallbackHookPropertyObjectIdentifier id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[56], objectInstanceValue=[56]
CallbackHookPropertyCharString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyEnum id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[79], value=[8]
CallbackHookPropertyBitString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[96],
CallbackHookPropertyBitString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[97],
CallbackHookPropertyUInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[98], value=[1]
CallbackHookPropertyEnum id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[107], value=[3]
CallbackHookPropertyEnum id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[112], value=[0]
CallbackHookPropertyInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[119], value=[0]
CallbackHookPropertyUInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[120], value=[389]
CallbackHookPropertyCharString id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyUInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[139], value=[14]
::CASBACnetStack::BACnetBusinessLogic::ProcessPropertyValueUsingHook() in file: X:\chipkin\cas-bacnet-stack\source\BACnetBusinessLogic.cpp(3622) - Fyi - Complex Property datatype=[25] is not supported by the Property Hooks.  Use XML processing instead
CallbackHookPropertyUInt id=[3], objectType=[8], objectInstance=[389999], propertyIdentifier=[155], value=[20]
FYI: Sending ReadProperty All message
FYI: Sending 19 bytes to 192.168.1.26:47808
FYI: Recving 432 bytes from 192.168.1.26:47808
CallbackHookPropertyUInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[11], value=[3000]
CallbackHookPropertyCharString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyBool id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[24], value=[True]
CallbackHookPropertyCharString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyCharString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyDate id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[56], year=[119], month=[119], day=[119], weekday=[119]
CallbackHookPropertyTime id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[57], hour=[16], minute=[16], second=[16], hundrethSecond=[16]
CallbackHookPropertyUInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[62], value=[1476]
CallbackHookPropertyCharString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyUInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[73], value=[0]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[75], objectTypeValue=[8], objectInstanceValue=[8]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[0], objectInstanceValue=[0]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[1], objectInstanceValue=[1]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[2], objectInstanceValue=[2]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[3], objectInstanceValue=[3]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[4], objectInstanceValue=[4]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[5], objectInstanceValue=[5]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[8], objectInstanceValue=[8]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[13], objectInstanceValue=[13]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[14], objectInstanceValue=[14]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[19], objectInstanceValue=[19]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[20], objectInstanceValue=[20]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[39], objectInstanceValue=[39]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[40], objectInstanceValue=[40]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[42], objectInstanceValue=[42]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[45], objectInstanceValue=[45]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[46], objectInstanceValue=[46]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[47], objectInstanceValue=[47]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[48], objectInstanceValue=[48]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[50], objectInstanceValue=[50]
CallbackHookPropertyObjectIdentifier id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[76], objectTypeValue=[56], objectInstanceValue=[56]
CallbackHookPropertyCharString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyEnum id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[79], value=[8]
CallbackHookPropertyBitString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[96],
CallbackHookPropertyBitString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[97],
CallbackHookPropertyUInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[98], value=[1]
CallbackHookPropertyEnum id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[107], value=[3]
CallbackHookPropertyEnum id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[112], value=[0]
CallbackHookPropertyInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[119], value=[0]
CallbackHookPropertyUInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[120], value=[389]
CallbackHookPropertyCharString id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[0]
CallbackHookPropertyUInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[139], value=[14]
::CASBACnetStack::BACnetBusinessLogic::ProcessPropertyValueUsingHook() in file: X:\chipkin\cas-bacnet-stack\source\BACnetBusinessLogic.cpp(3622) - Fyi - Complex Property datatype=[25] is not supported by the Property Hooks.  Use XML processing instead
CallbackHookPropertyUInt id=[4], objectType=[8], objectInstance=[389999], propertyIdentifier=[155], value=[20]
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
You pressed the 'Spacebar' key. Not assigned

Help:
  D - Send Whois message
  F - RegisterForeignDevice message
  C - Send SubscribeCOV message
  R - Send ReadProperty message
  A - Send ReadProperty All message
  W - Send WriteProperty message
  M - Send ReadProperty Multiple Asynch message
  E - Send WriteProperty Multiple Asynch message
  Q - Quit the server
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Sending WriteProperty message
FYI: Sending 26 bytes to 192.168.1.26:47808
FYI: Recving 9 bytes from 192.168.1.26:47808
CallbackHookSimpleAck originalInvokeId=[5], serverAckChoice=[15]
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Sending ReadProperty Multiple Asynch message
FYI: Sending 39 bytes to 192.168.1.26:47808
FYI: Recving 55 bytes from 192.168.1.26:47808
CallbackHookPropertyReal id=[6], objectType=[0], objectInstance=[0], propertyIdentifier=[85], value=[4008.926]
Send this value to Azure
CallbackHookPropertyEnum id=[6], objectType=[3], objectInstance=[3], propertyIdentifier=[85], value=[0]
CallbackHookError originalInvokeId=[6], errorChoice=[14], errorClass=[2],
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device
FYI: Sending WriteProperty Multiple Asynch message
FYI: Sending 56 bytes to 192.168.1.26:47808
FYI: Recving 24 bytes from 192.168.1.26:47808
CallbackHookError originalInvokeId=[7], errorChoice=[16], errorClass=[1],
FYI: Recving 42 bytes from 192.168.1.26:47808
::CASBACnetStack::BACnetUnconfirmedRequestProcessor::Process() in file: X:\chipkin\cas-bacnet-stack\source\BACnetUnconfirmedRequestProcessor.cpp(106) - Error - Unconfirmed Service=[2] is not supported by this device

```
