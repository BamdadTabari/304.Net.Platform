using Core.Base.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EntityFramework.Models;
public class Blog : BaseEntity
{
	public string description { get; set; }
	public string image { get; set; }
	public string image_alt { get; set; }
	public string blog_text { get; set; }
	public bool show_blog { get; set; }
	public string keywords { get; set; }
    public string meta_description { get; set; }
    public int estimated_read_time { get; set; }

    public long blog_category_id { get; set; }
	public BlogCategory blog_category { get; set; }
}
public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
	public void Configure(EntityTypeBuilder<Blog> builder)
	{
		builder.HasKey(x => x.id);
		builder.Property(x => x.description).IsRequired();
		builder.Property(x => x.name).IsRequired();
		builder.Property(x => x.slug).IsRequired();
		builder.Property(x => x.image).IsRequired();
		builder.Property(x => x.blog_text).IsRequired();
		builder.Property(x => x.keywords).IsRequired();
		builder.HasIndex(x => x.slug).IsUnique();

		builder.HasOne(x => x.blog_category)
			.WithMany(x => x.blogs)
			.HasForeignKey(x => x.blog_category_id)
			.OnDelete(DeleteBehavior.Cascade);

	}
}