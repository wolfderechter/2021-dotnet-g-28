using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class ContactPersonConfiguration : IEntityTypeConfiguration<ContactPerson>
    {
        public void Configure(EntityTypeBuilder<ContactPerson> builder)
        {
            builder.ToTable("ContactPerson");
            builder.HasKey(t => t.Email);
            //builder.Property(t => t.Compa)
            
            //builder.HasOne(t => t.Customer).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
