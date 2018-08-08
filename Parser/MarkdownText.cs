using System;

namespace Parser
{
    public class MarkdownText : MarkdownElement
    {
        public readonly string Text;

        private MarkdownText(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public static MarkdownText Parse(string text)
        {
            return new MarkdownText(text);
        }
    }
}