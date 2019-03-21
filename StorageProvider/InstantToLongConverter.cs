using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace StorageProvider
{
	static class InstantToLongConverter
	{
		public static long? ConvertToProvider(Instant? date)
		{
			if (date.HasValue)
			{
				return date.Value.ToUnixTimeMilliseconds();
			}
			else return null;
		}

		public static Instant? ConvertFromProvider(long? date)
		{
			if (date != null)
			{
				return Instant.FromUnixTimeMilliseconds((long) date);
			}
			else
			{
				return null;
			}
		}

		public static long ConvertToProvider(Instant date)
		{
			return date.ToUnixTimeMilliseconds();
		}

		public static Instant ConvertFromProvider(long date)
		{
			return Instant.FromUnixTimeMilliseconds(date);
		}
	}
}
