using PublicAddressBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicAddressBook.DTOs
{
	namespace ContactDTOs
	{
		public class ContactDTO
		{
			[Required]
			[StringLength(250)]
			public string Name { get; set; }
			[Required]
			public DateTime DateOfBirth { get; set; }
			[Required]
			[StringLength(250)]
			public string Address { get; set; }
			public List<string> PhoneNumbers { get; set; }
		}
	}
}
