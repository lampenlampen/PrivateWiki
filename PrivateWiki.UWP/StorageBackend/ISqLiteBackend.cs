using System.Threading.Tasks;

namespace PrivateWiki.StorageBackend
{
	public interface ISqLiteBackend : IStorageBackend
	{
		Task<int[]> CreateTablesAsync();
	}
}