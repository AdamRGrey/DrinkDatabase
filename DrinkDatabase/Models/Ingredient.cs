using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DrinkDatabase.Models
{
    /// <summary>
    /// effectively just its <see cref="Name"/>, but this way we can be sure to have a dropdown list and avoid typos.
    /// </summary>
    public class Ingredient
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
    }
}