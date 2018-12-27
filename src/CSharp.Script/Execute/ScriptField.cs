using System;
using System.Reflection;

namespace CSharp.Script.Execute
{
    public class ScriptField
    {
        private readonly FieldInfo _fieldInfo;

        public ScriptField(object scriptObject, FieldInfo fieldInfo)
        {
            ScriptObject = scriptObject;
            _fieldInfo = fieldInfo;
        }

        public object ScriptObject { get; }

        public Type Type => _fieldInfo.FieldType;

        public TReturnType Get<TReturnType>()
        {
            return (TReturnType)Get();
        }

        public object Get()
        {
            return _fieldInfo.GetValue(ScriptObject);
        }

        public void Set(object setValue)
        {
            _fieldInfo.SetValue(ScriptObject, setValue);
        }
    }
}