using System.Threading.Tasks;

namespace PrivateWiki.Services.StorageBackendService
{
	/// <summary>
	/// Provides access to a storage backend.
	/// </summary>
	public interface IStorageBackend
	{
		/// <summary>
		/// Checks if a <see cref="IStorageBackend"/> does exist.
		/// </summary>
		/// <returns></returns>
		Task<bool> ExistsAsync();

		/// <summary>
		/// Deletes the <see cref="IStorageBackend"/>
		/// </summary>
		/// <returns></returns>
		bool Delete();
	}
}