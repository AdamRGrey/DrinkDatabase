using DrinkDatabase.Controllers;
using DrinkDatabase.Models;
using DrinkDatabase.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure.AdamExtension;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkDatabase.Tests.Controllers
{
    [TestClass]
    public class DrinkControllerTest
    {
        IAppDBContext db;
        DrinkController dc;
        Drink d;

        public DrinkControllerTest()
        {
            //do I get to have constructors for test classes?
            db = new FakeAppDBContext();
            dc = new DrinkController(db);
        }
        [TestMethod]
        public void AddValidDrink()
        {
            d = new Drink();
            d.Glass = "rofl";
            d.Instructions = "pour";
            d.Name = "glass of water";
            d.Notes = "delicious! I'm having one right now!";

            var result = dc.Create(d);
            var drinks = db.Query<Drink>().ToList();
            
            Assert.IsNotNull(drinks);
            Assert.IsTrue(drinks.Contains(d));
        }
        [TestMethod]
        public void AddInvalidDrink()
        {
            //TODO:
        }
        [TestMethod]
        public void AddRedundantDrink()
        {
            //TODO:
        }
        [TestMethod]
        public void RemoveValidDrink()
        {
            AddValidDrink();

            Task t= dc.DeleteConfirmed(d.ID);
            t.Wait();

            var drinks = db.Query<Drink>().ToList();
            Assert.IsFalse(drinks.Contains(d));
        }
        [TestMethod]
        public void RemoveInvalidDrink()
        {
            //TODO:
        }
        [TestMethod]
        public void AddValidIngredientToDrink()
        {
            //TODO:
        }
        [TestMethod]
        public void AddInvalidIngredientToDrink()
        {
            //TODO:
        }
        [TestMethod]
        public void AddRedundantIngredientToDrink()
        {
            //TODO:
        }
        [TestMethod]
        public void RemoveIngredientFromDrink()
        {
            //TODO:
        }
    }
}
