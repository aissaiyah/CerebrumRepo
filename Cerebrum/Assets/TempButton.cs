using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempButton : MonoBehaviour
{
    public GameObject Summary;
	public GameObject ComplaintCanvas;
	public GameObject PatientCanvas;
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

	public void ComplaintPress()
	{
		ComplaintCanvas.SetActive(true);
	}
	
	public void ComplaintUnpress()
	{
		ComplaintCanvas.SetActive(false);
	}

	public void ParientPress()
	{
		PatientCanvas.SetActive(true);
	}


	public void PatientUnpress()
	{
		PatientCanvas.SetActive(false);
	}




}