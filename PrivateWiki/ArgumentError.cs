using FluentResults;

namespace PrivateWiki
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