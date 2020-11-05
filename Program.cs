using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;

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

        static void syncShellSort(ref int[] array, int devidersCount, Func<int, int, int> getDevider)
        {
            int d = -1;
            for (int di = 0; di < devidersCount; di++)
            {
                d = getDevider(di, d);
                if (d < 1) break;
                for (var i = d; i < array.Length; i++)
                {
                    var j = i;
                    while ((j >= d) && (array[j - d] > array[j]))
                    {
                        swap(ref array[j], ref array[j - d]);
                        j = j - d;
                    }
                }
            }
        }

        static Func<int, int, int> getParallelShelSortDevider(int start, int step)
        {
            return (int di, int d) => {
                if (di == 0) return start;
                else return d + step;
            };
        }

        static void parallelShellSort(ref int[] array, int threadsCount)
        {
            var threads = new Thread[threadsCount];
            int devidersCount = array.Length / threadsCount;
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread((object o) => {
                    int[] arr = (int[]) o;
                    syncShellSort(ref arr, devidersCount, getParallelShelSortDevider(i, threadsCount));
                });
                threads[i].Start((object)array);
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
            syncShellSort(ref array, array.Length, getParallelShelSortDevider(1, 0));
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
                // syncShellSort(ref a, a.Length / 2, (int di, int d) => {
                //     if (di == 0) return a.Length;
                //     else return d / 2;
                // });
                parallelShellSort(ref a, 4);
                sw.Stop();
                Console.WriteLine("{1}", len, Math.Round(sw.Elapsed.TotalMilliseconds));
            }
        }
    }
}
