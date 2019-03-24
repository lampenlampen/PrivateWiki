using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web;

namespace PrivateWiki
{
	class MyUriToStreamResolver : IUriToStreamResolver
	{
		public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
		{
			// TODO uri == null error
			if (uri == null) throw new Exception();

			string path = uri.AbsolutePath;

			return GetContent(path).AsAsyncOperation();
		}

		private async Task<IInputStream> GetContent(string path)
		{
			try
			{
				var localUir = new Uri($"ms-appdata:///local/{path}");
				var file = await StorageFile.GetFileFromApplicationUriAsync(localUir);

				var stream = await file.OpenAsync(FileAccessMode.Read);
				return stream;
			}
			catch (Exception)
			{
				throw new Exception("invalid Path");
			}
		}
	}
}
