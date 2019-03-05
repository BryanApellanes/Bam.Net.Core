﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Presentation.Components;
using Bam.Net.Presentation.Markdown;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;
using Markdig.Helpers;

namespace Bam.Net.Presentation.Tests
{
    class TestComponentParser: ComponentParser
    {
        public StringSlice CallReadUntil(StringSlice slice, char until)
        {
            return ReadUntil(slice, until, out int ignore);
        }
    }

    [Serializable]
    public class MarkdownTests : CommandLineTestInterface
    {
        [UnitTest]
        public void PipeTableShouldRenderMarkdownTable()
        {
            PipeTableDocumentComponent doc = new PipeTableDocumentComponent("Header 1", "Header 2", "Another", "And Another")
            {
                Title = "A Pipe Table Test"
            };
            doc.AddRow("some value", "more stuff here", "more", "again");
            string expected = @"#A Pipe Table Test
|Header 1|Header 2|Another|And Another|
|----------|----------|----------|----------|
|some value|more stuff here|more|again|
";
            string output = doc.Render();
            OutFormat(output);
            Expect.AreEqual(expected, output);
        }

        [UnitTest]
        public void ComponentParserReadUntil()
        {
            TestComponentParser parser = new TestComponentParser();
            StringSlice slice = new StringSlice("[!clientId:NameOfComponent(- or |)] # comment", 2, 45);
            StringSlice clientId = parser.CallReadUntil(slice, ':');
            Expect.AreEqual("clientId", clientId.ToString());
        }
    }
}
