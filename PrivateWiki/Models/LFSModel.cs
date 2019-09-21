using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace PrivateWiki.Models
{
	public class SyncModel
	{
		public string Title { get; set; }

		public string Subtitle { get; set; }

		
	}


	public class LFSModel : SyncModel, INotifyPropertyChanged
	{
		private string _targetToken;
		private string _targetPath;
		private SyncFrequency _syncFrequency;

		public string TargetToken
		{
			get => _targetToken;
			set { _targetToken = value; OnPropertyChanged(nameof(TargetToken));}
		}

		public string TargetPath
		{
			get => _targetPath;
			set { _targetPath = value; OnPropertyChanged(nameof(TargetPath)); }
		}

		public SyncFrequency SyncFrequency
		{
			get => _syncFrequency;
			set { _syncFrequency = value; OnPropertyChanged(nameof(SyncFrequency)); }
		}

		public LFSModel()
		{
			Title = "Local File System";
			Subtitle = "Local storage or network shares";
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public enum SyncFrequency
	{
		Never,
		Hourly,
		Daily,
		Weekly
	}
}
