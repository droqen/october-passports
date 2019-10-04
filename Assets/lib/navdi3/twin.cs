namespace navdi3
{
    using UnityEngine;
    [System.Serializable]
    /// <summary>twinteger: IntVector2</summary>
    public struct twin
    {
        public int x; public int y;
        public twin(Vector3Int v) { this.x = v.x; this.y = v.y; }
        public twin(Vector2Int v) { this.x = v.x; this.y = v.y; }
        public twin(Vector2 v2) { this.x = (int)v2.x; this.y = (int)v2.y; }
        public twin(int x, int y) { this.x = x; this.y = y; }
        public int taxicabLength { get { return Mathf.Abs(x) + Mathf.Abs(y); } }
        public float sqrLength { get { return x * x + y * y; } }

        public static twin operator +(twin p1, twin p2) { return new twin(p1.x + p2.x, p1.y + p2.y); }
        public static twin operator -(twin p1, twin p2) { return new twin(p1.x - p2.x, p1.y - p2.y); }
        public static twin operator -(twin p) { return new twin(-p.x, -p.y); }
        public static twin operator *(twin p1, twin p2) { return new twin(p1.x * p2.x, p1.y * p2.y); }
        public static twin operator /(twin p1, twin p2) { return new twin(p1.x / p2.x, p1.y / p2.y); }
        public static twin operator *(twin p, int i) { return new twin(p.x * i, p.y * i); }
        public static twin operator /(twin p, int i) { return new twin(p.x / i, p.y / i); }
        public static bool operator ==(twin p1, twin p2) { return p1.x == p2.x && p1.y == p2.y; }
        public static bool operator !=(twin p1, twin p2) { return p1.x != p2.x || p1.y != p2.y; }
        public static bool operator <(twin pL, twin pR) { return pL.x < pR.x && pL.y < pR.y; }
        public static bool operator >(twin pL, twin pR) { return pL.x > pR.x && pL.y > pR.y; }
        public static bool operator <=(twin pL, twin pR) { return pL.x <= pR.x && pL.y <= pR.y; }
        public static bool operator >=(twin pL, twin pR) { return pL.x >= pR.x && pL.y >= pR.y; }
        public static twin zero { get { return new twin(0, 0); } }
        public static twin one { get { return new twin(1, 1); } }
        public static twin right { get { return new twin(1, 0); } }
        public static twin up { get { return new twin(0, 1); } }
        public static twin left { get { return new twin(-1, 0); } }
        public static twin down { get { return new twin(0, -1); } }
        public static implicit operator Vector2(twin pos) { return new Vector2(pos.x, pos.y); }
        public static implicit operator Vector3(twin pos) { return new Vector3(pos.x, pos.y); }
        public static implicit operator Vector2Int(twin pos) { return new Vector2Int(pos.x, pos.y); }
        public static implicit operator Vector3Int(twin pos) { return new Vector3Int(pos.x, pos.y, 0); }
        override public bool Equals(object obj)
        {
            if (obj is twin) return this == (twin)obj;
            if (obj is Vector2) return this.x == ((Vector2)obj).x && this.y == ((Vector2)obj).y;
            return false;
        }
        override public int GetHashCode()
        {
            return x * short.MaxValue + y;
        }
        override public string ToString()
        {
            return string.Format("twin({0},{1})", x, y);
        }

        public twin Scale(float sx, float sy)
        {
            return new twin(Mathf.RoundToInt(this.x * sx), Mathf.RoundToInt(this.y * sy));
        }

        public twin Clamp(twinrect rect)
        {
            return new twin(Mathf.Clamp(x, rect.min.x, rect.max.x), Mathf.Clamp(y, rect.min.y, rect.max.y));
        }

        public static void StraightenCompass()
        {
            if (compassStraight) return;
            compass = new twin[] { right, up, left, down };
            compassStraight = true;
        }
        public static void ShuffleCompass()
        {
            if (compass == null) StraightenCompass();
            Util.shufl<twin>(ref compass);
            compassStraight = false;
        }
        public static twin[] compass = { right, up, left, down };
        static bool compassStraight;
    }
}