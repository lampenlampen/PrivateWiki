namespace PrivateWiki.Core
{
	public interface IConverter<in TInput, out TResult>
	{
		TResult Convert(TInput input);
	}
}