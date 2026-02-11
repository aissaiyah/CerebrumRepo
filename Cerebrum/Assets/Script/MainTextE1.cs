using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class MainTextE1 : MonoBehaviour
{
    // Start is called before the first frame update
    public string path; 
    public int BedNumber = 3, PatientNumber = 2, WallNumber = 6, ScreenNumber = 4;
    



    void Start()
    {
        path = Application.dataPath + "/ErjunWangStuff" + "/LevelGen.txt"; //Path of the File
        CreateText();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            LevelGen();
            Debug.Log("i");
        }
    }

    void CreateText()
    {
        
 
        //Create File if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Login Log \n\n");

        }
        //Content of the file
       
       
    }

    void Log1Start()
    {
        
        File.WriteAllText(path, "Do '01/LG01Start.txt'"); //writeall test will overwrite eveything
    }

   public void LevelGen()
    {
        string content = "Room \t" + 0 + "\tRoomXLGPatient_base \n";        // Default Room                 // \t = + tab

        //Overwrite Everything for Level Generation 
        File.WriteAllText(path, content);

        int CurrentPN = 0;
        for (int i = 1; i <= BedNumber; i++)  // Bed Generation
        {
            if (CurrentPN <= PatientNumber)
            {
                File.AppendAllText(path, "Bed \t" + i + "\t" + "PatientBed01\t" + "Patient\t" + "1\t" + "PatientTemp \n");
                CurrentPN = CurrentPN + 1;
            }
            else
                File.AppendAllText(path, "Bed \t" + i  + "\t" + "PatientBed01\t\n");
        }

        for (int i = 1; i <= WallNumber; i++) // Wall generation
        {
           // if(i < 4)
            File.AppendAllText(path, "Wall \t" + i + "\t" + "Poster0" + i +"\n");
           // else
           // File.AppendAllText(path, "Wall \t" + i + "\t" + "Poster04" + "\n");
        }

     /*   for (int i = 1; i <=ScreenNumber; i++) // Screen generation
        {
            if (i <= 4)
                File.AppendAllText(path, "Wall \t" + i + "\t" + "Poster0" + i + "\n");
            else
                File.AppendAllText(path, "Wall \t" + i + "\t" + "Poster04" + "\n");
        }*/

    }
}
