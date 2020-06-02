using System;
using System.Linq;

namespace PrivateWiki.Models.Pages
{
	public class Path : IEquatable<Path>
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

		private Path(string[] path, string name)
		{
			_namespaces = path;
			_title = name;
		}

		private Path(string name)
		{
			_title = name;
		}

		/// <summary>
		/// Returns true, if the path belongs to the system namespace.
		/// </summary>
		/// <returns></returns>
		public bool IsSystemNamespace()
		{
			if (_namespaces == null) return false;

			var a = _namespaces[0];

			return a != null && a.ToUpperInvariant() == "SYSTEM";
		}

		public override bool Equals(object obj)
		{
			return obj is Path path && FullPath.Equals(path.FullPath);
		}

		public override int GetHashCode()
		{
			return FullPath.GetHashCode();
		}

		public override string ToString()
		{
			return FullPath;
		}

		public static Path ofLink(string link)
		{
			var path = link.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);

			if (path.Length > 1)
			{
				var path2 = new string[path.Length - 1];
				Array.Copy(path, path2, path.Length - 1);

				return new Path(path2, path[path.Length - 1]);
			}

			return new Path(link);
		}

		public static Path of(string[] path, string name)
		{
			return new Path(path, name);
		}

		public bool Equals(Path other)
		{
			return FullPath == other.FullPath;
		}
	}
}