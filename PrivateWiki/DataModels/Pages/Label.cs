using System;
using System.Collections.Generic;

namespace PrivateWiki.DataModels.Pages
{
	public readonly struct Label
	{
		private static readonly Color DefaultColor = new Color(0, 0, 0);

		public Guid Id { get; }

		public string Key { get; }

		public string? Value { get; }

		public string Description { get; }

		public Color Color { get; }
		
		public Label(string value, string description) : this(Guid.NewGuid(), value, description, DefaultColor)
		{
		}
		
		public Label(Guid id, string value, string description) : this(id, value, description, DefaultColor)
		{
		}

		public Label(string value, string description, Color color) : this(Guid.NewGuid(), value, description, color)
		{
		}

		public Label(Guid id, string value, string description, Color color)
		{
			Id = id;

			var result = value.Split(new[] {"::"}, 1, StringSplitOptions.RemoveEmptyEntries);

			if (result.Length == 1)
			{
				Key = result[0];
				Value = "";
			}
			else
			{
				Key = result[0];
				Value = result[1];
			}

			Description = description;
			Color = color;
		}
		
		

		public static IList<Label> GetTestData() => testData;

		private static IList<Label> testData = new List<Label>
		{
			new Label("testKey1", "Description 1", System.Drawing.Color.Red.ToColor()),
			new Label("testKey2::testValue2", "Description 2"),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.Indigo.ToColor()),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.BurlyWood.ToColor()),
			new Label("testKey1", "Description 1", System.Drawing.Color.Red.ToColor()),
			new Label("testKey2::testValue2", "Description 2"),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.Indigo.ToColor()),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.BurlyWood.ToColor()),
			new Label("testKey1", "Description 1", System.Drawing.Color.Red.ToColor()),
			new Label("testKey2::testValue2", "Description 2"),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.Indigo.ToColor()),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.BurlyWood.ToColor())
		};
	}
}