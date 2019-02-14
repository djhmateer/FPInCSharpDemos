using System;

namespace Example3HOF
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine($"3 : {IsPrime(3)}");
            Console.WriteLine($"4 : {IsPrime(4)}");
            Console.WriteLine($"5 : {IsPrime(5)}");
        }

        // Pure function - input (number) will always produce the same output (bool)
        // as there is no shared state the function can be static
        static bool IsPrime(int number)
        {
            for (long i = 2; i < number; i++)
                if (number % i == 0)
                    return false;
            return true;
        }
    }
}
