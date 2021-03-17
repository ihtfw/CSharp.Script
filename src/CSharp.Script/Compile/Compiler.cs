using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CSharp.Script.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace CSharp.Script.Compile
{
    public class Compiler : ICompiler
    {
        private readonly UniqueNameGenerator _uniqueNameGenerator = new();

        public List<string> Usings { get; }
        public List<Assembly> References { get; }
        
        public Compiler() : this(Enumerable.Empty<string>(), Enumerable.Empty<Assembly>())
        {
        }

        public Compiler(IEnumerable<string> usings, IEnumerable<Assembly> references)
        {
            Usings = usings.ToList();
            References = references.ToList();
        }

        private IEnumerable<string> AssemblyLocations()
        {
            foreach (var reference in References)
            {
                yield return reference.Location;
            }

            yield return typeof(object).Assembly.Location;
            yield return typeof(Enumerable).Assembly.Location;
        }

        /// <inheritdoc />
        public virtual Assembly Compile(string sourceCode)
        {
            var fullSourceCode = BuildFullSourceCode(sourceCode);

            var references = AssemblyLocations().Distinct().Select(path => MetadataReference.CreateFromFile(path)).ToArray();

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(fullSourceCode);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            
            CSharpCompilation compilation = CSharpCompilation.Create(
                null,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: options);

            using var ms = new MemoryStream();
            
            EmitResult result = compilation.Emit(ms);

            if (result.Success)
            {
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                return assembly;
            }
            
            var compileErrors = new List<CompileError>();
            string errors = "";

            IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error);

            foreach (Diagnostic diagnostic in failures)
            {
                var compileError = new CompileError(diagnostic);

                errors += "Line number " + compileError.Line + ", Error Number: " + compileError.ErrorNumber + ", '" +
                          compileError.ErrorText + ";" + Environment.NewLine + Environment.NewLine;
                
                compileErrors.Add(compileError);
            }

            throw new CompileException(errors, compileErrors, sourceCode, fullSourceCode);
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