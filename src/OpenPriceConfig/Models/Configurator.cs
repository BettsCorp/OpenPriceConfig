using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OpenPriceConfig.Models
{
    public class Configurator
    {
        public int ID { get; set; }
        [StringLength(60)]
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public int FloorsNumber { get; set; }

        public virtual ICollection<Option> Options { get; set; }
    }
}
