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
            Option = 4,
            HorizontalRule = 5,
            LineBreak = 6
        }
        public int ID { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public const string DescriptionInfoText = "Text is displayed under the name of the item in the genrerated price list.";
        public string Description { get; set; }

        public const string DescriptionLocaleInfoText = "If set, the text in the Description field will be neglected. Instead the text pointed by DescriptionLocaleID will be used.\nThis comes handy when multiple items are to show the same text.";
        public int DescriptionLocaleID { get; set; }
        public Locale DescriptionLocale { get; set; }

        public const string OptionTagInfoText = "If Item type is set top Option, this value is used to group multiple options.\nSet the same (any) Option Tag value to the ones that are grouped";
        public string OptionTag { get; set; }

        public decimal Price { get; set; } 

        public List<BracketPricing> BracketPricing { get; set; }

        public Configurator Configurator { get; set; }

        public const string InputTypeInfoText = "Checkbox: The item can be independently be selected\nNumeric: Input value is a number\nText: Input value is a text\nOption: One of multiple values are selected (refer to OptionTag)\nHorizontalRule: Not an input, but a line to divide items\nLineBreak: Not an input. Just line break";
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

        public decimal GetPrice(int numberOfFloors)
        {
            if (BracketPricing == null || BracketPricing.Count == 0)
            {
                return Price;
            }
            else
            {
                return BracketPricing.Where(b => b.ForFloorNumber == numberOfFloors).Single().Price;
            }
        }

        public string GetDescription()
        {
            if (DescriptionLocaleID == 1)
                return Description;

            return DescriptionLocale.Text;
        }
    }
}
