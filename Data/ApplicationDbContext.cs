using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KulinarstvoASP.Models;

namespace KulinarstvoASP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recept> Recepti {  get; set; }
        public DbSet<Sastojak> Sastojci { get; set; }
        public DbSet<ReceptSastojak> ReceptSastojci { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Komentar> Komentari {  get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReceptSastojak>()
                .HasKey(rs => new { rs.ReceptId, rs.SastojakId });

            modelBuilder.Entity<ReceptSastojak>()
                .HasOne(rs => rs.Recept)
                .WithMany(r => r.SastojciZaRecept)
                .HasForeignKey(rs => rs.ReceptId);

            modelBuilder.Entity<ReceptSastojak>()
                .HasOne(rs => rs.Sastojak)
                .WithMany(s => s.Recepti)
                .HasForeignKey(rs => rs.SastojakId);
        }
    }
}
