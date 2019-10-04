namespace navdi3.bitfont
{
    using UnityEngine;
    public class CharLot : EntityLot
    {
        public FontLot fontLot;

        public static CharLot NewCharLot(FontLot fontLot, string name = "", Transform parent = null)
        {
            var gob = new GameObject("charlot{" + name + "}");
            gob.transform.SetParent(parent);
            gob.transform.localPosition = Vector3.zero;
            var lot = gob.AddComponent<CharLot>();
            lot.fontLot = fontLot;
            lot.lotName = name;
            return lot;
        }

        public void Print(string text)
        {

        }

        //public void Show(Vector2Int pos, char c)
        //{
        //    if (c == ' ')
        //    {

        //    }
        //}

        //public void Show(char c)
        //{
        //    spriter.sprite = font.GetCharSprite(c);
        //}
        //new public Char Setup()
        //{
        //    if (font == null) throw Dj.Crash("'Char' prototype " + this.gameObject.name + " doesn't have a Font assigned");
        //    return base.Setup<Char>();
        //}
    }
}