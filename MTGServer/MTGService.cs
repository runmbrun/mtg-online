#region Using Statements

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Reflection;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using MTG;

#endregion



namespace MTGServer
{
    public partial class MTGService : ServiceBase
    {

        #region Private Variables

        // Debug vars
        public Boolean Debug = false;

        // DB vars
        MTGDatabase MTGDB;

        // Error Handling vars
        private String _servicePath;
        private String[] _errorMessages;
        private Int32 _errorCount;

        // Network vars
        private Int32 _port = 4545;
        byte[] byteData = new byte[1024];

        //The ClientInfo structure holds the required information 
        // about every client connected to the server
        struct ClientInfo
        {
            //Socket of the client
            public Socket Socket;
            
            //Name by which the user logged into the chat room
            public String Player;
        }

        //The collection of all clients logged 
        //into the room (an array of type ClientInfo)
        ArrayList ClientList = null;
        
        //The main socket on which the server listens to the clients
        Socket serverSocket;        

        #endregion


        /// <summary>
        /// This is the constructor for the BuildService Class
        /// </summary>
        public MTGService()
        {
            InitializeComponent();

            // this is the string array of all the errors found during processing
            _errorMessages = new String[128];
            _errorMessages.Initialize();
            _errorCount = 0;

            try
            {
                // start the Event Logger
                if (!System.Diagnostics.EventLog.SourceExists("MTG Service"))
                {
                    System.Diagnostics.EventLog.CreateEventSource("MTG Service", "MTG Service Log");
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }

            //this.EventLog.Source
            eventLog1.Source = "MTG Service";
            eventLog1.Log = "MTG Service Log";
            
            // get the current folder the app is running in and save it            
            _servicePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

            if (_servicePath == "")
            {
                _servicePath = "c:\\temp\\";
                AddError(String.Format("ERROR: Can't find the path of the running EXE: "));
            }
            else
            {
                _servicePath += "\\";
            }

            // check to see if this is a debug version
#if DEBUG
            Debug = true;
#endif

            // read the Config file
            if (!ReadConfig())
            {
                // error!
                AddError(String.Format("ERROR: Unable to read the config file"));
            }

            ClientList = new ArrayList();
        }
        
        /// <summary>
        /// This function will read in a config file if it exists
        /// </summary>
        /// <returns></returns>
        public bool ReadConfig()
        {
            Boolean Read = false;

            Read = true;

            return Read;
        }

        /// <summary>
        /// This is the function that is called when this app is run as a service.
        /// This is the Start method of the Service.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog1.WriteEntry("MTG Service is now Starting");
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("MTG Service failed while trying to start: " + ex.Message);
            }

            // This is the new code that will start this service listening on a specific TCP port
            try
            {
                //We are using TCP sockets
                serverSocket = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Stream,
                                          ProtocolType.Tcp);

                //Assign the any IP of the machine and listen on port number 1000
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, _port);

                //Bind and listen on the given address
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(4);

