using System.Threading.Tasks;

namespace PrivateWiki.UWP.StorageBackend
{
	public interface ISqLiteBackend : IStorageBackend
	{
		Task<int[]> CreateTablesAsync();
	}
}