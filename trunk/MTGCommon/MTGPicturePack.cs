using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace MTG
{
    public class MTGPicturePack
    {
        /*
        public ArrayList _pictures = null;

        
        public ArrayList Pictures
        {
            get
            {
                return _pictures;
            }
            set
            {
                _pictures.Add(value);
            }
        }

        public MTGPicturePack()
        {
            _pictures = new ArrayList();
        }

        public void Add(Image image)
        {
            _pictures.Add(image);
        }*/

        public Dictionary<Int32, Image> Pictures;

        public MTGPicturePack()
        {
            Pictures = new Dictionary<Int32, Image>();
        }

        public void Add(Int32 id, Image image)
        {
            Pictures.Add(id, image);
        }
    }
}
