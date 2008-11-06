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
using MySql.Data.MySqlClient;
using System.Timers;
using System.Reflection;
using System.Threading;
using System.Net;
using System.Net.Sockets;

#endregion



namespace MTGServer
{
    public partial class MTGService : ServiceBase
    {

        #region Private Variables

        // Debug vars
        public Boolean Debug = false;

        // Threading vars
        //private static Thread threadFirst;

        // DB vars
        private static MySqlConnection m_Connection;
        private String m_strDBServer = "dbserver";
        private String m_strDBUser = "user";
        private String m_strDBPassword = "password";
        private String m_strDBName = "db";

        // Error Handling vars
        private String _servicePath;
        private String[] _errorMessages;
        private Int32 _errorCount;

        // Timer vars
        private static System.Timers.Timer m_Timer;
        private Boolean m_bReadyToCheck;
        private Int32 _timerInterval;
        private Object thisLock = new Object();

        // Network vars
        private Int32 _port = 4545;
        byte[] byteData = new byte[1024];

        //The ClientInfo structure holds 
        //the required information about every
        //client connected to the server
        struct ClientInfo
        {
            //Socket of the client
            public Socket Socket;
            
            //Name by which the user logged into the chat room
            //public String Name;
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

            // start the Event Logger
            if (!System.Diagnostics.EventLog.SourceExists("MTG Service"))
            {
                System.Diagnostics.EventLog.CreateEventSource("MTG Service", "MTG Service Log");
            }

            //this.EventLog.Source
            eventLog1.Source = "MTG Service";
            eventLog1.Log = "MTG Service Log";


            // this is the string array of all the errors found during processing
            _errorMessages = new String[128];
            _errorMessages.Initialize();
            _errorCount = 0;

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

            _timerInterval = 0;

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
            eventLog1.WriteEntry("MTG Service is now Starting");

            // initialize this variable so that we can begin checking
            // the database for requests that are ready to be built
            //m_bReadyToCheck = true;

            // First run the process on a separate thread once 
            // to clear out anything that is currently in the queue
            // This will prevent the Starting of this service to take too long
            //threadFirst = new Thread(new ThreadStart(BeginProcess));
            //threadFirst.Start();

            // now start the timer so that it will continually process
            //StartTimer();

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
        }

