using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotate : MonoBehaviour
{
    // Start is called before the first frame update
    public int speed;


    float duration = 1.5f;
    private float t = 0, time;
    public bool docolor;
    bool isReset = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(docolor == true)
        ColorChangerr();

      
        transform.Rotate(Vector3.up * speed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);
        time = time + Time.deltaTime;
    }

    void ColorChangerr()
    {
        Debug.Log(t);

        
            
            this.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.blue, t);

            if (t < 2)
            {
                t += Time.deltaTime / duration;
            }

            if (t >= 2)
                t = 0;

        
       /* if (isReset == true)
        {
            
            this.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.yellow, t);

            if (t < 2)
            {
                t += Time.deltaTime / duration;
            }

            if (t >= 2)
                t = 0;
           } */
        

    }
}
