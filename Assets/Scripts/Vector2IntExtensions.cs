using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int Up (this Vector2Int v)
        {
            --v.y;
            return v;
        }
        public static Vector2Int Down (this Vector2Int v)
        {
            ++v.y;
            return v;
        }
        public static Vector2Int Left (this Vector2Int v)
        {
            --v.x;
            return v;
        }
        public static Vector2Int Right (this Vector2Int v)
        {
            ++v.x;
            return v;
        }

        public static void Foreach (this Vector2Int v, Action<int, int> action)
        {
            for (int x = 0; x < v.x; ++x)
            {
                for (int y = 0; y < v.y; ++y)
                {
                    action (x, y);
                }
            }
        }
    }
}