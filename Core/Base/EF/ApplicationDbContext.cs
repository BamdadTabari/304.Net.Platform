using Core.EntityFramework.Models;
using Core.EntityFramework.Seed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

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
		modelBuilder.Entity<UserRole>().HasData(UserRoleSeed.All);
		modelBuilder.Entity<Role>().HasData(RoleSeed.All);
		modelBuilder.Entity<User>().HasData(UserSeed.All);
		modelBuilder.Entity<Option>().HasData(OptionSeed.All);
		modelBuilder.Entity<Province>().HasData(ProvinceSeed.All);
		modelBuilder.Entity<City>().HasData(CitySeed.All);

		base.OnModelCreating(modelBuilder);
	}
}