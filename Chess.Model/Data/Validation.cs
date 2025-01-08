namespace Chess.Model.Data
{
    using System;

    public static class Validation
    {
        public static void NotNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void InRange<T>(T obj, T min, T max, string paramName) where T : IComparable<T>
        {
            if (obj.CompareTo(min) < 0 || obj.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        public static bool IsInRange<T>(T obj, T min, T max) where T : IComparable<T>
        {
            return obj.CompareTo(min) >= 0 && obj.CompareTo(max) <= 0;
        }
    }
}