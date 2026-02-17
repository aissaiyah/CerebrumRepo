using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;
using Cerebrum;

public class OMHGlobalVariable : ObjectMessageHandlerBase
{ 
    public bool correctSet = false;

    Dictionary<string, string> variables = new Dictionary<string, string>();

    // This starts the message handler
    protected override void Start()
    {
        base.Start();

        //ikcontroller = GetComponentInChildren<IKController>();
        DebugConsole.print("MESSAGE HANDLER: " + gameObject.name + " FOUND MESHRENDERER " + mr);
    }

    protected override void Awake()
    {
        base.Awake();
        /*
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GVH");

        if (objs!=null && objs.Length > 1)
        {
            Destroy(this.gameObject);
        }*/
        DontDestroyOnLoad(this.gameObject);
    }


    public override bool HandleMessage(string msg, string param, out string retString)
    {
        retString = null;
        DebugConsole.print(this.name + ": OMHGV:Handle Message: before OMHB.HandleMessage: " + msg + " for " + this.name + " with param = " + param);
        bool retv = base.HandleMessage(msg, param, out retString);
        if (commandFound)
            return retv;
        DebugConsole.print(this.name + ": OMHGV:Handle Message: after OMHB.HandleMessage: " + msg + " for " + this.name + " with param = " + param);
        //OMHB doesn't return the evaluated parameter just in case you need to do something funky with it.
        if (param != null)// && param[0] == '$')
            param = GameManager.instance.ep.EvaluateParam(param);
        //            param =  ep.EvaluateParam(param);
        DebugConsole.print(this.name + ": OMHGV:Handle Message: after EvaluateParam: " + msg + " for " + this.name + " with param = " + param);


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
            case "setglobal":
            
                //print("GV:setvalue: " + varname + " = " + varvalue);
                variables.AddSafe(varname,varvalue);
                break;
            
            case "getglobal":  //IMPORTANT: All values except True/False returned as strings!
            
                string theValue;
                if (!variables.ContainsKey(varname))
                    theValue = "0";
                else
                    theValue = variables[varname];
                //print("GV:getvalue: " + varname + " = " + theValue);
                if (theValue=="True" || theValue=="False")
                    retString = theValue.ToLower(); //need to make into string expression
                else
                retString = "'"+ theValue + "'"; //need to make into string expression
                break;
            default:
                break;
        }

        return true;
    }



    /* Update is called once per frame
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
    */
}
