using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivateWiki.Services.StartupTask
{
	public class CompositeStartupTask : IStartupTask
	{
		private readonly IEnumerable<IStartupTask> _tasks;

		public CompositeStartupTask(IEnumerable<IStartupTask> tasks)
		{
			_tasks = tasks;
		}

		public async Task<bool> Execute()
		{
			foreach (var task in _tasks)
			{
				await task.Execute();
			}

			return true;
		}
	}
}