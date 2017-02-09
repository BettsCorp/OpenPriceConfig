using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class Option
    {

        public enum InputTypes
        {
            Checkbox = 1,
            Numeric = 2,
            Text = 3,
            Option = 4
        }
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

        public InputTypes InputType { get; set; }

        public void GenerateBracketPricings(int numberOfFloors)
        {
            BracketPricing = new List<Models.BracketPricing>();
            for (int i = 2; i <= numberOfFloors; i++)
            {
                BracketPricing.Add(new BracketPricing() { ForFloorNumber = i });
            }
        }

        public void UpdateBracketPricings(int numberOfFloors)
        {
            if (BracketPricing == null | BracketPricing.Count == 0)
                return;
                
            if (BracketPricing.Count < numberOfFloors)
            {
                //Make new bracket pricings
                var startingFloor = BracketPricing.Last().ForFloorNumber;
                for(int i = startingFloor+1; i <= numberOfFloors; i++)
                {
                    BracketPricing.Add(new Models.BracketPricing() { ForFloorNumber = i });
                }
            }
            else if(BracketPricing.Count > numberOfFloors)
            {
                //Remove bracket pricings
                for(int i = BracketPricing.Last().ForFloorNumber; i > numberOfFloors; i--)
                 {
                    BracketPricing.Remove(BracketPricing.Last());
                }
            }
        }
    }
}
