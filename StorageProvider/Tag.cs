using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace StorageProvider
{
	public class Tag
	{
		private int _alpha;
		private int _blue;
		private int _green;
		private int _red;

		[Key] public string Name { get; set; }

		[NotMapped]
		public Color Color
		{
			get => Color.FromArgb(_alpha, _red, _green, _blue);
			set
			{
				_alpha = value.A;
				_red = value.R;
				_green = value.G;
				_blue = value.B;
			}
		}

		public List<Tag> ChildTags { get; set; } = new List<Tag>();
	}
}