using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempButton : MonoBehaviour
{
    public GameObject Summary;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pressed()
    {
        Summary.SetActive(false);
    }

    public void Unpress()
    {
        Summary.SetActive(true);
    }
}
