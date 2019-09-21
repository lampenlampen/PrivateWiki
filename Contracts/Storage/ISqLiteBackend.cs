using System.Threading.Tasks;

namespace Contracts.Storage
{
	public interface ISqLiteBackend : IStorageBackend
	{
		Task<int> CreateTablesAsync();
	}
}