using ClinicWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 启用5个表
        public DbSet<DrAddress> DrAddresses { get; set; }

        public DbSet<DrName> DrNames { get; set; }

        public DbSet<Specialist> Specialists { get; set; }

        public DbSet<Title> Titles { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DrName>()
                .HasOne(d => d.DrAddr)
                .WithMany(p => p.DrNames)
                .HasForeignKey(d => d.DrAddrId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DrName>()
                .HasOne(d => d.Speciality)
                .WithMany(p => p.DrNames)
                .HasForeignKey(d => d.SpecialityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DrName>()
                .HasOne(d => d.Title)
                .WithMany(p => p.DrNames)
                .HasForeignKey(d => d.TitleId)
                .OnDelete(DeleteBehavior.Cascade);

            // 定义UserName Email唯一
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }

        
    }
}

