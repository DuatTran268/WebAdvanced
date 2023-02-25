using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(p => p.Id);  // id la khoa chinh
        builder.Property(p => p.Name)
            .HasMaxLength(50)   // toi da 50 ky tu
            .IsRequired();  // not null

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.UrlSlug)
            .HasMaxLength(50)
            .IsRequired();  // not null

        builder.Property(p => p.ShowOnMenu)
            .IsRequired()
            .HasDefaultValue(false);


        // error create database



    }
}
