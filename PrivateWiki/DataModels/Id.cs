using System;

namespace PrivateWiki.DataModels
{
	public abstract class IdBase
	{
		public Guid Id { get; }

		protected IdBase(Guid id)
		{
			Id = id;
		}

		protected bool Equals(IdBase other)
		{
			return Id.Equals(other.Id);
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((IdBase) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public static bool operator ==(IdBase? left, IdBase? right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(IdBase? left, IdBase? right)
		{
			return !Equals(left, right);
		}
	}
}