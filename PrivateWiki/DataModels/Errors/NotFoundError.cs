using FluentResults;

namespace PrivateWiki.DataModels.Errors
{
	public class NotFoundError : Error
	{
		public NotFoundError()
		{
		}

		public NotFoundError(string message) : base(message)
		{
		}

		public NotFoundError(string message, Error causedBy) : base(message, causedBy)
		{
		}
	}
}