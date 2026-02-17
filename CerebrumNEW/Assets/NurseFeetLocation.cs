using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class NurseFeetLocation : MonoBehaviour
{
    public Transform nurseTransform,parentTransform;
    // Start is called before the first frame update
    void Start()
    {

        parentTransform = transform.parent;
        nurseTransform =  transform.GetComponentsInChildren<Transform>()[1];
    }

    public const float PI = 3.14159265f;

    // Update is called once per frame
    void Update()
    {
        if (parentTransform==null)
            Start();
        float waistheight = 1f;
        float eyeheightdefault = 1.5f;
        float eyeheight = parentTransform.position.y;
        float yoffset = eyeheightdefault -eyeheight;
        float maxyoffset = 0.5f;
        float yadj = Mathf.Clamp(yoffset/maxyoffset,0f,1f);
        float xrot = parentTransform.localRotation.x;
        print("xrot1 = "+ xrot);
Vector3 eulers = this.transform.rotation.eulerAngles;
this.transform.rotation = Quaternion.Euler(new Vector3(-xrot,eulers.y,eulers.z));
//        transform.localRotation = (-xrot, 0.0f, 0.0f);
        float ang90 = PI/4f;
        xrot = Mathf.Clamp(xrot/ang90,0f,1f);
        print("xrot2 = "+ xrot);


        float zoffset = -yadj * xrot;
        print("zoffset = "+ zoffset);
//        return;

        Vector3 npos = nurseTransform.position;
        npos.y = 0f;
        nurseTransform.position= npos;

        Vector3 nlpos;
        nlpos = nurseTransform.localPosition;
        nlpos.z = zoffset;
        nurseTransform.localPosition= nlpos;


    }
}
