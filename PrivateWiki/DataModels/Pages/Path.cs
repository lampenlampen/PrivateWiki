using System;
using System.Linq;

namespace PrivateWiki.DataModels.Pages
{
	public record Path
	{
		private readonly string[]? _namespaces;

		public string? NamespaceString => _namespaces?.Aggregate((acc, el) => acc + ":" + el);

		public string Title { get; }

		public string FullPath
		{
			get
			{
				if (_namespaces != null)
				{
					return _namespaces.Aggregate((acc, el) => acc + ":" + el) + ":" + Title;
				}
				else
				{
					return Title;
				}
			}
		}

		private Path(string[] path, string name)
		{
			_namespaces = path;
			Title = name;
		}

		private Path(string name)
		{
			Title = name;
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
	}
}