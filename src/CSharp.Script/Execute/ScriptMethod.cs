using System.Reflection;

namespace CSharp.Script.Execute
{
    public class ScriptMethod
    {
        public object ScriptObject { get; }

        private readonly MethodInfo _methodInfo;

        public ScriptMethod(object scriptObject, MethodInfo methodInfo)
        {
            ScriptObject = scriptObject;
            _methodInfo = methodInfo;
        }

        public string Name => _methodInfo.Name;

        public TReturnType Invoke<TReturnType>(params object[] args)
        {
            return (TReturnType) Invoke(args);
        }

        public object Invoke(params object[] args)
        {
            return _methodInfo.Invoke(ScriptObject, args);
        }
    }
}