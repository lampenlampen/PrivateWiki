using System;

namespace PrivateWiki
{
	public enum KeyboardShortcut
	{
		EditPage,
		SearchPage,
		PrintPdfPage,
		NewPage,
		Escape
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
				"key:strg+n" => KeyboardShortcut.NewPage,
				"key:escape" => KeyboardShortcut.Escape,
				_ => throw new Exception($"keyString ({keyString}) is not valid.")
			};
		}
	}
}