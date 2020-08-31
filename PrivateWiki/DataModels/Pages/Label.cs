using System;
using System.Collections.Generic;
using System.Drawing;

namespace PrivateWiki.DataModels.Pages
{
	public class Label
	{
		private static readonly Color DefaultColor = Color.FromArgb(255, 0, 0, 0);

		public Guid Id { get; } = Guid.NewGuid();

		public string Key { get; }

		public string? Value { get; }

		public string Description { get; }

		public Color Color { get; } = DefaultColor;

		[Obsolete]
		public Label(string key, string description, string? value = null)
		{
			Description = description;
			Key = key;
			Value = value;
		}

		[Obsolete]
		public Label(string key, string description, Color color, string? value = null)
		{
			Color = color;
			Description = description;
			Key = key;
			Value = value;
		}

		public Label(string value, string description) : this(value, description, DefaultColor)
		{
		}

		public Label(string value, string description, Color color)
		{
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
			new Label("testKey1", "Description 1", Color.Red),
			new Label("testKey2", "Description 2", "testValue2"),
			new Label("testKey3", "Description 3", Color.Indigo, "testValue3"),
			new Label("testKey3", "Description 3", Color.BurlyWood, "testValue3"),
			new Label("testKey1", "Description 1", Color.Red),
			new Label("testKey2", "Description 2", "testValue2"),
			new Label("testKey3", "Description 3", Color.Indigo, "testValue3"),
			new Label("testKey3", "Description 3", Color.BurlyWood, "testValue3"),
			new Label("testKey1", "Description 1", Color.Red),
			new Label("testKey2", "Description 2", "testValue2"),
			new Label("testKey3", "Description 3", Color.Indigo, "testValue3"),
			new Label("testKey3", "Description 3", Color.BurlyWood, "testValue3")
		};
	}
}