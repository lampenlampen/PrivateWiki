using System.Threading.Tasks;

namespace PrivateWiki.Services.StorageBackendService
{
	public interface ISqLiteBackend : IStorageBackend
	{
		Task<int[]> CreateTablesAsync();
	}
}