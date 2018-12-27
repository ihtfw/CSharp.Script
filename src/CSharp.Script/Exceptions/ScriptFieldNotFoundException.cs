using System;

namespace CSharp.Script.Exceptions
{
    public class ScriptFieldNotFoundException : ApplicationException
    {
        public ScriptFieldNotFoundException(string fieldName) : base($"Script field '{fieldName}' not found")
        {
            
        }
    }
}