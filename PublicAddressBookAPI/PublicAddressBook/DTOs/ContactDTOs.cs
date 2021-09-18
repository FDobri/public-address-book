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

		public static class ContactDTOHelper
		{
			public static Contact ToContactDTO(this ContactDTO contactDTO, List<PhoneNumber> phoneNumbers)
			{
				return new Contact
				{
					Name = contactDTO.Name,
					DateOfBirth = contactDTO.DateOfBirth,
					Address = contactDTO.Address,
					PhoneNumbers = phoneNumbers
				};
			}
		}
	}
}
