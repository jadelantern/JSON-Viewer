﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TraciJsonWpfApp.Extentions
{
    public static class RichTextBoxExtentions
    {
        public static string GetText(this RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtb.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtb.Document.ContentEnd
            );
            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.

            return textRange.Text;
        }
    }
}
