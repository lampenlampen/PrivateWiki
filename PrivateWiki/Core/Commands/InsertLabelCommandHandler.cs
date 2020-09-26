using System.Threading.Tasks;
using PrivateWiki.Services.StorageBackendService;

namespace PrivateWiki.Core.Commands
{
	public class InsertLabelCommandHandler : IAsyncCommandHandler<InsertLabelCommand>
	{
		private readonly ILabelBackendService _backend;

		public InsertLabelCommandHandler(ILabelBackendService backend)
		{
			_backend = backend;
		}

		public async Task Execute(InsertLabelCommand command)
		{
			await _backend.InsertLabelAsync(command.Label);
		}
	}
}