using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace MTGCommon
{
    [Serializable]
    class MTGCollection
    {
        private ArrayList cards;
        public ArrayList Cards
        {
            get { return cards; }
            set { cards = value; }
        }

        // mmb - todo
        //private Dictionary <String,ArrayList> decks;


    }
}
