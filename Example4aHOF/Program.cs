using System;
using NUnit.Framework;
using static System.Console;
using static System.Math;

namespace Example4aHOF
{
    public enum BmiRange { Underweight, Healthy, Overweight }

    public static class Program
    {
        static void Main()
        {
            // Injecting functions as dependencies (so we are able to test the Run method below)
            // Passing impure functions into the Run HOF
            Run(Read, Write);
        }

        // HOF returns void, read is a function which takes a string a returns a double,
        // write function that takes a BmiRange and returns void 
        public static void Run(Func<string, double> read, Action<BmiRange> write)
        {
            // 1.input
            // multiple declarations C#3
            // using the injected function to do a Console.Read and Parse to do a double
            double weight = read("weight"),
                   height = read("height");

            // 2.computation
            // static function and extension method on double easy to test as both pure functions
            var bmiRange = CalculateBmi(height, weight)
                           .ToBmiRange();

            // 3.output
            // using injected function to Console.WriteLine
            write(bmiRange);
        }

        // Isolated the pure computational functions below from impure I/O
        static double CalculateBmi(double height, double weight)
            => Round(weight / Pow(height, 2), 2);

        // Extension method 
        public static BmiRange ToBmiRange(this double bmi)
            => bmi < 18.5 ? BmiRange.Underweight
                : 25 <= bmi ? BmiRange.Overweight
                : BmiRange.Healthy;

        // Impure functions (will not test)
        // I/O always considered a side effect (as what happens in the outside world will effect the double returned)
        static double Read(string field)
        {
            WriteLine($"Please enter your {field}");
            return double.Parse(ReadLine());
        }

        static void Write(BmiRange bmiRange)
            => WriteLine($"Based on your BMI, you are {bmiRange}");
    }

    public class BmiTests
    {
        // Easy to test the pure computational functions!
        [TestCase(1.80, 77, ExpectedResult = 23.77)]
        [TestCase(1.60, 77, ExpectedResult = 30.08)]
        public double CalculateBmi(double height, double weight)
            => CalculateBmi(height, weight);

        // testing ToBmiRange
        [TestCase(23.77, ExpectedResult = BmiRange.Healthy)]
        [TestCase(30.08, ExpectedResult = BmiRange.Overweight)]
        public BmiRange ToBmiRange(double bmi) => bmi.ToBmiRange();

        // testing Run
        // this is good as testing the actual output of the program (and not just units)
        // just not testing the impure functions (faking them)
        [TestCase(1.80, 77, ExpectedResult = BmiRange.Healthy)]
        [TestCase(1.60, 77, ExpectedResult = BmiRange.Overweight)]
        public BmiRange ReadBmi(double height, double weight)
        {
            var result = default(BmiRange);
            // defining two pure fake functions to pass into the HOF
            // takes a string as input (the field name) and returns a double
            // we don't need to double.Parse as we control the test data
            Func<string, double> read = s => s == "height" ? height : weight;
            // takes a BmiRange and returns void 
            // uses a local variable (result) to hold the value of BmiRange passed into the function, which the test returns
            Action<BmiRange> write = r => result = r;
            Program.Run(read, write);
            return result;
        }
    }
}
