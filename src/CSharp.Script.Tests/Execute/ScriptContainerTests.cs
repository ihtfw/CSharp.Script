using System.Linq;
using System.Reflection;
using System.Text;
using CSharp.Script.Compile;
using CSharp.Script.Execute;
using NUnit.Framework;

namespace CSharp.Script.Tests.Execute
{
    [TestFixture]
    public class ScriptContainerTests
    {
        ScriptContainer _scriptContainer;

        [SetUp]
        public void SetUp()
        {
            var compiler = new Compiler(Enumerable.Empty<string>(), Enumerable.Empty<Assembly>());
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
            _scriptContainer = new ScriptContainer(assembly);
        }

        [Test]
        public void ExecuteVoidTest()
        {
            var scriptMethodName = "VoidMethod";

            Assert.IsTrue(_scriptContainer.Methods.Contains(scriptMethodName));
            var retObj = _scriptContainer.Methods.Get(scriptMethodName).Invoke();
            Assert.IsNull(retObj);
        }

        [Test]
        public void ReturnSomeTest()
        {
            var scriptMethodName = "ReturnSome";

            var result = _scriptContainer.Methods.Get(scriptMethodName).Invoke<int>();

            Assert.AreEqual(10, result);
        }

        [Test]
        public void StateTest()
        {
            var stateField = _scriptContainer.Fields.Get("State");
            Assert.AreEqual(5, stateField.Get<int>());

            _scriptContainer.Methods.Get("UpdateState").Invoke();

            Assert.AreEqual(11, stateField.Get<int>());
        }

        [Test]
        public void StatePropTest()
        {
            var stateProp = _scriptContainer.Properties.Get("StateProp");
            Assert.AreEqual(3, stateProp.Get<int>());

            _scriptContainer.Methods.Get("UpdateStateProp").Invoke();

            Assert.AreEqual(4, stateProp.Get<int>());
        }

        [Test]
        public void SetProperties()
        {
            _scriptContainer.SetProperties((long)3);

            Assert.AreEqual(0, _scriptContainer.Properties.Get("IntProp").Get());
            Assert.AreEqual(3, _scriptContainer.Properties.Get("LongProp").Get());
            Assert.AreEqual(null, _scriptContainer.Properties.Get("StringProp").Get());
        }

        [Test]
        public void SetFields()
        {
            _scriptContainer.SetFields((long)3);

            Assert.AreEqual(0, _scriptContainer.Fields.Get("IntField").Get());
            Assert.AreEqual(3, _scriptContainer.Fields.Get("LongField").Get());
            Assert.AreEqual(null, _scriptContainer.Fields.Get("StringField").Get());
        }
    }
}
