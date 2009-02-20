using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;



namespace MTG
{
    class MTGDatabase
    {
        SqlConnection sqlConn;
        private String server = "localhost";
        private String user = "sa";
        private String password = "Password01";
        private String name = "mtg";
        private String connectionstring;
                
        private Boolean connected = false;
        public Boolean Connected
        {
            get { return connected; }
        }
        
        private String errorstring;
        public String GetLastError
        {
            get { return errorstring; }
        }



        /// <summary>
        /// 
        /// </summary>
        public MTGDatabase()
        {
            connectionstring = "server=" + server + ";uid=" + user + ";pwd=" + password + ";database=" + name + ";";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean Connect()
        {
            try
            {
                // create a new SqlConnection object with the appropriate connection string
                sqlConn = new SqlConnection(connectionstring);

                // open the connection
                sqlConn.Open();

                connected = true;
            }
            catch (Exception ex)
            {
                errorstring = String.Format("MTGDatabase ERROR: Failed to connect. [{0}]", ex.Message);
            }

            return Connected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean Disconnect()
        {
            Boolean Success = false;
            try
            {
                if (Connected)
                {
                    // close the connection
                    sqlConn.Close();
                }

                connected = false;
                Success = true;
            }
            catch (Exception ex)
            {
                errorstring = String.Format("MTGDatabase ERROR: Failed to connect. [{0}]", ex.Message);
            }

            return Success;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Boolean ValidateUser(String User, String Password)
        {
            Boolean Valid = false;
            Int32 Count = 0;
            String sql = "SELECT * FROM players WHERE name='" + User + "' and password='" + Password + "'";
            SqlCommand sqlComm;


            try
            {
                // create the command object
                sqlComm = new SqlCommand(sql, sqlConn);
                //Count = sqlComm.ExecuteScalar(); //mmb - this fails right now... why?

                SqlDataReader r = sqlComm.ExecuteReader();
                while (r.Read())
                {
                    string username = (string)r["name"];
                    int userID = (int)r["id"];
                    Count++;
                }
                r.Close();
            }
            catch (Exception ex)
            {
                errorstring = String.Format("MTGDatabase ERROR: DB Query in ValidateUser. [{0}]", ex.Message);
            }

            if (Count == 1)
            {
                Valid = true;
            }
            else
            {
                errorstring = String.Format("MTGDatabase ERROR: Invalid User. Count=[{0}]", Count);
            }            

            return Valid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Boolean IsAdmin(String User, String Password)
        {
            Boolean Admin = false;
            SqlCommand sqlComm;
            Int32 Count = 0;


            try
            {
                // create the command object
                sqlComm = new SqlCommand("SELECT * FROM players WHERE name='" + User + "' and password='" + Password + "' and admin = 1", sqlConn);

                SqlDataReader r = sqlComm.ExecuteReader();
                while (r.Read())
                {
                    string username = (string)r["name"];
                    int userID = (int)r["id"];
                    Count++;
                }
                r.Close();
            }
            catch (Exception ex)
            {
                errorstring = String.Format("MTGDatabase ERROR: DB Query in IsAdmin. [{0}]", ex.Message);
            }

            if (Count == 1)
            {
                Admin = true;
            }
            else
            {
                errorstring = String.Format("MTGDatabase ERROR: Non Admin User. Count=[{0}]", Count);
            }

            return Admin;
        }
    }
}
