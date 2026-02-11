    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScr : MonoBehaviour
{
    // Start is called before the first frame update
    public Text ScreenT, WallT, PatientT, BedT;
    public MainTextE1 MT;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OutPutLog()
    {
        MT.BedNumber = int.Parse(BedT.text);
        MT.ScreenNumber = int.Parse(ScreenT.text);
        MT.WallNumber = int.Parse(WallT.text);
        MT.PatientNumber = int.Parse(PatientT.text);
        MT.LevelGen();
    }
}
