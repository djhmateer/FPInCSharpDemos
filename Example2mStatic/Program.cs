using System;

namespace Example2mStatic
{
    class Program
    {
        static void Main(string[] args)
        {
            // C#3 Collection Initialiser
            var bob = new Person {Name = "Bob"};
            var alice = new Person {Name = "Alice"};
            bob.ChangeNameTo("Bob2");
            // bob and alice are separate instances of Person


            // Using static methods and a mutable Counter is very confusing
            // stuff in a non pure function (ie has side effects) so don't make static
            var result = Person.Stuff("Dave"); // Hello Dave 1
            Console.WriteLine(result);
            result = Person.Stuff("Ellie"); // Hello Ellie 2
            Console.WriteLine(result);
        }
    }

    class Person
    {
        public string Name { get; set; }

        public void ChangeNameTo(string name) => Name = name;

        static int Counter { get; set; }

        public static string Stuff(string name)
        {
            Counter++;
            return $"Hello {name} {Counter}";
        }
    }
}
