using System.Text;

namespace PrivateWiki.Markdig
{
	public static class MarkdigStyleSheet
	{
		public static class CodeBlockHeader
		{
			public static string BackgroundColor = "rgb(242, 242, 242)";
			public static string BorderColor = "rgb(227, 227, 227)";
			public static string BorderStyle = "solid";
			public static string BorderWidth = "1px";
			public static string FontSize = "12.8px";
			public static string FontWeight = "400";
			public static string LineHeight = "25.33px";
			public static string MarginTop = "16px";
			public static string MinHeight = "30px";
			public static string PaddingBottom = "2px";
			public static string PaddingLeft = "16px";
			public static string PaddingRight = "16px";
			public static string PaddingTop = "2px";
			public static string Color = "rgb(0,0,0)";

			public static string GetCodeBlockHeaderStyle()
			{
				var builder = new StringBuilder();
				builder.Append($"background-color:{BackgroundColor};");
				builder.Append($"border-bottom-color:rgba(0,0,0,1);");
				builder.Append($"border-bottom-style:none;");
				builder.Append($"border-bottom-width:0px;");
				builder.Append($"border-left-color:{BorderColor};");
				builder.Append($"border-left-style:{BorderStyle};");
				builder.Append($"border-left-width:{BorderWidth};");
				builder.Append($"border-right-color:{BorderColor};");
				builder.Append($"border-right-style:{BorderStyle};");
				builder.Append($"border-right-width:{BorderWidth};");
				builder.Append($"border-top-color:{BorderColor};");
				builder.Append($"border-top-style:{BorderStyle};");
				builder.Append($"border-top-width:{BorderWidth};");
				builder.Append($"box-sizing:content-box;");
				builder.Append($"color:rgba(0,0,0,1);");
				builder.Append($"display:flex;");
				builder.Append($"flex-direction:row;");
				builder.Append($"font-size:{FontSize};");
				builder.Append($"font-weight:{FontWeight};");
				builder.Append($"line-height:{LineHeight};");
				builder.Append($"margin-top:{MarginTop};");
				builder.Append($"min-height:{MinHeight};");

				return builder.ToString();
			}

			public static string GetCodeBlockLanguageSpanStyle()
			{
				var builder = new StringBuilder();
				builder.Append("box-sizing:content-box;");
				builder.Append($"color:{Color};");
				builder.Append("display:block;");
				builder.Append("flex-grow:1;");
				builder.Append($"font-size:{FontSize};");
				builder.Append($"font-weight:{FontWeight};");
				builder.Append($"line-height:{LineHeight};");
				builder.Append($"padding-bottom:{PaddingBottom};");
				builder.Append($"padding-left:{PaddingLeft};");
				builder.Append($"padding-right:{PaddingRight};");
				builder.Append($"padding-top:{PaddingTop};");

				return builder.ToString();
			}

			public static string GetCodeBlockButtonStyle()
			{
				var builder = new StringBuilder();
				builder.Append($"align-items:center;");
				builder.Append($"background-color:transparent;");
				builder.Append($"border:0px none {BorderColor};");
				builder.Append($"border-left-color:{BorderColor};");
				builder.Append($"border-left-style:{BorderStyle};");
				builder.Append($"border-left-width:{BorderWidth};");
				builder.Append($"box-sizing:content-box;");
				builder.Append($"color:{Color};");
				builder.Append($"cursor:pointer;");
				builder.Append($"display:flex;");
				builder.Append($"padding-bottom:{PaddingBottom};");
				builder.Append($"padding-left:{PaddingLeft};");
				builder.Append($"padding-right:{PaddingRight};");
				builder.Append($"padding-top:{PaddingTop};");

				return builder.ToString();
			}

			public static string GetCodeBlockButtonIconStyle()
			{
				var builder = new StringBuilder();

				// TODO Document Icon

				return builder.ToString();
			}
		}

		public static class CodeBlock
		{
			public static string BackgroundColor = "rgb(250, 250, 250)";
			public static string BorderColor = "rgb(227, 227, 227)";
			public static string BorderStyle = "solid";
			public static string BorderWidth = "1px";
			public static string FontSize = "13.93px";
			public static string FontWeight = "400";
			public static string LineHeight = "19px";
			public static string Margin = "0px";
			public static string Padding = "16px";


			public static string GetCodeBlockBoxStyle()
			{
				var builder = new StringBuilder();
				builder.Append($"background-color:{BackgroundColor};");
				builder.Append($"border-bottom-color:{BorderColor};");
				builder.Append($"border-bottom-style:{BorderStyle};");
				builder.Append($"border-bottom-width:{BorderWidth};");
				builder.Append($"border-left-color:{BorderColor};");
				builder.Append($"border-left-style:{BorderStyle};");
				builder.Append($"border-left-width:{BorderWidth};");
				builder.Append($"border-right-color:{BorderColor};");
				builder.Append($"border-right-style:{BorderStyle};");
				builder.Append($"border-right-width:{BorderWidth};");
				builder.Append($"border-top-color:{BorderColor};");
				builder.Append($"border-top-style:{BorderStyle};");
				builder.Append($"border-top-width:{BorderWidth};");
				builder.Append($"box-sizing:content-box;");
				builder.Append($"color:rgb(0,0,0);");
				builder.Append("direction:ltr;");
				builder.Append("display:block;");
				builder.Append($"font-size:{FontSize};");
				builder.Append($"font-weight:{FontWeight};");
				builder.Append($"line-height:{LineHeight};");
				builder.Append($"margin:{Margin};");
				builder.Append($"overflow:auto;");
				builder.Append($"padding:{Padding};");
				builder.Append($"text-align:left;");
				builder.Append($"white-space:pre;");
				builder.Append($"word-break:normal;");
				builder.Append($"word-spacing:0px;");
				builder.Append($"word-wrap:normal;");

				return builder.ToString();
			}

			public static string GetCodeBlockStyle()
			{
				var builder = new StringBuilder();
				builder.Append("border:0px none black;");
				builder.Append($"font-size:{FontSize};");
				builder.Append($"font-weight:{FontWeight};");
				builder.Append($"line-height:{LineHeight};");
				builder.Append($"padding:0px;");
				builder.Append($"text-align:left;");
				builder.Append($"white-space:pre;");
				builder.Append($"word-break:normal;");
				builder.Append($"word-spacing:0px;");
				builder.Append($"word-wrap:normal;");

				return builder.ToString();
			}
		}
	}
}