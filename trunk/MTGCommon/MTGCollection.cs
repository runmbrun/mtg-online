using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace MTG
{
    [Serializable]
    public class MTGCollection
    {
        private ArrayList cards;
        public ArrayList Cards
        {
            get { return cards; }
            set { cards = value; }
        }

        private Dictionary <String,ArrayList> decks;
        public Dictionary<String, ArrayList> Decks
        {
            get { return decks; }
            set { decks = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public MTGCollection()
        {
            cards = new ArrayList();
            decks = new Dictionary<String, ArrayList>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            cards.Clear();
            decks.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NewCollection"></param>
        public void Add(MTGCollection NewCollection)
        {
            foreach (Int32 cardnumber in NewCollection.Cards)
            {
                Cards.Add(cardnumber);
            }

            //mmb - todo
            // update decks too
        }
    }
}
