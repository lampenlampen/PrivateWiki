using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Pages
{
	public class Path
	{
		private string[]? _namespaces;

		public string? NamespaceString => _namespaces?.Aggregate((acc, el) => acc + ":" + el);

		public string[]? Namespaces => _namespaces;

		private string _title;

		public string Title => _title;

		public string FullPath
		{
			get
			{
				if (_namespaces != null)
				{
					return _namespaces.Aggregate((acc, el) => acc + ":" + el) + ":" + _title;
				}
				else
				{
					return _title;
				}
				
			}
		}

		public Path(string[] path, string name)
		{
			_namespaces = path;
			_title = name;
		}

		public Path(string name)
		{
			_title = name;
		}

		public override bool Equals(object obj)
		{
			return obj != null && FullPath.Equals(((Path) obj).FullPath);
		}

		public override int GetHashCode()
		{
			return FullPath.GetHashCode();
		}

		public override string ToString()
		{
			return FullPath;
		}
	}
}