        /// <summary>
        /// This is the function that is called when this app is run as a service.
        /// This is the Stop method of the Service.
        /// </summary>
        protected override void OnStop()
        {
            eventLog1.WriteEntry("MTG Service is Stopping");

            /*
            // stop the timer so we don't kick off the build portion any more
            m_Timer.Stop();

            // the number of milliseconds to sleep before checking to see if 
            // the app is still running
            Int32 iSecondsToSleep = 3000;
            
            // check to see if the first Thread is still running
            while (threadFirst.ThreadState == System.Threading.ThreadState.Running)
            {
                System.Threading.Thread.Sleep(iSecondsToSleep);
            }

            // now wait for the any current builds to complete before stopping
            while (!m_bReadyToCheck)
            {
                // sleep here for 3 seconds and then check if build is completed again
                System.Threading.Thread.Sleep(iSecondsToSleep);
            }

            // make sure that timer will not start again
            m_bReadyToCheck = false;
*/
            // close the database connection if it's open
            if (m_Connection != null)
            {
                m_Connection.Close();
            }

            // close all the network clients talking to this server
            if (ClientList != null)
            {
                foreach (ClientInfo client in ClientList)
                {
                    client.Socket.Close();
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
        /// This function attempts to connect to the mysql Database
        /// </summary>
        public bool DBConnect()
        {
            bool bConnected = false;


            if (m_Connection != null)
            {
                m_Connection.Close();
            }

            string strConn = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=true;Connection Lifetime=60;Connection Reset=true;Max Pool Size=4",
                m_strDBServer, m_strDBUser, m_strDBPassword, m_strDBName);

            try
            {
                m_Connection = new MySqlConnection(strConn);
                m_Connection.Open();

                bConnected = true;
            }
            catch (MySqlException ex)
            {
                AddError(String.Format("Error connecting to the server: {0}", ex.Message));
                AddError(String.Format("  Connection String= {0}", strConn));
                eventLog1.WriteEntry("Build MTGServer was not able to connect to the database");
            }

            return bConnected;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartTimer()
        {
            // Normally, the timer is declared at the class level, so
            // that it doesn't go out of scope when the method ends.
            // is executing. However, KeepAlive must be used at the
            // end of Main, to prevent the JIT compiler from allowing 
            // aggressive garbage collection to occur before Main 
            // ends.
            m_Timer = new System.Timers.Timer();

            // Hook up the Elapsed event for the timer.
            m_Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to the number of seconds in the config file (X * 1000 milliseconds).            
            m_Timer.Interval = _timerInterval;
            m_Timer.Enabled = true;

            // Keep the timer alive until the end of Main.
            GC.KeepAlive(m_Timer);
        }

        /// <summary>
        /// Specify what you want to happen when the Elapsed event is raised.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //BeginProcess();

            // thread this?
            //threadFirst.Start();
        }

        /// <summary>
        /// Here is the actual processing
        /// </summary>
        private void BeginProcess()
        {
            if (m_bReadyToCheck)
            {
                lock (thisLock)
                {
                    m_bReadyToCheck = false;

                    // what should be done if there are errors?
                    // options are to keep on going, or stop everything until the problem is fixed
                    // Right now I think we'll just keep on going to see what type of errors we run into
                    m_bReadyToCheck = true;
                }
            }
        }

        /// <summary>
        /// Adds an error string to the errors string array
        /// </summary>
        /// <param name="strErrorMessage"></param>
        private void AddError(String ErrorMessage)
        {
            _errorMessages[_errorCount++] = ErrorMessage;
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

                
                //Transform the array of bytes received from the user into an
                //intelligent form of object Data
                MTG.MTGNetworkPacket msgReceived = new MTG.MTGNetworkPacket(byteData);
                MTG.MTGNetworkPacket msgToSend = new MTG.MTGNetworkPacket();
                                
                
                //If the message is to login, logout, or simple text message
                //then when send to others the type of the message remains the same
                switch (msgReceived.OpCode)
                {
                        // Login
                    case MTG.MTGNetworkPacket.MTGOpCode.Login:

                        String result = "Success";

                        // verify that this user can log on
                        // mmb - db stuff                        
                        String data = msgReceived.Data.ToString();
                        String user = data.Substring(0, data.IndexOf(":"));
                        String pass = data.Substring(data.IndexOf(":") + 1);

                        if (user == "admin" && pass == "pass")
                        {
                            result += "Admin";
                        }
                        else if (user != "test" && pass != "pass")
                        {
                            // invalid login
                            result = "Failure";
                        }

                        //When a user logs in to the server then we add them to our list of clients
                        clientInfo = new ClientInfo();
                        clientInfo.Socket= clientSocket;
                        ClientList.Add(clientInfo);                        

                        //
                        msgToSend.OpCode = MTG.MTGNetworkPacket.MTGOpCode.Login;
                        msgToSend.Data = result;

                        message = msgToSend.ToByte();

                        //Send the name of the users in the chat room
                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientSocket);          

                        break;

                        // Logout
                    case MTG.MTGNetworkPacket.MTGOpCode.Logout:

                        //When a user wants to log out of the server then we search for her 
                        //in the list of clients and close the corresponding connection
                        foreach (ClientInfo client in ClientList)
                        {
                            if (client.Socket == clientSocket)
                            {
                                // mmb - do something...
                                ClientList.Remove(client);
                                break;
                            }
                        }

                        clientSocket.Close();

                        //mmb - logout

                        break;

                        // Buy
                    case MTG.MTGNetworkPacket.MTGOpCode.Purchase:

                        Int32 Number = Convert.ToInt32(msgReceived.Data.ToString());
                        ArrayList CardPack = new ArrayList();

                        // mmb - can buy multiple card packs
                        for (Int32 i = 0; i < Number; i++)
                        {
                            MTG.MTGCard card = new MTG.MTGCard();
                            card.ID = 1;
                            card.Name = "Test Card";
                            card.Power = "1";
                            card.Toughness = "1";
                            card.Type = "Creature";
                            CardPack.Add(card);
                        }

                        //
                        msgToSend.OpCode = MTG.MTGNetworkPacket.MTGOpCode.PurchaseReceive;
                        msgToSend.Data = CardPack;

                        message = msgToSend.ToByte();

                        //Send the name of the users in the chat room
                        clientSocket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(OnSend), clientSocket);          

                        break;
                }

                //If the user is logging out then we need not listen from her
                if (msgReceived.OpCode != MTG.MTGNetworkPacket.MTGOpCode.Logout)
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

