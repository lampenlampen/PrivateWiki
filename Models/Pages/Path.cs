using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Pages
{
	public class Path
	{
		private string[] _path;

		private string _name;

		public string FullPath => _path.Aggregate((acc, el) => acc + ":" + el) + ":" + _name;

		public Path(string[] path, string name)
		{
			_path = path;
			_name = name;
		}

		public Path(string name)
		{
			_path = new[] {"wiki"};
			_name = name;
		}
	}
}
