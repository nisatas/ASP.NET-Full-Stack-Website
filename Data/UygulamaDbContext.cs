using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BeygirMuhendisi.Web.Models;

namespace BeygirMuhendisi.Web.Data
{
    public class UygulamaDbContext : IdentityDbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }

        public DbSet<Tahmin> Tahminler { get; set; }
    }
}