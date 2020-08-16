using System;
using System.Collections.Generic;
using System.Drawing;

namespace PrivateWiki.DataModels.Pages
{
	public class Label
	{
		private static readonly Color DefaultColor = Color.FromArgb(255, 0, 0, 0);

		public Guid Id { get; }

		public string Key { get; }

		public string? Value { get; }

		public string Description { get; }

		public Color Color { get; } = DefaultColor;

		public Label(string key, string description, string? value = null)
		{
			Description = description;
			Key = key;
			Value = value;
		}

		public Label(string key, string description, Color color, string? value = null)
		{
			Color = color;
			Description = description;
			Key = key;
			Value = value;
		}

		public static IList<Label> GetTestData() => new List<Label>
		{
			new Label("testKey1", "Description 1", Color.Red),
			new Label("testKey2", "Description 2", "testValue2"),
			new Label("testKey3", "Description 3", Color.Indigo, "testValue3"),
			new Label("testKey3", "Description 3", Color.BurlyWood, "testValue3")
		};
	}
}