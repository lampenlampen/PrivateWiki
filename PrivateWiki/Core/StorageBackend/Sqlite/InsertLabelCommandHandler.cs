using System.Collections.Generic;
using System.Threading.Tasks;
using PrivateWiki.Core.Commands;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.StorageServices.Sql;
using PrivateWiki.Services.StorageServices.Sql.Sqlite;

namespace PrivateWiki.Core.StorageBackend.Sqlite
{
	public class InsertLabelCommandHandler : IAsyncCommandHandler<InsertLabelCommand>
	{
		private readonly ISqliteStorage _backend;

		public InsertLabelCommandHandler(ISqliteStorage backend)
		{
			_backend = backend;
		}

		public Task Execute(InsertLabelCommand command)
		{
			var sql = "";
			
			var parameters = new List<KeyValuePair<string, string>>();
		
			var sqliteCommand = new SqlCommand(sql, parameters);
			
			return _backend.ExecuteNonQueryAsync(sqliteCommand);
		}
	}
}