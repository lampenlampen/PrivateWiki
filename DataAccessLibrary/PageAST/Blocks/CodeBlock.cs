using System;
using System.Reflection;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
namespace DataAccessLibrary.PageAST.Blocks
{
	public class CodeBlock : IPageBlock
	{
		public Guid Id { get; set; }
		
		public string Code { get; set; }
		
		// TODO Refactor LanguageCode from string to enum
		public string LanguageCode { get; set; }

		public CodeBlock(string code, string languageCode)
		{
			Id = Guid.NewGuid();
			Code = code;
			LanguageCode = languageCode;
		}

		public CodeBlock(Guid id, string code, string languageCode)
		{
			Id = id;
			Code = code;
			LanguageCode = languageCode;
		}
	}
}