using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Delivery.Models
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

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Delivery.Models.Users> Users { get; set; }
        public System.Data.Entity.DbSet<Delivery.Models.Orders> Orders { get; set; }
        public System.Data.Entity.DbSet<Delivery.Models.Locations> Locations { get; set; }
        public System.Data.Entity.DbSet<Delivery.Models.Packages> Packages { get; set; }
        public System.Data.Entity.DbSet<Delivery.Models.Rewards> Rewards { get; set; }
        public System.Data.Entity.DbSet<Delivery.Models.OrderCompetitors> OrderCompetitors { get; set; }
        public System.Data.Entity.DbSet<Delivery.Models.Addresses> Addresses { get; set; }
    }
}