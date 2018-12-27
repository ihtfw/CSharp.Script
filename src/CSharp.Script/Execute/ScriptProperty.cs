using System;
using System.Reflection;

namespace CSharp.Script.Execute
{
    public class ScriptProperty
    {
        private readonly PropertyInfo _propertyInfo;

        public ScriptProperty(object scriptObject, PropertyInfo propertyInfo)
        {
            ScriptObject = scriptObject;
            _propertyInfo = propertyInfo;
        }
        public object ScriptObject { get; }

        public Type Type => _propertyInfo.PropertyType;

        public TReturnType Get<TReturnType>()
        {
            return (TReturnType)Get();
        }

        public object Get()
        {
            return _propertyInfo.GetValue(ScriptObject);
        }

        public void Set(object setValue)
        {
            _propertyInfo.SetValue(ScriptObject, setValue);
        }
    }
}