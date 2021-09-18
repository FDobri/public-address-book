using System.ComponentModel.DataAnnotations;

namespace PublicAddressBook.DTOs
{
	namespace PhoneNumberDTOs
	{
		public class PhoneNumberDTO
		{
			[Required]
			[StringLength(20)]
			public string Number { get; set; }
		}
	}
}
