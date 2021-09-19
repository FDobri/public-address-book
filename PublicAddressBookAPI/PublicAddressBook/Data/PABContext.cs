using Microsoft.EntityFrameworkCore;
using PublicAddressBook.Models;

namespace PublicAddressBook.Data
{
	public class PABContext : DbContext
	{
		public PABContext(DbContextOptions<PABContext> options) : base(options)
		{

		}

		public DbSet<Contact> Contact { get; set; }
		public DbSet<PhoneNumber> PhoneNumber { get; set; }
	}
}
