using System;
using System.Linq;
using System.Diagnostics;

namespace ConcurrentLab5
{
    class Program
    {
        static Random randNum = new Random();
        static int[] arr;
        static void swap(ref int a, ref int b)
        {
            var t = a;
            a = b;
            b = t;
        }

        static void syncShellSort(ref int[] array, int start, int stop = 1)
        {
            var d = start;
            while (d >= stop)
            {
                for (var i = d; i < array.Length; i++)
                {
                    var j = i;
                    while ((j >= d) && (array[j - d] > array[j]))
                    {
                        swap(ref array[j], ref array[j - d]);
                        j = j - d;
                    }
                }
                d = d / 2;
            }
        }

        static void parallelShellSort(ref int[] array)
        {

        }

        static int[] randomArr(int len)
        {
            return Enumerable
                    .Repeat(0, len)
                    .Select(i => randNum.Next(1, 1000))
                    .ToArray();
        }

        static int[] ascArr(int len)
        {
            return Enumerable
                    .Range(0, len)
                    .Select(i => i)
                    .ToArray();
        }

        static int[] descArr(int len)
        {
            return Enumerable
                    .Range(0, len)
                    .Select(i => len - i)
                    .ToArray();
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            for(int len = 5000; len < 50000000; len = (int)(len * 1.1))
            {
                int[] a = randomArr(len);
                // int[] a = ascArr(len);
                // int[] a = descArr(len);
                sw.Restart();
                syncShellSort(ref a, a.Length / 2);
                sw.Stop();
                Console.WriteLine("{1}", len, Math.Round(sw.Elapsed.TotalMilliseconds));
            }
        }
    }
}
