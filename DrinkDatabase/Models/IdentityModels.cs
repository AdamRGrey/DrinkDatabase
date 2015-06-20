using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.AdamExtension;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DrinkDatabase.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDBContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<DrinkDatabase.Models.Drink> Drinks { get; set; }

        public System.Data.Entity.DbSet<DrinkDatabase.Models.Ingredient> Ingredients { get; set; }

        public System.Data.Entity.DbSet<DrinkDatabase.Models.DrinkIngredient> DrinkIngredients { get; set; }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }

        /// <summary>
        /// Set<<typeparamref name="T"/>>().Add(<paramref name="target"/>)
        /// </summary>
        public void Add<T>(T target) where T : class
        {
            Set<T>().Add(target);
        }
        /// <summary>
        /// Set<<typeparamref name="T"/>>().Remove(<paramref name="target"/>)
        /// </summary>
        public void Remove<T>(T target) where T : class
        {
            Set<T>().Remove(target);
        }
        /// <summary>
        /// Set<<typeparamref name="T"/>>().Find(<paramref name="id"/>)
        /// </summary>
        public T Find<T>(params object[] keyValues) where T : class
        {
            return Set<T>().Find(keyValues);
        }
        /// <summary>
        /// Set<<typeparamref name="T"/>>().FindAsync(<paramref name="keyValues"/>)
        /// </summary>
        public Task<T> FindAsync<T>(params object[] keyValues) where T : class
        {
            return Set<T>().FindAsync(keyValues);
        }
    }
}