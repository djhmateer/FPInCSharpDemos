using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CSharp
{
    // Useful Functional aspects of C#
    // run from WSL -
    // dotnet build CSharp.csproj
    // dotnet run CSharp
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world CSharp");

            // C# 1.1
            // Conditional operator (ternary) as we aim to have everything an expression
            // rather than a sequence of statements (as they lead to more readable code)
            var rc = GetValue(true); //Some(Bob)

            // C# 2.0 (2005)
            // Generics

            // first class functions (and closures) in the form of anonymous delegates
            // Func<T> is a predefined type for a method that returns some value of T
            // Func returns a string
            Func<string> f = GetMessage;
            var result = f; // Hello World

            // Func accepts an int and returns an int
            Func<int, int> triple = delegate (int x) { return x * 3; };
            int a = triple(4); // 12

            // C# 3.0 (2007)
            // var keyword
            var sentence = "The answer is 42";

            // Lambda expressions
            Func<int, int> tripleb = x => x * 3;
            var b = tripleb(4); // 12

            // Extension methods
            var name = "dave".CapitaliseFirstLetter(); // Dave

            // LINQ (Language Integrated Query)
            // C#'s way of doing monads
            var listOfNames = new List<string> { "Bob", "Alice", "Dave", "Dougal" };
            // Lambda expression
            IEnumerable<string> r = listOfNames.Where(x => x.StartsWith("D")); // Dave, Dougal

            // extension method chaining in LINQ
            // railway oriented
            var rd = Enumerable.Range(1, 100)
               .Where(x => x % 20 == 0) // filter with a predicate (a function which returns a bool) here using a lambda expression so only get 20,40..
               .OrderBy(x => -x) // sort by descending into a new sequence
               .Select(x => $"{x}%"); // map each numerical value to a string suffixed by a % into a new sequence

            // Select is like Map
            // SelectMany is like Bind/Flatmap

            // C# 4.0 (2010)

            // C# 5.0 (2012)
            // async await

            // C# 6.0 (2015)
            // Expression body members for method and property get
            // Import static members of a type
            // Get only auto property for immutable objects
            // String interpolation

            // DotNetCore release (2016) which can run on Linux/Mac

            // C# 7.0, 7.1, 7.2, 7.3 (2017..2018)
            // tuple types and tuple named elements
            // pattern matching

            // C# 8 (2020?)
            // nullable and non-nullable reference types - solve same problem as Option<T>
        }

        // Functional language-ext library for Option<T> (Maybe), Some, None
        // C# 6 Expression body member =>
        static Option<string> GetValue(bool hasValue) => hasValue ? Some("Bob") : None;

        // Expression body syntax
        static string GetMessage() => "Hello world!";
    }

    // Static classes cannot be instantiated and only allow static members
    static class StringExtensions
    {
        // Extension method on string
        public static string CapitaliseFirstLetter(this string input) =>
            input.First().ToString().ToUpper() + input.Substring(1);
    }

    class Person
    {
        public string Name { get; } // C#6 Getter only auto-property 
    }
}
