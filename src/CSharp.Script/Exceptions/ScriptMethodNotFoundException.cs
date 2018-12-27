using System;

namespace CSharp.Script.Exceptions
{
    public class ScriptMethodNotFoundException : ApplicationException
    {
        public ScriptMethodNotFoundException(string methodName) : base($"Script method '{methodName}' not found")
        {
            
        }
    }
}
