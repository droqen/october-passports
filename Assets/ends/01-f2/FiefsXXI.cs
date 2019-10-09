using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using navdi3;
using navdi3.xxi;
using navdi3.bitfont;

public class FiefsXXI : BaseTilemapXXI
{

    public F2Client client;

    public FontLot fontLot;

    EntityLot players { get { return GetEntLot("players"); } }

    CharLot inputCharLot;
    string inputStr = "";

    private void Start()
    {
        inputCharLot = CharLot.NewCharLot(fontLot, "INPUT CHAR_LOT",
            localPosition: new Vector3(4, 4),
            startingText: "I");

        InitializeManualTT();
        RefreshClientView();
    }

    private void Update()
    {
        string lastInputStr = inputStr;
        foreach(var c in Input.inputString.ToLower())
        {
            if (c == '\n')
            {
                client.AttemptConnection();
                inputStr = "";

            }
            else if (char.IsControl(c))
            {
                if (c == (char)KeyCode.Backspace)
                {
                    inputStr = inputStr.Substring(0, inputStr.Length - 1);
                }

            } else if (
                Input.GetKey(KeyCode.LeftControl) ||
                Input.GetKey(KeyCode.RightControl) ||
                Input.GetKey(KeyCode.LeftCommand) ||
                Input.GetKey(KeyCode.RightCommand) )
            {
                // pass
            } else if (fontLot.alphabet.Contains(c.ToString()))
            {
                inputStr += c;
            }
        }
        if (lastInputStr != inputStr)
            inputCharLot.Print(inputStr + "I");
    }



    public void RefreshClientView()
    {
        // re-check and refresh client visualization, state of world, etc
        ClearAllTilesTT();
        if (client.IsConnected)
            new twinrect(0, 0, 9, 9).DoEach( cell => { Sett(cell, Random.Range(0, 9 + 1)); } );
        else
            new twinrect(0, 0, 9, 9).DoEach( cell => { Sett(cell, 0); } );
    }



    public override int[] GetSolidTileIds()
    {
        return new int[] { 3, 4, 5, 6, 7, 8, 9, };
    }
    public override int[] GetSpawnTileIds()
    {
        return new int[0];
    }
    public override void SpawnTileId(int TileId, Vector3Int TilePos)
    {
        throw new System.NotImplementedException();
    }
}
