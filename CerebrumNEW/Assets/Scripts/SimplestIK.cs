using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplestIK : MonoBehaviour
{
	public Transform endLink;
	public enum myAxis{ //axis of links (towards parent)
	X,
	Y,
	Z,
	negX,
	negY,
	negZ
	};
	public myAxis myaxis = myAxis.Z;

	public Transform goalPosition;
	public float tolerance = 0.01f;

	private Transform[] limbs;
	private float[] lens;//lengths between bones(limbs)

	public int limbsLength=0; 
	public bool doOrientation = false;
	public bool endOrientation = false;

	void SaveLimbsLen()
	{
		for (int i = 0; i < limbsLength - 1; i++)
			lens[i] = Vector3.Magnitude(limbs[i].position - limbs[i + 1].position);
	}

	void Awake()
	{
		if (endLink == null)
			endLink = this.transform;
		limbs = endLink.GetComponentsInParent<Transform>();//save all parent GameObject.Transform to array
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
		if (Vector3.Distance(limbs[0].position,goalPosition.position)< tolerance)
			return;
		FinalToRoot();//first phase 
		RootToFinal();//second phase
		if (doOrientation)
			FixOrientations();
	}

	void FinalToRoot()
	{
		limbs[0].position = goalPosition.position;
		for (int i = 1; i < limbsLength - 1; i++){// -1 because only doing segments
			limbs[i].position = limbs[i - 1].position + ((limbs[i].position - limbs[i - 1].position).normalized * lens[i]);
		}
	}



	void RootToFinal()
	{
		for (int i = limbsLength - 2; i >= 0; i--){
			limbs[i].position = limbs[i + 1].position + ((limbs[i].position - limbs[i + 1].position).normalized * lens[i]);
		}
	}

	void FixOrientations()
	{
		//from end to root, have parent lookat child, then put child position back
		//myAxis should be axis to parent
		for (int i = 0; i<limbsLength-1; i++){
			//print("lookat "+ i + ", name =" + limbs[i].name + ", len="+ lens[i]);
//			Quaternion myRotation = limbs[i].rotation;
			Vector3 myPos = limbs[i].position; //save my position
			Quaternion myRot = limbs[i].rotation; //save my orientation
			var parent = limbs[i+1];
			parent.LookAt(limbs[i]); //orient Z axis of parent
			if (myaxis==myAxis.X)
				parent.rotation = Quaternion.FromToRotation(Vector3.right, parent.forward);
			if (myaxis==myAxis.Y)
				parent.rotation = Quaternion.FromToRotation(Vector3.up, parent.forward);
			if (myaxis==myAxis.negX)
				parent.rotation = Quaternion.FromToRotation(Vector3.left, parent.forward);
			if (myaxis==myAxis.negY)
				parent.rotation = Quaternion.FromToRotation(Vector3.down, parent.forward);
			if (myaxis==myAxis.negZ)
				parent.rotation = Quaternion.FromToRotation(Vector3.back, parent.forward);
//				parent.Rotate( 90f,0f,0f, Space.Self);)
			//	limbs[i+1].Rotate(Vector3.right * 90f); 
			limbs[i].position = myPos;
			if (i==0 && endOrientation)
				limbs[0].rotation = goalPosition.rotation;
			else
				limbs[i].rotation = myRot;
			
			//PJD: need to replace these lines with options for axis and offset
			
//			limbs[i].localPosition = new Vector3(0f,lens[i],0f);
//			limbs[i].rotation = myRotation;
		}
	}
}
