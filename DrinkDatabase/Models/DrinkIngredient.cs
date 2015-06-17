using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DrinkDatabase.Models
{
    /// <summary>
    /// There isn't really a good way to model an object's arbitrarily sized contained map. So we're stuck making a separate object. /shrug, could be worse.
    /// </summary>
    public class DrinkIngredient
    {
        [Key]
        public int ID { get; set;}

        /// <summary>
        /// which <see cref="Ingredient"/> we are. Really we only care about the name
        /// </summary>
        public int IngredientID { get; set; }
        /// <summary>
        /// which <see cref="Drink"/> we go in to
        /// </summary>
        public int DrinkID { get; set; }
        [DisplayFormat(NullDisplayText="1 part")]
        /// <summary>
        /// how much of the <see cref="Ingredient"/> to include in this <see cref="Drink"/>. A string so that we can have arbitrary units, like "1 drop" or "1 shot" or "half a can"
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// e.g., one makes a 7&7 with "7-up" brand lemon-lime soda and "seagram's 7" brand american whiskey. Theoretically there's no reason you couldn't use brand-x equivalents. (call it a 6.9&7ish, lol)
        /// </summary>
        [DisplayFormat(NullDisplayText = "(any)")]
        public string Brand { get; set; }
    }
}