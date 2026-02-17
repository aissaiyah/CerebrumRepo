using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Cerebrum;
public class RoomSpecifier : MonoBehaviour
{
    // Start is called before the first frame update
    public string path; 
    public int RoomNumber = 0, BedNumber = 3, WallNumber = 6, ScreenNumber = 4, BinNumber = 0, CartNumber = 0;
    public bool NurseB, DocumentDeskB, PatientB;

    public RoomGenerator roomGenerator;

    
    public string [] content = {
            "Room\t" + 0 + "\tRoomXLGPatient_base \n",
            "Room\t" + 0 + "\tRoomLrgDoubleWard_base03 \n"
        };        // Default Room                 // \t = + tab
    



    void Start()
    {
        path = Application.dataPath + "/StreamingAssets" + "/LevelGen" + "/EWTestLevelGen.txt"; //Path of the File
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
        Room();
        // int CurrentPN = 0;

        Bed();

        PosterOnWall();

//        Screen();

        Bin();

        Cart();

        Nurse();
        BedsideHead();

print("Calling RoomGenerator");
roomGenerator.gameObject.SetActive(true);
//        RoomGenerator.LoadARoomFile("LevelGen/EWTestLevelGen.txt");
    }

    void Room()
    {

        //Overwrite Everything for Level Generation 
        File.WriteAllText(path, content[RoomNumber]);
    }

    void Bed()
    {
        for (int i = 1; i <= BedNumber; i++)  // Bed Generation
        {
            if(PatientB == true)
            {
                File.AppendAllText(path, "Bed " + i + "\t" + "PatientBedClickable01\t" + "Patient\t" + "2\t" + "Placeholder \n");
                PatientB = false;
            }
            else
            File.AppendAllText(path, "Bed " + i + "\t" + "PatientBed01\t" + "Patient\t" + "2\t" + "Placeholder \n");//PatientTemp

          
        }
    }
    
    void PosterOnWall()
    {
        for (int i = 1; i <= WallNumber; i++) // Wall generation
        {
            // if(i < 4)
            File.AppendAllText(path, "Wall\t" + i + "\t" + "Poster0" + i + "\n");
            // else
            // File.AppendAllText(path, "Wall\t" + i + "\t" + "Poster04" + "\n");
        }
    }
    void Screen()
    {
        for (int i = 1; i <= ScreenNumber; i++) // Screen generation
        {

            File.AppendAllText(path, "Screen\t" + i + "\t" + "PrivacyScreen01" + "\n");

        }
    }

    void Bin()
    {
        for (int i = 1; i <= BinNumber; i++) // Bin generation
        {
            File.AppendAllText(path, "Bin\t" + i + "\tWastebinScene06\n");
        }
    }
    void Cart()
    {
        for (int i = 1; i <= CartNumber; i++) // Bin generation
        {
            File.AppendAllText(path, "Cart\t" + i + "\tCOW00	Computer	1	ComputerSetCOW01	Misc	2	KidneyDish01	Misc	3	IVSet01	Misc	4	BloodBag	Misc	5	InfusionClamp01	Misc	6	LiquidSoap01	Misc	7	FoldedBloodPressureCuff01	Misc	8		Misc	9	Gloves01\n");
        }
    }

    //BedsideHeadLeft	1	MonitorStand02	Computer	1	VitalsMonitor01
    void BedsideHead()
    {
        for (int i = 1; i <= CartNumber; i++) // Bin generation
        {
            File.AppendAllText(path, "BedsideHead\t" + i + "\n");//	1	MonitorStand02	Computer	1	VitalsMonitor01\n");
            File.AppendAllText(path, "BedsideHeadLeft\t" + i + "\t	1	MonitorStand02	Computer	1	VitalsMonitor01\n");
        }
//Desk	6	DeskSet02	Computer	1		Phone	2		Document	3		Document	4
        File.AppendAllText(path, "Desk	6	DeskSet02	Computer	1		Phone	2		Document	3		Document	4\n");
File.AppendAllText(path, "Nurse	2	NurseClickable01\n");
File.AppendAllText(path, "Nurse	4	Placeholder\n");

    }

//Cart	4	COW00	Computer	1	ComputerSetCOW01	Misc	2	KidneyDish01	Misc	3	IVSet01	Misc	4	BloodBag	Misc	5	InfusionClamp01	Misc	6	LiquidSoap01	Misc	7	FoldedBloodPressureCuff01	Misc	8		Misc	9	Gloves01
//Bin	4	WastebinScene06																											

    void Nurse()
    {
        if(NurseB == true)
        {
            File.AppendAllText(path, "Nurse\t" + 1  + "\tNurseClickable01" + "\n");
        }
    }

    /*void DocumentDesk()
    {

    }*/
}
