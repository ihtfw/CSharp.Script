using System;
using System.Collections.Generic;
using CSharp.Script.Exceptions;

namespace CSharp.Script.Execute
{
    public class ScriptFields
    {
        private readonly Dictionary<string, ScriptField> _scriptFields = new Dictionary<string, ScriptField>();

        public ScriptFields(object scriptObject)
        {
            foreach (var fieldInfo in scriptObject.GetType().GetFields())
            {
                _scriptFields.Add(fieldInfo.Name, new ScriptField(scriptObject, fieldInfo));
            }
        }

        public IEnumerable<ScriptField> ForEachWhereType(Type propertyType)
        {
            foreach (var pair in _scriptFields)
            {
                var scriptField = pair.Value;

                if (scriptField.Type == propertyType)
                    yield return scriptField;
            }
        }

        public bool Contains(string scriptFieldName)
        {
            return _scriptFields.ContainsKey(scriptFieldName);
        }

        public ScriptField Get(string scriptFieldName)
        {
            var field = GetOrDefault(scriptFieldName);
            if (field == null)
                throw new ScriptFieldNotFoundException(scriptFieldName);

            return field;
        }

        public ScriptField GetOrDefault(string scriptFieldName)
        {
            if (!_scriptFields.TryGetValue(scriptFieldName, out var scriptMethod))
            {
                return null;
            }

            return scriptMethod;
        }
    }
}