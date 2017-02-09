using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class Locale
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string Tag { get; set; }

        public string Text { get; set; }
    }
}
