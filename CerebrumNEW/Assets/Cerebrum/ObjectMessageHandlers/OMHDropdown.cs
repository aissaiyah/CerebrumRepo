using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cerebrum;

public class OMHDropdown : ObjectMessageHandlerBase
{
    private TMP_Dropdown dropDown;
    
    protected override void Awake()
    {
        base.Awake();
        dropDown = this.gameObject.GetComponent<TMP_Dropdown>();
        UpdateDropdown(dropDown);
        dropDown.onValueChanged.AddListener(delegate
        {
            UpdateDropdown(dropDown);
        });
    }

    public void UpdateDropdown(TMP_Dropdown dd)
    {
        inputText = dd.options[dd.value].text;
    }

    public void ResetDropdown(TMP_Dropdown dd, int value)
    {
        dropDown.value = value;
        UpdateDropdown(dropDown);
    }

    public override bool HandleMessage(string msg, string param, out string retString)
    {
        retString = null;
        print(this.name + ": OMHGV:Handle Message: before OMHB.HandleMessage: " + msg + " for " + this.name + " with param = " + param);
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


        switch (msg)
        {
            case "restoredropdown":
                {
                    ResetDropdown(dropDown, int.Parse(param));
                    break;
                }
            case "dropdown.choice":
                {
                   retString = dropDown.value.ToString();
                   break;
                }
            case "checkaccuracy":
                {
                    DebugConsole.print("CheckAccuracy choice = " +dropDown.options[dropDown.value].text + ", param = "+ param);
                    return (dropDown.options[dropDown.value].text == param);
                }
            case "checkchoicenumber":
                {
                    string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    varname = temp[0];
                    string varvalue = null;
                    if (temp.Length > 1)
                        varvalue = GameManager.instance.ep.EvaluateParam(temp[1]);
                    int varint = int.Parse(varvalue);
                    DebugConsole.print("CheckChoiceNumber choice value = " +dropDown.value + "==" + varint);
                    return (dropDown.value == varint);
                }
            default:
                break;
        }

        return true;
    }
}
