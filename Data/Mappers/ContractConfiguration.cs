using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contract");
            builder.HasKey(t => t.ContractNr);
            //builder.HasOne(c => c.Type).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
            //builder.Property(t => t.Compa)
            //builder.HasOne(t => t.Customer).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
