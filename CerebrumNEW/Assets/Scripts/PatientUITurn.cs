using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientUITurn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Main, ID;
    public bool MainOn = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pressed()
    {
        if(MainOn == false)
        {
            
            Main.SetActive(true);
            ID.SetActive(false);
        }
        if (MainOn == true)
        {
            
            Main.SetActive(false);
            ID.SetActive(true);
        }
    }
}
