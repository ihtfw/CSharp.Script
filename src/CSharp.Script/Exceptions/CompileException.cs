using System;

namespace CSharp.Script.Exceptions
{
    public class CompileException : ApplicationException
    {
        public string SourceCode { get; }
        public string FullSourceCode { get; }

        public CompileException(string message, string sourceCode, string fullSourceCode) : base(message)
        {
            SourceCode = sourceCode;
            FullSourceCode = fullSourceCode;
        }
    }
}
