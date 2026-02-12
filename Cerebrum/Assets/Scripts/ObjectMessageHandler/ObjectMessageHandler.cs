using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using Cerebrum;

public class ObjectMessageHandler : ObjectMessageHandlerBase
{ 
    public bool correctSet = false;
    public bool chosenSet = false; 
    public bool trackTime = false;
    public bool isEmpty;
    public List<int> stages = new List<int>();
    public bool stageset;
    public float pointTotal = 0;
    public int salineAmount = 5;
    public int time = 0;
    public float percentComplete = 0.0f;
    public int totalMissed = 0;
    public int totalIncorrect = 0;
    public int totalCorrect = 0;

//    public float movementSpeed = 1f;
    public Material[] consentPages;
    public int pageNumber = 0;

 

    IKController ikcontroller;

    // This starts the message handler
    protected void Start()
    {
        base.Start();

        //ikcontroller = GetComponentInChildren<IKController>();
        DebugConsole.print("MESSAGE HANDLER: " + gameObject.name + " FOUND MESHRENDERER " + mr);
        if(mr != null && consentPages != null && consentPages.Length > 1)
        {
            mr.material = consentPages[0];
        }
        
//        SetItemText(itemText);
    }

    void getIKController()
    {
        ikcontroller = GetComponentInChildren<IKController>();
        if (ikcontroller==null)
            ikcontroller = this.gameObject.AddComponent<IKController>();
    
    }

    protected override void  Awake()
    {
        base.Awake();

    }

    void AddStages(int numItems)
    {
        for (int i = 0;i<numItems;i++)
        {
            stages.Add(-1);
        }
    }

    public int [] numStageItems = { 0,13,9,8,2,12,15,16 };

