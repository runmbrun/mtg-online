using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTGServer;



namespace TestMTGServer
{
    public partial class MTGServiceTesterForm : Form
    {
        /// <summary>
        /// Vars
        /// </summary>
        MTGService _service;
        Boolean _ServiceIsRunning;

        Boolean ServiceIsRunning
        {
            get { return _ServiceIsRunning; }
            set { _ServiceIsRunning = value; }
        }

        /// <summary>
        /// Initialize the Form
        /// </summary>
        public MTGServiceTesterForm()
        {
            InitializeComponent();

            buttonStop.Enabled = false;
            ServiceIsRunning = false;

            _service = new MTGService();

            // check for an auto start parameter
            String[] Parameters = Environment.GetCommandLineArgs();
            if (Parameters.Length == 2 && Parameters[1].ToUpper() == "AUTO")
            {
                // automatically start the service
                StartService();

                // minimize the form
                this.WindowState = FormWindowState.Minimized;                
            }
        }

        /// <summary>
        /// This button will start the service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!ServiceIsRunning)
            {
                StartService();
            }
        }

        /// <summary>
        /// Start the Service
        /// </summary>
        private void StartService()
        {
            string[] astrDummy = new string[] { "" };
            textBoxStatus.Text = "Starting";

            astrDummy.Initialize();

            _service.TestOnStart(astrDummy);
            ServiceIsRunning = true;
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;

            textBoxStatus.Text = "Started";
        }

        /// <summary>
        /// This button will stop the service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (ServiceIsRunning)
            {
                textBoxStatus.Text = "Stopping";

                _service.TestOnStop();
                ServiceIsRunning = false;
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;

                textBoxStatus.Text = "Stopped";
            }
        }
    }
}
