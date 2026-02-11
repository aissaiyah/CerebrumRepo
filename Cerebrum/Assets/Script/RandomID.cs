using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomID : MonoBehaviour
{
    // Start is called before the first frame update

    public int ID;
    
    public InputField MainID, WristID;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRandomID()
    {
        ID = Random.Range(10000, 30000);
        MainID.text = ID.ToString();
        WristID.text = ID.ToString();
    }
}
