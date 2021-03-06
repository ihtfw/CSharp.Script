﻿using System;
using System.Collections;
using System.Collections.Generic;
using CSharp.Script.Exceptions;

namespace CSharp.Script.Execute
{
    public class ScriptProperties : IEnumerable<ScriptProperty>
    {
        private readonly Dictionary<string, ScriptProperty> _scriptProperties = new();

        public ScriptProperties(object scriptObject)
        {
            foreach (var propertyInfo in scriptObject.GetType().GetProperties())
            {
                if (_scriptProperties.ContainsKey(propertyInfo.Name))
                {
                    throw new AlreadyExistsException($"Property {propertyInfo.Name} already added");
                }
                _scriptProperties.Add(propertyInfo.Name, new ScriptProperty(scriptObject, propertyInfo));
            }
        }

        public ScriptProperty this[string name] => Get(name);

        public IEnumerable<string> Names => _scriptProperties.Keys;

        public bool Contains(string scriptPropertyName)
        {
            return _scriptProperties.ContainsKey(scriptPropertyName);
        }

        public IEnumerable<ScriptProperty> ForEachWhereType(Type propertyType)
        {
            foreach (var pair in _scriptProperties)
            {
                var scriptProperty = pair.Value;

                if (scriptProperty.Type == propertyType)
                    yield return scriptProperty;
            }
        }

        public ScriptProperty Get(string scriptPropertyName)
        {
            var field = GetOrDefault(scriptPropertyName);
            if (field == null)
                throw new ScriptPropertyNotFoundException(scriptPropertyName);

            return field;
        }

        public ScriptProperty GetOrDefault(string scriptPropertyName)
        {
            if (!_scriptProperties.TryGetValue(scriptPropertyName, out var scriptMethod))
            {
                return null;
            }

            return scriptMethod;
        }

        public IEnumerator<ScriptProperty> GetEnumerator()
        {
            foreach (var key in _scriptProperties.Keys)
            {
                yield return _scriptProperties[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}