using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RoslynClassCreationAndCompile
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here I am creating a Compilation object
            CreateandCompile obj = new CreateandCompile();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(obj.CreateandCompileRun()); // Call the CreateandComplie Method

            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };


            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary
                ));

            // To run the compilation will use Emit and pass a Stream object to give me a EmitResult object
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    
                    // If successful I load the bytes into an Assembly object and use Reflection to Invoke the class 
                    Type type = assembly.GetType("RoslynClassCreationAndCompile.DisplayWord");
                    object objct = Activator.CreateInstance(type);
                    type.InvokeMember("Word",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        objct,
                        new object[] { "Hello World" });
                }
            }
            Console.ReadLine();
        }
    }
}
