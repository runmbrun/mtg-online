using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Net;
using System.Reflection;
using MTGCommon;



namespace MTGDataGatherer
{
    public partial class Form1 : Form
    {
        #region Variables
        /// <summary>
        /// Member Variables
        /// </summary>

        public static String OutputFile = @"output.htm";
        public static Boolean OutputResults = false;
        
        // define how many pages to fetch in parallel
        Int32 MaxPagesToFetchInParallel = 1;

        private BackgroundWorker FetchAsyncWorker = new BackgroundWorker();

        // timer start
        DateTime Start;

        // temporary storage
        public static MTG.MTGCardSet TempSet;
        public static ArrayList TempIDs;

        #endregion

        #region enums
        enum MTGEditions
        {
            FourthE,
            FifthE,
            TenthE
        }
        #endregion

        public struct TempData
        {
            public String _webpage;
            public String _mtgedition;
        }

        // delegate function for the threading
        delegate ArrayList FetchPage(Object param);

        /// <summary>
        /// This is used to make the progress bars non-block like
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="appname"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hwnd, string appname, string idlist);

        

        /// <summary>
        /// Initialize the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // init config vars
            textBoxFetchPagesInParallel.Text = Properties.Settings.Default.PagesToFetchInParallel;

            // setup the datagridview results panel
            SetupDataGridView();

            // init temp storage
            TempSet = new MTG.MTGCardSet();
            TempIDs = new ArrayList();
            LoadTempData();

            // setup the link in the settings tab... mmb - is this needed?
            linkLabelDefault.Links.Add(0, 33, "http://ww2.wizards.com/gatherer/");

            // choose which magic color to view
            // mmb - is this needed any more?

            // choose which Card Sets to fetch
            comboBoxFetchMTGEdition.Items.Add(MTGEditions.FourthE.ToString());
            comboBoxFetchMTGEdition.Items.Add(MTGEditions.FifthE.ToString());
            comboBoxFetchMTGEdition.Items.Add(MTGEditions.TenthE.ToString());
            comboBoxFetchMTGEdition.SelectedIndex = 0; // mmb - debugging only

