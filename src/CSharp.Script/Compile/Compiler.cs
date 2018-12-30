using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CSharp.Script.Exceptions;
using Microsoft.CSharp;

namespace CSharp.Script.Compile
{
    public class Compiler : ICompiler
    {
        private readonly UniqueNameGenerator _uniqueNameGenerator = new UniqueNameGenerator();
        readonly CSharpCodeProvider _codeProvider;
        readonly CompilerParameters _parameters;

        public List<string> Usings { get; }
        public List<Assembly> References { get; }

        public Compiler() : this(Enumerable.Empty<string>(), Enumerable.Empty<Assembly>())
        {

        }
        public Compiler(IEnumerable<string> usings, IEnumerable<Assembly> references)
        {
            Usings = usings.ToList();
            References = references.ToList();
            
            _codeProvider = new CSharpCodeProvider();

            _parameters = new CompilerParameters
            {
                GenerateInMemory = true
            };
            
            _parameters.ReferencedAssemblies.AddRange(References.Select(a => a.Location).ToArray());
        }

        /// <inheritdoc />
        public virtual Assembly Compile(string sourceCode)
        {
            var fullSourceCode = BuildFullSourceCode(sourceCode);
            CompilerResults results = _codeProvider.CompileAssemblyFromSource(_parameters, fullSourceCode);

            if (results.Errors.Count > 0)
            {
                var compileErrors = new List<CompileError>();
                string errors = "";
                foreach (CompilerError compErr in results.Errors)
                {
                    if (!compErr.IsWarning)
                    {
                        errors += "Line number " + compErr.Line + ", Error Number: " + compErr.ErrorNumber + ", '" + compErr.ErrorText + ";" + Environment.NewLine + Environment.NewLine;
                        compileErrors.Add(new CompileError(compErr));
                    }
                }

                if (compileErrors.Any())
                {
                    throw new CompileException(errors, compileErrors, sourceCode, fullSourceCode);
                }
            }

            var compiledAssembly = results.CompiledAssembly;
            return compiledAssembly;
        }

        /// <inheritdoc />
        public virtual string BuildFullSourceCode(string sourceCode)
        {
            string extendsClass = null;
            if (sourceCode.StartsWith("extends"))
            {
                var toIndex = sourceCode.IndexOf(';');
                if (toIndex > 0)
                {
                    extendsClass = sourceCode.Substring(7, toIndex - 7);
                }

                sourceCode = sourceCode.Substring(toIndex+1);
            }

            var sb = new StringBuilder();

            foreach (var @using in Usings)
            {
                sb.AppendLine($"using {@using};");
            }

            sb.Append("namespace ").Append(_uniqueNameGenerator.Generate()).AppendLine(" {");
            sb.Append("public class ").Append(_uniqueNameGenerator.Generate());
            if (extendsClass != null)
            {
                sb.Append(":").Append(extendsClass);
            }
            sb.AppendLine(" {");
            sb.AppendLine(sourceCode);
            sb.AppendLine("}}");

            return sb.ToString();
        }
    }
}