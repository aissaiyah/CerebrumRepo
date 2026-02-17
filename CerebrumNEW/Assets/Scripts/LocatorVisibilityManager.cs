using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorVisibilityManager : LocatorVisibility
{
//    public static bool allOn=false;
//    public bool on=true;

    void Awake()
    {
//        if (!on)
        allOn = on;
    }

}
