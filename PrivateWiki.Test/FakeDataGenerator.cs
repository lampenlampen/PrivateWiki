using System;
using System.Linq;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Test
{
	public static class FakeDataGenerator
	{
		private static readonly Random Random = new Random();

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[Random.Next(s.Length)]).ToArray());
		}

		public static Label GetNewLabel()
		{
			var id = Guid.NewGuid();
			var key = RandomString(4);
			var value = RandomString(5);
			var description = RandomString(15);
			var color = new Color(15, 15, 15);

			return new Label(id, key, value, description, color);
		}
	}
}