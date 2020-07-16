using FluentResults;

namespace PrivateWiki.DataModels.Errors
{
	public class UnauthorizedAccessError : Error
	{
		public UnauthorizedAccessError()
		{
		}

		public UnauthorizedAccessError(string message) : base(message)
		{
		}

		public UnauthorizedAccessError(string message, Error causedBy) : base(message, causedBy)
		{
		}
	}
}