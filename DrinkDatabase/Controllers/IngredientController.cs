using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DrinkDatabase.Models;
using System.Data.Entity.Infrastructure.AdamExtension;

namespace DrinkDatabase.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        private IAppDBContext db;

        /// <summary>
        /// normal constructor - used for normal runtime.
        /// </summary>
        public IngredientController()
        {
            db = new ApplicationDbContext();
        }
        
        /// <summary>
        /// Abnormal constructor. Used for tests. Give it your own <param name="DBC">database context</param>, e.g., a test.
        /// </summary>
        public IngredientController(IAppDBContext DBC)
        {
            db = DBC;
        }

        // GET: Ingredient
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await db.Query<Ingredient>().ToListAsync());
        }

        // GET: Ingredient/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredient ingredient = await db.FindAsync<Ingredient>(id.GetValueOrDefault());
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View(ingredient);
        }

        // GET: Ingredient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ingredient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                db.Add(ingredient);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ingredient);
        }

        // GET: Ingredient/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredient ingredient = await db.FindAsync<Ingredient>(id.GetValueOrDefault());
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View(ingredient);
        }

        // POST: Ingredient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingredient).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ingredient);
        }

        // GET: Ingredient/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredient ingredient = await db.FindAsync<Ingredient>(id.GetValueOrDefault());
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            return View(ingredient);
        }

        // POST: Ingredient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ingredient ingredient = await db.FindAsync<Ingredient>(id);
            db.Remove(ingredient);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
