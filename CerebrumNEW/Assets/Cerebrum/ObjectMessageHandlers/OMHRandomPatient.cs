using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using Cerebrum;

public class OMHRandomPatient : ObjectMessageHandlerBase
{
    public string WhichPatientNums = "12345";
    public bool AddEWS;
    public Vector2 EWSRange;
    public string[] RandomPatientNums;
    private static System.Random rng = new System.Random();

    // This starts the message handler
    protected override void Start()
    {
        base.Start();

        DebugConsole.print("MESSAGE HANDLER: " + gameObject.name);
        DebugConsole.print("OMHRandomPatient:WhichPatientNums before shuffle = "+ WhichPatientNums);

        if (AddEWS)
        {
            var temp = rng.Next(1, WhichPatientNums.Length + 1);
            WhichPatientNums = WhichPatientNums.Replace(temp.ToString(), "");
            WhichPatientNums += rng.Next((int)EWSRange.x, (int)EWSRange.y + 1);
        }

        RandomPatientNums = new string[WhichPatientNums.Length];

        for (int i = 0; i < WhichPatientNums.Length; i++)
        {
            RandomPatientNums[i] = System.Convert.ToString(WhichPatientNums[i]);
        }
        DebugConsole.print("OMHRandomPatient:WhichPatientNums before shuffle = "+ RandomPatientNums);
        Shuffle(RandomPatientNums);
        DebugConsole.print("OMHRandomPatient:WhichPatientNums after shuffle = "+ RandomPatientNums);
    }

    protected override void Awake()
    {
        base.Awake();
    }


    public void Shuffle(string[] randomPatientNums)
    {
        int n = randomPatientNums.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = randomPatientNums[k];
            randomPatientNums[k] = randomPatientNums[n];
            randomPatientNums[n] = value;
        }
    }

    public override bool HandleMessage(string msg, string param, out string retString)
    {
        retString = null;
        DebugConsole.print(this.name + ": OMHRP:Handle Message: before OMHRP.HandleMessage: " + msg + " for " + this.name + " with param = " + param);
        bool retv = base.HandleMessage(msg, param, out retString);
        if (commandFound)
            return retv;
        DebugConsole.print(this.name + ": OMHRP:Handle Message: after OMHRP.HandleMessage: " + msg + " for " + this.name + " with param = " + param);
        //OMHB doesn't return the evaluated parameter just in case you need to do something funky with it.
        if (param != null)// && param[0] == '$')
            param = GameManager.instance.ep.EvaluateParam(param);
        //            param =  ep.EvaluateParam(param);
        DebugConsole.print(this.name + ": OMHRP:Handle Message: after EvaluateParam: " + msg + " for " + this.name + " with param = " + param);


        /*  COMMANDS */

        string varname = param;

        char[] separators = new char[] { ' ', '\t' };

        string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        varname = temp[0];
        string varvalue = null;
        if (temp.Length > 1)
            varvalue = GameManager.instance.ep.EvaluateParam(temp[1]);

        //        bool correct = (param2 == null || param2 == "true" || param2 == "1");
        //        stages[int.Parse(param)] = (correct?1:0);//true;
        /**/
        switch (msg)
        {
            case "getrandompatient":
                string theValue;
                theValue = RandomPatientNums[int.Parse(varname)-1];
                retString = "'" + theValue + "'";
                DebugConsole.print("Bed "+ varname + "gets random patient " + theValue);
                break;

            default:
                break;
        }

        return true;
    }
}
