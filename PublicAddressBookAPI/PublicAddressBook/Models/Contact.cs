using PublicAddressBook.DTOs.ContactDTOs;
using System;
using System.Collections.Generic;

namespace PublicAddressBook.Models
{
	public class Contact
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Address { get; set; }

		public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
	}

	public static class ContactExtensionMethods
	{
		public static ContactDTO ConvertToContactDTO(this Contact contact)
		{
			List<string> newPhoneNumbers = new List<string>();
			ContactDTO newContactDTO = new ContactDTO{
				Name = contact.Name,
				DateOfBirth = contact.DateOfBirth,
				Address = contact.Address
			};

			if (contact.PhoneNumbers != null)
			{
				foreach (PhoneNumber phoneNumber in contact.PhoneNumbers)
				{
					if (phoneNumber != null)
					{
						newPhoneNumbers.Add(phoneNumber.Number);
					}
				}
			}

			newContactDTO.PhoneNumbers = newPhoneNumbers;

			return newContactDTO;
		}

		public static List<ContactDTO> ConvertToContactDTOs(this List<Contact> contacts)
		{
			List<ContactDTO> contactDTOs = new List<ContactDTO>();
			contacts.ForEach(p => contactDTOs.Add(p.ConvertToContactDTO()));

			return contactDTOs;
		}
	}
}
