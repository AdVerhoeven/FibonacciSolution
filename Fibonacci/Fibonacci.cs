using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Fibonacci
{

    // This Singleton implementation is called "double check lock". It is safe
    // in multithreaded environment and provides lazy initialization for the
    // Singleton object.
    public class Fibonacci : IEnumerable<BigInteger>
    {
        #region Constants
        const decimal phi = 1.61803398874989484820458683436563811772030917980576286213544862270526046281890244970720720418939113748475408807538m;
        const decimal sqrt5 = 2.23606797749978969640917366873127623544061835961152572427089724541052092563780489941441440837878227496950817615077m;
        #endregion
        #region Properties and Fields
        //The complete and accurately generate list of fibonacci numbers
        private static List<BigInteger> fib = new List<BigInteger>() { 0, 1 };
        //The publicly accessible fibonacci sequence
        public IReadOnlyCollection<BigInteger> Sequence = fib.AsReadOnly();
        //Singleton instance
        private static Fibonacci _instance;

        // We now have a lock object that will be used to synchronize threads
        // during first access to the Singleton.
        private static readonly object _lock = new object();
        #endregion
        #region Constructor
        private Fibonacci() { }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new instance if it does not yet exist.
        /// </summary>
        /// <returns>A Fibonacci instance</returns>
        public static Fibonacci GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (_instance == null)
                    {
                        _instance = new Fibonacci();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Generates the next fibonacci number in the sequence.
        /// </summary>
        /// <returns>The newly regenerated fibonacci number</returns>
        public BigInteger GetNext()
        {
            Next();
            return fib[fib.Count - 1];
        }

        /// <summary>
        /// Generates the next fibonacci number in the sequence.
        /// </summary>
        public void Next()
        {
            //TODO: thread safe?
            fib.Add(fib[fib.Count - 1] + fib[fib.Count - 2]);
        }

        /// <summary>
        /// Gets a fibonacci number at a given index/position n without calculating previous elements. Accurate up to position 128.
        /// </summary>
        /// <param name="n">The index or position of the number to return.</param>
        /// <returns>The element of the fibonacci sequence at the given position.</returns>
        public BigInteger GetElement(int n)
        {
            if(n > 128)
            {
                Trace.TraceWarning("The number that is return is incorrect.");
            }
            var phin = phi.Pow(n);
            var psin = (1 - phi).Pow(n);
            var res = (phin - psin) / sqrt5;
            return (BigInteger)res;
        }

        #region IENumerable implementation
        public IEnumerator<BigInteger> GetEnumerator()
        {
            return ((IEnumerable<BigInteger>)fib).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<BigInteger>)fib).GetEnumerator();
        }
        #endregion
        #endregion
    }

    public static class DecimalExtension
    {
        public static decimal Pow(this decimal d, long n)
        {
            decimal y = 1;
            while (true)
            {
                if((n & 1) == 1)
                {
                    y *= d;
                }
                n >>= 1;
                if(n == 0)
                {
                    break;
                }
                d *= d;
            }
            return y;
        }
    }
}
