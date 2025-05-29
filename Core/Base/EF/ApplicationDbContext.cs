using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Core.Base.EF;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // بارگذاری تمام کانفیگ‌های IEntityTypeConfiguration<T> که در اسمبلی جاری وجود دارد
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // اگر می‌خواهید داده‌های اولیه (Seed) وارد کنید، کامنت‌ها را باز کنید
        //modelBuilder.Entity<City>().HasData(CitySeed.All);

        base.OnModelCreating(modelBuilder);
    }
}