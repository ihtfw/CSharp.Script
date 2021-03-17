using System.Collections.Generic;
using System.Reflection;
using CSharp.Script.Exceptions;

namespace CSharp.Script.Execute
{
    public class ScriptMethod
    {
        public object ScriptObject { get; }

        private readonly List<MethodInfo> _methodInfos = new();

        public ScriptMethod(object scriptObject, MethodInfo methodInfo)
        {
            ScriptObject = scriptObject;

            Name = methodInfo.Name;
            _methodInfos.Add(methodInfo);
        }

        public string Name { get; }

        public void AddOverload(MethodInfo methodInfo)
        {
            _methodInfos.Add(methodInfo);
        }

        public TReturnType Invoke<TReturnType>(params object[] args)
        {
            return (TReturnType) Invoke(args);
        }

        public object Invoke(params object[] args)
        {
            if (_methodInfos.Count == 1)
            {
                return _methodInfos[0].Invoke(ScriptObject, args);
            }

            foreach (var methodInfo in _methodInfos)
            {
                if (IsMatch(methodInfo, args))
                {
                    return methodInfo.Invoke(ScriptObject, args);
                }
            }

            throw new ScriptMethodOverloadNotFoundException(Name, args);
        }

        private bool IsMatch(MethodInfo methodInfo, object[] args)
        {
            var parameterInfos = methodInfo.GetParameters();
            if (parameterInfos.Length != args.Length)
            {
                return false;
            }

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var info = parameterInfos[i];

                if (!info.ParameterType.IsInstanceOfType(arg))
                {
                    return false;
                }
            }

            return true;
        }
    }
}