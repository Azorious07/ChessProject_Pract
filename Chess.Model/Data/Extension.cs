namespace Chess.Model.Data
{
    using System;
    using System.Collections.Generic;

    public static class Extension
    {
        public static IEnumerable<T> Repeat<T>(this T start, Func<T, T> next)
        {
            while (true)
            {
                yield return start;
                start = next(start);
            }
        }

        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}