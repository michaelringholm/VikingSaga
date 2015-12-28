using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Global
{
    public static class FormattedTextParser
    {
        private static bool GetText(string text, int start, char endChar, out int end)
        {
            end = start;
            int len = text.Length;
            if (start == len)
                return false;

            while (end < len && text[end] != endChar)
                end++;

            return true;
        }

        private static Color GetColor(string c)
        {
            switch(c)
            {
                case "WHITE": return Colors.White;
                case "ORANGE": return Colors.Orange;
                default: throw new ArgumentException("Color not found: " + c);
            }
        }

        // <B+><B-> = bold
        // <I+><I-> = italic
        // <U+><U-> = underline
        // <F+><F-> = font size
        // <BR> = newline
        // <C White> = color white
        public static IEnumerable<Inline> Parse(string text, FontFamily fontFamily, double defaultFontSize)
        {
            const double FontSizeStep = 2;
            double fontSize = defaultFontSize;

            var fontStyle = FontStyles.Normal;
            var fontWeight = FontWeights.Normal;
            TextDecorationCollection textDecoration = null;
            Color color = Colors.White;

            int end;
            int start = 0;

            while (GetText(text, start, '<', out end))
            {
                string plainText = text.Substring(start, end - start);
                if (plainText != string.Empty)
                {
                    var run = new Run(plainText);
                    run.FontFamily = fontFamily;
                    run.FontSize = fontSize;
                    run.FontWeight = fontWeight;
                    run.FontStyle = fontStyle;
                    run.Foreground = new SolidColorBrush(color);
                    if (textDecoration != null)
                        run.TextDecorations.Add(textDecoration);

                    yield return run;

                    if (end == text.Length)
                        break;
                }

                start = end;
                if (!GetText(text, start, '>', out end))
                    throw new ArgumentException("Missing '>'");

                end++; // Include '>'
                
                string tag = text.Substring(start, end - start).ToUpper();
                if (tag.StartsWith("<C "))
                {
                    string strColor = tag.Substring(3, tag.Length - 4);
                    color = GetColor(strColor);
                }
                else
                {
                    switch (tag)
                    {
                        case "<B>": fontWeight = FontWeights.Bold; break;
                        case "</B>": fontWeight = FontWeights.Normal; break;
                        case "<I>": fontStyle = FontStyles.Italic; break;
                        case "</I>": fontStyle = FontStyles.Normal; break;
                        case "<U>": textDecoration = TextDecorations.Underline; break;
                        case "</U>": textDecoration = null; break;
                        case "<F+>": fontSize += FontSizeStep; break;
                        case "<F->": fontSize -= FontSizeStep; break;
                        case "<BR>": yield return new Run(Environment.NewLine); break;
                        default: throw new ArgumentException("Unknown tag: " + tag);
                    }
                }
                start = end;
            }
        }
    }
}
