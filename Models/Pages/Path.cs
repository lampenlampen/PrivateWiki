using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Pages
{
	public class Path
	{
		private string[] _namespaces;

		private string _title;

		public string FullPath => _namespaces.Aggregate((acc, el) => acc + ":" + el) + ":" + _title;

		public Path(string[] path, string name)
		{
			_namespaces = path;
			_title = name;
		}

		public Path(string name)
		{
			_namespaces = new[] {"wiki"};
			_title = name;
		}
	}
}
