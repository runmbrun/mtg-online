#region Using
using System;
using System.Collections;
using System.Linq;
using System.Text;
#endregion


namespace MTG
{
    public class MTGDeck
    {
        private String _name;
        public String Name
        {
            set { _name = value; }
            get { return _name; }
        }

        private ArrayList _cards;
        public ArrayList Cards
        {
            set { _cards = value; }
            get { return _cards; }
        }

        public void Shuffle()
        {
            // shuffle the arraylist of cards
            // mmb - todo
        }
    }
}
