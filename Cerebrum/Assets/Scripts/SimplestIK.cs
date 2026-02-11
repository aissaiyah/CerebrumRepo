using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplestIK : MonoBehaviour
{
	public Transform goalPosition;

	private Transform[] limbs;
	private float[] lens;//lengths between bones(limbs)

	public int limbsLength=0; 
	void SaveLimbsLen()
	{
		for (int i = 0; i < limbsLength - 1; i++)
			lens[i] = Vector3.Magnitude(limbs[i].position - limbs[i + 1].position);
	}

	void Awake()
	{
		limbs = GetComponentsInParent<Transform>();//save all parent GameObject.Transform to array
		if (limbsLength<=0)
			limbsLength = limbs.Length;
		lens = new float[limbsLength - 1];
		SaveLimbsLen();//save lengths between bones(limbs)
	}

	void Update()
	{
		FABRIK();
	}

	void FABRIK()
	{
		FinalToRoot();//first phase 
		RootToFinal();//second phase
	}

	void FinalToRoot()
	{
		limbs[0].position = goalPosition.position;
		for (int i = 1; i < limbsLength - 1; i++){
			limbs[i].position = limbs[i - 1].position + ((limbs[i].position - limbs[i - 1].position).normalized * lens[i]);
		}
	}

	void RootToFinal()
	{
		for (int i = limbsLength - 2; i >= 0; i--){
			limbs[i].position = limbs[i + 1].position + ((limbs[i].position - limbs[i + 1].position).normalized * lens[i]);
		}
		for (int i = 0; i<limbsLength-2; i++){
//			print("lookat "+ i + ", name =" + limbs[i].name + ", len="+ lens[i]);
			Quaternion myRotation = limbs[i].rotation;
			limbs[i+1].LookAt(limbs[i]);
			//PJD: need to replace these lines with options for axis and offset
			limbs[i+1].Rotate(Vector3.right * 90f); 
			
			limbs[i].localPosition = new Vector3(0f,.1f/*lens[i]*/,0f);
			limbs[i].rotation = myRotation;
		}
	}
}
