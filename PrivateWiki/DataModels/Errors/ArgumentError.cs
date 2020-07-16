using FluentResults;

namespace PrivateWiki.DataModels.Errors
{
	public class ArgumentError : Error
	{
		public ArgumentError()
		{
		}

		public ArgumentError(string message) : base(message)
		{
		}
	}
}