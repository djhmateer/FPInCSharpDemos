using System;
using System.Linq;
using Xunit;

namespace Euler1Article
{

    public class Program
    {
        // If we list all the natural numbers below 10 that are multiples of 3 or 5,
        // we get 3, 5, 6 and 9.The sum of these multiples is 23.
        // Find the sum of all the multiples of 3 or 5 below 1000.

        // Expression body member for method
        // String interpolation to print the value
        // Passing argument 1000 to the Run method parameter
        private static void Main() => Console.WriteLine($"Answer is: {Run(1000)}");

        // 1. Imperative approach. Take every n from 1..1000 and see if it is divisible by 3 or 5
        private static int Run(int n)
        {
            int total = 0;
            for (var i = 1; i < n; i++)
                if (i % 3 == 0 || i % 5 == 0)
                    total += i;
            return total;
        }





        // 2. Functional approach using Linq
        // Create a Range, then pass a Lambda Expression (Anonymous Function creating a Delegate)
        // to the Where extension method
        // finishing with a Sum extension method
        private static int RunLinq(int n) => 
            Enumerable.Range(1, n - 1)
             .Where(x => x % 3 == 0 || x % 5 == 0)
             .Sum();


        [Fact]
        public void RunTest() => Assert.Equal(23, Run(10));
        [Fact]
        public void RunLinqTest() => Assert.Equal(23, RunLinq(10));
        [Fact]
        public void RunLinqTestFinal() => Assert.Equal(233168, RunLinq(1000));
    }
}
