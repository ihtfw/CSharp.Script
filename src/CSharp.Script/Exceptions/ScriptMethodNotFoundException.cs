namespace CSharp.Script.Exceptions
{
    public class ScriptMethodNotFoundException : CSharpScriptException
    {
        public ScriptMethodNotFoundException(string methodName) : base($"Script method '{methodName}' not found")
        {
            
        }
    }
}
