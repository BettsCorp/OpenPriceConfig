using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class Option
    {
        public int ID { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DescriptionLocaleID { get; set; }
        public Locale DescriptionLocale { get; set; }

        public string OptionTag { get; set; }

        public decimal Price { get; set; } 

        public List<BracketPricing> BracketPricing { get; set; }

        public Configurator Configurator { get; set; }

        public void GenerateBracketPricings(int numberOfFloors)
        {
            BracketPricing = new List<Models.BracketPricing>();
            for (int i = 2; i <= numberOfFloors; i++)
            {
                BracketPricing.Add(new BracketPricing() { ForFloorNumber = i });
            }
        }
    }
}
