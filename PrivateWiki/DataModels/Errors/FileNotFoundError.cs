using FluentResults;

namespace PrivateWiki.DataModels.Errors
{
	public class FileNotFoundError : Error
	{
		public FileNotFoundError()
		{
		}

		public FileNotFoundError(string message) : base(message)
		{
		}

		public FileNotFoundError(string message, Error causedBy) : base(message, causedBy)
		{
		}
	}
}