namespace PublicAddressBook.ErrorHandling
{
	/// <summary>
	/// Class used for handling errors. A return type from a repository should be of this type if 
	/// a method does not return any information except if the action succeeded or not (boolean Success).
	/// Used primarily for operations that modify data.
	/// </summary>
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

	/// <summary>
	/// Generic class used for handling errors. Has a generic return value in 
	/// case a repository method should return an object of some type.
	/// Used primarily for operations that fetch data.
	/// </summary>
	public class RepositoryResult<T> : RepositoryAction
	{
		public T Value { get; }

		public RepositoryResult(bool success, T value, string message = null) : base(success, message)
		{
			Value = value;
		}
	}
}
