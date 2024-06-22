namespace Misc
{
    using System;


    public static class ArrayExtensions
    {
        private static Random rng = new Random();
        
        public static T[] ShuffledCopy<T>(this T[] array)
        {
            var newArray = (T[])array.Clone();
            var n = newArray.Length;
            for (var i = n - 1; i > 0; i--)
            {
                var j = rng.Next(i + 1);
                (newArray[i], newArray[j]) = (newArray[j], newArray[i]);
            }

            return newArray;
        }
    }
}