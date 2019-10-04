namespace navdi3
{
    using UnityEngine;
    using System.Collections.Generic;
    [System.Serializable]
    /// <summary>twinteger: IntVector2</summary>
    public struct twinrect
    {
        public twin min;
        public twin mid { get { return (min + max) / 2; } }
        public twin max;
        public twinrect(int x1, int y1, int x2, int y2)
        {
            this.min = new twin(x1, y1); this.max = new twin(x2, y2);
        }
        public twinrect(twin min, twin max)
        {
            this.min = min; this.max = max;
        }
        public twin size { get { return this.max - this.min + twin.one; } }
        public static twinrect operator +(twinrect p1, twinrect p2) { return new twinrect(p1.min + p2.min, p1.max + p2.max); }
        public static twinrect operator -(twinrect p1, twinrect p2) { return new twinrect(p1.min - p2.min, p1.max - p2.max); }
        public static twinrect operator +(twinrect r, twin t) { return new twinrect(r.min + t, r.max + t); }
        public static twinrect operator -(twinrect r, twin t) { return new twinrect(r.min - t, r.max - t); }
        public bool Contains(twin point)
        {
            return point >= min && point <= max;
        }
        public void DoEach(System.Action<twin> func)
        {
            for (twin point = min; point.y <= max.y; point.x = min.x, point.y++)
            {
                for (; point.x <= max.x; point.x++)
                {
                    func(point);
                }
            }
        }
        public int GetArea()
        {
            twin size = max - min + twin.one;
            if (size.x < 0) size.x = -size.x;
            if (size.y < 0) size.y = -size.y;
            return size.x * size.y;
        }
        public twin[] GetAllPoints(bool shuffled = false)
        {
            List<twin> allPointsList = new List<twin>();
            DoEach((point) => { allPointsList.Add(point); });
            twin[] allPoints = allPointsList.ToArray();
            if (shuffled) Util.shufl<twin>(ref allPoints);
            return allPoints;
        }

        override public string ToString()
        {
            return string.Format("twinrect({0},{1},{2},{3})", min.x, min.y, max.x, max.y);
        }
    }
}