using System.Collections.Generic;
using System.Linq;
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
			var tasks = _tasks.Select(task => task.Execute()).ToList();

			await Task.WhenAll(tasks);

			return true;
		}
	}
}