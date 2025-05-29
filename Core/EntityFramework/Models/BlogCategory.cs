using Core.Base.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EntityFramework.Models;
public class BlogCategory : BaseEntity
{
    public string? description { get; set; }

    public ICollection<Blog> blogs { get; set; }
}

public class BlogCategoryConfiguration : IEntityTypeConfiguration<BlogCategory>
{
    public void Configure(EntityTypeBuilder<BlogCategory> builder)
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.name).IsRequired();
        builder.Property(x => x.slug).IsRequired();
        builder.HasIndex(x => x.slug).IsUnique();
    }
}