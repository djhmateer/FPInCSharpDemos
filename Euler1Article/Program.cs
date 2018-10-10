using System;
using Xunit;

namespace Euler1Article
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        [Fact]
        public void A() => Assert.Equal(1, 1);
    }
}
