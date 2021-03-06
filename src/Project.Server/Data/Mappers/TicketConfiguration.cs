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
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(t => t.TicketNr);
            builder.Property(t => t.Attachments)
             .HasConversion(
                 v => JsonConvert.SerializeObject(v),
                 v => JsonConvert.DeserializeObject<List<string>>(v));
            //builder.HasMany(t => t.Reactions).WithOne(t => t.TicketId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            //builder.HasMany(t => t.ContactPersons).WithOne(t => t.Company).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
