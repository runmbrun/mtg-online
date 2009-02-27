using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace MTG
{
    [Serializable]
    public class MTGCard : IComparable 
    {
        private Int32 _id;
        private String _name;
        private String _cost;
        private String _type;
        private String _power;
        private String _toughness;
        private String _text;
        private String _picLocation;
        private Image _pic;
        private String _rarity;
        private String _flavor;
        private Int32 _quantity;
        private String _number;
        private Int32 _sortby;


        public MTGCard()
        {
            _id = -1;
            _name = "";
            _cost = "";
            _type = "";
            _power = "";
            _toughness = "";
            _text = "";
            _picLocation = "";
            _rarity = "";
            _pic = null;
            _quantity = 0;
            _number = "0";
            _sortby = 0;
        }

        public MTGCard(Int32 id, String name, String cost, String type, String power, String toughness, String text, String piclocation)
        {
            _id = id;
            _name = name;
            _cost = cost;
            _type = type;
            _power = power;
            _toughness = toughness;
            _text = text;
            _picLocation = piclocation;
            _pic = null;
            _quantity = 0;
            _number = "0";
            _sortby = 0;
        }

        // 0
        public Int32 ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
        // 1
        public String Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        // 2
        public String Cost
        {
            get { return this._cost; }
            set { this._cost = value; }
        }
        // 3
        public String Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        // 4
        public String Power
        {
            get { return this._power; }
            set { this._power = value; }
        }
        // 5
        public String Toughness
        {
            get { return this._toughness; }
            set { this._toughness = value; }
        }
        // 6
        public String Text
        {
            get { return this._text; }
            set { this._text = value; }
        }
        // 7
        public String PicLocation
        {
            get { return this._picLocation; }
            set { this._picLocation = value; }
        }
        // 8
        public Image Pic
        {
            get { return this._pic; }
            set { this._pic = value; }
        }
        // 9
        public String Rarity
        {
            get { return this._rarity; }
            set { this._rarity = value; }
        }
        // 10
        public String Flavor
        {
            get { return this._flavor; }
            set { this._flavor = value; }
        }
        // 11
        public Int32 Quantity
        {
            get { return this._quantity; }
            set { this._quantity = value; }
        }
        // 12
        public String Number
        {
            get { return this._number; }
            set { this._number = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is MTGCard)
            {
                MTGCard otherRowInformation = (MTGCard)obj;
                
                switch (_sortby)
                {
                    case 0:
                        return Convert.ToInt32(this._id).CompareTo(Convert.ToInt32(otherRowInformation._id));
                    case 1:
                        return this._name.CompareTo(otherRowInformation._name);
                    case 2:
                        return this._cost.CompareTo(otherRowInformation._cost);
                    case 3:
                        return this._type.CompareTo(otherRowInformation._type);
                    case 4:
                        return this._power.CompareTo(otherRowInformation._power);
                    case 11:
                        return Convert.ToInt32(this._quantity).CompareTo(Convert.ToInt32(otherRowInformation._quantity));
                    case 12:
                        return Convert.ToInt32(this._number).CompareTo(Convert.ToInt32(otherRowInformation._number));
                    default:
                        // if not found or defined, then sort by name
                        return this._name.CompareTo(otherRowInformation._name);
                }
            }
            else
            {
                throw new ArgumentException("Object is not a RowInformation");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColumnToSort"></param>
        public void SetSorting(Int32 ColumnToSort)
        {
            _sortby = ColumnToSort;
        }
    }
}
