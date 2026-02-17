using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class OutPutPatient : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Name, Gender, ID, BloodO2, BloodPressure, Pulse, HeartRate, RR;
    public string PatientName;
    public string path;
    void Start()
    {
        path = Application.dataPath + "/" + PatientName + ".txt"; //Path of the File
     
    }

        // Update is called once per frame
        void Update()
    {
        
    }

    public void OutputLog()
    {

    }
}
