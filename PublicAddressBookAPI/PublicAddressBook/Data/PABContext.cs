using Microsoft.EntityFrameworkCore;
using PublicAddressBook.Models;

namespace PublicAddressBook.Data
{
	public class PABContext : DbContext
	{
		public PABContext(DbContextOptions<PABContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PhoneNumber>()
				.HasOne(p => p.Contact);

			modelBuilder.Entity<Contact>()
				.HasMany(p => p.PhoneNumbers);
		}

		public DbSet<Contact> Contact { get; set; }
		public DbSet<PhoneNumber> PhoneNumber { get; set; }
	}
}