    public override bool HandleMessage(string msg, string param, out string retString)
    {
        retString = null;
        DebugConsole.print(this.name + ": OMH:Handle Message: before OMHB.HandleMessage: " + msg + " for " + this.name + " with param = "+ param);
        bool retv = base.HandleMessage(msg,param,out retString);
        if (commandFound)
            return retv;
        DebugConsole.print(this.name + ": OMH:Handle Message: after OMHB.HandleMessage: " + msg + " for " + this.name + " with param = "+ param);

        //OMHB doesn't return the evaluated parameter just in case you need to do something funky with it.
        if (param != null && param[0] == '$') 
            param =  GameManager.instance.ep.EvaluateParam(param);
//            param =  ep.EvaluateParam(param);
        DebugConsole.print(this.name + ": OMH:Handle Message: after EvaluateParam: " + msg + " for " + this.name + " with param = "+ param);


        /*  COMMANDS */



        if (msg == "setmaterial")
        {
            param =  GameManager.instance.ep.EvaluateParam(param);
            string matName = param;
            DebugConsole.print("Loading material "+ matName);
            // Assigns a material named "Assets/Resources/XXXX" to the object.
            Material newMat = Resources.Load(matName, typeof(Material)) as Material;
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material = newMat;
        }else 
        if (msg == "gain")
        {
            if (param != null)
            {
                pointTotal += int.Parse(param);
            }
            else
            {
                pointTotal += 1;
            }
        }

        else if(msg == "lose")
        {
            if(pointTotal > 0)
            {
                if (param != null)
                {
                    pointTotal += int.Parse(param);
                }
                else
                {
                    pointTotal += 1;
                }
            }
        }
        else if(msg == "reducesaline")
        {
            if(salineAmount > 0)
            {
                salineAmount--;
            }
        }

        else if(msg == "raiseinterval")
        {
            interval--;
        }


        else if (msg == "raisesaline")
        {
                salineAmount++;
        }

        else if (msg == "setsaline")
        {
            salineAmount = int.Parse(param);
            if(salineAmount < 0)
            {
                salineAmount = 0;
            }
        }

        else if(msg == "salineamount")
        {
            if(salineAmount == int.Parse(param))
            {
                return true;
            }
            else
            {
                return false;
            }
        } 


        else if(msg == "increasetime")
        {
            time += 15;
        }

        else if(msg == "decreasetime")
        {
            if(time > 0)
            {
                time -= 15;
            }
        }

        else if(msg == "timeequals")
        {
            if(int.Parse(param) == time)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if(msg == "says")
        {
            if(param == inputText)
            {
                Debug.Log("My name is " + gameObject.name + " my input text equals " + inputText);
                return true;
            }
            else
            {
                return false;
            }
        }

        else if(msg == "write")
        {
            if(gameObject.GetComponent<TextMeshProUGUI>() != null)
            {
                gameObject.GetComponent<TextMeshProUGUI>().text = param;
            }
            else
            {
                return true;
            }
        }

        else if(msg == "numitems")
        {
            int numItems = int.Parse(param);
            AddStages(numItems);
            return true;
        }



        ////////////////////////////////////////////////////////////////////
        else if(msg == "setstages")
        {
            int stageNum = int.Parse(param);
            int numItems = numStageItems[stageNum];
            AddStages(numItems);
            return true;
              
        }

        else if(msg == "perfect")
        {
            if(gameObject.GetComponent<UITextShower>() != null)
            {
                gameObject.GetComponent<UITextShower>().perfect = true;
            }
        }
        
        else if (msg == "good")
        {
            if (gameObject.GetComponent<UITextShower>() != null)
            {
                gameObject.GetComponent<UITextShower>().good = true;
            }
        }

        else if (msg == "bad")
        {
            if (gameObject.GetComponent<UITextShower>() != null)
            {
                gameObject.GetComponent<UITextShower>().bad = true;
            }
        }
        else if (msg == "taskcorrect")
        {
                char[] separators = new char[] { ' ','\t'};

                string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                param = temp[0];
                string param2 = null;
                if (temp.Length>1)
                    param2 =  GameManager.instance.ep.EvaluateParam(temp[1]);
//                    param2 =  ep.EvaluateParam(temp[1]);

            bool correct = (param2 == null || param2 == "true" || param2 == "1");
            stages[int.Parse(param)] = (correct?1:0);//true;

        }
        else if (msg == "complete")
        {
            stages[int.Parse(param)] = 1;//true;
        }
        else if (msg == "incomplete")
        {
            stages[int.Parse(param)] = 0;//false;
        }
        else if (msg == "unanswered")
        {
            stages[int.Parse(param)] = -1;
        }

        else if(msg == "iscomplete")
        {
            DebugConsole.print("iscomplete: param = "+ param + ", num = " + int.Parse(param));
            return (stages[int.Parse(param)]>=0);
        }

        else if(msg == "taskright")
        {
            if(stages[int.Parse(param)] == 0 || stages[int.Parse(param)] == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        else if (msg == "taskwrong")
        {
            if (stages[int.Parse(param)] == 0 || stages[int.Parse(param)] == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if(msg == "rangecomplete")
        {
            char[] separators = new char[] { ' ', ',', '-' };

            string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int start = int.Parse(temp[0]);
            int stop = int.Parse(temp[1]);
            int totalComplete = 0;

            for(int i = start; i < stop +1; i++)
            {
                if(stages[i]>=0)
                {
                    totalComplete++;
                }
            }

            Debug.LogWarning("Total: " + totalComplete);
            pointTotal = totalComplete;
            if(totalComplete == stop - start + 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if(msg == "getcompletepercent")
        {
            Debug.LogWarning(pointTotal);
            int numItems = stages.Count;
            //if (!missesCounted)

            for (int i = 0; i < numItems; i++)
            {
                if (stages[i] <0) //== false)
                    totalMissed++;
                else if (stages[i] ==0) //== false)
                    totalIncorrect++;
                else
                    totalCorrect++;
            }
            //            missesCounted = true;
        
            //int percentComplete = (int)scorer.percentComplete;
            int percentCorrect = totalCorrect *100/ numItems;
            DebugConsole.print("totalCorrect = "+ totalCorrect + " , numItems = " + numItems + ", %= "+ percentCorrect);
            percentComplete = percentCorrect;
//            percentComplete = (pointTotal/stages.Count) * 100;
            Debug.LogWarning(percentComplete);
        }

        else if(msg == "allcomplete")
        {
            
            if (percentComplete == 100)
            {
                return true;
            }
            else
            {
                return false;
            }    
        }

        else if(msg == "currentpageis")
        {
            int currentMat = pageNumber;
            Debug.LogWarning("Current Mat is number: " + currentMat);
            if (param == currentMat.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if(msg == "percentachieved")
        {
            if(percentComplete > int.Parse(param))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if(msg == "showtext")
        {
            if(GetComponent<UITextShower>() != null)
            {
                GetComponent<UITextShower>().DisplayText();
            }
        }

        else if(msg == "stageset")
        {
            return stageset;
        }

        else if(msg == "correctset")
        {
            correctSet = true;
        }

        else if(msg == "iscorrect")
        {
            return correctSet;
        }

        else if(msg == "playerchose")
        {
            chosenSet = true;
        }

        else if(msg == "playerreplace")
        {
            chosenSet = false;
        }

        else if(msg == "ischosen")
        {
            return chosenSet;
        }

        else if(msg == "setdummy")
        {
            if(GetComponent<UITextShower>())
            {
                GetComponent<UITextShower>().dummypoint = true;
            }
        }

        else if(msg == "open")
        {
            if(GetComponent<UISize>() != null)
            {    
                GetComponent<UISize>().Grow();
            }
        }

        else if(msg == "close")
        {
            if (GetComponent<UISize>() != null)
            {
                GetComponent<UISize>().Shrink();
            }
        }

        else if(msg == "clickoff")
        {
            clickable = false;
        }

        else if (msg == "clickon")
        {
            clickable = true;
        }
        /** IK Commands */

        // GRAB
        else if (msg == "headturn")
        {
            getIKController();
            //print("headTurn " + param);
            GameObject go=gm.FindGameObject(param);
    //        GameObject go=GameObject.Find(param); //moveTo object's position
            DebugConsole.print("head turn game object "+ go.name);
            DebugConsole.print("ikcontroller= "+ ikcontroller);
            //ikcontroller.rightHandObj= go.transform;
            ikcontroller.leftHandObj = null;
            ikcontroller.rightHandObj = null;
            ikcontroller.lookObj = go.transform;
            StartCoroutine(Grab(1.0f));
        }
        else if (msg.StartsWith("grab")){
            if (msg == "grableft" || msg == "grab")
            {
                DebugConsole.print("grabLeft ");
                getIKController();
//                if (msg == "grableft")
//                    ikcontroller.rightHandObj = null;

                GameObject go=gm.FindGameObject(param);
                //GameObject go=GameObject.Find(param); //moveTo object's position
                    DebugConsole.print("grabbing game object "+ go.name);
                ikcontroller.leftHandObj= go.transform;
                ikcontroller.lookObj = go.transform;
            }
            if (msg == "grabright" || msg == "grab")
            {
                DebugConsole.print("grabRight ");
                getIKController();
//                if (msg == "grabright")
//                    ikcontroller.leftHandObj = null;
                GameObject go=gm.FindGameObject(param);

                //GameObject go=GameObject.Find(param); //moveTo object's position
                DebugConsole.print("grabbing game object "+ go.name);
                ikcontroller.rightHandObj= go.transform;
                ikcontroller.lookObj = go.transform;
            }
                StartCoroutine(Grab(1.0f));
        }
        else if (msg == "release")
        {
            DebugConsole.print("release ");
            getIKController();
            
//            GameObject go=GameObject.Find(param); //moveTo object's position
//                DebugConsole.print("releasing game object "+ go.name);
//            ikcontroller.rightHandObj= go;
//            ikcontroller.lookObj = go;
            StartCoroutine(Release(1.0f));


            //do something...
        }
        else{
            string errmsg = "Command not found! " + msg + " for " + this.name + " with param = "+ param;
            Debug.LogError(errmsg);
            
        }


        return true;
    }



    ////////////////////////////////////////////////////////////////////////////
    /// Human animation functions
    ////////////////////////////////////////////////////////////////////////////

 
    private IEnumerator Grab(float duration)
    {
        ikcontroller.ikActive = true;
        float t = 0.0f;
        while (t < duration)
        {
            //DebugConsole.print("Grabbing "+ transform.rotation);
            t += Time.deltaTime;
            ikcontroller.ikStrength = t/duration;
            yield return null;
        }
    }
    private IEnumerator Release(float duration)
    {
        float t = 0.0f;
        while (t < duration)
        {
            //DebugConsole.print("Grabbing "+ transform.rotation);
            t += Time.deltaTime;
            ikcontroller.ikStrength = 1.0f - t/duration;
            yield return null;
        }
        ikcontroller.ikActive = false;
        ikcontroller.rightHandObj = null;
        ikcontroller.leftHandObj = null;
    }


    // Update is called once per frame
    protected override void Update()
    {
        //DebugConsole.print("OMH:Update1");

        base.Update();

        if(salineAmount > 0)
        {
            isEmpty = false;
        }
        else
        {
            isEmpty = true;
        }

    }
}
    ////////////////////////// OLD UNUSED CODE ///////////////////////////////////


    /*
    protected override void FixedUpdate()
    {
//        DebugConsole.print("OMH:FixedUpdate");
        base.FixedUpdate();

    }
    */
/*

        else if(msg == "next")
        {
            
            if (pageNumber != 5)
            {
                mr.material = consentPages[pageNumber + 1];
                pageNumber++;
            }
        }

        else if(msg == "previous")
        {
            
            if (pageNumber != 0)
            {
                mr.material = consentPages[pageNumber - 1];
                pageNumber--;
            }
        }


        else if(msg == "parentto" || msg == "attachto")
        {
            GameObject go = GameObject.Find(param);
            if(transform.parent != null)
            {
                transform.parent = null;
                transform.parent = go.transform;
            }
            else
            {
                transform.parent = go.transform;
            }
        }

        else if(msg == "changeshader")
        {
            Material[] mats = mr.materials;
            if(param == "normal")
            {
                mats[1] = outline_normal;
                mr.materials = mats;
            }
            else if(param == "good")
            {
                mats[1] = outline_good;
                mr.materials = mats;
            }
            else if(param == "yellow")
            {
                mats[1] = outline_good;
                mr.materials = mats;
            }
            else if (param == "bad")
            {
                mats[1] = outline_bad;
                mr.materials = mats;
            }
            else if(param == "off")
            {
                Array.Resize(ref mats, mats.Length - 1);
            }
        }

        else if(msg == "shadercheck")
        {
            Material[] mats = mr.materials;
            if (param == "good")
            {
                if (mats[1].name == outline_good.name)
                {
                    return true;
                }
                else
                {
                    Debug.LogWarning("Current Highlight " + mats[1].name);
                    return false;
                }
            }
            else if(param == "bad")
            {
                
                if (mats[1].name == outline_bad.name)
                {
                    return true;
                }
                else
                {
                    Debug.LogWarning("Current Highlight " + mats[1].name);
                    return false;
                }
            }
            else if(param == "normal")
            {
                if (mats[1].name == outline_normal.name)
                {
                    return true;
                }
                else
                {
                    Debug.LogWarning("Current Highlight " + mats[1].name);
                    return false;
                }
            }
        }

                   /* Replaced with "numItems" above


            if(param == "1")
            {
                int IDDesk = -1;
                int FormDesk = -1;
                int PatientMove = -1;
                int NurseCall = -1;
                int PatName = -1;
                int PatID = -1;
                int Blood = -1;
                int IDBlood = -1;
                int SecondName = -1;

                stages.Add(IDDesk);
                stages.Add(FormDesk);
                stages.Add(PatientMove);
                stages.Add(NurseCall);
                stages.Add(PatName);
                stages.Add(PatID);
                stages.Add(Blood);
                stages.Add(IDBlood);
                stages.Add(SecondName);
         }

            if(param == "2")
            {
                int compDone = -1;
                int phoneDone = -1;
                int GXMpatient = -1;
                int GXMblood = -1;
                int GXMvalid = -1;
                int consentDates = -1;
                int consentSignature = -1;
                int consentPatient = -1;
                int consentIC = -1;
                

                stages.Add(compDone);
                stages.Add(phoneDone);
                stages.Add(GXMpatient);
                stages.Add(GXMblood);
                stages.Add(GXMvalid);
                stages.Add(consentDates);
                stages.Add(consentSignature);
                stages.Add(consentPatient);
                stages.Add(consentIC);

                stageset = true;
            }
            if(param == "3")
            {
                
                int openBox = -1;
                int removeSelector = -1;
                int openFridge = -1;
                int iceTable = -1;
                int iceBox = -1;
                int selectorBox = -1;
                int GXMbox = -1;
                int closeBox = -1;

                stages.Add(openBox);
                stages.Add(removeSelector);
                stages.Add(openFridge);
                stages.Add(iceTable);
                stages.Add(iceBox);
                stages.Add(selectorBox);
                stages.Add(GXMbox);
                stages.Add(closeBox);

            }
            if (param == "4")
            {
                int RightSet = -1;
                int DishAdded = -1;

                stages.Add(RightSet);
                stages.Add(DishAdded);
            }
            if(param == "5")
            {
                int PatientClick = -1;
                int NurseClick = -1;
                int CorrectName = -1;
                int GXMCorrect = -1;
                int TagCorrect = -1;
                int LabelCorrect = -1;
                int BloodBagFront = -1;
                int TransfusionStart = -1;
                int GXMBlood = -1;
                int LabelBlood = -1;
                int Finish = -1;
                int BloodBagFinish = -1;

                stages.Add(PatientClick);
                stages.Add(NurseClick);
                stages.Add(CorrectName);
                stages.Add(GXMCorrect);
                stages.Add(LabelCorrect);
                stages.Add(BloodBagFront);
                stages.Add(TransfusionStart);
                stages.Add(GXMBlood);
                stages.Add(TagCorrect);
                stages.Add(LabelBlood);
                stages.Add(Finish);
                stages.Add(BloodBagFinish);
            }
            if (param == "6")
            {
                int Educate = -1;
                int VitalsOn = -1;
                int Sanitizer = -1;
                int Gloves = -1;
                int AlcoholFirst = -1;
                int Syringe = -1;
                int Trash = -1;
                int InfusionCombination = -1;
                int BloodPole = -1;
                int Squeeze = -1;
                int Clamp = -1;
                int CuffOn = -1;
                int Disinfect = -1;
                int Release = -1;
                int SecondMonitor = -1;

                stages.Add(Educate);
                stages.Add(VitalsOn);
                stages.Add(Sanitizer);
                stages.Add(Gloves);
                stages.Add(AlcoholFirst);
                stages.Add(Syringe);
                stages.Add(Trash);
                stages.Add(InfusionCombination);
                stages.Add(BloodPole);
                stages.Add(Squeeze);
                stages.Add(Clamp);
                stages.Add(CuffOn);
                stages.Add(Disinfect);
                stages.Add(Release);
                stages.Add(SecondMonitor);
            }
            if(param == "7")
            {
                int MonitorFirst = -1;
                int CannulaFirst = -1;
                int PatientSymptomFirst = -1;
                int CannulaSecond = -1;
                int PatientSymptomSecond = -1;
                int Chamber = -1;
                int MonitorSecond = -1;
                int CannulaThird = -1;
                int PatientSymptomThird = -1;
                int Clamp = -1;
                int Soap = -1;
                int Gloves = -1;
                int Wipe = -1;
                int Syringe = -1;
                int Leave = -1;
                int CleanConnector = -1;

                stages.Add(MonitorFirst);
                stages.Add(CannulaFirst);
                stages.Add(PatientSymptomFirst);
                stages.Add(CannulaSecond);
                stages.Add(PatientSymptomSecond);
                stages.Add(Chamber);
                stages.Add(MonitorSecond);
                stages.Add(CannulaThird);
                stages.Add(PatientSymptomThird);
                stages.Add(Clamp);
                stages.Add(Soap);
                stages.Add(Gloves);
                stages.Add(Wipe);
                stages.Add(Syringe);
                stages.Add(Leave);
                stages.Add(CleanConnector);
            }
        }
*/