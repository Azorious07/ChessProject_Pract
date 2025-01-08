namespace Chess.Model.Data
{
    using System;

    public class Just<T> : IMaybe<T>
    {
        public readonly T Value;

        public Just(T value)
        {
            Validation.NotNull(value, nameof(value));
            this.Value = value;
        }

        public bool HasValue => true;

        public IMaybe<V> Bind<V>(Func<T, IMaybe<V>> func)
        {
            return func(this.Value);
        }

        public void Do(Action<T> func)
        {
            func(this.Value);
        }

        public void DoOrElse(Action<T> func, Action _)
        {
            func(this.Value);
        }

        public T GetOrElse(T _)
        {
            return this.Value;
        }

        public T GetOrElse(Func<T> _)
        {
            return this.Value;
        }

        public V GetOrElse<V>(Func<T, V> func, V _)
        {
            return func(this.Value);
        }

        public V GetOrElse<V>(Func<T, V> func, Func<V> _)
        {
            return func(this.Value);
        }

        public IMaybe<V> Map<V>(Func<T, V> func)
        {
            return new Just<V>(func(this.Value));
        }
    }
}