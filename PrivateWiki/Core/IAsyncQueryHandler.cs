using System.Threading.Tasks;

namespace PrivateWiki.Core
{
	public interface IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
	{
		Task<TResult> HandleAsync(TQuery query);
	}
}