using System;

namespace PrivateWiki.DataModels
{
	public abstract record IdBase
	{
		public Guid Id { get; }

		protected IdBase(Guid id)
		{
			Id = id;
		}
	}
}