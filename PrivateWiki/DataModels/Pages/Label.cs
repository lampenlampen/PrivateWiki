using System;
using System.Collections.Generic;

namespace PrivateWiki.DataModels.Pages
{
	public class Label
	{
		private static readonly Color DefaultColor = new Color(0, 0, 0);

		public Guid Id { get; }

		public string Key { get; }

		public string? Value { get; }

		public string Description { get; }

		public Color Color { get; }

		public LabelId LabelId => new LabelId(Id);

		public Label(string value, string description, Color color) : this(Guid.NewGuid(), value, description, color) { }

		public Label(Guid id, string key, string value, string description, Color color)
		{
			Id = id;
			Key = key;
			Value = value;
			Description = description;
			Color = color;
		}

		public Label(Guid id, string value, string description, Color color)
		{
			Id = id;

			var result = value.Split(new[] {"::"}, 2, StringSplitOptions.RemoveEmptyEntries);

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

		protected bool Equals(Label other)
		{
			return Id.Equals(other.Id) && Key == other.Key && Value == other.Value && Description == other.Description && Color.Equals(other.Color);
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Label) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Id.GetHashCode();
				hashCode = (hashCode * 397) ^ Key.GetHashCode();
				hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Description.GetHashCode();
				hashCode = (hashCode * 397) ^ Color.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Label? left, Label? right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Label? left, Label? right)
		{
			return !Equals(left, right);
		}

		[Obsolete]
		public static IList<Label> GetTestData() => testData;

		[Obsolete] private static IList<Label> testData = new List<Label>
		{
			new Label("testKey1", "Description 1", System.Drawing.Color.Red.ToColor()),
			new Label("testKey2::testValue2", "Description 2", DefaultColor),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.Indigo.ToColor()),
			new Label("testKey4::testValue4", "Description 4", System.Drawing.Color.BurlyWood.ToColor()),
			new Label("testKey5", "Description 1", System.Drawing.Color.SlateGray.ToColor()),
			new Label("testKey6::testValue6", "Description 6", DefaultColor),
			new Label("testKey7::testValue7", "Description 7", System.Drawing.Color.Blue.ToColor()),
			new Label("testKey8::testValue8", "Description 8", System.Drawing.Color.Orange.ToColor()),
			new Label("testKey9", "Description 1", System.Drawing.Color.DodgerBlue.ToColor()),
			new Label("testKey10::testValue10", "Description 10", DefaultColor),
			new Label("testKey11::testValue11", "Description 11", System.Drawing.Color.Green.ToColor()),
			new Label("testKey12::testValue12", "Description 12", System.Drawing.Color.Indigo.ToColor())
		};
	}
}