using System.Threading.Tasks;

namespace PrivateWiki.Data
{
	public interface IFilesystemProvider
	{
		Task WriteTextAsync(File file, string content);

		Task<File> PickFile(string fileExtension);

		Task<string> ReadTextAsync(File file);
	}
}