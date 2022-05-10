using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardCollector.Extensions
{
    public static class Extensions
    {
        public static void Apply<T> (this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action.Invoke(item);
        }

        public static async Task Apply<T>(this IAsyncEnumerable<T> source, Action<T> action)
        {
            await foreach(var item in source) action.Invoke(item);
        }
        
        public static async Task Apply<T> (this IEnumerable<T> source, Func<T, Task> action)
        {
            foreach (var item in source) await action.Invoke(item);
        }

        public static async Task Apply<T>(this IAsyncEnumerable<T> source, Func<T, Task> action)
        {
            await foreach(var item in source) await action.Invoke(item);
        }
    }
}