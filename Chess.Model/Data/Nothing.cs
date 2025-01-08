namespace Chess.Model.Data
{
    using System;

    public class Nothing<T> : IMaybe<T>
    {
        public static readonly Nothing<T> Instance = new();

        public bool HasValue => false;

        public IMaybe<V> Bind<V>(Func<T, IMaybe<V>> _)
        {
            return Nothing<V>.Instance;
        }

        public void Do(Action<T> _)
        {
            return;
        }

        public void DoOrElse(Action<T> _, Action alternative)
        {
            alternative();
        }

        public T GetOrElse(T alternative)
        {
            return alternative;
        }

        public T GetOrElse(Func<T> alternative)
        {
            return alternative();
        }

        public V GetOrElse<V>(Func<T, V> _, V alternative)
        {
            return alternative;
        }

        public V GetOrElse<V>(Func<T, V> _, Func<V> alternative)
        {
            return alternative();
        }

        public IMaybe<V> Map<V>(Func<T, V> _)
        {
            return Nothing<V>.Instance;
        }
    }
}