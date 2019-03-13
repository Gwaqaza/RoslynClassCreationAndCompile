using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace RoslynClassCreationAndCompile
{
    public class CreateandCompile
    {
        public string CreateandCompileRun()
        {
            // Creating a namespace
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("RoslynClassCreationAndCompile")).NormalizeWhitespace();
            @namespace = @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            // Public class called "DiaplayWord
            var classDeclaration = SyntaxFactory.ClassDeclaration("DisplayWord");
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Just a variable
            var variableDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName("string"))
                .AddVariables(SyntaxFactory.VariableDeclarator("displayMe"));
            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // The body block of the method
            var syntaxWrite = SyntaxFactory.ParseStatement("Console.WriteLine(message);");

            // Creating a parameter for the method
            List<ParameterSyntax> paramList = new List<ParameterSyntax>
            {
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("message")).WithType(SyntaxFactory.ParseTypeName("string"))
            };

            // A method called "Word", with a parameter: public void Word(string message)
            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Word")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(paramList.ToArray())
                .WithBody(SyntaxFactory.Block(syntaxWrite));

            classDeclaration = classDeclaration.AddMembers(fieldDeclaration, methodDeclaration);

            @namespace = @namespace.AddMembers(classDeclaration);

            var code = @namespace
                .NormalizeWhitespace()
                .ToFullString();

            return code;
        }
    }
}