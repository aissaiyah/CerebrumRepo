using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Cerebrum;
using UnityEngine.Networking;

public class UIManagerScript : ObjectMessageHandlerBase
{
    [System.Serializable]
    public class ToolVar
    {
        //This is the string that gets changed to seach for the correct directory, currently set to the overall directory
        public string DirectoryFolderName = "ToolDirectory", ToolFolderDirectory;
        //Button prefab to instantiate the buttons when needed, Canvas is needed to show the buttons
        public GameObject ToolInventory, ButtonCanvas;
        public string[] ButtonFiles; //contains list of Button files
    }
    public ToolVar[] ToolVariables;
    public GameObject ToolButtonPrefab, ToolDescriptionHolder;
    
    public bool enableDescription = true;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < ToolVariables.Length; i++)
        {
            ToolVariables[i].ToolFolderDirectory = System.IO.Path.Combine(Application.streamingAssetsPath, ToolVariables[i].DirectoryFolderName);
            ToolVariables[i].ToolInventory.SetActive(false);
        }
        ToolDescriptionHolder.SetActive(false);
    }

    private IEnumerator SendRequest(string url, GameObject BCanvas, GameObject TInventory, string[] fileNames, string filename)
    {
        url = System.IO.Path.Combine(url, filename);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
           

            if (request.isNetworkError || request.isHttpError)
            {
                DebugConsole.print("Send Request - Network Error");
            }
            else
            {
                try
                {
                    DebugConsole.print("UIManagerScript.FindFiles:folderDir=" + url);
                    string result = request.downloadHandler.text;
                    string[] linesInFile = result.Split('\n');
                    //For each file in the directory, if it is a .txt file it creates a button for it
                    CreateWebGLButton(url, BCanvas, TInventory, linesInFile);
                }
                catch (Exception)
                {
                    DebugConsole.print("Send request failed");
                }
            }
        }
    }

    public void OpenToolInventory()
    {
        DebugConsole.print("UIManagerScript.OpenToolInventory:enter");
        DebugConsole.print("UIManagerScript.OpenToolInventory:# of ToolVariables = "+ ToolVariables.Length);
        for (int i = 0; i < ToolVariables.Length; i++)
        {
            ToolVariables[i].ToolInventory.SetActive(true);
            if (ToolVariables[i].ButtonCanvas.transform.childCount < 1)
            {
                foreach (Transform child in ToolVariables[i].ButtonCanvas.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                FindFiles(ToolVariables[i].ToolFolderDirectory, ToolVariables[i].ButtonCanvas, ToolVariables[i].ToolInventory, ToolVariables[i].ButtonFiles);
            }
            else
            {
                foreach (Transform child in ToolVariables[i].ButtonCanvas.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        if (enableDescription)
        {
            DebugConsole.print("UIManagerScript.OpenToolInventory:ToolDescHolder.SetActive");
            ToolDescriptionHolder.SetActive(true);
        }
        DebugConsole.print("UIManagerScript.OpenToolInventory:exit");
    }

    void FindFiles(string FolderDirectory, GameObject BCanvas, GameObject TInventory, string[] fileNames)
    {
        // if webGL, this will be something like "http://..."
        string assetPath = Application.streamingAssetsPath;

        bool isWebGl = assetPath.Contains("://") || assetPath.Contains(":///");
        try
        {
            if (isWebGl)
            {
                foreach (string FilePaths in fileNames)
                {
                    StartCoroutine(SendRequest(FolderDirectory, BCanvas, TInventory, fileNames, FilePaths));
                }
            }
            else // desktop app
            {
                DebugConsole.print("UIManagerScript.FindFiles:folderDir=" + FolderDirectory);
                //For each file in the directory, if it is a .txt file it creates a button for it
                foreach (string FilePaths in Directory.GetFiles(FolderDirectory, "*.txt"))
                {
                    CreateButton(FilePaths, BCanvas, TInventory);
                }

                //Gets all folders within the initial directory to also search through them
                string[] internalFolders = Directory.GetDirectories(FolderDirectory);
                foreach (string folder in internalFolders)
                {
                    DebugConsole.print("UIManagerScript.FindFiles:folder=" + folder);
                    FindFiles(folder, BCanvas, TInventory, fileNames);
                }
            }
        }
        catch
        {
            DebugConsole.print("Send request failed");
        }
    }

    //Creates a button with the text of the button being the name of the tool and places it within the button list
    void CreateButton(string file, GameObject BCanvas, GameObject TInventory)
    {
        DebugConsole.print("UIManagerScript.CreateButton:file="+ file);
        var tempString = Path.GetFileNameWithoutExtension(file).Remove(0, 1);
        GameObject newButton = Instantiate(ToolButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newButton.name = tempString + "Button";
        newButton.transform.SetParent(BCanvas.transform);
        newButton.GetComponent<ToolButtonHandler>().ToolDescriptionHolder = ToolDescriptionHolder;
        newButton.GetComponent<ToolButtonHandler>().ToolInventory = TInventory;

        //Reads in txt file and splits it based on new lines.
        //lines[0] is the tool name
        //lines[1] is the description
        //lines[2] is the name of the sprite in the Resources folder

        var sr = new StreamReader(file);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        var lines = fileContents.Split("\n"[0]);
        newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lines[0];
        newButton.GetComponent<ToolButtonHandler>().ToolName = lines[0];
        newButton.GetComponent<ToolButtonHandler>().ToolDescriptionText = lines[1];
        //Checks to see if the txt file has a location for the sprite. Will need updated if we add more information/lines to the txt file
        if (lines.Length > 2)
        {
            var sp = Resources.Load<Sprite>(lines[2]);
            newButton.GetComponent<Image>().sprite = sp;
        }
        else
        {
            var sp = Resources.Load<Sprite>("Sprites/Tool");
            newButton.GetComponent<Image>().sprite = sp;
        }
        DebugConsole.print("UIManagerScript.CreateButton:exit");
    }

    //Creates a button with the text of the button being the name of the tool and places it within the button list
    void CreateWebGLButton(string file, GameObject BCanvas, GameObject TInventory, string[] linesInFile )
    {
        DebugConsole.print("UIManagerScript.CreateButton:file=" + file);
        var tempString = Path.GetFileNameWithoutExtension(file).Remove(0, 1);
        GameObject newButton = Instantiate(ToolButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newButton.name = tempString + "Button";
        newButton.transform.SetParent(BCanvas.transform);
        newButton.transform.localScale = new Vector3(1f, 1f, 1f);
        newButton.GetComponent<ToolButtonHandler>().ToolDescriptionHolder = ToolDescriptionHolder;
        newButton.GetComponent<ToolButtonHandler>().ToolInventory = TInventory;

        //Reads in txt file and splits it based on new lines.
        //lines[0] is the tool name
        //lines[1] is the description
        //lines[2] is the name of the sprite in the Resources folder

        newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = linesInFile[0];
        newButton.GetComponent<ToolButtonHandler>().ToolName = linesInFile[0];
        newButton.GetComponent<ToolButtonHandler>().ToolDescriptionText = linesInFile[1];
        //Checks to see if the txt file has a location for the sprite. Will need updated if we add more information/lines to the txt file
        if (linesInFile.Length > 2)
        {
            var sp = Resources.Load<Sprite>(linesInFile[2]);
            newButton.GetComponent<Image>().sprite = sp;
        }
        else
        {
            var sp = Resources.Load<Sprite>("Sprites/Tool");
            newButton.GetComponent<Image>().sprite = sp;
        }
        DebugConsole.print("UIManagerScript.CreateButton:exit");
    }

    //Close tool inventory if not wanting to use a tool
    public void CloseToolInventory()
    {
        for (int i = 0; i < ToolVariables.Length; i++)
        {
            ToolVariables[i].ToolInventory.SetActive(false);
        }

        if (enableDescription)
        {
            CloseToolDescription();
        }
    }
    //This is for the close button on the tool description to close the window
    public void CloseToolDescription()
    {
        ToolDescriptionHolder.SetActive(false);
    }

    public override bool HandleMessage(string msg, string param, out string retString)
    {
        retString = null;
//        print(this.name + ": OMH UIMAN:Handle Message: before OMHB.HandleMessage: " + msg + " for " + this.name + " with param = " + param);
        bool retv = base.HandleMessage(msg, param, out retString);
        if (commandFound)
            return retv;
        DebugConsole.print(this.name + ": OMH UIMAN:Handle Message: after OMHB.HandleMessage: " + msg + " for " + this.name + " with param = " + param);

        //OMHB doesn't return the evaluated parameter just in case you need to do something funky with it.
        if (param != null)// && param[0] == '$')
            param = GameManager.instance.ep.EvaluateParam(param);
        //            param =  ep.EvaluateParam(param);
        DebugConsole.print(this.name + ": OMH UIMAN:Handle Message: after EvaluateParam: " + msg + " for " + this.name + " with param = " + param);


        /*  COMMANDS */
        switch (msg)
        {
            case "opentoolinventory":
                {
                    OpenToolInventory();
                    break;
                }
            case "closetoolinventory":
                {
                    CloseToolInventory();
                    break;
                }
            case "enabletooldescription":
                {
                    enableDescription = true;
                    break;
                }
            case "disabletooldescription":
                {
                    enableDescription = false;
                    break;
                }
            default:
                break;
        }

        return true;
    }
}