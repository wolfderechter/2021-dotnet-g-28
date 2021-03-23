﻿using _2021_dotnet_g_28.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Data.Mappers
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        
public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(r => r.ReactionId);
            
        }
    }
}
