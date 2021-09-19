using PublicAddressBook.DTOs.PhoneNumberDTOs;
using System.Collections.Generic;

namespace PublicAddressBook.Models
{
	public class PhoneNumber
	{
		public int Id { get; set; }
		public string Number { get; set; }

		public Contact Contact { get; set; }
		public int ContactId { get; set; }
	}

	public static class PhoneNumberExtensionMethods
	{
		public static PhoneNumberDTO ConvertToPhoneNumberDTO(this PhoneNumber phoneNumber)
		{
			return new PhoneNumberDTO
			{
				Number = phoneNumber.Number
			};
		}

		public static List<PhoneNumberDTO> ConvertToPhoneNumberDTOs(this List<PhoneNumber> phoneNumbers)
		{
			List<PhoneNumberDTO> phoneNumberDTOs = new List<PhoneNumberDTO>();
			foreach (PhoneNumber phoneNumber in phoneNumbers)
			{
				phoneNumberDTOs.Add(new PhoneNumberDTO
				{
					Number = phoneNumber.Number
				});
			}

			return phoneNumberDTOs;
		}
	}
}
