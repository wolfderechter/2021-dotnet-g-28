using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");
            builder.HasKey(t => t.CompanyNr);
            builder.HasMany(t => t.Contracts).WithOne(t => t.Company).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(t => t.ContactPersons).WithOne(t => t.Company).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(t => t.Tickets).WithOne().IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
