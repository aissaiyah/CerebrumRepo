using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class PatientInformation : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Name, Gender, ID, BloodO2, BloodPressure, Pulse, HeartRate, RR;
    public InputField FalseId;
    //public OutPutPatient OP;
    public string path;
    void Start()
    {
        path = Application.dataPath + "/" + this.gameObject.name + ".txt"; //Path of the File
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OutPutLogInfor()
    {
        File.WriteAllText(path, "Name:\t" + Name.text + "\n");
        File.AppendAllText(path, "Gender:\t" + Gender.text + "\n");
        File.AppendAllText(path, "ID:\t" + ID.text);
        if(FalseId.text != null)
        {
            File.AppendAllText(path, "\t" +"False ID:\t" + FalseId.text + "\n");
        }else
            File.AppendAllText(path, "\n");

        File.AppendAllText(path, "Blood Oxygen:\t" + BloodO2.text + "\n");
        File.AppendAllText(path, "Blood Pressure:\t" + BloodPressure.text + "\n");
        File.AppendAllText(path, "Pulse:\t" + Pulse.text + "\n");
        File.AppendAllText(path, "Heart Rate:\t" +  HeartRate.text + "\n");
        File.AppendAllText(path, "Respiratory Rate:\t" + RR.text + "\n");
        /*OP.PatientName = this.gameObject.name;
        OP.Name = Name;
        OP.Gender = Gender;
        OP.ID = ID;
        OP.BloodO2 = BloodO2;
        OP.BloodPressure = BloodPressure;
        OP.Pulse = Pulse;
        OP.HeartRate = HeartRate;
        OP.RR = RR;*/

    }
}
