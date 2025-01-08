namespace Chess.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMaybe<T>
    {
        bool HasValue { get; }

        IMaybe<V> Bind<V>(Func<T, IMaybe<V>> func);

        void Do(Action<T> func);

        void DoOrElse(Action<T> func, Action alternative);

        T GetOrElse(T alternative);

        T GetOrElse(Func<T> alternative);

        V GetOrElse<V>(Func<T, V> func, V alternative);

        V GetOrElse<V>(Func<T, V> func, Func<V> alternative);

        IMaybe<V> Map<V>(Func<T, V> func);
    }

    public static class MaybeExtension
    {
        public static IMaybe<T> Guard<T>(this IMaybe<T> maybe, Predicate<T> predicate)
        {
            return maybe.Bind(p => predicate(p) ? maybe : Nothing<T>.Instance);
        }

        public static IEnumerable<T> FilterMaybes<T>(this IEnumerable<IMaybe<T>> items)
        {
            return items.SelectMany(i => i.Yield());
        }

        public static IMaybe<T> Find<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    return new Just<T>(item);
                }
            }

            return Nothing<T>.Instance;
        }

        public static IEnumerable<T> ToEnumerable<T>(this IMaybe<IEnumerable<T>> maybe)
        {
            return maybe.Yield().SelectMany(p => p);
        }

        public static IEnumerable<T> Yield<T>(this IMaybe<T> maybe)
        {
            return maybe.GetOrElse
            (
                v => v.Yield(),
                Enumerable.Empty<T>()
            );
        }
    }
}