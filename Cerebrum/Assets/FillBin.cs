using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBin : MonoBehaviour
{
    public Color color;
    public GameObject [] binObjects;
    public int numberOfObjects;
    public bool assorted=false;
    private IEnumerator coroutine;
    Vector3 origpos;
    // Start is called before the first frame update
    void Awake1()
    {
        
        Physics.gravity = new Vector3(0, -100.0F, 0);
        int rand = Random.Range(0,binObjects.Length-1);

        this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color =color; 
        origpos = this.transform.position;
        this.transform.position = new Vector3(0,-100,0f);
        //print("FillBin:Awake:" + this.name + ", pos = "+ this.transform.position);
        Vector3 pos = this.transform.position;

        pos.y += 0.4f;
        Quaternion quat = this.transform.rotation;

        for (int i = 0; i< numberOfObjects;i++)
        {
            if (assorted)
                rand = Random.Range(0,binObjects.Length-1);

            pos.y += 0.3f;
            pos.x += Random.Range(-0.05f, 0.05f);
            pos.z += Random.Range(-0.05f, 0.05f);
            GameObject go = Instantiate(binObjects[rand],pos,quat);//,this.transform);
            go.AddComponent<Rigidbody>();
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.maxDepenetrationVelocity = 0.001f;
            go.transform.SetParent(this.transform);

        }

        coroutine = WaitToFreeze(2.0f);
        StartCoroutine(coroutine);
//        Physics.gravity = new Vector3(0, -9.8F, 0);
        
    }

    private IEnumerator WaitToFreeze(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //print("FillBin:WaitToFreeze " + Time.time);
        GameObject[] allChildren = new GameObject[transform.childCount];
        int i = 0;
        //Find all child obj and store to that array
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject.GetComponent<Rigidbody>());
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            if (child.transform.position.y < this.transform.position.y )
//                child.gameObject.SetActive(false);
                DestroyImmediate(child.gameObject);
        }        
        Physics.gravity = new Vector3(0, -9.8F, 0);
        this.transform.position = origpos;
    }
    void Start()
    {
        Awake1();
    } 
    // Update is called once per frame
    void Update()
    {
        
    }
}
