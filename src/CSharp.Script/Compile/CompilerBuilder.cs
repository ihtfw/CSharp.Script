using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharp.Script.Compile
{
    public class CompilerBuilder
    {
        public HashSet<string> Usings { get; } = new HashSet<string>();

        public HashSet<Assembly> References { get; } = new HashSet<Assembly>();

        public HashSet<Type> Types { get; } = new HashSet<Type>();
        
        public virtual ICompiler Build()
        {
            Types.Add(typeof(Enumerable));
            Types.Add(typeof(File));
            Types.Add(typeof(List<>));
            Types.Add(typeof(StringBuilder));

            foreach (var type in Types)
            {
                References.Add(type.Assembly);
                Usings.Add(type.Namespace);
            }
            
            Usings.Add("System");
            Usings.Add("System.Text");
            Usings.Add("System.IO");
            Usings.Add("System.Linq");
            Usings.Add("System.Collections.Generic");

            var compiler = new Compiler(Usings, References);
            return new CacheCompiler(compiler);
        }
    }
}