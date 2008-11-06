using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;



namespace MTGClient
{
    public partial class MTGClientForm : Form
    {
        // Debug vars
        Boolean Debug = true;

        // Class vars
        Boolean Connected = false;

        // Network vars
        public Socket clientSocket; //The main client socket
        private byte[] byteData;
        Int32 _port = 4545;
        String _ip = "127.0.0.1";


        /// <summary>
        /// 
        /// </summary>
        public MTGClientForm()
        {
            InitializeComponent();

            SetupDataGridView();
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

                    if (!Connected)
                    {
                        ConnectToServer();
                    }

                    while (!Connected)
                    {
                        // wait for the timeout... // mmb - fix this!
                    }

                    if (Connected)
                    {
                        //Fill the info for the message to be send
                        MTG.MTGNetworkPacket packet = new MTG.MTGNetworkPacket();
                        String data = String.Format(textBoxUser.Text + ":" + textBoxPassword.Text);

                        packet.OpCode = MTG.MTGNetworkPacket.MTGOpCode.Login;
                        packet.Data = data;

                        byte[] ConvertedData = packet.ToByte();

                        //Send it to the server
                        clientSocket.BeginSend(ConvertedData, 0, ConvertedData.Length, SocketFlags.None, new AsyncCallback(OnSendAndWait), null);
                        AddStatus("Client is sending a LOGIN message to server.");
                    }
                }
                else if (buttonLogin.Text == "Cancel")
                {
                    // cancel logging into server
                }
                else if (buttonLogin.Text == "Logout")
                {
                    // log out of server
                    if (Connected)
                    {
                        //Fill the info for the message to be send
                        MTG.MTGNetworkPacket packet = new MTG.MTGNetworkPacket();

                        packet.OpCode = MTG.MTGNetworkPacket.MTGOpCode.Logout;
                        //packet.Data = null;

                        byte[] ConvertedData = packet.ToByte();

                        //Send it to the server
                        clientSocket.BeginSend(ConvertedData, 0, ConvertedData.Length, SocketFlags.None, new AsyncCallback(OnSendAndClose), null);
                        AddStatus("Client is sending a LOGOUT message to server.");
                    }
                }
            }
            catch (Exception ex)
            {
                AddError("buttonLogin_Click:  Unable to send message to the server. [" + ex.Message + "]");
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
                AddError("ConnectToServer: [" + ex.Message + "]");
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
                //EnableLoginButton(false);
                SetLoginButtonText("Logout");

                AddStatus("Client is connected to server.");
            }
            catch (Exception ex)
            {
                AddError("OnConnect: [" + ex.Message + "]");
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
                AddStatus("Client has successfully sent a message to the server.");

                // message was successfully sent
                clientSocket.EndSend(ar);

                // now wait for a reply
                 byteData = new byte[1024];
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);

            }
            catch (ObjectDisposedException)
            { 
                //mmb - what to do here?
            }
            catch (Exception ex)
            {
                AddError("OnSendAndWait:  Unable to send message to the server. [" + ex.Message + "]");
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
                AddStatus("Client has successfully sent a message to the server.");
                AddStatus("Client has logged out!");

                // message was successfully sent
                clientSocket.EndSend(ar);
                clientSocket.Close();

                SetLoginButtonText("Login");
                Connected = false;
            }
            catch (ObjectDisposedException)
            {
                //mmb - what to do here?
            }
            catch (Exception ex)
            {
                AddError("OnSendAndWait:  Unable to send message to the server. [" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                AddStatus("Client has received a message from the server.");

                clientSocket.EndReceive(ar);

                MTG.MTGNetworkPacket packet = new MTG.MTGNetworkPacket(byteData);

                //Accordingly process the message received
                switch (packet.OpCode)
                {
                    case MTG.MTGNetworkPacket.MTGOpCode.Login:
                        AddStatus("Login: [" + packet.Data.ToString() + "]");
                        break;

                    case MTG.MTGNetworkPacket.MTGOpCode.Logout:
                        AddStatus("Logout: [" + packet.Data.ToString() + "]");
                        break;

                    case MTG.MTGNetworkPacket.MTGOpCode.PurchaseReceive:
                        AddStatus("Purchase: [" + packet.Data.ToString() + "]");
                        ArrayList List = (ArrayList)packet.Data;
                        AddToCollection(List);
                        break;
                }

                //mmb - and wait for more data?
                //byteData = new byte[1024];
                //clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
            catch (ObjectDisposedException)
            { 
                //mmb
            }
            catch (Exception ex)
            {
                AddError("OnReceive:  Unable to receive message to the server. [" + ex.Message + "]");
            }
        }

        #region Status and Error Handling

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        void AddError(String Message)
        {
            if (Debug)
            {
                AddToResults(Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        void AddStatus(String Message)
        {
            if (Debug)
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
            }
        }

        #endregion
        
        #region  MultiThreaded Handling

        delegate void SetLoginButtonCallback(Boolean Enable);
        delegate void SetLoginButtonTextCallback(String Text);
        delegate void AddToCollectionCallback(ArrayList List);

        public void EnableLoginButton(Boolean Enable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.buttonLogin.InvokeRequired)
            {
                SetLoginButtonCallback d = new SetLoginButtonCallback(EnableLoginButton);
                this.Invoke(d, new object[] { Enable });
            }
            else
            {
                // enable or disable the login button
                buttonLogin.Enabled = Enable;
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

        public void AddToCollection(ArrayList List)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewCollection.InvokeRequired)
            {
                AddToCollectionCallback d = new AddToCollectionCallback(AddToCollection);
                this.Invoke(d, new object[] { List });
            }
            else
            {
                // mmb - add to the current collection, don't overwrite
                dataGridViewCollection.DataSource = List;
                UpdateGrid();
            }
        }

        #endregion

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
                MTG.MTGNetworkPacket packet = new MTG.MTGNetworkPacket();
                String data = "1";

                packet.OpCode = MTG.MTGNetworkPacket.MTGOpCode.Purchase;
                packet.Data = data;

                byte[] ConvertedData = packet.ToByte();

                //Send it to the server
                clientSocket.BeginSend(ConvertedData, 0, ConvertedData.Length, SocketFlags.None, new AsyncCallback(OnSendAndWait), null);
                AddStatus("Client is sending a PURCHASE message to server.");
            }
        }
    }
}
