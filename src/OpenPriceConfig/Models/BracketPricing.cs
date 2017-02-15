using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class BracketPricing
    {

        public int ID { get; set; }

        public int ForFloorNumber { get; set; }
        public int Level { get; set; }

        public decimal Price { get; set; }

        public int OptionID { get; set; }
        public Option Option { get; set; }
    }
}
