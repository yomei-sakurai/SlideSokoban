using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class StringExtensions
    {
        public static Color ToColor (this string self)
        {
            var color = default (Color);
            if (!ColorUtility.TryParseHtmlString (self, out color))
            {
                Debug.LogWarning ("不明なカラーコード:" + self);
            }
            return color;
        }
    }
}