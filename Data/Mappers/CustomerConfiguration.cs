using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(t => t.CustomerNr);
            builder.HasMany(t => t.Contracts).WithOne(t => t.Customer).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(t => t.ContactPersons).WithOne(t => t.Customer).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
