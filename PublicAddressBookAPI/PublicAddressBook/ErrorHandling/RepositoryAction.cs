using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicAddressBook.ErrorHandling
{
	public class RepositoryAction
	{
		public bool Success { get; set; }
		public string Message { get; set; }

		public RepositoryAction(bool success, string message = null)
		{
			Success = success;
			Message = message;
		}

		public static implicit operator bool(RepositoryAction action)
		{
			return action.Success;
		}
	}

	public class RepositoryResult<T> : RepositoryAction
	{
		public T Value { get; }

		public RepositoryResult(bool success, T value, string message = null) : base(success, message)
		{
			Value = value;
		}
	}
}
