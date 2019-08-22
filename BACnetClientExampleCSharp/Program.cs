using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace CASBACnetStack
{
    class Program
    {
        static void Main(string[] args)
        {
            BACnetServer bacnetServer = new BACnetServer();
            bacnetServer.Run();
        }

        unsafe class BACnetServer
        {
            // UDP 
            UdpClient udpServer;
            IPEndPoint RemoteIpEndPoint;

            // Settings 
            const UInt16 SETTING_BACNET_PORT = 47808;
            const UInt32 SETTING_BACNET_DEVICE_INSTANCE = 389000;
            const string SETTING_BACNET_SERVER_IP_ADDRESS = "192.168.1.26";

            // Version 
            const string APPLICATION_VERSION = "0.0.2";

            // User Input
            ConsoleKey subOption = ConsoleKey.NoName;   // If set to NoName, no suboption.  See CheckUserInput for more info

            public void Run()
            {
                Console.WriteLine("Starting BACnet Client Example CSharp version {0}.{1}", APPLICATION_VERSION, CIBuildVersion.CIBUILDNUMBER);
                Console.WriteLine("https://github.com/chipkin/BACnetClientExampleCSharp");
                Console.WriteLine("FYI: BACnet Stack version: {0}.{1}.{2}.{3}",
                    CASBACnetStackAdapter.GetAPIMajorVersion(),
                    CASBACnetStackAdapter.GetAPIMinorVersion(),
                    CASBACnetStackAdapter.GetAPIPatchVersion(),
                    CASBACnetStackAdapter.GetAPIBuildVersion());

                // Send/Recv callbacks. 
                CASBACnetStackAdapter.RegisterCallbackSendMessage(SendMessage);
                CASBACnetStackAdapter.RegisterCallbackReceiveMessage(RecvMessage);
                CASBACnetStackAdapter.RegisterCallbackGetSystemTime(CallbackGetSystemTime);

                // Data type callbacks 
                CASBACnetStackAdapter.BACnetStack_RegisterHookIAm(CallbackHookIAm);
                CASBACnetStackAdapter.BACnetStack_RegisterHookIHave(CallbackHookIHave);
                CASBACnetStackAdapter.BACnetStack_RegisterHookError(CallbackHookError);
                CASBACnetStackAdapter.BACnetStack_RegisterHookReject(CallbackHookReject);
                CASBACnetStackAdapter.BACnetStack_RegisterHookAbort(CallbackHookAbort);
                CASBACnetStackAdapter.BACnetStack_RegisterHookSimpleAck(CallbackHookSimpleAck);
                CASBACnetStackAdapter.BACnetStack_RegisterHookTimeout(CallbackHookTimeout);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyBitString(CallbackHookPropertyBitString);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyBool(CallbackHookPropertyBool);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyCharString(CallbackHookPropertyCharString);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyDate(CallbackHookPropertyDate);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyDouble(CallbackHookPropertyDouble);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyEnum(CallbackHookPropertyEnum);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyNull(CallbackHookPropertyNull);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyObjectIdentifier(CallbackHookPropertyObjectIdentifier);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyOctString(CallbackHookPropertyOctString);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyInt(CallbackHookPropertyInt);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyReal(CallbackHookPropertyReal);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyTime(CallbackHookPropertyTime);
                CASBACnetStackAdapter.BACnetStack_RegisterHookPropertyUInt(CallbackHookPropertyUInt);

                // Add the device. 
                CASBACnetStackAdapter.AddDevice(SETTING_BACNET_DEVICE_INSTANCE);

                // Enable services to support
                CASBACnetStackAdapter.SetServiceEnabled(SETTING_BACNET_DEVICE_INSTANCE, CASBACnetStackAdapter.SERVICES_SUPPORTED_I_AM, true);
                // Disabling WhoIs processing so this example does not respond to the WhoIs message it sends
                CASBACnetStackAdapter.SetServiceEnabled(SETTING_BACNET_DEVICE_INSTANCE, CASBACnetStackAdapter.SERVICES_SUPPORTED_WHO_IS, false);    
                
                
                // All done with the BACnet setup. 
                Console.WriteLine("FYI: CAS BACnet Stack Setup, successfuly");

                // Open the BACnet port to recive messages. 
                this.udpServer = new UdpClient(SETTING_BACNET_PORT);
                this.RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Main loop.                
                Console.WriteLine("FYI: Starting main loop");
                PrintHelp();
                for (; ;)
                {
                    CASBACnetStackAdapter.Loop();
                    CheckUserInput();
                }
            }
            public ulong CallbackGetSystemTime()
            {
                // https://stackoverflow.com/questions/9453101/how-do-i-get-epoch-time-in-c
                return (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            }
            public UInt16 SendMessage(System.Byte* message, UInt16 messageLength, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, Boolean broadcast)
            {
                if (connectionStringLength < 6 || messageLength <= 0)
                {
                    return 0;
                }

                // Extract the connection string into a IP address and port. 
                IPAddress ipAddress;
                if (broadcast)
                {
                    // Note: for the sake of this example, simply setting the last octet if the message should be sent as a broadcast.
                    ipAddress = new IPAddress(new byte[] { connectionString[0], connectionString[1], connectionString[2], 0xFF });
                }
                else
                {
                    ipAddress = new IPAddress(new byte[] { connectionString[0], connectionString[1], connectionString[2], connectionString[3] });
                }
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, (connectionString[4] + connectionString[5] * 256));

                // Debug 
                Console.WriteLine("FYI: Sending {0} bytes to {1}", messageLength, ipEndPoint.ToString());

                
			    // XML decode (debug) 
                IntPtr xmlBuffer = Marshal.AllocHGlobal(1024 * 5);
                int bufferLength = CASBACnetStackAdapter.DecodeAsXML((char*)message, messageLength, xmlBuffer, 1024 * 5);
                string xmlStringBuffer = Marshal.PtrToStringAnsi(xmlBuffer, bufferLength);
                Marshal.FreeHGlobal(xmlBuffer);// Free HGlobal memory
                Console.WriteLine(xmlStringBuffer);
                Console.WriteLine("");

                // Copy from the unsafe pointer to a Byte array. 
                byte[] sendBytes = new byte[messageLength];
                Marshal.Copy((IntPtr)message, sendBytes, 0, messageLength);
   
                try
                {
                    this.udpServer.Send(sendBytes, sendBytes.Length, ipEndPoint);
                    return (UInt16)sendBytes.Length;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return 0;
            }
            public UInt16 RecvMessage(System.Byte* message, UInt16 maxMessageLength, System.Byte* receivedConnectionString, System.Byte maxConnectionStringLength, System.Byte* receivedConnectionStringLength, System.Byte* networkType)
            {
                try
                {
                    if (this.udpServer.Available > 0)
                    {
                        // Data buffer for incoming data.  
                        byte[] receiveBytes = this.udpServer.Receive(ref this.RemoteIpEndPoint);
                        byte[] ipAddress = RemoteIpEndPoint.Address.GetAddressBytes();
                        byte[] port = BitConverter.GetBytes(UInt16.Parse(RemoteIpEndPoint.Port.ToString()));

                        // Copy from the unsafe pointer to a Byte array. 
                        Marshal.Copy(receiveBytes, 0, (IntPtr)message, receiveBytes.Length);

                        // Copy the Connection string 
                        Marshal.Copy(ipAddress, 0, (IntPtr)receivedConnectionString, 4);
                        Marshal.Copy(port, 0, (IntPtr)receivedConnectionString + 4, 2);
                        *receivedConnectionStringLength = 6;

                        // Debug 
                        Console.WriteLine("FYI: Recving {0} bytes from {1}", receiveBytes.Length, RemoteIpEndPoint.ToString());

                    
					    // XML decode (debug) 
                        IntPtr xmlBuffer = Marshal.AllocHGlobal(1024 * 5);
                        int bufferLength = CASBACnetStackAdapter.DecodeAsXML((char*)message, (ushort)receiveBytes.Length, xmlBuffer, 1024 * 5);
                        string xmlStringBuffer = Marshal.PtrToStringAnsi(xmlBuffer, bufferLength);
                        Marshal.FreeHGlobal(xmlBuffer);// Free HGlobal memory
                        Console.WriteLine(xmlStringBuffer);
                        Console.WriteLine("");
					
                        // Return length. 
                        return (ushort)receiveBytes.Length;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return 0;
            }

            private byte[] CreateConnectionString(IPEndPoint endpoint)
            {
                byte[] connectionString = new byte[6];
                endpoint.Address.GetAddressBytes().CopyTo(connectionString, 0);
                connectionString[4] = (byte)(endpoint.Port);
                connectionString[5] = (byte)(endpoint.Port >> 8);
                return connectionString;
            }

            public byte* PointerData(byte[] safe)
            {
                fixed (byte* converted = safe)
                {
                    return converted;
                }
            }

            private void CheckUserInput()
            {
                if (!Console.KeyAvailable)
                {
                    return; // Nothing to do. 
                }
                ConsoleKeyInfo inputKey = Console.ReadKey(true);
                Console.WriteLine(">{0}", inputKey.Key);

                // Check if in a subOption
                if(subOption != ConsoleKey.NoName)
                {
                    CheckUserSubOption(inputKey.Key);
                    return;
                }

                byte[] connectionStringAsBytes = CreateConnectionString(new IPEndPoint(IPAddress.Parse(SETTING_BACNET_SERVER_IP_ADDRESS), SETTING_BACNET_PORT));
                byte* connectionStringPointer = PointerData(connectionStringAsBytes);

                const UInt16 timeToLive = 60 * 5; // 5 Min 

                switch (inputKey.Key)
                {
                    case ConsoleKey.D:
                        Console.WriteLine("WhoIs Menu:");
                        Console.WriteLine("  L - Send a Local Broadcast WhoIs message");
                        Console.WriteLine("  W - Send a Local Broadcast WhoIs message with Limits");
                        Console.WriteLine("  R - Send a Remote Broadcast WhoIs message");
                        Console.WriteLine("  G - Send a Global Broadcast WhoIs message");
                        Console.WriteLine("  Q - Exit WhoIs menu");
                        subOption = inputKey.Key; ;
                        break;

                    case ConsoleKey.F:
                        Console.WriteLine("FYI: Sending RegisterForeignDevice message");
                        CASBACnetStackAdapter.SendRegisterForeignDevice(timeToLive, connectionStringPointer, (byte)connectionStringAsBytes.Length);
                        break;

                    case ConsoleKey.C:
                        Console.WriteLine("FYI: Sending SubscribeCOV message");
                        CASBACnetStackAdapter.SendSubscribeCOV(null, 500, CASBACnetStackAdapter.OBJECT_TYPE_ANALOG_INPUT, 0, false, timeToLive, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, 0, null, 0);
                        break;

                    case ConsoleKey.R:
                        Console.WriteLine("FYI: Sending ReadProperty message");
                        CASBACnetStackAdapter.BuildReadProperty(CASBACnetStackAdapter.OBJECT_TYPE_ANALOG_INPUT, 0, CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE, false, 0);
                        CASBACnetStackAdapter.SendReadProperty(null, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, 0, null, 0);
                        break;

                    case ConsoleKey.A:
                        Console.WriteLine("FYI: Sending ReadProperty All message");
                        CASBACnetStackAdapter.BuildReadProperty(CASBACnetStackAdapter.OBJECT_TYPE_DEVICE, 389999, CASBACnetStackAdapter.PROPERTY_IDENTIFIER_ALL, false, 0);
                        CASBACnetStackAdapter.SendReadProperty(null, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, 0, null, 0);
                        break;

                    case ConsoleKey.W:
                        Console.WriteLine("FYI: Sending WriteProperty message");

                        String valueString = "99.6";
                        IntPtr ptS = Marshal.StringToHGlobalAnsi(valueString);

                        CASBACnetStackAdapter.BuildWriteProperty(CASBACnetStackAdapter.DATA_TYPE_REAL, ptS, (uint)valueString.Length, CASBACnetStackAdapter.OBJECT_TYPE_ANALOG_OUTPUT, 1, CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE, false, 0, true, 8);
                        CASBACnetStackAdapter.SendWriteProperty(null, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, 0, null, 0);
                        break;
                    case ConsoleKey.M:
                        Console.WriteLine("FYI: Sending ReadProperty Multiple Asynch message");

                        CASBACnetStackAdapter.ReadPropertyAsync[] readPropertyValues = new CASBACnetStackAdapter.ReadPropertyAsync[3];

                        readPropertyValues[0] = new CASBACnetStackAdapter.ReadPropertyAsync()
                        {
                            flags = 0x00,
                            objectInstance = 0,
                            objectType = CASBACnetStackAdapter.OBJECT_TYPE_ANALOG_INPUT,
                            propertyIdentifier = CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE,
                            propertyArrayIndex = 0
                        };
                        readPropertyValues[1] = new CASBACnetStackAdapter.ReadPropertyAsync()
                        {
                            flags = 0x00,
                            objectInstance = 3,
                            objectType = CASBACnetStackAdapter.OBJECT_TYPE_BINARY_INPUT,
                            propertyIdentifier = CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE,
                            propertyArrayIndex = 0
                        };
                        readPropertyValues[2] = new CASBACnetStackAdapter.ReadPropertyAsync()
                        {
                            flags = 0x01, // Use array index 
                            objectInstance = 13,
                            objectType = CASBACnetStackAdapter.OBJECT_TYPE_MULTI_STATE_INPUT,
                            propertyIdentifier = CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE,
                            propertyArrayIndex = 5
                        };


                        CASBACnetStackAdapter.SendReadPropertyAsync(null, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, 0, null, 0, readPropertyValues);
                        break;
                    case ConsoleKey.E:
                        Console.WriteLine("FYI: Sending WriteProperty Multiple Asynch message");

                        CASBACnetStackAdapter.WritePropertyAsync[] writePropertyValues = new CASBACnetStackAdapter.WritePropertyAsync[3];

                        writePropertyValues[0] = new CASBACnetStackAdapter.WritePropertyAsync()
                        {
                            flags = 0x00,
                            objectInstance = 1,
                            objectType = CASBACnetStackAdapter.OBJECT_TYPE_ANALOG_OUTPUT,
                            propertyIdentifier = CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE,
                            propertyArrayIndex = 0,
                            dataType = CASBACnetStackAdapter.DATA_TYPE_REAL,
                            priority = 0,
                            valueAsString = "1.19"
                        };
                        writePropertyValues[1] = new CASBACnetStackAdapter.WritePropertyAsync()
                        {
                            flags = 0x00,
                            objectInstance = 1,
                            objectType = CASBACnetStackAdapter.OBJECT_TYPE_BINARY_OUTPUT,
                            propertyIdentifier = CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE,
                            propertyArrayIndex = 0,
                            dataType = CASBACnetStackAdapter.DATA_TYPE_ENUMERATED,
                            priority = 0,
                            valueAsString = "1"
                        };
                        writePropertyValues[2] = new CASBACnetStackAdapter.WritePropertyAsync()
                        {
                            flags = 0x01 | 0x02, // 0x01 = use propertyArrayIndex, 0x02 = use priority
                            objectInstance = 14,
                            objectType = CASBACnetStackAdapter.OBJECT_TYPE_MULTI_STATE_OUTPUT,
                            propertyIdentifier = CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE,
                            propertyArrayIndex = 0,
                            dataType = CASBACnetStackAdapter.DATA_TYPE_ENUMERATED,
                            priority = 8,
                            valueAsString = "7"
                        };


                        CASBACnetStackAdapter.SendWritePropertyAsync(null, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, 0, null, 0, writePropertyValues);
                        break;
                    default:
                        Console.WriteLine("You pressed the '{0}' key. Not assinged", inputKey.Key);

                        PrintHelp();

                        break;
                } // Switch 
            }

            private void PrintHelp()
            {
                Console.WriteLine("Help:");
                Console.WriteLine("  D - Send Whois message");
                Console.WriteLine("  F - RegisterForeignDevice message");
                Console.WriteLine("  C - Send SubscribeCOV message");
                Console.WriteLine("  R - Send ReadProperty message");
                Console.WriteLine("  A - Send ReadProperty All message");
                Console.WriteLine("  W - Send WriteProperty message");
                Console.WriteLine("  M - Send ReadProperty Multiple Asynch message");
                Console.WriteLine("  E - Send WriteProperty Multiple Asynch message");
            }

            private void CheckUserSubOption(ConsoleKey input)
            {
                byte[] connectionStringAsBytes = CreateConnectionString(new IPEndPoint(IPAddress.Parse(SETTING_BACNET_SERVER_IP_ADDRESS), SETTING_BACNET_PORT));
                byte* connectionStringPointer = PointerData(connectionStringAsBytes);

                switch (subOption)
                {
                    case ConsoleKey.D:
                        switch(input)
                        {
                            case ConsoleKey.L:
                                Console.WriteLine("FYI: Sending a Local Broadcast WhoIs message");
                                CASBACnetStackAdapter.SendWhoIs(connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, true, 0, null, 0);
                                break;
                            case ConsoleKey.W:
                                Console.WriteLine("FYI: Sending a Local Broadcast WhoIs message with limits of devices with device instances between 389900 and 389999");
                                CASBACnetStackAdapter.SendWhoIsWithLimits(389900, 389999, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, true, 0, null, 0);
                                break;
                            case ConsoleKey.R:
                                Console.WriteLine("FYI: Sending a Remote Broadcast WhoIs message to DNET 7");
                                CASBACnetStackAdapter.SendWhoIs(connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, true, 7, null, 0);
                                break;
                            case ConsoleKey.G:
                                Console.WriteLine("FYI: Sending a Global Broadcast WhoIs message");
                                CASBACnetStackAdapter.SendWhoIs(connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, true, 0xFFFF, null, 0);
                                break;
                            case ConsoleKey.Q:
                                Console.WriteLine("FYI: Exiting the WhoIs menu");
                                subOption = ConsoleKey.NoName;
                                break;
                            default:
                                Console.WriteLine("You pressed the '{0}' key. Not an assigned WhoIs option", input);

                                Console.WriteLine("Help:");
                                Console.WriteLine("  L - Send a Local Broadcast WhoIs message");
                                Console.WriteLine("  W - Send a Local Broadcast WhoIs message with Limits");
                                Console.WriteLine("  R - Send a Remote Broadcast WhoIs message");
                                Console.WriteLine("  G - Send a Global Broadcast WhoIs message");
                                Console.WriteLine("  Q - Exit WhoIs menu");
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            private void ReadProperty()
            {
                byte[] connectionStringAsBytes = CreateConnectionString(new IPEndPoint(IPAddress.Parse(SETTING_BACNET_SERVER_IP_ADDRESS), SETTING_BACNET_PORT));
                byte* connectionStringPointer = PointerData(connectionStringAsBytes);
                Console.WriteLine("FYI: Sending ReadProperty message");
                CASBACnetStackAdapter.BuildReadProperty(CASBACnetStackAdapter.OBJECT_TYPE_ANALOG_INPUT, 0, CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE, false, 0);
                CASBACnetStackAdapter.SendReadProperty(null, connectionStringPointer, (byte)connectionStringAsBytes.Length, CASBACnetStackAdapter.NETWORK_TYPE_IP, 0, null, 0);

            }


            void CallbackHookIAm(UInt32 deviceIdentifier, UInt32 maxApduLengthAccepted, System.Byte segmentationSupported, UInt16 vendorIdentifier, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookIAm deviceIdentifier=[{0}]", deviceIdentifier);
            }

            void CallbackHookIHave(UInt32 deviceIdentifier, UInt16 objectType, UInt32 objectInstance, char* objectName, UInt32 objectNameLength, System.Byte objectNameEncoding, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookIHave deviceIdentifier=[{0}], objectType=[{1}], objectInstance=[{2}]", deviceIdentifier, objectType, objectInstance);
            }


            void CallbackHookError(System.Byte originalInvokeId, UInt32 errorChoice, UInt32 errorClass, UInt32 errorCode, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength, bool useObjectProperty, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier)
            {
                Console.WriteLine("CallbackHookError originalInvokeId=[{0}], errorChoice=[{1}], errorClass=[{2}]", originalInvokeId, errorChoice, errorClass);
            }


            void CallbackHookReject(System.Byte originalInvokeId, UInt32 rejectReason, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookReject originalInvokeId=[{0}], rejectReason=[{1}]", originalInvokeId, rejectReason);
            }


            void CallbackHookAbort(System.Byte originalInvokeId, bool sentByServer, UInt32 abortReason, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookAbort originalInvokeId=[{0}], abortReason=[{1}]", originalInvokeId, abortReason);
            }


            void CallbackHookSimpleAck(System.Byte originalInvokeId, UInt32 serverAckChoice, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookSimpleAck originalInvokeId=[{0}], serverAckChoice=[{1}]", originalInvokeId, serverAckChoice);
            }


            void CallbackHookTimeout(System.Byte originalInvokeId, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookTimeout originalInvokeId=[{0}]", originalInvokeId);
            }


            void CallbackHookPropertyBitString(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, bool* value, UInt32 length, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyBitString id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], ", id, objectType, objectInstance, propertyIdentifier);
            }


            void CallbackHookPropertyBool(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, bool value, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyBool id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], value=[{4}] ", id, objectType, objectInstance, propertyIdentifier, value);
            }


            void CallbackHookPropertyCharString(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, char* value, UInt32 length, System.Byte encoding, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyCharString id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}]", id, objectType, objectInstance, propertyArrayIndex);
            }


            void CallbackHookPropertyDate(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, System.Byte year, System.Byte month, System.Byte day, System.Byte weekday, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyDate id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], year=[{4}], month=[{4}], day=[{4}], weekday=[{4}] ", id, objectType, objectInstance, propertyIdentifier, year, month, day, weekday);
            }


            void CallbackHookPropertyDouble(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, double value, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyDouble id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], value=[{4}] ", id, objectType, objectInstance, propertyIdentifier, value);
            }


            void CallbackHookPropertyEnum(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, UInt32 value, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyEnum id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], value=[{4}] ", id, objectType, objectInstance, propertyIdentifier, value);
            }


            void CallbackHookPropertyNull(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyNull id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], value=[{4}] ", id, objectType, objectInstance, propertyIdentifier);
            }


            void CallbackHookPropertyObjectIdentifier(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, UInt16 objectTypeValue, UInt32 objectInstanceValue, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyObjectIdentifier id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], objectTypeValue=[{4}], objectInstanceValue=[{4}] ", id, objectType, objectInstance, propertyIdentifier, objectTypeValue, objectInstanceValue);
            }


            void CallbackHookPropertyOctString(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, System.Byte* value, UInt32 length, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyOctString id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}]", id, objectType, objectInstance, propertyIdentifier);
            }


            void CallbackHookPropertyInt(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, Int32 value, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyInt id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], value=[{4}] ", id, objectType, objectInstance, propertyIdentifier, value);
            }


            void CallbackHookPropertyReal(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, float value, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyReal id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], value=[{4}] ", id, objectType, objectInstance, propertyIdentifier, value);

                if (objectType == CASBACnetStackAdapter.OBJECT_TYPE_ANALOG_INPUT && objectInstance == 0 && propertyIdentifier == CASBACnetStackAdapter.PROPERTY_IDENTIFIER_PRESENT_VALUE)
                {
                    // Got the value 
                    Console.WriteLine("Example: Send this value to Azure");
                }
            }


            void CallbackHookPropertyTime(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, System.Byte hour, System.Byte minute, System.Byte second, System.Byte hundrethSecond, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyTime id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], hour=[{4}], minute=[{4}], second=[{4}], hundrethSecond=[{4}] ", id, objectType, objectInstance, propertyIdentifier, hour, minute, second, hundrethSecond);
            }

            void CallbackHookPropertyUInt(UInt32 id, System.Byte service, UInt16 objectType, UInt32 objectInstance, UInt32 propertyIdentifier, bool usePropertyArrayIndex, UInt32 propertyArrayIndex, UInt32 value, System.Byte* connectionString, System.Byte connectionStringLength, System.Byte networkType, UInt16 network, System.Byte* sourceAddress, System.Byte sourceAddressLength)
            {
                Console.WriteLine("CallbackHookPropertyUInt id=[{0}], objectType=[{1}], objectInstance=[{2}], propertyIdentifier=[{3}], value=[{4}] ", id, objectType, objectInstance, propertyIdentifier, value);
            }
        }





    }
}
