using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class FaqConfiguration : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.ToTable("Faq");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Solution)
                         .HasConversion(
                             v => JsonConvert.SerializeObject(v),
                             v => JsonConvert.DeserializeObject<List<string>>(v));

        }
    }
}
