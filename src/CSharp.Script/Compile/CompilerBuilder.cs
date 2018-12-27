using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSharp.Script.Compile
{
    public class CompilerBuilder
    {
        public HashSet<string> Usings { get; } = new HashSet<string>();

        public HashSet<Assembly> References { get; } = new HashSet<Assembly>();

        public HashSet<Type> Types { get; } = new HashSet<Type>();
        
        public ICompiler Build()
        {
            foreach (var type in Types)
            {
                References.Add(type.Assembly);
                Usings.Add(type.Namespace);
            }

            var compiler = new Compiler(Usings, References);
            return new CacheCompiler(compiler);
        }
    }
}