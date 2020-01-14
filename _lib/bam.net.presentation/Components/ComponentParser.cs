using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Presentation.Components
{
    public class ComponentParser : InlineParser
    {
        public ComponentParser() { }
        public ComponentParser(IComponentResolver componentResolver)
        {
            OpeningCharacters = new char[] { '!' };
            ComponentResolver = componentResolver;
        }

        public IComponentResolver ComponentResolver { get; set; }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            // [!clientId:NameOfComponent(- or |)] # comment
            // ```
            // init: function(component) {}
            // click: function(component) {}
            // mouseover: function(component){}
            // ... other event handlers etc ...
            // ```
            bool match = false;
            int start = slice.Start;
            int end = slice.Start;
            char openSquareBracket = slice.PeekCharExtra(-1);
            char shouldBeWhitespace = slice.PeekCharExtra(-2);
            if(shouldBeWhitespace.IsWhiteSpaceOrZero() && openSquareBracket == '[')
            {
                slice.NextChar();
                StringSlice clientId = ReadUntil(slice, ':', out end);
                StringSlice nameOfComponent = ReadUntil(slice, ']', out end);
                string nameOfComponentString = nameOfComponent.ToString();
                //bool block = true;
                //if (nameOfComponentString.EndsWith("|"))
                //{
                //    nameOfComponentString = nameOfComponentString.Truncate(1);
                //    block = false;
                //}
                if (ComponentResolver.IsValidComponentName(nameOfComponentString, out LeafInline component))
                {
                    int inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);
                    component.Span.Start = inlineStart;
                    component.Span.End = inlineStart + (end - start) + 1;
                    processor.Inline = component;

                    match = true;
                }
            }

            return match;
        }

        protected StringSlice ReadUntil(StringSlice slice, char until, out int end)
        {
            char current = slice.CurrentChar;
            int start = slice.Start;
            end = start;
            slice.NextChar();
            while(current != until || current.IsWhiteSpaceOrZero())
            {
                end = slice.Start;
                current = slice.NextChar();
            }
            return new StringSlice(slice.Text, start, end);
        }
    }
}
