using System.Linq;

namespace CSharp.Script.Exceptions
{
    public class ScriptMethodOverloadNotFoundException : CSharpScriptException
    {
        public ScriptMethodOverloadNotFoundException(string methodName, object[] args)
            : base($"Script method '{methodName}' with proper overload for args ({string.Join(", ", args.Select(o => o.GetType().Name))}) not found")
        {
        }
    }
}