                //Accept the incoming clients
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
            }
            catch (Exception ex)
            {
                AddError(String.Format("OnStart: Server TCP {0}", ex.Message));
            }

            // database connection initialization
            MTGDB = new MTGDatabase();
            if (!MTGDB.Connect())
            {
                // error!
                AddError(String.Format("ERROR: Unable to connect to the database. {0}", MTGDB.GetLastError));
            }
        }

        /// <summary>
        /// This is the function that is called when this app is run as a service.
        /// This is the Stop method of the Service.
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                eventLog1.WriteEntry("MTG Service is Stopping");
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("MTG Service failed while try to stop the service: " + ex.Message);
            }

             // close all the network clients talking to this server
            if (ClientList != null)
            {
                foreach (ClientInfo client in ClientList)
                {
                    client.Socket.Close();
                }
            }

            // close down the database connection
            if (MTGDB.Connected)
            {
                if (MTGDB.Disconnect())
                {
                    // error!
                    AddError(String.Format("ERROR: Unable to disconnect to the database. {0}", MTGDB.GetLastError));
                }
            }
        }

        #region Test OnStart and OnStop Functions

        /// <summary>
        /// This function is called by the TestBuilder app to trick the BuilderService into
        /// thinking it is running as a service.
        /// This is the Start method of the Service.
        /// </summary>
        /// <param name="args"></param>
        public void TestOnStart(string[] args)
        {
            OnStart(args);
        }

        /// <summary>
        /// This function is called by the TestBuilder app to trick the BuilderService into
        /// thinking it is running as a service.
        /// This is the Stop method of the Service.
        /// </summary>        
        public void TestOnStop()
        {
            OnStop();
        }
        #endregion

        /// <summary>
        /// Adds an error string to the errors string array
        /// </summary>
        /// <param name="strErrorMessage"></param>
        private void AddError(String ErrorMessage)
        {
            _errorMessages[_errorCount++] = ErrorMessage;
            // mmb - need to write this to a log file too!
        }

        /// <summary>
        /// When a connection is accepted, this function is called
        /// </summary>
        /// <param name="ar"></param>
        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = serverSocket.EndAccept(ar);

                //Start listening for more clients
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);

                //Once the client connects then start receiving the commands from them
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
            }
            catch (Exception ex)
            {
                String.Format("OnAccept: {0}", ex.Message);
            }
        }

        /// <summary>
        /// When a packet is received, this function is called
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                clientSocket.EndReceive(ar);

                ClientInfo clientInfo;
                byte[] message = null;
                String User = "Player [Unknown] ";

                
                //Transform the array of bytes received from the user into an
                //intelligent form of object Data
                MTGNetworkPacket msgReceived = new MTGNetworkPacket(byteData);
                MTGNetworkPacket msgToSend = new MTGNetworkPacket();
                                
                
                //If the message is to login, logout, or simple text message
                //then when send to others the type of the message remains the same
                switch (msgReceived.OpCode)
                {
                    // *** Login ***
                    case MTGNetworkPacket.MTGOpCode.Login:

                        Int32 result = 0;

                        // verify that this user can log on
                        String data = msgReceived.Data.ToString();
                        String user = data.Substring(0, data.IndexOf(":"));
                        String pass = data.Substring(data.IndexOf(":") + 1);

                        // now check the user against the database...
                        if (MTGDB.ValidateUser(user, pass))
                        {   
                            result = 1;

                            if (MTGDB.IsAdmin(user, pass))
                            {
                                result += 1;
                            }
                        }

                        //When a user logs in to the server then we add them to our list of clients
                        clientInfo = new ClientInfo();
                        clientInfo.Socket= clientSocket;
                        clientInfo.Player = user;
                        ClientList.Add(clientInfo);

                        // Send back a login packet to the user to tell them if user was validate against db
                        msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.Login;
                        msgToSend.Data = result;

                        message = msgToSend.ToByte();

                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientSocket);

                        // this player isn't a valid player, so disconnect the client
                        if (result == 0)
                        {
                            // error, just disconnect this connection
                            foreach (ClientInfo client in ClientList)
                            {
                                if (client.Socket == clientSocket)
                                {
                                    // remove this user from the client list
                                    ClientList.Remove(client);
                                    break;
                                }
                            }
                        }
                        else
                        {

                            // mmb - todo
                            // save data like online field, login time, etc

                            //Send the name of the users in the chat room... User has logged in.
                            // Send back a login packet to the user to tell them if user was validate against db
                            msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.Chat;
                            msgToSend.Data = "Player [" + user + "] is now online";
                            message = msgToSend.ToByte();
                            foreach (ClientInfo client in ClientList)
                            {
                                client.Socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), client.Socket);
                            }
                            
                            // Query DB to get Player's collection and send it back to that client
                            MTGCollection collection = MTGDB.GetPlayerCollection(user);
                            if (collection.Cards.Count > 0)
                            {
                                // send the collection data for this user to his client  
                                msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.ReceiveCollection;
                                msgToSend.Data = collection;
                                message = msgToSend.ToByte();
                                clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientSocket);
                            }
                        }                        

                        break;
                        
                    // *** Logout ***
                    case MTGNetworkPacket.MTGOpCode.Logout:

                        // mmb - todo
                        // save data like online field, logout time, etc

                        // send an acknowledgement logout message back to client
                        msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.Logout;
                        msgToSend.Data = "1";

                        message = msgToSend.ToByte();

                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSendAndClose), clientSocket);


                        //Send the name of the users in the chat room... User has logged in.
                        // Send back a login packet to the user to tell them if user was validate against db
                        msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.Chat;
                        
                        foreach (ClientInfo client in ClientList)
                        {
                            if (client.Socket == clientSocket)
                            {
                                User = "Player ["+ client.Player + "] ";
                            }
                        }
                        msgToSend.Data = "Player [" + User + "] is offline";
                        message = msgToSend.ToByte();


                        //When a user wants to log out of the server then we search for her 
                        //in the list of clients and close the corresponding connection
                        foreach (ClientInfo client in ClientList)
                        {
                            if (client.Socket == clientSocket)
                            {
                                // remove user from client list
                                ClientList.Remove(client);
                                break;
                            }
                            else
                            {
                                //Send the name of this logged out player to all the players in the chat room... 
                                client.Socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), client.Socket);
                            }
                        }

                        // break the network connection with the client
                        clientSocket.Close();

                        break;
                        
                    // *** Buy ***
                    case MTGNetworkPacket.MTGOpCode.Purchase:

                        // Determine what is purchased
                        String Purchase = msgReceived.Data.ToString();
                        MTGCollection Collection = DeterminePurchases(Purchase);

                        // send back the udpated collection with the purchased items in it
                        msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.PurchaseReceive;
                        msgToSend.Data = Collection;
                        message = msgToSend.ToByte();                        
                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientSocket);          

                        break;

                    // *** Chat ***
                    case MTGNetworkPacket.MTGOpCode.Chat:
                        
                        // First, get the string 
                        String ChatData = msgReceived.Data.ToString();

                        // now find out the player's name
                        foreach (ClientInfo client in ClientList)
                        {
                            if (client.Socket == clientSocket)
                            {
                                // this is the player, collect the name
                                User = "[" + client.Player + "] ";
                            }
                        }

                        // Check to see if this is a command
                        // examples:
                        // /who
                        // /help
                        // /commands
                        // /friends
                        if (ChatData.StartsWith("/"))
                        {
                            String Outbound = "UNKOWN";


                            switch (ChatData.Substring(1).ToUpper())
                            {
                                case "WHO":
                                    // list all the online players
                                    // mmb - TODO
                                    Outbound = "An unknown number of people are online";
                                    break;
                                default:
                                    Outbound = "Unknown Command";
                                    break;
                            }

                            // Create the outgoing chat packet
                            msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.Chat;
                            msgToSend.Data = Outbound;
                            message = msgToSend.ToByte();
                            clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientSocket);
                        }
                        else
                        {
                            // This is just a normal chat message, so send it to all the other online players

                            // Create the outgoing chat packet
                            msgToSend.OpCode = MTGNetworkPacket.MTGOpCode.Chat;
                            msgToSend.Data = User + ChatData;

                            message = msgToSend.ToByte();

                            foreach (ClientInfo client in ClientList)
                            {
                                // send chat message back to other players
                                // including the player who sent it
                                client.Socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), client.Socket);
                            }
                        }

                        break;

                    // *** Catchall...
                    default:
                        // record this because it shouldn't be happening
                        AddError(String.Format("OnReceive: Unknown Packet Recieved. {0}", msgReceived.OpCode));
                        break;
                }

                //If the user is logging out then we need not listen from her
                if (msgReceived.OpCode != MTGNetworkPacket.MTGOpCode.Logout)
                {
                    //Start listening to the message send by the user
                    clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
                }
            }
            catch (Exception ex)
            {
                AddError(String.Format("OnReceive: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Purchase"></param>
        /// <returns></returns>
        private MTGCollection DeterminePurchases(String Purchase)
        {
            MTGCollection Collection = new MTGCollection();

            // mmb - todo:

            // Info about Purchase String
            // Edition:TypeOfPurchase:Quanity
            // Edition Example: 10th is the only edition we have right now.
            // TypeOfPurchase Example: Foil, Preconstructed Theme Deck, Starter Deck, and/or Fat Pack
            // Quantity Example:  1, 2, 3, etc.  How many do they want to buy?


//Set 	                    Set symbol 	            Set code     	Release date 	    Size    Common 	Uncommon 	Rare 	Basic Land 	Other
//Core Set - Tenth Edition 	A Roman-numeral ten 	10E 	        July 14, 2007[13] 	383 	121 	121 	    121 	20 	        —
            


            //Tenth Edition  features 383 cards, including randomly inserted premium versions of all cards in the set. 
            //It is available in booster packs, preconstructed theme decks, fat packs, and two-player starter packs.

            // Magic the Gathering Tenth Edition Core Set includes: 
            // a Player's Guide with complete visual encyclopedia, 
            // two card boxes with panoramic art and six plastic dividers, 
            // six Tenth Edition booster packs, 
            // 40 card Tenth Edition basic land pack, 
            // Tenth Edition Spindown life counter, (available only in the fat pack), 
            // plus one random Pro Tour Player card. 
            
            // Magic The Gathering - 10th Edition Theme Deck Set Of 5 Includes
            // Each starter theme deck includes a pre-constructed, ready to play 40-card deck
            // A strategy insert, and a random Pro Player card.
            // Theme deck includes:
            // Kamahl's Temper (Red), Arcanis's Guile (Blue), Molimo's Might (Green) , Evincar's Tyranny (Black), Cho-Manno's Resolve (White) Theme Decks

            // Foil = 15 Cards.  1 Land, 1 Rare or Mythic Rare, 3 Uncommon, and 10 Common.

            /*
             * 	
Kamahl's Temper
Core Set - Tenth Edition Theme Deck
            
             * Patience is no virtue. With the pit fighter Kamahl and a horde of temper-challenged "haste" creatures at your command, 
             * you'll scorch your way to victory before your enemy even finds the snooze button. 
             * Attack and burn anything that stands in your way. Why wait?

#	Name	            Rarity	Cost
1	Raging Goblin	        C   Red Mana
1	Viashino Sandscout	    C	1 ManaRed Mana
2	Bloodrock Cyclops	    C	2 ManaRed Mana
2	Bogardan Firefiend	    C	2 ManaRed Mana
1	Prodigal Pyromancer	    C	2 ManaRed Mana
2	Lightning Elemental	    C	3 ManaRed Mana
1	Furnace Whelp	        U	2 ManaRed ManaRed Mana
2	Thundering Giant	    U	3 ManaRed ManaRed Mana
1	Kamahl, Pit Fighter	    R	4 ManaRed ManaRed Mana
1	Shock	                C	Red Mana
2	Incinerate	            C	1 ManaRed Mana
2	Spitting Earth	        C	1 ManaRed Mana
1	Threaten	            U   2 ManaRed Mana
1	Beacon of Destruction	R	3 ManaRed ManaRed Mana
1	Blaze	                U	X ManaRed Mana
1	Dragon's Claw	        U	2 Mana
1	Phyrexian Vault	        U	3 Mana
17	Mountain		
	Kamahl's Temper

* = from a previous set
             * */


            try
            {
                // Edition:TypeOfPurchase:Quantity
                Int32 FirstColon = Purchase.IndexOf(":");
                Int32 SecondColon = Purchase.IndexOf(":", FirstColon + 1);
                String Edition = Purchase.Substring(0, FirstColon);
                String TypeOfPurchase = Purchase.Substring(FirstColon + 1, SecondColon - FirstColon - 1);
                Int32 Quantity = Convert.ToInt32(Purchase.Substring(SecondColon + 1));

                // mmb - don't do anything about the Edition right now... Will always be 10E

                // loop for the quantity
                for (Int32 i = 0; i < Quantity; i++)
                {
                    // 
                    switch (TypeOfPurchase)
                    {
                        case "FOIL":

                            // Foil = 15 Cards.  1 Land, 1 Rare or Mythic Rare, 3 Uncommon, and 10 Common.

                            // mmb - make cards picks random!

                            // mmb - but for current testing use these...

                            // land x 1
                            Collection.Cards.Add(129559);

                            // commons x 10
                            Collection.Cards.Add(130522);
                            Collection.Cards.Add(135185);
                            Collection.Cards.Add(130985);
                            Collection.Cards.Add(132106);
                            Collection.Cards.Add(135194);
                            Collection.Cards.Add(129671);
                            Collection.Cards.Add(134758);
                            Collection.Cards.Add(135216);
                            Collection.Cards.Add(129579);
                            Collection.Cards.Add(129533);

                            // uncommon x 3
                            Collection.Cards.Add(135267);
                            Collection.Cards.Add(129459);
                            Collection.Cards.Add(129495);

                            // rare x 1
                            Collection.Cards.Add(106426);

                            break;
                        case "KAMAHLSTEMPER":

                            // Kamahl's Temper Red Theme Deck for 10th Edition

                            // lands x 17
                            for (i = 0; i < 17; i++)
                            {
                                Collection.Cards.Add(129650);
                            }

                            // commons x 14
                            Collection.Cards.Add(129688);
                            Collection.Cards.Add(130387);
                            Collection.Cards.Add(130384);
                            Collection.Cards.Add(130384);
                            Collection.Cards.Add(130534);
                            Collection.Cards.Add(130534);
                            Collection.Cards.Add(134752);
                            Collection.Cards.Add(129624);
                            Collection.Cards.Add(129624);
                            Collection.Cards.Add(129624);
                            Collection.Cards.Add(134751);
                            Collection.Cards.Add(134751);
                            Collection.Cards.Add(136509);
                            Collection.Cards.Add(136509);

                            // uncommon x 7
                            Collection.Cards.Add(130386);
                            Collection.Cards.Add(130381);
                            Collection.Cards.Add(130381);
                            Collection.Cards.Add(129767);
                            Collection.Cards.Add(129484);
                            Collection.Cards.Add(129527);
                            Collection.Cards.Add(135281);

                            // rare x 2
                            Collection.Cards.Add(106398);
                            Collection.Cards.Add(135262);

                            break;
                    }
                }

                // Add these purchases to the player's current collection
                // mmb - todo

                // if this is a preconstructed theme deck, then add it to the decks too
                // mmb - todo
            }
            catch (Exception ex)
            {
                AddError(String.Format("DeterminePurchases: {0}", ex.Message));
            }

            // send back the players newly updated collection
            return Collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        public void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (Exception ex)
            {
                AddError(String.Format("OnSend: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void OnSendAndClose(IAsyncResult ar)
        {
            try
            {
                // message was successfully sent
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
                client.Close();
            }
            catch (ObjectDisposedException exo)
            {
                AddError("OnSendAndClose:  Unable to send message to the server. ObjectDisposed.  [" + exo.Message + "]");
            }
            catch (Exception ex)
            {
                AddError("OnSendAndClose:  Unable to send message to the server. [" + ex.Message + "]");
            }
        }



        #region Unused Code - This code will eventually be deleted, but can't be just yet...
        /*
                                int nIndex = 0;
                        foreach (ClientInfo client in clientList)
                        {
                            if (client.socket == clientSocket)
                            {
                                clientList.RemoveAt(nIndex);
                                break;
                            }
                            ++nIndex;
                        }

        */
        #endregion
    }
}

