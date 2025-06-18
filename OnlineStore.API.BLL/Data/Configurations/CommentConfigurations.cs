using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineStore.API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Repository.Data.Configurations
{
    public class CommentConfigurations : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(e => e.product)
               .WithMany()
               .HasForeignKey(e => e.productId).IsRequired();

            builder.HasOne(e => e.User)
               .WithMany()
               .HasForeignKey(e => e.UserId).IsRequired();

            builder.Property(c => c.Text).IsRequired().HasMaxLength(500);
        }
    }
}
