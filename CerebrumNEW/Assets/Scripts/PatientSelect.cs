using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public Button A, B, C;
    public bool Abuttom, Bbuttom, Cbutton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        if(Abuttom == true)
        {
            var colors = A.GetComponent<Button>().colors;
            colors.normalColor = new Color(200, 200, 200);
            A.GetComponent<Button>().colors = colors;
        }
    }
}
