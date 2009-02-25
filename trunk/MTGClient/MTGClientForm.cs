﻿#region Includes
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MTG;
#endregion



namespace MTGClient
{
    public partial class MTGClientForm : Form
    {
        // Debug vars
        Int32 Debug = 1;

        // Class vars
        Boolean Connected = false;
        Boolean Receiving = false;

        // Network vars
        public Socket clientSocket; //The main client socket
        private byte[] byteData;
        Int32 _port = 4545;
        String _ip = "127.0.0.1";
        Int32 PacketSize = 1024;
        private byte[] WaitingData;
        private Int32 WaitingForData = 1;        

        // Card Sets
        ArrayList CardSets;
        MTGCollection Collection;


        /// <summary>
        /// 
        /// </summary>
        public MTGClientForm()
        {
            InitializeComponent();

            // init the collection data
            Collection = new MTGCollection();

            // setup the collection data grid view
            SetupDataGridView();            

            // Welcome the player
            UpdateStatusStrip("Welcome to Magic the Gathering Online!");

            // Disable all the extra tabs until someone is logged in
            EnableTabPages(false);

            // Load the picture data set            
            CardSets = new ArrayList();
            try
            {                
                // Attempt to load the last cardset saved
                Stream stream = File.Open("10.dat", FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                MTGCardSet cards = new MTGCardSet();
                cards = (MTGCardSet)bformatter.Deserialize(stream);
                stream.Close();

                CardSets.Add(cards);
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    LogError(String.Format("  **ERROR: ", ex.Message));
                }
            }
            
            WaitingData = new byte[PacketSize];
        }

        #region DataGridVew Functions

        /// <summary>
        /// Setup the DataGridView table
        /// </summary>
        private void SetupDataGridView()
        {
            dataGridViewCollection.ColumnCount = 0;

            dataGridViewCollection.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridViewCollection.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewCollection.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewCollection.Font, FontStyle.Bold);

            dataGridViewCollection.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridViewCollection.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCollection.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewCollection.GridColor = Color.Black;
            dataGridViewCollection.RowHeadersVisible = false;

            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dataGridViewCollection.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewCollection.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            dataGridViewCollection.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCollection.MultiSelect = true;
            dataGridViewCollection.ReadOnly = true;
            dataGridViewCollection.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCollection.AutoResizeColumns();
        }        

        /// <summary>
        /// 
        /// </summary>
        private void UpdateGrid()
        {
            dataGridViewCollection.Columns[0].Visible = false;
            dataGridViewCollection.Columns[6].Visible = false;
            dataGridViewCollection.Columns[7].Visible = false;
            dataGridViewCollection.Columns[8].Visible = false;
            dataGridViewCollection.Columns[9].Visible = false;
            dataGridViewCollection.Columns[10].Visible = false;
            dataGridViewCollection.AutoResizeColumns();
            dataGridViewCollection.AutoResizeRows();
            dataGridViewCollection.Refresh();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonLogin.Text == "Login")
                {
                    // login to server
                    SetLoginButtonText("Cancel");
                    EnableUserTextBox(false);
                    EnablePasswordTextBox(false);

                    if (!Connected)
                    {
                        UpdateStatusStrip("Connecting to the MTG Server...");
                        ConnectToServer();
                    }

                    while (!Connected)
                    {
                        // wait for the timeout... // mmb - fix this!
                    }

                    if (Connected)
                    {
                        UpdateStatusStrip("Connected to the MTG Server Successfully.");

                        //Fill the info for the message to be send
                        MTGNetworkPacket packet = new MTGNetworkPacket();
                        String data = String.Format(textBoxUser.Text + ":" + textBoxPassword.Text);

                        packet.OpCode = MTGNetworkPacket.MTGOpCode.Login;
                        packet.Data = data;

                        byte[] ConvertedData = packet.ToByte();

                        //Send it to the server
                        clientSocket.BeginSend(ConvertedData, 0, ConvertedData.Length, SocketFlags.None, new AsyncCallback(OnSendAndWait), null);
                        LogDebug("Client is sending a LOGIN message to server.");

                        UpdateStatusStrip("Logging on to the MTG Server...");
                    }
                }
                else if (buttonLogin.Text == "Cancel")
                {
                    // cancel logging into server
                    EnableUserTextBox(true);
                    EnablePasswordTextBox(true);
                }
                else if (buttonLogin.Text == "Logout")
                {
                    // log out of server
                    if (Connected)
                    {
                        //Fill the info for the message to be send
                        MTGNetworkPacket packet = new MTGNetworkPacket();

                        packet.OpCode = MTGNetworkPacket.MTGOpCode.Logout;

                        byte[] ConvertedData = packet.ToByte();

                        //Send it to the server
                        clientSocket.BeginSend(ConvertedData, 0, ConvertedData.Length, SocketFlags.None, new AsyncCallback(OnSendAndClose), null);
                        LogDebug("Client is sending a LOGOUT message to server.");

                        UpdateStatusStrip("Logging off the MTG Server...");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("buttonLogin_Click:  Unable to send message to the server. [" + ex.Message + "]");
            }  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ConnectToServer()
        {
            Boolean Connecting = true;

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAddress = IPAddress.Parse(_ip);

                //Server is listening on specific port
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, _port);

                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
            }
            catch (Exception ex)
            {
                LogError("ConnectToServer: [" + ex.Message + "]");
                Connecting = false;
            }

