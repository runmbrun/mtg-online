using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace MTG
{
    [Serializable]
    public class MTGCardSet
    {
        private DateTime LastFetched;
        public ArrayList CardSet = null;


        /// <summary>
        /// 
        /// </summary>
        public MTGCardSet()
        {
            CardSet = new ArrayList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Row"></param>
        public void Add(MTGCard Row)
        {
            CardSet.Add(Row);
            LastFetched = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Column"></param>
        public void Sort(Int32 Column)
        {
            // sort each card's sorting field
            foreach (MTGCard card in CardSet)
            {
                card.SetSorting(Column);
            }

            // now resort the array with the new sorting field
            CardSet.Sort();
        }
    }
}
