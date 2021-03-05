using System.Collections.Generic;
using PrivateWiki.Core;

namespace PrivateWiki.Services.StartupTask
{
	public class CompositeStartupTask : IStartupTask
	{
		private readonly IEnumerable<IStartupTask> _tasks;

		public CompositeStartupTask(IEnumerable<IStartupTask> tasks)
		{
			_tasks = tasks;
		}

		public void Handle(Null _)
		{
			foreach (var startupTask in _tasks)
			{
				startupTask.Handle(_);
			}
		}
	}
}