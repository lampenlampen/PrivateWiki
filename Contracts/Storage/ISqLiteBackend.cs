using System.Threading.Tasks;
using Models.Storage;

namespace Contracts.Storage
{
	public interface ISqLiteBackend : IStorageBackend
	{
		Task<int> CreateTablesAsync();
	}
}