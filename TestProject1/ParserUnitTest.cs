using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrivateWiki.Parser;

namespace TestProject1
{
    [TestClass]
    class ParserUnitTest
    {
        [TestMethod]
        public void ParseYamlTest()
        {
            var markdown = @"---
name: testyaml
---
# Header 1

lorem ipsum";

            var parser = new MarkdigParser();
        }
    }
}
