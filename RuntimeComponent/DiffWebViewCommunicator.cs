using Windows.Foundation.Metadata;

namespace RuntimeComponent
{
	[AllowForWeb]
	public sealed class DiffWebViewCommunicator
	{
		public string Old { get; set; } = "old";

		public string New { get; set; } = "New";


		// Html2Diff library options

		/// <summary>
		/// Shows a list of files before the diff.
		/// </summary>
		public bool ShowFiles { get; set; } = false;

		/// <summary>
		/// Shows differences level in each line.<br/>
		/// Possible values: "word", "char" or "none".
		/// </summary>
		public string DiffStyle { get; set; } = "words";

		/// <summary>
		/// Matching Style.<br/>
		/// Possible values: "lines", "words" or "none"
		/// </summary>
		public string Matching { get; set; } = "words";

		/// <summary>
		/// Output format<br/>
		/// Possible values: "side-by-side", "line-by-line"
		/// </summary>
		public string OutputFormat { get; set; } = "line-by-line";

		/// <summary>
		/// Input format<br/>
		/// Possible values: "diff", "json"
		/// </summary>
		public string InputFormat { get; set; } = "diff";


		// JsDiff library options

		/// <summary>
		/// Filename for the old file.
		/// </summary>
		public string OldFilename { get; set; } = "";

		/// <summary>
		/// Filename for the new file.
		/// </summary>
		public string NewFilename { get; set; } = "";

		/// <summary>
		/// Additional information to include in the old file header.
		/// </summary>
		public string OldHeader { get; set; } = "";

		/// <summary>
		/// Additional information to include in the new file header.
		/// </summary>
		public string NewHeader { get; set; } = "";

		/// <summary>
		/// Describes how many lines of context should be included.
		/// </summary>
		public int Context { get; set; } = 5;
	}
}