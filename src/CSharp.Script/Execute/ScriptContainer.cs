using System;
using System.Linq;
using System.Reflection;

namespace CSharp.Script.Execute
{
    public class ScriptContainer
    {
        /// <summary>
        /// Will use first type in assembly
        /// </summary>
        /// <param name="assembly"></param>
        public ScriptContainer(Assembly assembly) : this(assembly.GetTypes().First())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public ScriptContainer(Type type)
        {
            var scriptObject = Activator.CreateInstance(type);
            
            Methods = new ScriptMethods(scriptObject);
            Fields = new ScriptFields(scriptObject);
            Properties = new ScriptProperties(scriptObject);

            Methods.GetOrDefault("SetDefaultValues")?.Invoke();
        }

        public ScriptMethods Methods { get; }
        public ScriptFields Fields { get; }
        public ScriptProperties Properties { get; }

        /// <summary>
        /// Will set all properties matched by object type
        /// </summary>
        /// <param name="objects"></param>
        public void SetProperties(params object[] objects)
        {
            foreach (var obj in objects)
            {
                var type = obj.GetType();

                foreach (var scriptProperty in Properties.ForEachWhereType(type))
                {
                    scriptProperty.Set(obj);
                }
            }
        }

        /// <summary>
        /// Will set all fields matched by object type
        /// </summary>
        /// <param name="objects"></param>
        public void SetFields(params object[] objects)
        {
            foreach (var obj in objects)
            {
                var type = obj.GetType();

                foreach (var scriptField in Fields.ForEachWhereType(type))
                {
                    scriptField.Set(obj);
                }
            }
        }
    }
}