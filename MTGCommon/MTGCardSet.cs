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

        public MTGCardSet()
        {
            CardSet = new ArrayList();
        }

        /* mmb - can I do this anymore?
        public void Add(String[] Row)
        {
            MTGCard row = new MTGCard();
            row.ID = Row[0];
            row.Name = Row[1];
            row.Cost = Row[2];
            // ...
            CardSet.Add(row);
            LastFetched = DateTime.Now;
        }
         * */

        public void Add(MTGCard Row)
        {
            CardSet.Add(Row);
            LastFetched = DateTime.Now;
        }
    }
}
