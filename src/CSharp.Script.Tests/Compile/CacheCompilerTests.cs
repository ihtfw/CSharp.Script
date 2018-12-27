using System.Linq;
using System.Reflection;
using CSharp.Script.Compile;
using NUnit.Framework;

namespace CSharp.Script.Tests.Compile
{
    [TestFixture]
    public class CacheCompilerTests
    {
        [Test]
        public void Compile_ReturnsSameAssembly_OnSameSourceCode()
        {
            var compiler = new Compiler(Enumerable.Empty<string>(), Enumerable.Empty<Assembly>());
            var cachedCompiler = new CacheCompiler(compiler);

            var sourceCode = @"public string Foo(){ return ""HelloWorld""; }";

            var assembly = cachedCompiler.Compile(sourceCode);
            var assembly2 = cachedCompiler.Compile(sourceCode);

            Assert.AreEqual(assembly, assembly2);
        }
    }
}
