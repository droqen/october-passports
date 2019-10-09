namespace navdi3.bitfont
{
    using UnityEngine;
    public class CharLot : EntityLot
    {
        public FontLot fontLot;

        public static CharLot NewCharLot(FontLot fontLot, string name = "", Vector3 localPosition = default(Vector3), string startingText = "", Transform parent = null)
        {
            var gob = new GameObject("charlot{" + name + "}");
            gob.transform.SetParent(parent);
            gob.transform.localPosition = localPosition;
            var lot = gob.AddComponent<CharLot>();
            lot.fontLot = fontLot;
            lot.lotName = name;
            lot.Print(startingText);
            return lot;
        }

        public void Print(string text)
        {
            Clear();
            twin pos = twin.zero;
            foreach(var c in text)
            {
                Show(pos, c);
                pos.x++;
            }
        }

        public void Show(twin pos, char c)
        {
            fontLot.SpawnChar(c, transform, transform.position + new Vector3( pos.x * fontLot.charWidth, pos.y * fontLot.lineHeight ));
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