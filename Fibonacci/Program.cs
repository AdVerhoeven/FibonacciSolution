using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;

namespace Fibonacci
{
    class Program
    {
        static void Main(string[] args)
        {
            Fibonacci fibonacci = Fibonacci.GetInstance();

            int index = 1;
            while (true)
            {
                
                BigInteger e = fibonacci.GetElement(index);
                BigInteger last = fibonacci.Sequence.Last();
                if (e != last)
                {
                    Console.WriteLine($"Mismatch at: {index,15}\tDelta: {e-last,15}");
                    break;
                }
                index++;
                fibonacci.Next();
            }

            foreach (var item in fibonacci)
            {
                Console.WriteLine(item);
            }
        }
    }
}
