using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using Cerebrum;

public class OMHScoreManager : ObjectMessageHandlerBase
{ 
    public bool correctSet = false;
    public bool chosenSet = false; 
    public bool trackTime = false;
    public bool isEmpty;
    public bool isEWS;
    public List<int> stages = new List<int>();
    public bool stageset;
    public float pointTotal = 0;
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
    protected override void Start()
    {
        base.Start();

        //ikcontroller = GetComponentInChildren<IKController>();
        DebugConsole.print("MESSAGE HANDLER: " + gameObject.name + " FOUND MESHRENDERER " + mr);
        if(mr != null && consentPages != null && consentPages.Length > 1)
        {
            mr.material = consentPages[0];
        }
        base.Start();
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
        if (gm==null)
            gm = GameManager.instance; //Hack:  Really should pass in GM
        DebugConsole.print("OMH:HandleMessage: gm="+ gm);

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

        string [] parameters;
        GetEvaluatedParams(param, out parameters);

        /*  COMMANDS */


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
                gameObject.GetComponent<TextMeshProUGUI>().text = param.Replace("***", "\n");
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

        else if(msg == "strikethrough")
        {
            if(GetComponent<TextMeshProUGUI>() != null)
            {
                GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
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

        else if(msg == "quit")
        {
            Application.Quit();
        }
        else
        {
            string errmsg = "Command not found! " + msg + " for " + this.name + " with param = "+ param;
            Debug.LogError(errmsg);
            
        }


        return true;
    }



   

    // Update is called once per frame
    protected override void Update()
    {
        //DebugConsole.print("OMH:Update1");

        base.Update();


    }
}
    ////////////////////////// OLD UNUSED CODE ///////////////////////////////////

