using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace MTG
{
    public class MTGNetworkPacket
    {
        #region Class Variables

        private MTGOpCode _opcode;
        private Int32 _packetLength;
        private Object _data;

        public MTGOpCode OpCode
        {
            get { return _opcode; }
            set { _opcode = value; }
        }
        public Int32 PacketLength
        {
            get { return _packetLength; }
            set { _packetLength = value; }
        }
        public Object Data
        {
            get { return _data; }
            set 
            { 
                _data = value;

                //if (_data.GetType() == typeof(String))
                if (OpCode == MTGOpCode.Login || OpCode == MTGOpCode.Purchase)
                {
                    _packetLength = _data.ToString().Length;
                }
                else
                {
                    //_packetLength = Marshal.SizeOf(_data);                    

                    //byte[] b = (byte[])_data;
                    //_packetLength = b.Length;

                    MemoryStream memStream1 = new MemoryStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memStream1, _data);
                    byte[] bytes = memStream1.GetBuffer();
                    memStream1.Close();
                    _packetLength = bytes.Length;
                }
            }
        }

        #endregion


        public enum MTGOpCode
        {
            Login,      //Log into the server
            Logout,     //Logout of the server
            Purchase,   //Purchase some cards
            PurchaseReceive,   //Purchase of some cards are being received
            Null        //No command
        }

        /// <summary>
        /// 
        /// </summary>
        public MTGNetworkPacket()
        {
            _opcode = MTGOpCode.Null;
            _packetLength = 0;
            _data = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public MTGNetworkPacket(byte[] data)
        {
            //The first four bytes are for the OpCode
            OpCode = (MTGOpCode)BitConverter.ToInt32(data, 0);

            //The next four store the length of the Object
            PacketLength = BitConverter.ToInt32(data, 4);

            //This checks for a null message field
            if (PacketLength > 0)
            {                
                //if (Data.GetType() == typeof(String))
                if (OpCode == MTGOpCode.Login || OpCode == MTGOpCode.Purchase)
                {
                    Data = Encoding.UTF8.GetString(data, 8, PacketLength);
                }
                else
                {
                    //Data = Encoding.UTF8.GetString(data, 8, PacketLength);
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream memStream2 = new MemoryStream(data, 8, PacketLength);
                    _data = (Object)formatter.Deserialize(memStream2);
                }
            }
            else
            {
                _data = null;
            }
        }

         /// <summary>
         /// Converts the Data structure into an array of bytes
         /// </summary>
         /// <returns></returns>
         public byte[] ToByte()
         {
             List<byte> result = new List<byte>();

             //First four are for the Command
             result.AddRange(BitConverter.GetBytes(Convert.ToInt32(OpCode)));

            //Length of the data
             if (Data != null)
             {
                 result.AddRange(BitConverter.GetBytes(PacketLength));
                                  
                 //if (Data.GetType() == typeof(String))
                 if (OpCode == MTGOpCode.Login || OpCode == MTGOpCode.Purchase)
                 {
                     result.AddRange(Encoding.UTF8.GetBytes(Data.ToString()));
                 }
                 else
                 {
                     //byte[] b = (byte[])Data;

                     MemoryStream memStream1 = new MemoryStream();
                     BinaryFormatter formatter = new BinaryFormatter();
                     formatter.Serialize(memStream1, _data);
                     byte[] bytes = memStream1.GetBuffer();
                     memStream1.Close();
                     
                     result.AddRange(bytes);
                 }                 
             }
             else
             {
                 result.AddRange(BitConverter.GetBytes(0));
             }

             return result.ToArray();
         }
    }
}
