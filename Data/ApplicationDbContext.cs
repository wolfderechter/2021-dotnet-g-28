using _2021_dotnet_g_28.Data.Mappers;
using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2021_dotnet_g_28.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<SupportManager> SupportManagers { get; set; }
        
        public DbSet<ContactPerson> contactPeople { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new ContactPersonConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new SupportManagerConfiguration());
        }

    }
}
