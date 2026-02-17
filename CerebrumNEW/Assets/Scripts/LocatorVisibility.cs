using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorVisibility : MonoBehaviour
{
    public static bool allOn=false;
    public bool on=true;


    void Start()
    {
        gameObject.SetActive(on && allOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
