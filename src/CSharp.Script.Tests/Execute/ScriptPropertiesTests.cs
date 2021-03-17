using NUnit.Framework;

namespace CSharp.Script.Tests.Execute
{
    [TestFixture]
    public class ScriptPropertiesTests
    {
        [Test]
        public void Lamba()
        {
            var container = TestUtils.GetContainer(@"public int Foo => 1;");

            var actual = container.Properties["Foo"].Get<int>();

            Assert.AreEqual(1, actual);
        }
    }
}