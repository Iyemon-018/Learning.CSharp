// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Reflection;
using CSharp11;

var attributes = typeof(TypeAttributeUseClass)
                .GetMethod(nameof(TypeAttributeUseClass.Method))
                .GetCustomAttributes();
Console.WriteLine($"{typeof(BeforeTypeAttribute)}: {attributes.OfType<BeforeTypeAttribute>().First().ParamType}");
Console.WriteLine($"{typeof(GenericAttribute<>)}: {attributes.OfType<GenericAttribute<string>>().First().ParamType}");


