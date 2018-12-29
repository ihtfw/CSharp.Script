using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSharp.Script.Exceptions;

namespace CSharp.Script.Execute
{
    public class ScriptMethods
    {
        private readonly Dictionary<string, ScriptMethod> _scriptMethods = new Dictionary<string, ScriptMethod>();

        public ScriptMethods(object scriptObject)
        {
            foreach (var methodInfo in scriptObject.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => !m.IsSpecialName))
            {
                if (_scriptMethods.ContainsKey(methodInfo.Name))
                {
                    throw new AlreadyExistsException($"Method {methodInfo.Name} already added");
                }
                _scriptMethods.Add(methodInfo.Name, new ScriptMethod(scriptObject, methodInfo));
            }
        }

        public bool Contains(string scriptMethodName)
        {
            return _scriptMethods.ContainsKey(scriptMethodName);
        }

        public ScriptMethod Get(string scriptMethodName)
        {
            var method = GetOrDefault(scriptMethodName);
            if (method == null)
                throw new ScriptMethodNotFoundException(scriptMethodName);

            return method;
        }

        public ScriptMethod GetOrDefault(string scriptMethodName)
        {
            if (!_scriptMethods.TryGetValue(scriptMethodName, out var scriptMethod))
            {
                return null;
            }

            return scriptMethod;
        }
    }
}