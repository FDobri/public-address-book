using Microsoft.EntityFrameworkCore;
using PublicAddressBook.Data;
using PublicAddressBook.DTOs.ContactDTOs;
using PublicAddressBook.DTOs.PhoneNumberDTOs;
using PublicAddressBook.ErrorHandling;
using PublicAddressBook.Models;
using PublicAddressBook.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicAddressBook.Repositories
{
	public class ContactRepository : IContactRepository
	{
		private const int NUM_CONTACTS_PER_PAGE = 20;

		private PABContext _context;

		public ContactRepository(PABContext context)
		{
			_context = context;
		}

		public async Task<RepositoryAction> AddNewContacts(List<ContactDTO> contactDTOs)
		{
			foreach (ContactDTO contactDTO in contactDTOs)
			{
				if (await _context.Contact.FirstOrDefaultAsync(p => p.Name == contactDTO.Name && p.Address == contactDTO.Address) != null)
				{
					return new RepositoryAction(false, $"Already found contact with name '{contactDTO.Name}' and address '{contactDTO.Address}'.");
				}

				List<PhoneNumber> phoneNumbers = null;
				if (contactDTO.PhoneNumbers != null)
				{
					phoneNumbers = new List<PhoneNumber>();
					foreach (string phoneNumber in contactDTO.PhoneNumbers.Distinct().ToList())
					{
						phoneNumbers.Add(new PhoneNumber
						{
							Number = phoneNumber
						});
					}
				}
				else
				{
					return new RepositoryAction(false, "Unable to add a contact without a phone number.");
				}

				Contact newContact = new Contact
				{
					Name = contactDTO.Name,
					DateOfBirth = contactDTO.DateOfBirth,
					Address = contactDTO.Address,
					PhoneNumbers = phoneNumbers
				};

				_context.Contact.Add(newContact);
				await _context.SaveChangesAsync();
			}

			return new RepositoryAction(true, "Contact(s) successfully added.");
		}

		public async Task<RepositoryAction> AddPhoneNumberToContact(int contactID, PhoneNumberDTO phoneNumberDTO)
		{
			Contact existingContact = await _context.Contact.FirstOrDefaultAsync(p => p.Id == contactID);

			if (existingContact == null)
			{
				return new RepositoryAction(false, $"User with id '{contactID}' not found.");
			}

			if (await _context.PhoneNumber.FirstOrDefaultAsync(p => p.Number == phoneNumberDTO.Number && p.ContactId == contactID) != null)
			{
				return new RepositoryAction(false, $"Number '{phoneNumberDTO.Number}' already exists for contact '{existingContact.Name}'");
			}

			_context.PhoneNumber.Add(new PhoneNumber
			{
				Number = phoneNumberDTO.Number,
				Contact = existingContact
			});

			await _context.SaveChangesAsync();
			return new RepositoryAction(true, $"Successfully added number '{phoneNumberDTO.Number}' to contact with id '{contactID}'");
		}

		public async Task<RepositoryAction> RemovePhoneNumberFromContact(int contactID, string number)
		{
			Contact existingContact = await _context.Contact.FirstOrDefaultAsync(p => p.Id == contactID);

			if (existingContact == null)
			{
				return new RepositoryAction(false, $"User with id '{contactID}' not found.");
			}

			PhoneNumber existingNumber = await _context.PhoneNumber.FirstOrDefaultAsync(p => p.Number == number && p.Contact == existingContact);
			
			if (existingNumber == null)
			{
				return new RepositoryAction(false, $"Phone number '{number}' for user '{existingContact.Name}' not found.");
			}

			_context.PhoneNumber.Remove(existingNumber);
			await _context.SaveChangesAsync();
			return new RepositoryAction(true, $"Successfully removed phone number '{number}'.");
		}

		public async Task<RepositoryResult<List<ContactDTO>>> GetAllContacts()
		{
			List<Contact> contacts = await _context.Contact.Include(p => p.PhoneNumbers).ToListAsync();

			if (contacts == null)
			{
				return new RepositoryResult<List<ContactDTO>>(false, null, "No contacts to fetch.");
			}

			return new RepositoryResult<List<ContactDTO>>(true, contacts.ConvertToContactDTOs(), "Successfully fetched contacts.");
		}

		/// <summary>
		/// Returns a sorted list (first by name, then by address, in ascending order)
		/// </summary>
		public async Task<RepositoryResult<List<ContactDTO>>> GetContactsPaged(int page)
		{
			List<Contact> contacts = await _context.Contact.Include(p => p.PhoneNumbers).OrderBy(p => p.Name).ThenBy(p => p.Address).ToListAsync();

			int beginIndex = Math.Clamp((page - 1) * NUM_CONTACTS_PER_PAGE, 0, int.MaxValue);
			if (beginIndex > contacts.Count || page < 1)
			{
				return new RepositoryResult<List<ContactDTO>>(false, null, $"Page '{page}' does not exist.");
			}

			int endIndex = Math.Clamp(page * NUM_CONTACTS_PER_PAGE, beginIndex, contacts.Count);

			List<ContactDTO> pagedContacts = new List<ContactDTO>();

			for (int i = beginIndex; i < endIndex; i++)
			{
				if (contacts[i] != null)
				{
					pagedContacts.Add(contacts[i].ConvertToContactDTO());
				}
			}

			return new RepositoryResult<List<ContactDTO>>(true, pagedContacts, $"Successfully fetched contacts on page '{page}'");
		}

		public async Task<RepositoryResult<List<ContactDTO>>> GetContactsContainingName(string name)
		{
			List<Contact> contacts = await _context.Contact.Include(p => p.PhoneNumbers).Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();

			if (contacts.Count == 0)
			{
				return new RepositoryResult<List<ContactDTO>>(true, contacts.ConvertToContactDTOs(), $"No contacts containing '{name}' name");
			}

			return new RepositoryResult<List<ContactDTO>>(true, contacts.ConvertToContactDTOs(), $"Successfully fetched contacts containing '{name}'");
		}

		public async Task<RepositoryResult<List<ContactDTO>>> GetContactsContainingAddress(string address)
		{
			List<Contact> contacts = await _context.Contact.Include(p => p.PhoneNumbers).Where(p => p.Address.ToLower().Contains(address.ToLower())).ToListAsync();

			if (contacts.Count == 0)
			{
				return new RepositoryResult<List<ContactDTO>>(true, contacts.ConvertToContactDTOs(), $"No contacts containing '{address}' address");
			}

			return new RepositoryResult<List<ContactDTO>>(true, contacts.ConvertToContactDTOs(), $"Successfully fetched contacts containing '{address}'");
		}

		public async Task<RepositoryResult<List<ContactDTO>>> GetContactsByKeyword(string keyword)
		{
			List<Contact> contacts = await _context.Contact.Include(p => p.PhoneNumbers).Where(p => p.Name.ToLower().Contains(keyword.ToLower()) || p.Address.ToLower().Contains(keyword.ToLower())).ToListAsync();

			if (contacts.Count == 0)
			{
				return new RepositoryResult<List<ContactDTO>>(true, contacts.ConvertToContactDTOs(), $"No contacts containing '{keyword}'");
			}

			return new RepositoryResult<List<ContactDTO>>(true, contacts.ConvertToContactDTOs(), $"Successfully fetched contacts containing '{keyword}'");
		}

		public async Task<RepositoryResult<ContactDTO>> GetContactById(int id)
		{
			Contact existingContact = await _context.Contact.Include(p => p.PhoneNumbers).FirstOrDefaultAsync(p => p.Id == id);

			if (existingContact == null)
			{
				return new RepositoryResult<ContactDTO>(false, null, $"Couldn't find contact with id '{id}'");
			}
			else
			{
				return new RepositoryResult<ContactDTO>(true, existingContact.ConvertToContactDTO(), $"Successfully fetched contact with id '{id}'");
			}
		}

		public async Task<RepositoryResult<List<PhoneNumberDTO>>> GetContactNumbers(int id)
		{
			Contact existingContact = await _context.Contact.Include(p => p.PhoneNumbers).FirstOrDefaultAsync(p => p.Id == id);

			if (existingContact == null)
			{
				return new RepositoryResult<List<PhoneNumberDTO>>(false, null, $"Couldn't find contact with id '{id}'");
			}

			List<PhoneNumber> phoneNumbers = await _context.PhoneNumber.Where(p => p.Contact == existingContact).ToListAsync();
			if (phoneNumbers.Count == 0)
			{
				return new RepositoryResult<List<PhoneNumberDTO>>(true, phoneNumbers.ConvertToPhoneNumberDTOs(), $"Couldn't find numbers for contact with id '{id}'");
			}
			else
			{
				return new RepositoryResult<List<PhoneNumberDTO>>(true, phoneNumbers.ConvertToPhoneNumberDTOs(), $"Successfully found numbers for contact with id '{id}'");
			}
		}

		public async Task<RepositoryResult<PhoneNumberDTO>> GetContactNumber(int id, string number)
		{
			Contact existingContact = await _context.Contact.Include(p => p.PhoneNumbers).FirstOrDefaultAsync(p => p.Id == id);

			if (existingContact == null)
			{
				return new RepositoryResult<PhoneNumberDTO>(false, null, $"Couldn't find contact with id '{id}'");
			}

			PhoneNumber phoneNumber = await _context.PhoneNumber.FirstOrDefaultAsync(p => p.Contact == existingContact && p.Number == number);
			if (phoneNumber == null)
			{
				return new RepositoryResult<PhoneNumberDTO>(false, null, $"Couldn't find number '{number}' for contact with id '{id}'");
			}

			return new RepositoryResult<PhoneNumberDTO>(true, phoneNumber.ConvertToPhoneNumberDTO(), $"Couldn't find number '{number}' for contact with id '{id}'");
		}

		public async Task<RepositoryAction> UpdateContact(int id, ContactDTO contactDTO)
		{
			Contact existingContact = await _context.Contact.Include(p => p.PhoneNumbers).FirstOrDefaultAsync(p => p.Id == id);

			if (existingContact == null)
			{
				return new RepositoryAction(false, $"Couldn't find contact with id '{id}'");
			}

			if (await _context.Contact.FirstOrDefaultAsync(p => p.Name == contactDTO.Name && p.Address == contactDTO.Address && p != existingContact) != null)
			{
				return new RepositoryAction(false, $"Contact with name '{contactDTO.Name}' and address '{contactDTO.Address}' already exists.");
			}

			List<PhoneNumber> phoneNumbers = null;
			if (contactDTO.PhoneNumbers != null)
			{
				phoneNumbers = new List<PhoneNumber>();
				foreach (string phoneNumber in contactDTO.PhoneNumbers.Distinct().ToList())
				{
					phoneNumbers.Add(new PhoneNumber
					{
						Number = phoneNumber,
						Contact = existingContact
					});
				}
			}
			else
			{
				return new RepositoryAction(false, "Unable to edit a contact without a phone number.");
			}

			existingContact.Name = contactDTO.Name;
			existingContact.DateOfBirth = contactDTO.DateOfBirth;
			existingContact.Address = contactDTO.Address;
			existingContact.PhoneNumbers.Clear();
			existingContact.PhoneNumbers = phoneNumbers;

			await _context.SaveChangesAsync();

			return new RepositoryAction(true, $"Contact '{contactDTO.Name}' successfully updated.");
		}

		public async Task<RepositoryAction> DeleteContact(int id)
		{
			Contact existingContact = await _context.Contact.FirstOrDefaultAsync(p => p.Id == id);

			if (existingContact != null)
			{
				List<PhoneNumber> phoneNumbers = await _context.PhoneNumber.Where(p => p.Contact == existingContact).ToListAsync();
				_context.RemoveRange(phoneNumbers);
				_context.Remove(existingContact);
				await _context.SaveChangesAsync();
				return new RepositoryAction(true, $"Successfully removed contact with id: '{id}'");
			}
			else
			{
				return new RepositoryAction(false, $"Unable to find contact with id: '{id}'");
			}
		}
	}
}
