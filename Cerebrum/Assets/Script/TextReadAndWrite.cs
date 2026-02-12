using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TextReadAndWrite : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset text1,text2;
    public string path;
    void Start()
    {
        path = Application.dataPath + "/TextAdd.txt"; //Path of the File
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Login Log \n\n");

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            File.AppendAllText(path,text1.ToString() + "\n");
            File.AppendAllText(path, text2.ToString());
        }
    }
}
