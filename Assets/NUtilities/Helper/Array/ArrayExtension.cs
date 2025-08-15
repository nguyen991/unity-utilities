using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NUtilities.Helper.Array
{
    public static class ArrayExtension
    {
        public static IList<T> Shuffle<T>(this IList<T> array)
        {
            int n = array.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (array[j], array[i]) = (array[i], array[j]);
            }
            return array;
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            return Shuffle((IList<T>)array).ToArray();
        }
    }
}
