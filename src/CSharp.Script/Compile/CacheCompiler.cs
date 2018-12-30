using System.Collections.Generic;
using System.Reflection;

namespace CSharp.Script.Compile
{
    public class CacheCompiler : ICompiler
    {
        private readonly ICompiler _compiler;
        private readonly Dictionary<string, Assembly> _cache = new Dictionary<string, Assembly>();

        public CacheCompiler(ICompiler compiler)
        {
            _compiler = compiler;
        }

        /// <inheritdoc />
        public virtual Assembly Compile(string sourceCode)
        {
            lock (_cache)
            {
                if (_cache.TryGetValue(sourceCode, out var assembly))
                {
                    return assembly;
                }
            }

            var compiledAssembly = _compiler.Compile(sourceCode);
            lock (_cache)
            {
                if (!_cache.ContainsKey(sourceCode))
                {
                    _cache.Add(sourceCode, compiledAssembly);
                }
            }

            return compiledAssembly;
        }

        /// <inheritdoc />
        public virtual string BuildFullSourceCode(string sourceCode)
        {
            return _compiler.BuildFullSourceCode(sourceCode);
        }
    }
}