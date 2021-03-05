using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractType>
    {
        public void Configure(EntityTypeBuilder<ContractType> builder)
        {
            builder.ToTable("ContactTypes");
            builder.HasKey(c => c.Name);
        }
    }

}
