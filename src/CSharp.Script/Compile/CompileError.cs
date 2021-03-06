﻿using Microsoft.CodeAnalysis;

namespace CSharp.Script.Compile
{
    public class CompileError
    {
        public CompileError()
        {
            
        }

        internal CompileError(Diagnostic diagnostic)
        {
            var startLinePosition = diagnostic.Location.GetLineSpan().StartLinePosition;
            Line = startLinePosition.Line + 1;
            Column = startLinePosition.Character;
            ErrorNumber = diagnostic.Id;
            ErrorText = diagnostic.GetMessage();
        }

        /// <summary>Gets or sets the line number where the source of the error occurs.</summary>
        /// <returns>The line number of the source file where the compiler encountered the error.</returns>
        public int Line
        {
            get; set;
        }

        /// <summary>Gets or sets the column number where the source of the error occurs.</summary>
        /// <returns>The column number of the source file where the compiler encountered the error.</returns>
        public int Column
        {
            get; set;
        }

        /// <summary>Gets or sets the error number.</summary>
        /// <returns>The error number as a string.</returns>
        public string ErrorNumber { get; set; }

        /// <summary>Gets or sets the text of the error message.</summary>
        /// <returns>The text of the error message.</returns>
        public string ErrorText
        {
            get; set;
        }
    }
}