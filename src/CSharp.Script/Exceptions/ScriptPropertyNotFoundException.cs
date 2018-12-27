using System;

namespace CSharp.Script.Exceptions
{
    public class ScriptPropertyNotFoundException : ApplicationException
    {
        public ScriptPropertyNotFoundException(string propertyName) : base($"Script property '{propertyName}' not found")
        {
            
        }
    }
}