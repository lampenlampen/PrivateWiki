using FluentResults;

namespace PrivateWiki.DataModels.Errors
{
	public class DeserializationError : Error
	{
		public DeserializationError()
		{
		}

		public DeserializationError(string message) : base(message)
		{
		}

		public DeserializationError(string message, Error causedBy) : base(message, causedBy)
		{
		}
	}
}