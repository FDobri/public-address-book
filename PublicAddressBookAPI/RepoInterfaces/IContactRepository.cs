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
		Task<RepositoryAction> AddPhoneNumberToContact(int contactID, PhoneNumberDTO phoneNumberDTO);
		Task<RepositoryAction> RemovePhoneNumberFromContact(int contactID, string number);
		Task<RepositoryResult<List<ContactDTO>>> GetAllContacts();
		Task<RepositoryResult<List<ContactDTO>>> GetContactsPaged(int page);
		Task<RepositoryResult<List<ContactDTO>>> GetContactsContainingName(string name);
		Task<RepositoryResult<List<ContactDTO>>> GetContactsContainingAddress(string address);
		Task<RepositoryResult<List<ContactDTO>>> GetContactsByKeyword(string keyword);
		Task<RepositoryResult<ContactDTO>> GetContactById(int id);
		Task<RepositoryAction> UpdateContact(int contactId, ContactDTO contactDTO);
		Task<RepositoryAction> DeleteContact(int contactId);
	}
}
