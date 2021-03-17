using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSharp.Script.Exceptions;

namespace CSharp.Script.Execute
{
    public class ScriptMethods : IEnumerable<ScriptMethod>
    {
        private readonly Dictionary<string, ScriptMethod> _scriptMethods = new();

        public ScriptMethods(object scriptObject)
        {
            foreach (var methodInfo in scriptObject.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => !m.IsSpecialName))
            {
                if (_scriptMethods.TryGetValue(methodInfo.Name, out var scriptMethod))
                {
                    scriptMethod.AddOverload(methodInfo);
                    continue;
                }
                _scriptMethods.Add(methodInfo.Name, new ScriptMethod(scriptObject, methodInfo));
            }
        }
        public ScriptMethod this[string name] => Get(name);

        public IEnumerable<string> Names => _scriptMethods.Keys;

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

        public IEnumerator<ScriptMethod> GetEnumerator()
        {
            foreach (var key in _scriptMethods.Keys)
            {
                yield return _scriptMethods[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}