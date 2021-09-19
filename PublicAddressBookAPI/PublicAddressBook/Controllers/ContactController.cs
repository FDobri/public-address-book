using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicAddressBook.DTOs.ContactDTOs;
using PublicAddressBook.DTOs.PhoneNumberDTOs;
using PublicAddressBook.ErrorHandling;
using PublicAddressBook.Hubs;
using PublicAddressBook.RepoInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicAddressBook.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContactController : ControllerBase
	{
		private readonly IContactRepository _contactRepository;
		private readonly IHubContext<UpdateHub> _hubContext;

		public ContactController(IContactRepository contactRepository, IHubContext<UpdateHub> hubContext)
		{
			_contactRepository = contactRepository;
			_hubContext = hubContext;
		}

		[HttpPost]
		public async Task<IActionResult> CreateContact(ContactDTO contactDTO)
		{
			RepositoryAction action = await _contactRepository.AddNewContacts(new List<ContactDTO>() { contactDTO });

			if (!action)
			{
				return BadRequest(action.Message);
			}

			await _hubContext.Clients.All.SendAsync("UpdateContacts", _contactRepository.GetAllContacts().Result.Value);
			return Ok(action.Message);
		}

		[HttpPost("{contactID}/phoneNumber")]
		public async Task<IActionResult> AddPhoneNumberToContact(int contactID, PhoneNumberDTO phoneNumberDTO)
		{
			RepositoryAction action = await _contactRepository.AddPhoneNumberToContact(contactID, phoneNumberDTO);

			if (!action)
			{
				return BadRequest(action.Message);
			}

			await _hubContext.Clients.All.SendAsync("UpdateContacts", _contactRepository.GetAllContacts().Result.Value);
			return Ok(action.Message);
		}

		[HttpDelete("{contactID}/phoneNumber/{number}")]
		public async Task<IActionResult> RemovePhoneNumberFromContact(int contactID, string number)
		{
			RepositoryAction action = await _contactRepository.RemovePhoneNumberFromContact(contactID, number);

			if (!action)
			{
				return BadRequest(action.Message);
			}

			await _hubContext.Clients.All.SendAsync("UpdateContacts", _contactRepository.GetAllContacts().Result.Value);
			return Ok(action.Message);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllContacts()
		{
			RepositoryResult<List<ContactDTO>> result = await _contactRepository.GetAllContacts();

			if (!result)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Value);
		}

		[HttpGet("page/{page}")]
		public async Task<IActionResult> GetContactsPaged(int page)
		{
			RepositoryResult<List<ContactDTO>> result = await _contactRepository.GetContactsPaged(page);

			if (!result)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Value);
		}

		[HttpGet("containsName/{name}")]
		public async Task<IActionResult> GetContactsContainingName(string name)
		{
			RepositoryResult<List<ContactDTO>> result = await _contactRepository.GetContactsContainingName(name);

			if (!result)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Value);
		}

		[HttpGet("containsAddress/{address}")]
		public async Task<IActionResult> GetContactsContainingAddress(string address)
		{
			RepositoryResult<List<ContactDTO>> result = await _contactRepository.GetContactsContainingAddress(address);

			if (!result)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Value);
		}

		[HttpGet("search/{keyword}")]
		public async Task<IActionResult> GetContactsByKeyword(string keyword)
		{
			RepositoryResult<List<ContactDTO>> result = await _contactRepository.GetContactsByKeyword(keyword);

			if (!result)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Value);
		}
		
		[HttpGet("{id}")]
		public async Task<IActionResult> GetContactById(int id)
		{
			RepositoryResult<ContactDTO> result = await _contactRepository.GetContactById(id);

			if (!result)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Value);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateContact(int id, ContactDTO contactDTO)
		{
			RepositoryAction action = await _contactRepository.UpdateContact(id, contactDTO);

			if (!action)
			{
				return BadRequest(action.Message);
			}

			await _hubContext.Clients.All.SendAsync("UpdateContacts", _contactRepository.GetAllContacts().Result.Value);
			return Ok(action.Message);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContact(int id)
		{
			RepositoryAction action = await _contactRepository.DeleteContact(id);

			if (!action)
			{
				return BadRequest(action.Message);
			}

			await _hubContext.Clients.All.SendAsync("UpdateContacts", _contactRepository.GetAllContacts().Result.Value);
			return Ok(action.Message);
		}
	}
}
