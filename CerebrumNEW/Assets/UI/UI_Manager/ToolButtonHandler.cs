using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolButtonHandler : MonoBehaviour
{
    public string ToolName, ToolDescriptionText;
    public GameObject ToolDescriptionHolder, ToolInventory;//, ToolApprovalWindow;

    //This shows a description of the tool based off of the txt file
    public void ShowDescription()
    {
        ToolDescriptionHolder.transform.GetChild(0).GetComponent<Text>().text = ToolDescriptionText;
    }
    
    
    //Used before message handler was put on
    /*
    //Pops up a window to check if the tool chosen was the one the player wants
    void ToolApproval()
    {
        ToolApprovalWindow.SetActive(true);
        ToolDescriptionHolder.SetActive(false);
        ToolApprovalWindow.transform.GetChild(0).GetComponent<Text>().text = "Do you want to use the " + ToolName + "?";
        ToolApprovalWindow.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UseTool);
    }

    //This is determined by the txt file on what to do when using the tool
    void UseTool()
    {
        //This is where you put the scripting to read the txt file to actually use a tool
        print("using " + ToolName);
        //
        ToolInventory.SetActive(false);
        ToolDescriptionHolder.SetActive(false);
        ToolApprovalWindow.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        ToolApprovalWindow.SetActive(false);
    }*/
}
