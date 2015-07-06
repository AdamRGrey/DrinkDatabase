using DrinkDatabase.Controllers;
using DrinkDatabase.Models;
using DrinkDatabase.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure.AdamExtension;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DrinkDatabase.Tests.Controllers
{
    [TestClass]
    public class IngredientControllerTest
    {
        IAppDBContext db;
        IngredientController ic;

        public IngredientControllerTest()
        {
            db = new FakeAppDBContext();
            ic = new IngredientController(db);
        }
        [TestMethod]
        public void AddValidIngredient()
        {
            Ingredient i = new Ingredient();
            i.Name = "water";

            var result = ic.Create(i);
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(RedirectToRouteResult));
            Assert.IsNotNull(db.Query<Ingredient>());
            Assert.IsNotNull(db.Find<Ingredient>(i.ID));
        }
        [TestMethod]
        public void AddRedundantIngredient()
        {
            Ingredient i = new Ingredient();
            i.Name = "water";
            Ingredient i2 = new Ingredient();
            i2.Name = "water";

            var result = ic.Create(i);
            result.Wait();
            result = ic.Create(i2);
            result.Wait();

            Assert.IsInstanceOfType(result.Result, typeof(HttpStatusCodeResult));
            Assert.IsTrue((result.Result as HttpStatusCodeResult).StatusCode == (int)HttpStatusCode.BadRequest);
            Assert.IsTrue(db.Query<Ingredient>().ToList().Contains(i));
            Assert.IsFalse(db.Query<Ingredient>().ToList().Contains(i2));
        }
        [TestMethod]
        public void RemoveValidIngredient()
        {
            //TODO:
            Assert.Fail("not implemented");
        }
        [TestMethod]
        public void RemoveInvalidIngredient()
        {
            //TODO:
            Assert.Fail("not implemented");
        }
        [TestMethod]
        public void EditIngredient()
        {
            //TODO:
            Assert.Fail("not implemented");
        }
    }
}
