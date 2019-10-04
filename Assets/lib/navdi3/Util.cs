namespace navdi3
{
    using UnityEngine;
    using System.Collections.Generic;
    public class Util
    {
        // shufl: list
        public static void shufl<T>(ref List<T> a, int start = -1, int end = -1)
        {
            if (start < 0) start = 0;
            if (end < 0) end = a.Count;
            for (int i = start; i < end - 1; i++)
            {
                int j = Random.Range(i, end);
                if (i == j) continue;
                T temp = a[i];
                a[i] = a[j];
                a[j] = temp;
            }
        }

        // shufl: array
        public static void shufl<T>(ref T[] a, int start = -1, int end = -1)
        {
            if (start < 0) start = 0;
            if (end < 0) end = a.Length;
            for (int i = start; i < end - 1; i++)
            {
                int j = Random.Range(i, end);
                if (i == j) continue;
                T temp = a[i];
                a[i] = a[j];
                a[j] = temp;
            }
        }

        public static int[] shufl_order(int length)
        {
            var items = range(0, length);
            shufl(ref items);
            return items;
        }
        public static int[] range(int start_inclusive, int end_exclusive)
        {
            int[] items = new int[end_exclusive - start_inclusive];
            for (int i = 0; i < items.Length; i++) items[i] = start_inclusive + i;
            return items;
        }

        public static int tow(int a, int b, int rate)
        {
            if (a + rate < b) return a + rate; else if (a - rate > b) return a - rate; else return b;
        }
        public static float tow(float a, float b, float rate)
        {
            if (a + rate < b) return a + rate; else if (a - rate > b) return a - rate; else return b;
        }
        public static float remap(float a1, float b1, float a2, float b2, float originalValue)
        {
            return (originalValue - a1) / (b1 - a1) * (b2 - a2) + a2;
        }

        public static bool boundbump(GameObject a, Vector3 move, GameObject b)
        {
            return boundbump(a.GetComponent<BoxCollider2D>().bounds, move, b.GetComponent<BoxCollider2D>().bounds);
        }
        public static bool boundbump(Bounds a, Vector3 move, Bounds b)
        {
            a.center += move;
            return a.Intersects(b);
        }
    }
}