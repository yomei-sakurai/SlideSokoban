using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ListExtensions
    {
        public static List<T> GetRangeCustom<T> (this List<T> list, int index, int count)
        {
            var lastIndex = Mathf.Min (index + count - 1, list.Count - 1);
            return list.GetRange (index, lastIndex - index + 1);
        }
    }
}