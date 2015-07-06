using DrinkDatabase.Controllers;
using DrinkDatabase.Models;
using DrinkDatabase.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure.AdamExtension;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;

namespace DrinkDatabase.Tests.Controllers
{
    [TestClass]
    public class DrinkControllerTest
    {
        IAppDBContext db;
        DrinkController dc;

        public DrinkControllerTest()
        {
            //do I get to have constructors for test classes?
            db = new FakeAppDBContext();
            dc = new DrinkController(db);
        }
        [TestMethod]
        public void AddValidDrink()
        {
            var d = getValidDrink();

            var result = dc.Create(d);
            var drinks = db.Query<Drink>().ToList();
            
            Assert.IsNotNull(drinks);
            Assert.IsTrue(drinks.Contains(d));
        }

        private static Drink getValidDrink()
        {

            var d = new Drink();
            d.Glass = "rofl";
            d.Instructions = "pour";
            d.Name = "glass of water";
            d.Notes = "delicious! I'm having one right now!";
            return d;
        }
        [TestMethod]
        public void AddInvalidDrink()
        {
            var d = new Drink();

            var result = dc.Create(d);
            var drinks = db.Query<Drink>().ToList();

            if(drinks != null)
            {
                Assert.IsFalse(drinks.Contains(d));
            }
        }
        [TestMethod]
        public void AddRedundantDrink_overridesOld()
        {
            var d1 = getValidDrink();
            var d2 = getValidDrink();

            var result1 = dc.Create(d1);
            var result2 = dc.Create(d2);
            var drinks = db.Query<Drink>().ToList();

            Assert.IsNotNull(drinks);
            Assert.IsFalse(drinks.Contains(d1));
            Assert.IsTrue(drinks.Contains(d2));
        }
        [TestMethod]
        public void RemoveValidDrink()
        {
            var d = getValidDrink();
            var result = dc.Create(d);

            var t = dc.DeleteConfirmed(d.ID);
            t.Wait();

            var drinks = db.Query<Drink>().ToList();
            Assert.IsFalse(drinks.Contains(d));
        }
        [TestMethod]
        public void RemoveInvalidDrink()
        {
            var d = getValidDrink();
            var drinks = db.Query<Drink>().ToList();
            Assert.IsFalse(drinks.Contains(d));

            var t = dc.DeleteConfirmed(d.ID);
            t.Wait();
            
            Assert.IsInstanceOfType(t.Result, typeof(System.Web.Mvc.HttpNotFoundResult));
        }
        [TestMethod]
        public void AddValidIngredientToDrink()
        {
            var d = getValidDrink();
            var result = dc.Create(d);
            result.Wait();
            var i = new Ingredient();
            i.Name = "water";
            db.Add<Ingredient>(i);
            
            result=dc.AddIngredient(d.ID, i.ID);
            result.Wait();
            
            Assert.IsInstanceOfType(result.Result, typeof(RedirectToRouteResult));

            Assert.IsNotNull(d.DrinkIngredients.Where(di => i.ID == di.IngredientID));
            Assert.IsNotNull(db.Query<DrinkIngredient>().Where(di => i.ID == di.IngredientID));
        }
        [TestMethod]
        public void AddInvalidIngredientToDrink()
        {
            var d = getValidDrink();
            var result = dc.Create(d);
            result.Wait();
            var i = new Ingredient();
            i.Name = "water";
            db.Add<Ingredient>(i);

            result = dc.AddIngredient(d.ID, i.ID);
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(HttpStatusCodeResult));
            Assert.IsTrue((result.Result as HttpStatusCodeResult).StatusCode == (int)HttpStatusCode.BadRequest);
            if(d.DrinkIngredients != null)
            {
                Assert.IsFalse(d.DrinkIngredients.Where(di => i.ID == di.IngredientID) != null);
            } 
            Assert.IsNull(db.Query<DrinkIngredient>().Where(di => di.IngredientID == i.ID));
        }
        [TestMethod]
        public void AddRedundantIngredientToDrink()
        {
            var d = getValidDrink();
            var result = dc.Create(d);
            result.Wait();
            var i = new Ingredient();
            i.Name = "water";
            db.Add<Ingredient>(i);

            result = dc.AddIngredient(d.ID, i.ID);
            result.Wait();
            result = dc.AddIngredient(d.ID, i.ID);
            result.Wait();

            //TODO:
            Assert.IsInstanceOfType(result.Result, (new HttpStatusCodeResult(HttpStatusCode.BadRequest).GetType()));
            Assert.IsTrue(db.Query<DrinkIngredient>().Where(di => di.IngredientID == i.ID).Count() == 1);
            Assert.IsTrue(d.DrinkIngredients.Where(di => di.IngredientID == i.ID).Count() == 1); 
        }
        [TestMethod]
        public void RemoveIngredientFromDrink()
        {
            var d = getValidDrink();
            var result = dc.Create(d);
            result.Wait();
            var i = new Ingredient();
            i.Name = "water";
            db.Add<Ingredient>(i);
            result = dc.AddIngredient(d.ID, i.ID);
            result.Wait();


            result = dc.RemoveIngredient(d.ID, i.ID);
            result.Wait();


            Assert.IsNull(db.Query<DrinkIngredient>().Where(di => di.IngredientID == i.ID));
            Assert.IsNull(d.DrinkIngredients.Where(di => di.IngredientID == i.ID));
        }
    }
}
