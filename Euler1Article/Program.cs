﻿using System;
using System.Linq;
using Xunit;

namespace Euler1Article
{
    public class Program
    {
        // Project Euler Problem 1
        // If we list all the natural numbers below 10 that are multiples of 3 or 5,
        // we get 3, 5, 6 and 9. The sum of these multiples is 23.
        // Find the sum of all the multiples of 3 or 5 below 1000.

        private static void Main()
        {
            Console.WriteLine($"Answer is: {Run(10)}");
        }

        // 1. Imperative approach. Take every number from 1..n-1 and see if it is divisible by 3 or 5
        private static int Run(int n)
        {
            int total = 0;
            for (var i = 1; i < n; i++)
            {
                if (i % 3 == 0 || i % 5 == 0)
                {
                    total += i;
                }
            }
            return total;
        }




        // 2. Functional approach using LINQ
        // Create a Range, then pass a Lambda Expression
        // to the Where extension method then Sum extension method
        private static int RunLinq(int n) =>
            Enumerable.Range(1, n - 1) // creates a sequence [1,2,3..9]
                .Where(x => x % 3 == 0 || x % 5 == 0)
                // if predicate (function which takes parameter(s) and returns a bool
                // returns true, keep element, so we get [3,5,6,9]
                .Sum();


        [Fact]
        public void RunTest() => Assert.Equal(23, Run(10));
        [Fact]
        public void RunLinqTest() => Assert.Equal(23, RunLinq(10));
        [Fact]
        public void RunLinqTestFinal() => Assert.Equal(233168, RunLinq(1000));
    }
}
