using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class OfferViewModel
    {

        public List<OfferItem> Items { get; set; } = new List<OfferItem>();

        public decimal PriceSum { get { return Items.Sum(i => i.Price); } }
        

        public class OfferItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string TextValue { get; set; }
        }

    }
}
