using System;
using System.Collections.Generic;

namespace PrivateWiki.Core
{
	public interface IRepository<T>
	{
		public void Create(T item);

		public T Read(Predicate<T> where);

		public IList<T> ReadAll();

		public void Update(T item);

		public void Delete(T item);
	}
}