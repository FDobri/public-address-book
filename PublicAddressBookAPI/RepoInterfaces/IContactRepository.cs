using PublicAddressBook.DTOs.ContactDTOs;
using PublicAddressBook.DTOs.PhoneNumberDTOs;
using PublicAddressBook.ErrorHandling;
using PublicAddressBook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicAddressBook.RepoInterfaces
{
	public interface IContactRepository
	{
		Task<RepositoryAction> AddNewContacts(List<ContactDTO> contactDTO);
		Task<RepositoryAction> AddPhoneNumberToContact(int id, PhoneNumberDTO phoneNumberDTO);
		Task<RepositoryAction> RemovePhoneNumberFromContact(int id, string number);
		Task<RepositoryResult<List<ContactDTO>>> GetAllContacts();
		Task<RepositoryResult<List<ContactDTO>>> GetContactsPaged(int page);
		Task<RepositoryResult<List<ContactDTO>>> GetContactsContainingName(string name);
		Task<RepositoryResult<List<ContactDTO>>> GetContactsContainingAddress(string address);
		Task<RepositoryResult<List<ContactDTO>>> GetContactsByKeyword(string keyword);
		Task<RepositoryResult<ContactDTO>> GetContactById(int id);
		Task<RepositoryResult<List<PhoneNumberDTO>>> GetContactNumbers(int id);
		Task<RepositoryResult<PhoneNumberDTO>> GetContactNumber(int id, string number);
		Task<RepositoryAction> UpdateContact(int id, ContactDTO contactDTO);
		Task<RepositoryAction> DeleteContact(int id);
	}
}
