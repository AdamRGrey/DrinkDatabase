using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DrinkDatabase.Models
{
    public class Drink
    {
        [Key]
        public int ID { get; set; }
        [StringLength(128, MinimumLength=3)]
        public string Name { get; set; }
        public string Instructions { get; set; }
        [StringLength(128, MinimumLength = 3)]
        public string Glass { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<DrinkIngredient> DrinkIngredients { get; set; }
    }
}