using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class OfferViewModel
    {

        public string Name { get; set; }

        public List<OfferItem> Items { get; set; } = new List<OfferItem>();

        public decimal PriceSum { get { return Items.Sum(i => i.Price); } }

        public int Discount { get; set; }
        

        public class OfferItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string TextValue { get; set; }
            public bool HasPrice { get; set; }
            public string OptionTag { get; set; }
        }

    }
}
