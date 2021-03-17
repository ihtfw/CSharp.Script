using System;

namespace CSharp.Script.Exceptions
{
    public class ScriptPropertyNotFoundException : CSharpScriptException
    {
        public ScriptPropertyNotFoundException(string propertyName) : base($"Script property '{propertyName}' not found")
        {
            
        }
    }
}