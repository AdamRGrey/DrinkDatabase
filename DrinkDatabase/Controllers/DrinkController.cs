using DrinkDatabase.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.AdamExtension;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DrinkDatabase.Controllers
{
    [Authorize]
    public class DrinkController : Controller
    {
        private IAppDBContext db;

        /// <summary>
        /// normal constructor - used for normal runtime.
        /// </summary>
        public DrinkController()
        {
            db = new ApplicationDbContext();
        }
        /// <summary>
        /// Abnormal constructor. Used for tests. Give it your own <param name="DBC">database context</param>, e.g., a test.
        /// </summary>
        public DrinkController(IAppDBContext DBC)
        {
            db = DBC;
        }
        // GET: Drink
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await db.Query<Drink>().ToListAsync());
        }

        // GET: Drink/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drink drink = await db.Query<Drink>().Include(d => d.DrinkIngredients).Where(d => d.ID == id).SingleAsync();
            if (drink == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.DrinkIngredients = drink.DrinkIngredients;
            return View(drink);
        }

        // GET: Drink/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drink/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Instructions,Glass,Notes")] Drink drink)
        {
            if (ModelState.IsValid)
            {
                db.Add(drink);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(drink);
        }

        // GET: Drink/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drink drinkToUpdate = await db.Query<Drink>()
                .Include(d => d.DrinkIngredients)
                .Where(d => d.ID == id)
                .SingleAsync();
            if (drinkToUpdate == null)
            {
                return HttpNotFound();
            }
            if (drinkToUpdate.DrinkIngredients == null)
            {
                drinkToUpdate.DrinkIngredients = new HashSet<DrinkIngredient>();
            }
            ViewBag.DrinkIngredients = drinkToUpdate.DrinkIngredients;
            return View(drinkToUpdate);
        }

        // POST: Drink/Edit/n
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Instructions,Glass,Notes")] Drink drink,
            [Bind(Include= "ID,Amount,Brand,IngredientID,DrinkID")] IEnumerable<DrinkIngredient> DrinkIngredients)
        {
            if (ModelState.IsValid)
            {
                var whichToDelete = Request.Form.AllKeys.Where(s => s.Contains("DeleteDrinkIngredients["));
                List<int> deletionList = new List<int>();
                foreach (var s in whichToDelete)
                {
                    var firstBracket = s.IndexOf('[') + 1;
                    int thisIndex;
                    if (int.TryParse(s.Substring(firstBracket, s.IndexOf(']', firstBracket) - firstBracket), out thisIndex))
                    {
                        if(Request.Form[s] == "on")
                            deletionList.Add(thisIndex);
                    }
                }

                db.Entry(drink).State = EntityState.Modified;
                if (DrinkIngredients != null)
                {
                    foreach (var item in DrinkIngredients)
                    {
                        if(deletionList.Contains(item.ID))
                        {
                            drink.DrinkIngredients.Remove(item);
                            db.Entry(item).State = EntityState.Deleted;
                        }
                        else
                        {
                            db.Entry(item).State = EntityState.Modified;
                        }
                    }
                }
                await db.SaveChangesAsync();


                if (Request.IsAjaxRequest())
                {
                    ViewBag.DrinkIngredients = drink.DrinkIngredients;
                    return PartialView("_EditableDrink", drink);
                }
                return RedirectToAction("Index");
            }
            return View(drink);
        }

        // GET: Drink/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drink drink = await db.Query<Drink>().FirstAsync(d => d.ID == id);
            if (drink == null)
            {
                return HttpNotFound();
            }
            return View(drink);
        }

        // POST: Drink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Drink drink = await db.FindAsync<Drink>(id);
            if (drink == null)
                return HttpNotFound("couldn't find the drink to remove");
            db.Remove(drink);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> AddIngredient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drink drink = await db.FindAsync<Drink>(id.GetValueOrDefault());
            if (drink == null)
            {
                return HttpNotFound();
            }
            var ingredients = db.Query<Ingredient>().OrderBy(q => q.Name).ToList();
            SelectList holdThis = new SelectList(ingredients, "ID", "Name", null);
            ViewData.Add("ingredientID", holdThis.AsEnumerable());

            return View(drink);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddIngredient(int id, int ingredientID)
        {
            Drink drink = await db.FindAsync<Drink>(id);
            if (drink == null)
                return HttpNotFound();

            Ingredient ingredient = db.Query<Ingredient>().First(i => i.ID == ingredientID);
            if (ingredient == null)
                return HttpNotFound();
            if (drink.DrinkIngredients == null)
                drink.DrinkIngredients = new HashSet<DrinkIngredient>();
            if (drink.DrinkIngredients.Any(di => di.IngredientID == ingredient.ID))
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            
            //conundrum. Either rewrite this to avoid using a feature so I can test it, or get rhinomocks (or something) involved.
            drink.DrinkIngredients.Add(new DrinkIngredient()
            {
                DrinkID = id,
                IngredientID = ingredient.ID
            });
            await db.SaveChangesAsync();

            return RedirectToAction("Edit/" + id);
        }
        [AllowAnonymous]
        public ActionResult DrinkIngredientDetails(int? id)
        {
            if (id == null)
                return HttpNotFound();
            DrinkIngredient di = db.Find<DrinkIngredient>(id.GetValueOrDefault());
            if (di == null)
                return HttpNotFound();

            var ingredient = db.Find<Ingredient>(di.IngredientID);
            if (ingredient == null)
                return HttpNotFound();
            ViewBag.ingredientName = ingredient.Name;

            return PartialView(di);
        }

        public ActionResult DrinkIngredientEdit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            DrinkIngredient di = db.Find<DrinkIngredient>(id.GetValueOrDefault());
            if (di == null)
                return HttpNotFound();

            var ingredient = db.Find<Ingredient>(di.IngredientID);
            if (ingredient == null)
                return HttpNotFound();

            var ingredients = db.Query<Ingredient>().OrderBy(q => q.Name).ToList();
            SelectList holdThis = new SelectList(ingredients, "ID", "Name", di.IngredientID);
            ViewData.Add("DrinkIngredients[" + id + "].ingredientID", holdThis.AsEnumerable());
            
            return PartialView(di);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public Task<ActionResult> RemoveIngredient(int p1, int p2)
        {
            throw new System.NotImplementedException();
        }
    }
}
