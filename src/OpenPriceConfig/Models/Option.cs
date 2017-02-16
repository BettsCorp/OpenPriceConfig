using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class Option
    {

        public enum BracketPricingTypes
        {
            SinglePrice,
            FloorsNumber,
            WiresNumber
        }

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

        [Display(Name = "Description")]
        public int DescriptionLocaleID { get; set; }
        public Locale DescriptionLocale { get; set; }

        public const string OptionTagInfoText = "If Item type is set top Option, this value is used to group multiple options.\nSet the same (any) Option Tag value to the ones that are grouped";
        [Display(Name = "Option tag")]
        public string OptionTag { get; set; }

        public decimal Price { get; set; } 

        [Display(Name="Bracket pricing type")]
        public BracketPricingTypes BracketPricingType { get; set; }
        public List<BracketPricing> BracketPricing { get; set; } = new List<Models.BracketPricing>();

        public Configurator Configurator { get; set; }

        public const string InputTypeInfoText = "Checkbox: The item can be independently be selected\nNumeric: Input value is a number\nText: Input value is a text\nOption: One of multiple values are selected (refer to OptionTag)\nHorizontalRule: Not an input, but a line to divide items\nLineBreak: Not an input. Just line break";
        [Display(Name = "Input type")]
        public InputTypes InputType { get; set; }

        public void UpdateBracketPricings()
        {
            var levels = GetLevels();

            if (BracketPricing.Count < levels)
            {
                //Make new bracket pricings
                int startingLevel = 0;
                if(BracketPricing != null && BracketPricing.Count != 0)
                {
                    startingLevel = BracketPricing.Last().Level;
                }

                for(int i = startingLevel + 1; i <= levels; i++)
                {
                    BracketPricing.Add(new Models.BracketPricing() { Level = i });
                }
            }
            else if(BracketPricing.Count > levels)
            {
                //Remove bracket pricings
                for(int i = BracketPricing.Last().Level; i > levels; i--)
                 {
                    BracketPricing.Remove(BracketPricing.Last());
                }
            }
        }

        public decimal GetPrice(int numberOfFloors, int numberOfWires)
        {
            switch(BracketPricingType)
            {
                case BracketPricingTypes.SinglePrice:
                    return Price;
                case BracketPricingTypes.FloorsNumber:
                    return BracketPricing.Where(b => b.Level == numberOfFloors).Single().Price;
                case BracketPricingTypes.WiresNumber:
                    return BracketPricing.Where(b => b.Level == numberOfWires).Single().Price;
            }

            return 0M;
        }

        public int GetLevels()
        {
            if (Configurator == null)
                throw new Exception("Configurator must be queried");

            int levels = 0;

            switch (BracketPricingType)
            {
                case BracketPricingTypes.FloorsNumber:
                    levels = Configurator.FloorsNumber;
                    break;
                case BracketPricingTypes.WiresNumber:
                    levels = Configurator.WiresNumber;
                    break;
            }

            return levels;
        }
    }
}