            return Connecting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);

                //We are connected so we login into the server
                Connected = true;
                LogDebug("Client is connected to server.");
            }
            catch (Exception ex)
            {
                LogError("OnConnect: [" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void OnSendAndWait(IAsyncResult ar)
        {
            try
            {
                LogDebug("Client has successfully sent a message to the server.");

                // message was successfully sent
                clientSocket.EndSend(ar);

                // clear out the waiting data vars
                WaitingData = new byte[PacketSize];
                WaitingForData = 1;

                if (!Receiving)
                {
                    // now wait for a reply
                    byteData = new byte[PacketSize];
                    clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);

                    Receiving = true;
                }

            }
            catch (ObjectDisposedException exo)
            { 
                LogError("OnSendAndWait:  Unable to send message to the server. ObjectDisposed. [" + exo.Message + "]");
            }
            catch (Exception ex)
            {
                LogError("OnSendAndWait:  Unable to send message to the server. [" + ex.Message + "]");
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
                LogDebug("Client has successfully sent a message to the server.");
                LogDebug("Client has logged out!");

                // message was successfully sent
                clientSocket.EndSend(ar);
                clientSocket.Close();

                SetLoginButtonText("Login");
                EnableUserTextBox(true);
                EnablePasswordTextBox(true);

                EnableTabPages(false);

                Connected = false;

                UpdateStatusStrip("Successfully Logged off the MTG Server");
            }
            catch (ObjectDisposedException)
            {
                //mmb - what to do here?
            }
            catch (Exception ex)
            {
                LogError("OnSendAndWait:  Unable to send message to the server. [" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceive(IAsyncResult ar)
        {
            Int32 size = 0;


            try
            {
                LogDebug("Client has received a message from the server.");

                if (ar.IsCompleted)
                {
                    size = clientSocket.EndReceive(ar);


                    if (size < PacketSize)
                    {
                        if (WaitingForData > 1)
                        {
                            byte[] temp = new byte[PacketSize * WaitingForData];
                            WaitingData.CopyTo(temp, 0);
                            byteData.CopyTo(temp, WaitingData.Length);
                            WaitingData = new byte[PacketSize * WaitingForData];
                            WaitingData = temp;
                        }
                        else
                        {
                            WaitingForData = 1;
                            byteData.CopyTo(WaitingData, 0);
                        }

                        MTGNetworkPacket packet = new MTGNetworkPacket(WaitingData);

                        //Accordingly process the message received
                        switch (packet.OpCode)
                        {
                            case MTGNetworkPacket.MTGOpCode.Login:
                                String result = packet.Data.ToString();
                                //AddStatus("Login: [" + result + "]");                                

                                // was login successful?
                                if (result != "0")
                                {
                                    // if so, then change login button, make edit boxes uneditable, etc
                                    SetLoginButtonText("Logout");
                                    EnableUserTextBox(false);
                                    EnablePasswordTextBox(false);

                                    LogInfo("Login successful!  Welcome to the MTG Server!");
                                    UpdateStatusStrip("Successfully Logged on to the MTG Server");

                                    EnableTabPages(true);
                                }
                                else
                                {
                                    // if not, then disconnect from server
                                    UpdateStatusStrip("Failed to Log on to the MTG Server");

                                    // message was successfully sent
                                    clientSocket.Close();

                                    SetLoginButtonText("Login");
                                    EnableUserTextBox(true);
                                    EnablePasswordTextBox(true);

                                    Connected = false;
                                    LogInfo("Log on has failed!  Disconnected");
                                }
                                break;

                            case MTGNetworkPacket.MTGOpCode.Logout:

                                // mmb - this won't happen!

                                LogDebug("Logout: [" + packet.Data.ToString() + "]");
                                UpdateStatusStrip("Successfully Logged off the MTG Server");

                                EnableTabPages(false);

                                break;

                            case MTGNetworkPacket.MTGOpCode.PurchaseReceive:
                                LogDebug("PurchaseReceive: [" + packet.Data.ToString() + "]");
                                //ArrayList List = (ArrayList)packet.Data;
                                MTGCollection NewCards = (MTGCollection)packet.Data;
                                UpdateStatusStrip("New Foil of cards bought.");
                                AddToCollection(NewCards);
                                UpdateStatusStrip("New Foil of cards added to collection.");
                                break;

                            case MTGNetworkPacket.MTGOpCode.ReceiveCollection:
                                LogDebug("ReceiveCollection: [" + packet.Data.ToString() + "]");
                                Collection.Clear();
                                Collection = (MTGCollection)packet.Data;
                                UpdateStatusStrip("Player Collection has been received.");
                                break;

                            case MTGNetworkPacket.MTGOpCode.Chat:
                                Log(packet.Data.ToString());
                                break;
                        }

                        WaitingData = new byte[PacketSize];
                    }
                    else
                    {
                        // wait for more data?
                        LogDebug("OnReceive:  Waiting for more data from server...");
                                                
                        byte[] temp = new byte[PacketSize * WaitingForData];
                        WaitingData.CopyTo(temp, 0);
                        byteData.CopyTo(temp, (PacketSize * (WaitingForData - 1)));
                        WaitingData = new byte[PacketSize * WaitingForData];
                        WaitingData = temp;
                        WaitingForData++;
                    }
                }
            }
            catch (ObjectDisposedException exo)
            {
                String Error = "OnReceive:  Unable to receive. ObjectDisposed. [" + exo.Message + "]";
                //AddError(Error);
                LogError("Connection to the server was terminated.");
            }
            catch (Exception ex)
            {
                LogError("OnReceive:  Unable to receive. [" + ex.Message + "]");
            }

            if (size != 0)
            {
                // now wait for a reply
                byteData = new byte[PacketSize];
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NewCards"></param>
        private void AddToCollection(MTGCollection NewCards)
        {
            Collection.Add(NewCards);
            ArrayList Display = new ArrayList();
            foreach (Int32 cardnumber in Collection.Cards)
            {
                MTGCard card = new MTGCard();                

                foreach (MTGCard cardInfo in ((MTGCardSet)CardSets[0]).CardSet)
                {
                    if (cardInfo.ID == cardnumber)
                    {
                        card = cardInfo;
                        Display.Add(card);
                        break;
                    }
                }
            }
            UpdateCollection(Display);
        }

        #region Logging, Status and Error Handling

        // there are 3 types of logging...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        void Log(String Message)
        {
            
            AddToResults(Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Message"></param>
        void Log(Int32 Type, String Message)
        {
            if (Debug >= Type)
            {
                AddToResults(Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        void LogError(String Message)
        {
            if (Debug >= 1)
            {
                AddToResults(Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        void LogInfo(String Message)
        {
            if (Debug >= 2)
            {
                AddToResults(Message);
            }
        }

        void LogDebug(String Message)
        {
            if (Debug >= 3)
            {
                AddToResults(Message);
            }
        }

        delegate void SetResultsCallback(String Information);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void AddToResults(String Message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBoxResults.InvokeRequired)
            {
                SetResultsCallback d = new SetResultsCallback(AddToResults);
                this.Invoke(d, new object[] { Message });
            }
            else
            {
                this.listBoxResults.Items.Add(Message);
                this.listBoxResults.SelectedIndex = this.listBoxResults.Items.Count - 1;
            }
        }

        #endregion
        
        #region  MultiThreaded Handling

        delegate void EnableLoginButtonCallback(Boolean Enable);
        delegate void EnableTabPagesCallback(Boolean Enable);
        delegate void EnableUserTextboxCallback(Boolean Enable);
        delegate void EnablePasswordTextboxCallback(Boolean Enable);
        delegate void SetLoginButtonTextCallback(String Text);
        delegate void UpdateStatusStripCallback(String Text);
        delegate void UpdateCollectionCallback(ArrayList NewCards);

        public void EnableLoginButton(Boolean Enable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.buttonLogin.InvokeRequired)
            {
                EnableLoginButtonCallback d = new EnableLoginButtonCallback(EnableLoginButton);
                this.Invoke(d, new object[] { Enable });
            }
            else
            {
                // enable or disable the login button
                buttonLogin.Enabled = Enable;
            }
        }

        private void EnableTabPages(Boolean Enable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.tabControl1.InvokeRequired)
            {
                EnableTabPagesCallback d = new EnableTabPagesCallback(EnableTabPages);
                this.Invoke(d, new object[] { Enable });
            }
            else
            {
                // enable or disable the tab pages
                if (Enable)
                {
                    tabControl1.TabPages.Add(tabPageCollection);
                    tabControl1.TabPages.Add(tabPageStore);
                    tabControl1.TabPages.Add(tabPageLobby);
                }
                else
                {
                    tabControl1.TabPages.Remove(tabPageCollection);
                    tabControl1.TabPages.Remove(tabPageStore);
                    tabControl1.TabPages.Remove(tabPageLobby);
                }
            }            
        }

        public void EnableUserTextBox(Boolean Enable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBoxUser.InvokeRequired)
            {
                EnableUserTextboxCallback d = new EnableUserTextboxCallback(EnableUserTextBox);
                this.Invoke(d, new object[] { Enable });
            }
            else
            {
                // enable or disable the login button
                textBoxUser.Enabled = Enable;
            }
        }

        public void EnablePasswordTextBox(Boolean Enable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBoxPassword.InvokeRequired)
            {
                EnablePasswordTextboxCallback d = new EnablePasswordTextboxCallback(EnablePasswordTextBox);
                this.Invoke(d, new object[] { Enable });
            }
            else
            {
                // enable or disable the login button
                textBoxPassword.Enabled = Enable;
            }
        }

        public void SetLoginButtonText(String Text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.buttonLogin.InvokeRequired)
            {
                SetLoginButtonTextCallback d = new SetLoginButtonTextCallback(SetLoginButtonText);
                this.Invoke(d, new object[] { Text });
            }
            else
            {
                // enable or disable the login button
                buttonLogin.Text = Text;
            }
        }

        public void UpdateStatusStrip(String Text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusStrip1.InvokeRequired)
            {
                UpdateStatusStripCallback d = new UpdateStatusStripCallback(UpdateStatusStrip);
                this.Invoke(d, new object[] { Text });
            }
            else
            {
                // enable or disable the login button
                toolStripStatusLabel1.Text = Text;
            }
        }

        public void UpdateCollection(ArrayList NewCards)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewCollection.InvokeRequired)
            {
                UpdateCollectionCallback d = new UpdateCollectionCallback(UpdateCollection);
                this.Invoke(d, new object[] { NewCards });
            }
            else
            {
                // mmb - add to the current collection, don't overwrite
                dataGridViewCollection.DataSource = NewCards;
                UpdateGrid();
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBuy_Click(object sender, EventArgs e)
        {
            //dataGridViewcollection

            if (!Connected)
            {
                // error!
            }
            else
            {
                //Fill the info for the message to be send
                MTGNetworkPacket packet = new MTGNetworkPacket();
                String data = "1";

                packet.OpCode = MTGNetworkPacket.MTGOpCode.Purchase;
                packet.Data = data;

                byte[] ConvertedData = packet.ToByte();

                //Send it to the server
                clientSocket.BeginSend(ConvertedData, 0, ConvertedData.Length, SocketFlags.None, new AsyncCallback(OnSendAndWait), null);
                
                LogInfo("Client is sending a PURCHASE message to server.");
                UpdateStatusStrip("Attempting to buy a foil of cards...");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCollection_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCollection.SelectedRows.Count > 0)
            {
                Int32 id = (Int32)dataGridViewCollection.SelectedRows[0].Cells[0].Value;
                foreach (MTGCard card in ((MTGCardSet)CardSets[0]).CardSet)
                {
                    if (card.ID == id)
                    {
                        pictureBoxCardImage.BackgroundImage = card.Pic;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChat_Click(object sender, EventArgs e)
        {
            // send some chat text to the server to be sent to all the online users
            
            //Fill the info for the message to be send
            MTGNetworkPacket packet = new MTGNetworkPacket();
            String data = String.Format(textBoxChat.Text);

            packet.OpCode = MTGNetworkPacket.MTGOpCode.Chat;
            packet.Data = data;

            byte[] ConvertedData = packet.ToByte();

            //Send it to the server
            clientSocket.BeginSend(ConvertedData, 0, ConvertedData.Length, SocketFlags.None, new AsyncCallback(OnSendAndWait), null);
            LogInfo("Client is sending a CHAT message to the server.");

            UpdateStatusStrip("Logging off the MTG Server...");
        }
    }
}
