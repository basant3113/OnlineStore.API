using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Repository.Data.Configurations
{
    public class FavouriteConfigurations : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.HasKey(f => new { f.UserId, f.productId });

            builder.HasOne(f => f.product)
                   .WithMany()
                   .HasForeignKey(f => f.productId);


            builder.HasOne(f => f.User)
                   .WithMany()
                   .HasForeignKey(f => f.UserId);
        }
    }
}
