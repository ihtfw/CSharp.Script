using System;
using System.Collections.Generic;
using System.Linq;
using CSharp.Script.Compile;

namespace CSharp.Script.Exceptions
{
    public class CompileException : CSharpScriptException
    {
        public IReadOnlyList<CompileError> CompileErrors { get; }

        public string SourceCode { get; }
        public string FullSourceCode { get; }

        public CompileException(string message, IReadOnlyList<CompileError> compileErrors, string sourceCode, string fullSourceCode) : base(message)
        {
            CompileErrors = compileErrors.ToList();
            SourceCode = sourceCode;
            FullSourceCode = fullSourceCode;
        }
    }
}
