using _2021_dotnet_g_28.Data.Mappers;
using _2021_dotnet_g_28.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2021_dotnet_g_28.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<SupportManager> SupportManagers { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<ContactPerson> ContactPeople { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new ContactPersonConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new FaqConfiguration());
            modelBuilder.ApplyConfiguration(new SupportManagerConfiguration());
            modelBuilder.ApplyConfiguration(new ContractTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReactionConfiguration());
            
        }

    }
}
