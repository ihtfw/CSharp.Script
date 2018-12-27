using System.Reflection;

namespace CSharp.Script.Compile
{
    public interface ICompiler
    {
        /// <summary>
        /// Compiles source code to assembly
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        Assembly Compile(string sourceCode);

        /// <summary>
        /// Addd everything that source code needs, to became compilable
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        string BuildFullSourceCode(string sourceCode);
    }
}