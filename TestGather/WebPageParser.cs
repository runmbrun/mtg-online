using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Reflection;
using MTG;




namespace TestMTGGatherer
{
    class WebPageParser
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WebPage"></param>
        public static void FetchInitialPage(Object param)
        {
            Form1.TempData temp = (Form1.TempData)param;
            String WebPage = temp._webpage;
            String Edition = temp._mtgedition;
            ArrayList Results;


            try
            {
                // Create a 'WebRequest' object with the specified url.
                WebRequest myWebRequest = WebRequest.Create(WebPage);

                // Send the 'WebRequest' and wait for response.
                WebResponse myWebResponse = myWebRequest.GetResponse();

                // Obtain a 'Stream' object associated with the response object.
                Stream ReceiveStream = myWebResponse.GetResponseStream();

                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                // Pipe the stream to a higher level stream reader with the required encoding format.
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                String strResponse = readStream.ReadToEnd();

                #region DEBUG
                
                // this is for debugging only
                if (Form1.OutputResults)
                {
                    // Create the output file.
                    using (FileStream fs = File.Create(Form1.OutputFile)) { }
                    // Open the stream and write to it
                    using (FileStream fs = File.OpenWrite(Form1.OutputFile))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("   ");
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                        info = new UTF8Encoding(true).GetBytes(strResponse);
                        fs.Write(info, 0, info.Length);
                    }
                }
                #endregion

                // close the reading stream
                readStream.Close();

                // Release the resources of response object.
                myWebResponse.Close();

                // now parse the web page to get just the data we need
                Results = ParseData(strResponse, Edition);                

                // store the Results
                foreach (Int32 id in Results)
                {
                    Form1.TempIDs.Add(id);
                }
            }
            catch (Exception ex)
            {
                // mmb - do something
                String Error = String.Format("ERROR: {0}", ex.Message);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strResponse"></param>
        private static ArrayList ParseData(String strResponse, String Edition)
        {
            ArrayList Results = new ArrayList();
            Int32 iStart = 0;
            Int32 iNext = 0;


            // here are a list of the lines to parse out to get the proper data
            String strFindFirst = "<tr onmouseover=\"this.style.backgroundColor=";
            String strFindNext = "<td align=\"left\" valign=\"top\" onclick=\"javascript:openDetailsWindow(";
                      
            
            try
            {
                // loop and pull out each rank
                while ((iStart = (strResponse.IndexOf(strFindFirst, iStart) + strFindFirst.Length)) > strFindFirst.Length)
                {
                    Int32 ID = 0;

                    // Parse the ID of the card
                    iStart = strResponse.IndexOf(strFindNext, iStart) + strFindNext.Length;
                    iNext = strResponse.IndexOf(")", iStart) - iStart;
                    ID = Convert.ToInt32(strResponse.Substring(iStart, iNext));

                    // now add to the results text box
                    Results.Add(ID);

                    // update start column
                    iStart = iStart + iNext + 1;
                }
            }
            catch (Exception ex)
            {
                String Error = String.Format("ERROR: ", ex.Message);
                //Log(String.Format("  **ERROR: ", ex.Message));
                //MessageBox.Show(String.Format("  **ERROR[ParseData]: {0}", ex.Message));
            }

            return Results;             
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string ParseCost(String Cost)
        {
            String SimplifiedCost = "? Mana";

            // mmb - do this!

            return SimplifiedCost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public static void FetchCardPage(Object param)
        {
            Form1.TempData temp = (Form1.TempData)param;
            String WebPage = temp._webpage;
            String Edition = temp._mtgedition;


            try
            {
                // Create a 'WebRequest' object with the specified url.
                WebRequest myWebRequest = WebRequest.Create(WebPage);

                // Send the 'WebRequest' and wait for response.
                WebResponse myWebResponse = myWebRequest.GetResponse();

                // Obtain a 'Stream' object associated with the response object.
                Stream ReceiveStream = myWebResponse.GetResponseStream();

                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                // Pipe the stream to a higher level stream reader with the required encoding format.
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                String strResponse = readStream.ReadToEnd();

                #region DEBUG

                // this is for debugging only
                if (Form1.OutputResults)
                {
                    // Create the output file.
                    using (FileStream fs = File.Create(Form1.OutputFile)) { }
                    // Open the stream and write to it
                    using (FileStream fs = File.OpenWrite(Form1.OutputFile))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("   ");
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                        info = new UTF8Encoding(true).GetBytes(strResponse);
                        fs.Write(info, 0, info.Length);
                    }
                }
                #endregion

                // close the reading stream
                readStream.Close();

                // Release the resources of response object.
                myWebResponse.Close();

                // now parse the web page to get just the data we need
                Form1.TempSet.Add(ParseCardData(strResponse, Edition));
            }
            catch (Exception ex)
            {
                // mmb - do something
                String Error = String.Format("ERROR: {0}", ex.Message);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strResponse"></param>
        /// <param name="Edition"></param>
        /// <returns></returns>
        private static MTGCard ParseCardData(String strResponse, String Edition)
        {
            MTGCard Card = new MTGCard();
            Int32 iStart = 0;
            Int32 iNext = 0;


            // here are a list of the lines to parse out to get the proper data for the individual card's web pages...
            String strFindPic = "<img ID=\"_imgCardImage\"  src=\"";
            String strFindID = "<form name=\"form1\" method=\"post\" action=\"CardDetails.aspx?id="; //<form name="form1" method="post" action="CardDetails.aspx?id=2084"
            String strFindName = "<b><span id=\"_lblCardTitle\">";              //<span id="_lblCardTitle">Abomination<
            String strFindRarity = "<span id=\"_lblSetRarity\">";               //<span id="_lblSetRarity"><i>Fourth Edition</i> uncommon<                        
            String strFindCost = "<span id=\"_lblCost\">";                      //<span id="_lblCost"><img src='http://resources.wizards.com/magic/images/symbols/Symbol_3_mana.gif' alt='3 Mana' border='0'/><img src='http://resources.wizards.com/magic/images/symbols/black_mana.gif' width='12' height='12' alt='Black Mana' border='0'/><img src='http://resources.wizards.com/magic/images/symbols/black_mana.gif' width='12' height='12' alt='Black Mana' border='0'/></span>
            String strFindType = "<span id=\"_lblCardType\">";                  //<span id="_lblCardType">Creature - Horror</span>
            String strFindPowerToughness = "<span id=\"_lblPowerToughness\">";  //<span id="_lblPowerToughness">2/6</span>
            String strFindText = "<span id=\"_lblRulesText\">";                 //<span id="_lblRulesText">...</span>
            String strFindFlavorText = "<span id=\"lblFlavorText\">";         //<span id="_lblFlavorText"></span>


            try
            {                   
                // Parse the information about the card on this web page...
                
                // ID
                iStart = strResponse.IndexOf(strFindID, iStart) + strFindID.Length;
                iNext = strResponse.IndexOf("\"", iStart) - iStart;
                Card.ID = Convert.ToInt32(strResponse.Substring(iStart, iNext));

                // Picture link
                //<img ID="_imgCardImage"  src="http://resources.wizards.com/Magic/Cards/4E/en-us/Card2084.jpg" alt="Magic: The Gathering" Height="285" Width="200" Border="0"/></a>
                iStart = strResponse.IndexOf(strFindPic, iStart) + strFindPic.Length;
                iNext = strResponse.IndexOf("\"", iStart) - iStart;
                Card.PicLocation = strResponse.Substring(iStart, iNext);                

                // Name
                iStart = strResponse.IndexOf(strFindName, iStart) + strFindName.Length;
                iNext = strResponse.IndexOf("<", iStart) - iStart;
                Card.Name = strResponse.Substring(iStart, iNext);

                // Rarity
                iStart = (strResponse.IndexOf(strFindRarity, iStart) + strFindRarity.Length);
                iStart = (strResponse.IndexOf("</i> ", iStart) + 5);
                iNext = strResponse.IndexOf("<", iStart) - iStart;
                Card.Rarity = strResponse.Substring(iStart, iNext);

                // Printings
                // mmb - do we care about this?

                // Cost
                iStart = (strResponse.IndexOf(strFindCost, iStart) + strFindCost.Length);
                iNext = strResponse.IndexOf("</span>", iStart) - iStart;
                Card.Cost = ParseCost(strResponse.Substring(iStart, iNext));

                // Type
                iStart = (strResponse.IndexOf(strFindType, iStart) + strFindType.Length);
                iNext = strResponse.IndexOf("<", iStart) - iStart;
                Card.Type = strResponse.Substring(iStart, iNext);

                // Power and Toughness
                iStart = (strResponse.IndexOf(strFindPowerToughness, iStart) + strFindPowerToughness.Length);
                iNext = strResponse.IndexOf("<", iStart) - iStart;
                String temp = strResponse.Substring(iStart, iNext);
                if (temp.IndexOf("/") >= 0)
                {
                    Card.Power = temp.Substring(0, temp.IndexOf("/"));
                    Card.Toughness = temp.Substring(temp.IndexOf("/") + 1);                    
                }

                // Text
                iStart = (strResponse.IndexOf(strFindText, iStart) + strFindText.Length);
                iNext = strResponse.IndexOf("<", iStart) - iStart;
                Card.Text = strResponse.Substring(iStart, iNext);

                // Flavor Text
                iStart = (strResponse.IndexOf(strFindFlavorText, iStart) + strFindFlavorText.Length);
                iNext = strResponse.IndexOf("<", iStart) - iStart;
                Card.Flavor = strResponse.Substring(iStart, iNext);

                // now download the picture of the card
                Card.Pic = DownloadPicture(Card.PicLocation);
            }
            catch (Exception ex)
            {
                String Error = String.Format("ERROR: ", ex.Message);
                //Log(String.Format("  **ERROR: ", ex.Message));
                //MessageBox.Show(String.Format("  **ERROR[ParseData]: {0}", ex.Message));
            }

            return Card;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        public static Image DownloadPicture(String URL)
        {
            Image CardImage = null;

            try
            {
                String Images = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Images\\";

                // Create a 'WebRequest' object with the specified url.
                WebRequest myWebRequest = WebRequest.Create(URL);

                // Send the 'WebRequest' and wait for response.
                WebResponse myWebResponse = myWebRequest.GetResponse();

                // Obtain a 'Stream' object associated with the response object.
                CardImage = Image.FromStream(myWebResponse.GetResponseStream());

                #region DEBUG

                // this is for debugging only
                if (Form1.OutputResults)
                {
                    // save to computer
                    if (!Directory.Exists(Images))
                    {
                        Directory.CreateDirectory(Images);
                    }

                    Int32 test1 = URL.LastIndexOf("/") + 5;
                    Int32 test2 = URL.LastIndexOf(".");
                    String ImageName = URL.Substring(test1, test2 - test1) + ".jpg";

                    CardImage.Save(Images + ImageName);
                }
                #endregion

                // Release the resources of response object.
                myWebResponse.Close();
            }
            catch (Exception ex)
            {
                // mmb - do something
                String Error = String.Format("ERROR: {0}", ex.Message);

            }

            return CardImage;
        }
    }
}
