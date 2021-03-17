using CSharp.Script.Exceptions;
using NUnit.Framework;

namespace CSharp.Script.Tests.Execute
{
    [TestFixture]
    public class ScriptMethodTests
    {
        [Test]
        public void Overloads()
        {
            var container = TestUtils.GetContainer(
                @"public int Foo() => 1;",
                @"public int Foo(int b) => 2;",
                @"public int Foo(string c) => 3;");

            var sut = container.Methods.Get("Foo");
            Assert.AreEqual(1, sut.Invoke<int>());
            Assert.AreEqual(2, sut.Invoke<int>(1));
            Assert.AreEqual(3, sut.Invoke<int>("hey!"));
        }

        [Test]
        public void OverloadNoFound()
        {
            var container = TestUtils.GetContainer(
                @"public int Foo() => 1;",
                @"public int Foo(int b) => 2;");

            var sut = container.Methods.Get("Foo");

            Assert.Throws<ScriptMethodOverloadNotFoundException>(() =>
            {
                sut.Invoke<int>("hey!");
            });
        }
    }
}
