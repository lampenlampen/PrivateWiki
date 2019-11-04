using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBackend.Git
{
	class GitBackendException : Exception
	{
		public GitBackendException(string message) : base(message)
		{
		}
	}
}
