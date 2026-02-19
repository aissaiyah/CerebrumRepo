/** OMHPatientChooser.cs
 * <summary>
 * Patient Chooser game object allows designer to input details about the scenario including the following:
 * - non-EWS patient prefab references to choose from
 * - EWS patient prefab references to choose from
 * - Number of Beds in scene
 * - Number of non-EWS patients in scene
 * - Number of EWS patients in scene
 * Obviously, the number of patients in the scene cannot exceed the number of beds in the scene or an error
 * will be thrown.
 * </summary>
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Cerebrum;

public class OMHPatientChooser : ObjectMessageHandlerBase
{
    public string[] EWSPatientScripts;
    public string[] nonEWSPatientScripts;
    public int numBedsInScene = 0;
    public int numEWSPatientsInScene = 0;
    public int numNonEWSPatientsInScene = 0;

    private string[] bedScript;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("OMHPatientChooser: " + gameObject.name + " AWAKE");

    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("OMHPatientChooser: " + gameObject.name + " STARTED");
        AudioClip clip = Resources.Load<AudioClip>("Sound/freesound_community-cough-73464");

        // Check to make sure the number of beds and patients are correct
        if (numNonEWSPatientsInScene + numEWSPatientsInScene > numBedsInScene) {
            Debug.LogError("OMHPatientChooser: TOO MANY PATIENTS FOR NUMBER OF BEDS IN SCENARIO!!");
        }
        if (numEWSPatientsInScene > EWSPatientScripts.Length)
        {
            Debug.LogError("OMHPatientChooser: Not enough different EWS patient scripts: " + (numEWSPatientsInScene - EWSPatientScripts.Length) + " PATIENT(S) WILL BE DUPLICATED");
            if (EWSPatientScripts.Length == 0)
            {
                Debug.LogError("OMHPatientChooser: -=FAILURE=- NO EWS PATIENT SCRIPTS SELECTED FOR THIS SCENE");
            }
        }
        if (numNonEWSPatientsInScene > nonEWSPatientScripts.Length) {
            Debug.LogError("OMHPatientChooser: Not enough different non-EWS patient scripts: " + (numNonEWSPatientsInScene - nonEWSPatientScripts.Length) + " PATIENT(S) WILL BE DUPLICATED");
            if (nonEWSPatientScripts.Length == 0)
            {
                Debug.LogError("OMHPatientChooser: -=FAILURE=- NO non-EWS PATIENT SCRIPTS SELECTED FOR THIS SCENE");
            }
        }
        RandomizeBeds();
    }
    
    void RandomizeBeds()
    {
        Debug.Log("OMHPatientChooser: RandomizeBeds()");
        int bedNumber = -1;
        int patientNumber = -1;
        // Note that valid values for bedNumber start at 0 and go up to numBedsInScene - 1
        // patientNumber also starts at 0 and goes up to PatientScripts.Length - 1

        List<int> bedsPicked = new List<int>();
        List<int> EWSPatientsPicked = new List<int>();
        List<int> nonEWSPatientsPicked = new List<int>();
        bedScript = new string[numBedsInScene];

        for (int i = 1; i <= numEWSPatientsInScene; i++) {
            Debug.Log("OMHPatientChooser: EWS Patients Loop, i=" + i);

            while (bedNumber < 0 || bedsPicked.Contains(bedNumber))
            {
                bedNumber = UnityEngine.Random.Range(0, numBedsInScene);
                Debug.Log(" -- -- OMHPatientChooser: EWS Patients Picking Random Bed = " + bedNumber);
            }
            while (patientNumber < 0 || EWSPatientsPicked.Contains(patientNumber))
            {
                patientNumber = UnityEngine.Random.Range(0, EWSPatientScripts.Length);
                Debug.Log(" -- -- OMHPatientChooser: EWS Patients Picking Random Patient = " + patientNumber);
            }
            Debug.Log("OMHPatientChooser: Picked EWS selection =" + i + " Bed = " + bedNumber + " Patient = " + patientNumber);

            bedScript[bedNumber] = EWSPatientScripts[patientNumber];
            bedsPicked.Add(bedNumber);
            EWSPatientsPicked.Add(patientNumber);
            // if we need more EWS Patients than the number of different EWS sciripts, reset the list
            if (EWSPatientsPicked.Count >= EWSPatientScripts.Length) {
                // All EWS Patient options have been picked for the scene, but we may still need more
                EWSPatientsPicked = new List<int>();
            }

            bedNumber = -1;
            patientNumber = -1;
        }

        for (int i = 1; i <= numNonEWSPatientsInScene; i++)
        {
            Debug.Log("OMHPatientChooser: non-EWS Patients Loop, i=" + i);
            while (bedNumber < 0 || bedsPicked.Contains(bedNumber))
            {
                bedNumber = UnityEngine.Random.Range(0, numBedsInScene);
                Debug.Log(" -- -- OMHPatientChooser: non-EWS Patients Picking Random Bed = " + bedNumber);
            }
            while (patientNumber < 0 || nonEWSPatientsPicked.Contains(patientNumber))
            {
                patientNumber = UnityEngine.Random.Range(0, nonEWSPatientScripts.Length);
                Debug.Log(" -- -- OMHPatientChooser: non-EWS Patients Picking Random Patient = " + patientNumber);
            }
            Debug.Log("OMHPatientChooser: Picked non-EWS selection =" + i + " Bed = " + bedNumber + " Patient = " + patientNumber);

            bedScript[bedNumber] = nonEWSPatientScripts[patientNumber];
            bedsPicked.Add(bedNumber);
            nonEWSPatientsPicked.Add(patientNumber);
            // if we need more non-EWS Patients than the number of different non-EWS scripts, reset the list
            if (nonEWSPatientsPicked.Count >= nonEWSPatientScripts.Length)
            {
                // All EWS Patient options have been picked for the scene, but we may still need more
                nonEWSPatientsPicked = new List<int>();
            }

            bedNumber = -1;
            patientNumber = -1;
        }
        Debug.Log("OMHPatientChooser: all patients picked, BED ROSTER=======================");
        for (int i = 0; i < numBedsInScene; i++) {
            Debug.Log("OMHPatientChooser: Bed " + (i + 1) + " : " + bedScript[i]);
        }
        Debug.Log("OMHPatientChooser: ================ END BED ROSTER=======================");
    }
}
