using System;

namespace PrivateWiki
{
	public enum KeyboardShortcut
	{
		EditPage,
		SearchPage,
		PrintPdfPage
	}

	public static class KeyboardShortcutConverter
	{
		public static KeyboardShortcut Parse(string keyString)
		{
			return keyString switch
			{
				"key:strg+e" => KeyboardShortcut.EditPage,
				"key:strg+p" => KeyboardShortcut.PrintPdfPage,
				"key:strg+s" => KeyboardShortcut.SearchPage,
				_ => throw new Exception($"keyString ({keyString}) is not valid.")
			};
		}
	}
}