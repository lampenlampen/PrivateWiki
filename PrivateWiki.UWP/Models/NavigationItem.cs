using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Newtonsoft.Json;

#nullable enable

namespace PrivateWiki.UWP.Models
{
	public class NavigationItem : INotifyPropertyChanged
	{
		public Guid Id { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class DividerItem : NavigationItem
	{
	}

	public class HeaderItem : NavigationItem
	{
		private string? _text;

		public string? Label
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChanged(nameof(Label));
			}
		}
	}

	public class LinkItem : NavigationItem
	{
		private string? _text;

		public string? Label
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChanged(nameof(Label));
			}
		}

		public Guid? PageId { get; set; }
	}

	public class NavigationItemJsonSerializer
	{
		private JsonWriter Writer { get; set; }

		public NavigationItemJsonSerializer(JsonWriter writer)
		{
			Writer = writer;
		}

		public JsonWriter WriteJson(NavigationItem item)
		{
			switch (item)
			{
				case DividerItem dividerItem:
					DividerItemWriteJson(dividerItem);
					break;
				case HeaderItem headerItem:
					HeaderItemWriteJson(headerItem);
					break;
				case LinkItem linkItem:
					LinkItemWriteJson(linkItem);
					break;
			}

			return Writer;
		}

		private void DividerItemWriteJson(DividerItem item)
		{
			Writer.WriteStartObject();
			Writer.WritePropertyName("type");
			Writer.WriteValue("divider");
			Writer.WriteEndObject();
		}

		private void HeaderItemWriteJson(HeaderItem item)
		{
			Writer.WriteStartObject();
			Writer.WritePropertyName("type");
			Writer.WriteValue("header");
			Writer.WritePropertyName("label");
			Writer.WriteValue(item.Label);
			Writer.WriteEndObject();
		}

		private void LinkItemWriteJson(LinkItem item)
		{
			Writer.WriteStartObject();
			Writer.WritePropertyName("type");
			Writer.WriteValue("link");
			Writer.WritePropertyName("label");
			Writer.WriteValue(item.Label);
			Writer.WritePropertyName("link_id");
			Writer.WriteValue(item.PageId);
			Writer.WriteEndObject();
		}
	}
}