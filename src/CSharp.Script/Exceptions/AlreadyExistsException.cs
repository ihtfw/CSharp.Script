using System;

namespace CSharp.Script.Exceptions
{
    public class AlreadyExistsException : CSharpScriptException
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }
    }
}
