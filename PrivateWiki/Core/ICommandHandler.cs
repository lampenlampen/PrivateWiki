namespace PrivateWiki.Core
{
	public interface ICommandHandler<TCommand>
	{
		void Handle(TCommand command);
	}
}