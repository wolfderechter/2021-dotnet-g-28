using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class SupportManagerConfiguration : IEntityTypeConfiguration<SupportManager>
    {
        public void Configure(EntityTypeBuilder<SupportManager> builder)
        {
            builder.ToTable("SupportManagers");
            builder.HasKey(t => t.Id);

            
        }
    }
}
