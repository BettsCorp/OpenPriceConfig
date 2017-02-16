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

        [Display(Name = "Max floor number")]
        public int FloorsNumber { get; set; }

        [Display(Name = "Max wires number")]
        public int WiresNumber { get; set; }

        public virtual ICollection<Option> Options { get; set; }
    }
}
