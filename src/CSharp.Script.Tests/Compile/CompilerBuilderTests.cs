using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Script.Compile;
using CSharp.Script.Execute;
using NUnit.Framework;

namespace CSharp.Script.Tests.Compile
{
    [TestFixture]
    public class CompilerBuilderTests
    {
        [Test]
        public void BuildTest()
        {
            var compilerBuilder = new CompilerBuilder();
            var compiler = compilerBuilder.Build();
            var assembly = compiler.Compile(@"public string Foo(){ return ""Hello World!""; }");
            var scriptContainer = new ScriptContainer(assembly);
            var returnValue = scriptContainer.Methods.Get("Foo").Invoke<string>();

            Assert.AreEqual("Hello World!", returnValue);
        }

        [Test]
        public void Using()
        {
            var myUsing = "MySpecialUsing";

            var compilerBuilder = new CompilerBuilder();
            
            compilerBuilder.Usings.Add(myUsing);
            var compiler = compilerBuilder.Build();

            var fullSourceCode = compiler.BuildFullSourceCode(@"public string Foo(){ return ""Hello World!""; }");
            Assert.IsTrue(fullSourceCode.Contains(myUsing));
        }
    }
}
