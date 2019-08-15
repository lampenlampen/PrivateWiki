using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace PrivateWiki.Utilities
{
	static class ColorExtension
	{
		public static Windows.UI.Color ToWindowsUiColor(this System.Drawing.Color old)
		{
			return Color.FromArgb(old.A, old.R, old.G, old.B);
		}
	}
}
