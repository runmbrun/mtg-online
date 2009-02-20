using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.ObjectModel;



namespace MTGDataGatherer
{
    public partial class Form1 : Form
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        //delegate void SetTextCallback(String Message);
        delegate void SetResultsCallback(String[] Information);
        delegate void SetResultsResizeCallback();
        delegate void SetDeployFolderButtonCallback(Boolean Enable);
        delegate String GetComboBoxValueCallback();
        delegate void SetWaitCursorCallback(Boolean Wait);

/*
        /// <summary>
        /// Log to the output...
        /// This method demonstrates a pattern for making thread-safe
        /// calls on a Windows Forms control. 
        /// 
        /// If the calling thread is different from the thread that
        /// created the TextBox control, this method creates a
        /// SetTextCallback and calls itself asynchronously using the
        /// Invoke method.
        ///
        /// If the calling thread is the same as the thread that created
        /// the TextBox control, the Text property is set directly. 
        /// </summary>
        /// <param name="Message"></param>
        public void Log(String Message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBoxResults.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Log);
                this.Invoke(d, new object[] { Message });
            }
            else
            {
                this.listBoxResults.Items.Add(Message);
            }
        }

        public void LogColored(String Message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBoxResults.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Log);
                this.Invoke(d, new object[] { Message });
            }
            else
            {
                // mmb - this doesn't work!  Colors all text to red, and then black again, not just the 1 line
                this.listBoxResults.ForeColor = Color.Red;
                this.listBoxResults.Items.Add(Message);
                this.listBoxResults.ForeColor = Color.Black;
            }
        }
*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void AddToResults(String[] Message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewResults.InvokeRequired)
            {
                SetResultsCallback d = new SetResultsCallback(AddToResults);
                this.Invoke(d, new object[] { Message });
            }
            else
            {
                this.dataGridViewResults.Rows.Add(Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResizeResults()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewResults.InvokeRequired)
            {
                SetResultsResizeCallback d = new SetResultsResizeCallback(ResizeResults);
                this.Invoke(d, new object[] { });
            }
            else
            {
                // resize the control for the new deployable file(s) data
                dataGridViewResults.AutoResizeColumns();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Enable"></param>
        public void EnableFetchButton(Boolean Enable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.dataGridViewResults.InvokeRequired)
            {
                SetDeployFolderButtonCallback d = new SetDeployFolderButtonCallback(EnableFetchButton);
                this.Invoke(d, new object[] { Enable });
            }
            else
            {
                // enable or disable the deploy folder button
                buttonFetch.Enabled = Enable;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Index"></param>
        public void WaitCursor(bool Wait)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                SetWaitCursorCallback d = new SetWaitCursorCallback(WaitCursor);
                this.Invoke(d, new object[] { Wait });
            }
            else
            {
                // enable or disable the wait cursor
                if (Wait)
                {
                    this.Cursor = Cursors.WaitCursor;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Index"></param>
        public String GetComboBoxValue()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.comboBoxFetchMTGEdition.InvokeRequired)
            {
                //GetComboBoxValueCallback d = new GetComboBoxValueCallback(GetComboBoxValue);
                //this.Invoke(d, new object[] {  });

                GetComboBoxValueCallback getDelegate = delegate() { return comboBoxFetchMTGEdition.SelectedItem.ToString().ToLower(); };
                return (string)EndInvoke(BeginInvoke(getDelegate, null));

                //return comboBoxFetchInstruments.SelectedItem.ToString().ToLower();
            }
            else
            {
                // enable or disable the combobox for Presets
                return comboBoxFetchMTGEdition.SelectedItem.ToString().ToLower();
            }
        }


        /*
        public class SafeTextBox : TextBox
        {
            delegate void Set(string text);
            delegate string Get();   override public string Text
            {
                set
                {
                    if (InvokeRequired)
                    {
                        Set setDelegate = delegate(string text) {Text = text;};
                        BeginInvoke(setDelegate, new object[] {value});
                    }
                    else
                        base.Text = value;
                }
                get
                {
                    if (InvokeRequired)
                    {
                        Get getDelegate = delegate() {return Text;};
                        return (string)EndInvoke(BeginInvoke(getDelegate, null));
                    }
                    else
                        return base.Text;
                }
            }
        }
        */
    }
}
