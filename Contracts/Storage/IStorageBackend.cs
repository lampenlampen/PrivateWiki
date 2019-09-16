using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Pages;

namespace Contracts.Storage
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