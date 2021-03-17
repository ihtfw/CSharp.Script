using System;
using System.Runtime.Serialization;

namespace CSharp.Script.Exceptions
{
    public class CSharpScriptException : ApplicationException
    {
        public CSharpScriptException()
        {
        }

        public CSharpScriptException(string message) : base(message)
        {
        }

        public CSharpScriptException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CSharpScriptException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}