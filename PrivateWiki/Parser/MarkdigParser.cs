using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;

namespace PrivateWiki.Parser
{
    public class MarkdigParser
    {
        MarkdownPipeline pipeline;

        private void init()
        {
            if (pipeline == null) pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseYamlFrontMatter().Build();
        }

        public Markdig.Syntax.MarkdownDocument Parse(string markdown)
        {
            init();

            var dom = Markdown.Parse(markdown);
            return dom;
        }
    }
}
