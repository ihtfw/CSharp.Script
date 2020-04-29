using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CSharp.Script.Compile;
using CSharp.Script.Exceptions;
using NUnit.Framework;

namespace CSharp.Script.Tests.Compile
{
    [TestFixture]
    public class CompilerTests
    {
        [Test]
        public void BuildFullCodeTextTest()
        {
            var compiler = new Compiler();
            var fullText = compiler.BuildFullSourceCode(@"public string Foo(){ return ""HelloWorld""; }");

            Console.WriteLine(fullText);
        }

        [Test]
        public void CompileTest()
        {
            var compiler = new Compiler();
            var sourceCodeBuilder = new StringBuilder();

            sourceCodeBuilder.AppendLine(@"public void SetDefaultValues(){ StateProp = 3; }");

            sourceCodeBuilder.AppendLine(@"public void VoidMethod(){}");
            sourceCodeBuilder.AppendLine(@"public int ReturnSome() { return 10; }");

            sourceCodeBuilder.AppendLine(@"public int State = 5;");
            sourceCodeBuilder.AppendLine(@"public void UpdateState() { State = 11; }");

            sourceCodeBuilder.AppendLine(@"public int StateProp { get; set; }");
            sourceCodeBuilder.AppendLine(@"public void UpdateStateProp() { StateProp++; }");

            sourceCodeBuilder.AppendLine(@"public long LongProp { get; set; }");
            sourceCodeBuilder.AppendLine(@"public int IntProp { get; set; }");
            sourceCodeBuilder.AppendLine(@"public string StringProp { get; set; }");

            sourceCodeBuilder.AppendLine(@"public long LongField;");
            sourceCodeBuilder.AppendLine(@"public int IntField;");
            sourceCodeBuilder.AppendLine(@"public string StringField;");

            var assembly = compiler.Compile(sourceCodeBuilder.ToString());

            Console.WriteLine(compiler.BuildFullSourceCode(sourceCodeBuilder.ToString()));
        }

        [Test]
        public void ExtendsSyntax()
        {
            var compiler = new Compiler(new[]
                {
                    "CSharp.Script.Tests"
                },
                new[]
                {
                    Assembly.GetExecutingAssembly()
                });

            var sourceCode = @"extends TestClassToExtend; public string Foo(){ return ""HelloWorld""; }";

            var assembly = compiler.Compile(sourceCode);

            var types = assembly.GetTypes();
            Assert.AreEqual(1, types.Length);

            var type = types[0];

            Assert.IsTrue(typeof(TestClassToExtend).IsAssignableFrom(type));
        }

        [Test]
        public void ThrowsCompileExceptionOnBadSyntax()
        {
            var compiler = new Compiler();
            Assert.Throws<CompileException>(() =>
            {
                compiler.Compile("vpiruvnqeru9");
            });
        }

        [Test]
        public void BaseTempDir()
        {
            var dir = TestContext.CurrentContext.WorkDirectory;
            var compiler = new Compiler
            {
                BaseTempDir = dir
            };
            Console.WriteLine(dir);
            var assembly = compiler.Compile(@"public string Foo(){ return ""HelloWorld""; }");
        }

        /// <summary>
        /// bugfix of same file name on recompilation
        /// </summary>
        [Test]
        public void ParallelCompilation()
        {
            var compiler = new Compiler();

            Parallel.For(0, 100, i =>
            {
                var assembly = compiler.Compile(@"public string Foo(){ return ""HelloWorld""; }");
            });
        }
    }
}