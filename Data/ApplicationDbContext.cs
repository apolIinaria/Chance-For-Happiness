using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChanceForHappiness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ChanceForHappiness.Data
{
    /// <summary>
    /// Основний клас контексту бази даних для застосунку,
    /// який наслідується від DbContext із Entity Framework Core.
    /// Надає доступ до таблиць та визначає їх структуру.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        /// <summary>
        /// Метод налаштування моделі даних - визначає структуру таблиць,
        /// зв'язки між ними та обмеження на поля
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Adoption>()
                .HasOne(a => a.Animal)
                .WithMany()
                .HasForeignKey(a => a.AnimalId);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Adoption>()
                .Property(a => a.ApplicantName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Adoption>()
                .Property(a => a.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Adoption>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Volunteer>()
                .Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Volunteer>()
                .Property(v => v.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Volunteer>()
                .Property(v => v.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}