            // Create a background worker thread that fetches data from the web site asnyc'ly and Reports Progress and Supports Cancellation            
            FetchAsyncWorker.WorkerReportsProgress = true;
            FetchAsyncWorker.WorkerSupportsCancellation = true;
            FetchAsyncWorker.ProgressChanged += new ProgressChangedEventHandler(FetchAsync_ProgressChanged);
            // can one of these be made for all controls so invoke won't have to be done?
            FetchAsyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FetchAsync_RunWorkerCompleted);
            FetchAsyncWorker.DoWork += new DoWorkEventHandler(FetchAsync_DoWork);

            // Silently suppress P/Invoke errors if not running on XP/Vista...
            try 
            {
                SetWindowTheme(progressBar1.Handle, "", "");
            }
            catch { }
        }

        /// <summary>
        /// Setup the DataGridView table
        /// </summary>
        private void SetupDataGridView()
        {
            dataGridViewResults.ColumnCount = 0;

            dataGridViewResults.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridViewResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewResults.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewResults.Font, FontStyle.Bold);
            
            dataGridViewResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridViewResults.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewResults.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewResults.GridColor = Color.Black;
            dataGridViewResults.RowHeadersVisible = false;
            
            // Set the background color for all rows and for alternating rows. 
            // The value for alternating rows overrides the value for all rows. 
            dataGridViewResults.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridViewResults.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            
            dataGridViewResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewResults.MultiSelect = true;
            dataGridViewResults.ReadOnly = true;
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.AutoResizeColumns();
        }

        /// <summary>
        /// fetch the data from the magic the gathering web site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFetch_Click(object sender, EventArgs e)
        {
            // timer start
            Start = DateTime.Now;

            
            // If the background thread is running then clicking this
            // button causes a cancel, otherwise clicking this button
            // launches the background thread.
            if (FetchAsyncWorker.IsBusy)
            {
                buttonFetch.Enabled = false;
                //Log("Cancelling Search...");

                // Notify the worker thread that a cancel has been requested.
                // The cancel will not actually happen until the thread in the
                // DoWork checks the bwAsync.CancellationPending flag, for this
                // reason we set the label to "Cancelling...", because we haven't
                // actually cancelled yet.
                FetchAsyncWorker.CancelAsync();
            }
            else
            {
                buttonFetch.Text = "Cancel";
                
                //Log("Search Started...");

                // turn the mouse cursor to hourglass for the wait                
                Cursor.Current = Cursors.WaitCursor;

                // clear out the results page
                dataGridViewResults.DataSource = null;
                dataGridViewResults.Refresh();

                // clear out the time results box
                textBoxTimer.Text = "";

                // get how many pages need to be fetched
                MaxPagesToFetchInParallel = Convert.ToInt32(textBoxFetchPagesInParallel.Text);

                // Kickoff the worker thread to begin it's DoWork function.
                FetchAsyncWorker.RunWorkerAsync();
            }
        }

        private void linkLabelDefault_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            try
            {
                // mmb - fix this... if it's even needed
                Process.Start(e.Link.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("This must not be working... [{0}]", ex.Message));
            }
        }

        #region Fetch Async Background Worker

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FetchAsync_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This function fires on the UI thread so it's safe to edit
            // the UI control directly, no funny business with Control.Invoke.
            // Update the progressBar with the integer supplied to us from the
            // ReportProgress() function.  Note, e.UserState is a "tag" property
            // that can be used to send other information from the
            // BackgroundThread to the UI thread.

            progressBar1.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FetchAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // The sender is the BackgroundWorker object we need it to
                // report progress and check for cancellation.
                BackgroundWorker bwAsync = sender as BackgroundWorker;

                // start the wait cursor
                WaitCursor(true);

                TempSet = new MTG.MTGCardSet();
                String SelectedEdition = GetComboBoxValue();

                if (GetComboBoxValue() == MTGEditions.FourthE.ToString().ToLower())
                {
                    SelectedEdition = "Fourth%20Edition";
                }
                else if (GetComboBoxValue() == MTGEditions.TenthE.ToString().ToLower())
                {
                    SelectedEdition = "Tenth%20Edition";
                }


                // create the web page to search...
                // example: 
                //   http://ww2.wizards.com/gatherer/index.aspx?term=&Field_Name=on&Field_Rules=on&Field_Type=on&setfilter=Fourth%20Edition
                String InitialWebPage = "http://ww2.wizards.com/gatherer/index.aspx";
                InitialWebPage += "?term=&Field_Name=on&Field_Rules=on&Field_Type=on&setfilter=" + SelectedEdition;

                // Store the temp data that will be sent to the web page parser                
                TempData temp = new TempData();
                temp._webpage = InitialWebPage;
                temp._mtgedition = SelectedEdition;

                // Now pull back all the card IDs
                WebPageParser.FetchInitialPage(temp);
                
                // update progression
                bwAsync.ReportProgress(0);
                                
                // create a var for all the threads
                Thread[] WebFetchingThreads = new Thread[TempIDs.Count];
                Int32 Page = 0;
                Int32 PagesCompleted = 0;

                // cycle through the pages to get all the scores
                foreach (Int32 id in TempIDs)
                {
                    // update progression
                    bwAsync.ReportProgress(Convert.ToInt32(Page * (100.0 / TempIDs.Count)));                    
                    
                    // create the web page to search...
                    // example: 
                    //  http://ww2.wizards.com/gatherer/CardDetails.aspx?&id=2085
                    String CardWebPage = "http://ww2.wizards.com/gatherer/CardDetails.aspx?&id=";
                    // just fill these in for now
                    CardWebPage += id.ToString();

                    // The constructor for the Thread class requires a ThreadStart 
                    // delegate that represents the method to be executed on the 
                    temp = new TempData();
                    temp._webpage = CardWebPage;
                    temp._mtgedition = SelectedEdition;

                    // Create the thread object, passing in the serverObject.StaticMethod method 
                    // using a ThreadStart delegate.
                    WebFetchingThreads[Page] = new Thread(WebPageParser.FetchCardPage);
                    
                    // Start the thread.
                    // Note that on a uniprocessor, the new thread does not get any processor 
                    // time until the main thread is preempted or yields.
                    WebFetchingThreads[Page].Start(temp);
                                                            
                    // Periodically check if a cancellation request is pending.
                    // If the user clicks cancel the line
                    // m_AsyncWorker.CancelAsync(); if ran above.  This
                    // sets the CancellationPending to true.
                    // You must check this flag in here and react to it.
                    // We react to it by setting e.Cancel to true and leaving.
                    if (bwAsync.CancellationPending)
                    {
                        // stop the job...
                        //Log("Searching stopped!");
                        break;
                    }

                    // Only allow so many pages to be fetched in parallel
                    if ((Page - PagesCompleted) >= (MaxPagesToFetchInParallel - 1))
                    {
                        // wait for the first run thread to return before letting another one start
                        WebFetchingThreads[Page].Join();

                        PagesCompleted++;
                        
                        // update progression
                        bwAsync.ReportProgress(Convert.ToInt32((PagesCompleted) * (100.0 / TempIDs.Count)));
                    }
                    
                    Page++;
                }

                // now wait and make sure that all web pages have returned back
                foreach (Thread t in WebFetchingThreads)
                {                    
                    t.Join();
                }

                bwAsync.ReportProgress(100);
            }
            catch (Exception ex)
            {
                //Log(String.Format("  **ERROR: ", ex.Message));
                MessageBox.Show(String.Format("  **ERROR[FetchAsync_DoWork]: {0}", ex.Message));
            }

            // stop the wait cursor
            WaitCursor(false);
        }

        /// <summary>
        /// The background process is complete. We need to inspect
        /// our response to see if an error occurred, a cancel was
        /// requested or if we completed successfully.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FetchAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonFetch.Text = "Fetch";
            buttonFetch.Enabled = true;

            // Check to see if an error occurred in the background process.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }

            // Check to see if the background process was cancelled.
            if (e.Cancelled)
            {
                //Log("Search has been cancelled!");
            }
            else
            {
                //Log("Search has been completed!");

                // Everything completed normally.  process the response using e.Result
                
                System.TimeSpan Length = Start.Subtract(DateTime.Now);
                textBoxTimer.Text = Length.Duration().ToString();
                   
                // update the progress bar to 100 percent
                progressBar1.Value = 100;

                dataGridViewResults.DataSource = TempSet.CardSet;
                UpdateGrid();

                // turn the mouse cursor to hourglass for the wait
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateGrid()
        {
            dataGridViewResults.Columns[0].Visible = false;
            dataGridViewResults.Columns[6].Visible = false;
            dataGridViewResults.Columns[7].Visible = false;
            dataGridViewResults.Columns[8].Visible = false;
            dataGridViewResults.Columns[9].Visible = false;
            dataGridViewResults.Columns[10].Visible = false;
            dataGridViewResults.AutoResizeColumns();
            dataGridViewResults.AutoResizeRows();
            dataGridViewResults.Refresh();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFilter_Click(object sender, EventArgs e)
        {
            // mmb - todo
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {
            // mmb - todo
        }

        /// <summary>
        /// Use the current data to serialize into a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Attempt to save the last cardset saved
                if (TempSet != null && TempSet.CardSet.Count > 0)
                {
                    Stream stream = File.Open("cardset.dat", FileMode.Create);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, TempSet);
                    stream.Close();
                }
            }
            catch(Exception ex)
            {
                String Error = String.Format("{0}", ex.Message);
            }
            
            // pages to fetch in parallel
            Properties.Settings.Default.PagesToFetchInParallel = textBoxFetchPagesInParallel.Text;
            Properties.Settings.Default.Save();            
        }

        /// <summary>
        /// Load the serialized data
        /// </summary>
        private void LoadTempData()
        {            
            // Attempt to load the last cardset saved
            try
            {
                //Open the file written above and read values from it.
                Stream stream = File.Open("cardset.dat", FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                TempSet = (MTG.MTGCardSet)bformatter.Deserialize(stream);
                stream.Close();

                dataGridViewResults.DataSource = TempSet.CardSet;
                UpdateGrid();
            }
            catch (Exception ex)
            {
                if (!ex.Message.StartsWith("Could not find file"))
                {
                    //Log(String.Format("  **ERROR: ", ex.Message));
                    MessageBox.Show(String.Format("  **ERROR[LoadTempData]: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewResults_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count > 0)
            {
                pictureBoxCardImage.BackgroundImage = (Image)dataGridViewResults.SelectedRows[0].Cells[8].Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCardSetLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog();

            if (load.ShowDialog() == DialogResult.OK)
            {
                // Attempt to load the last cardset saved
                try
                {
                    //Open the file written above and read values from it.
                    Stream stream = File.Open(load.FileName, FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();

                    TempSet = (MTG.MTGCardSet)bformatter.Deserialize(stream);
                    stream.Close();

                    dataGridViewResults.DataSource = TempSet.CardSet;
                    UpdateGrid();
                }
                catch (Exception ex)
                {
                    if (!ex.Message.StartsWith("Could not find file"))
                    {
                        //Log(String.Format("  **ERROR: ", ex.Message));
                        MessageBox.Show(String.Format("  **ERROR[LoadTempData]: {0}", ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCardSetSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();


            save.AddExtension = true;
            save.DefaultExt = "dat";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Attempt to save the last cardset saved
                    if (TempSet != null && TempSet.CardSet.Count > 0)
                    {
                        Stream stream = File.Open(save.FileName, FileMode.Create);
                        BinaryFormatter bformatter = new BinaryFormatter();
                        bformatter.Serialize(stream, TempSet);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    String Error = String.Format("{0}", ex.Message);
                }
            }
        }
    }
}




/// MMB - TODO List
/// 
/* ***Here are some items that need to be completed eventually
 * Fix the Property.Settings.Default issue - Change the Namespace of this file!!!
 * */

