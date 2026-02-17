    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScr : MonoBehaviour
{
    // Start is called before the first frame update
//    public int RoomNumber = 0, BedNumber = 3, WallNumber = 6, ScreenNumber = 4, BinNumber = 0, CartNumber = 0;
    public Text RoomT, BedT, WallT, ScreenT, BinT, CartT ;
    public RoomSpecifier MT;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomChoice(int c)
    {
        MT.RoomNumber = c;
    }
    
    public void OutPutLog()
    {
//    public int RoomNumber = 0, BedNumber = 3, WallNumber = 6, ScreenNumber = 4, BinNumber = 0, CartNumber = 0;
//        MT.RoomNumber = int.Parse(RoomT.text);
        MT.BedNumber = int.Parse(BedT.text);
        MT.WallNumber = int.Parse(WallT.text);
        MT.ScreenNumber = int.Parse(ScreenT.text);
        MT.BinNumber = int.Parse(BinT.text);
        MT.CartNumber = int.Parse(CartT.text);
        MT.LevelGen();
    }
}
