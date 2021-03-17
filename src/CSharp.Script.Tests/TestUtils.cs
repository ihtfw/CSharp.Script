using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CSharp.Script.Compile;
using CSharp.Script.Execute;

namespace CSharp.Script.Tests
{
    static class TestUtils
    {
        public static ScriptContainer GetContainer(params string[] lines)
        {
            var compiler = new Compiler(Enumerable.Empty<string>(), Enumerable.Empty<Assembly>());
            var sourceCodeBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                sourceCodeBuilder.AppendLine(line);
            }

            var assembly = compiler.Compile(sourceCodeBuilder.ToString());
            return new ScriptContainer(assembly);
        }
    }
}
