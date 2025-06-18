using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.BLL.Data.Configurations
{
    public class ProductConfiguretions : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Property(p => p.TypeId).IsRequired(false);
            builder.Property(p => p.ApplicationUserId).IsRequired(true);
            builder.Property(p => p.Name).IsRequired(true);
            builder.Property(p => p.Description).IsRequired(true);
            builder.Property(p => p.PictureUrl).IsRequired(true);
            builder.Property(p => p.OldPrice).IsRequired(true);
            builder.Property(p => p.NewPrice).IsRequired(true);

        }
    }
}
