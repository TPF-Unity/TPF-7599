namespace Misc
{
    using System;

    public static class ArrayExtensions
    {
        private static Random rng = new Random();

        // Extension method to create a new shuffled copy of an array
        public static T[] ShuffledCopy<T>(this T[] array)
        {
            T[] newArray = (T[])array.Clone();
            int n = newArray.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (newArray[i], newArray[j]) = (newArray[j], newArray[i]);
            }

            return newArray;
        }
    }
}