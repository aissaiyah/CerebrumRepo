using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[ExecuteAlways]
public class HandPose : MonoBehaviour
{
    public enum HandShapes{ None, L_grip, L_extension, L_pinch, L_ring, L_thumb, L_tripod, L_tweeze, L_dip, L_knuckle, L_pond, L_spread, L_perpendicular, R_grip, R_extension, R_pinch, R_ring, R_thumb, R_tripod, R_tweeze, R_dip, R_knuckle, R_pond, R_spread, R_perpendicular  }

    public HandShapes handShape;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }


    public string lastAnimName;


    void OnValidate()
    {
        ChangeHandShape();
    }

    void ChangeHandShape()
     {
        string animName;
        print("DrawGizmo:");//Time.deltaTime);

        animator = GetComponent<Animator>();
        if (animator == null)
            return;

        //get hand shape name as string
        animName = Enum.GetName(typeof(HandShapes), handShape);  
        if (animName == lastAnimName)
            return;

        //turn off last handshape
        var handShapeNames = Enum.GetNames(typeof(HandShapes));
        foreach (string lastAnimName2 in handShapeNames){
            print("DrawGizmo turn off " + lastAnimName2);
            if (lastAnimName2 != animName)
                animator.SetBool(lastAnimName2, false);
        }

        //turn on new handshape
        lastAnimName = animName;
        print("DrawGizmo turn on " + animName);
        animator.SetBool(animName, true);

        //force animator transition
        for (int i = 0; i < 100; i++)
            animator.Update(0.01f);


     }    
}
