using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cerebrum;

public class VitalsMH : ObjectMessageHandlerBase
{
    public int HRate = 80;
    public int SpO2 = 98;
    public string Pressure = "125/85";
    public float RespRate = 14;

    public Text HRateText;
    public Text SpO2Text;
    public Text PressureText;
    public Text RespRateText;

    public SWP_HeartRateMonitor heartRateMonitor;

    // Start is called before the first frame update
    void Start()
    {
        heartRateMonitor.BeatsPerMinute = HRate;
        HRateText.text = HRate.ToString();
        SpO2Text.text = SpO2.ToString() + "%";
        PressureText.text = Pressure.ToString();
        RespRateText.text = RespRate.ToString();
        
    }
    public override bool HandleMessage(string msg, string param, out string retString)
    {
        retString = null;
        DebugConsole.print(this.name + ": VitalsMH:Handle Message: before OMHB.HandleMessage: " + msg + " for " + this.name + " with param = "+ param);
        bool retv = base.HandleMessage(msg,param,out retString);
        if (commandFound)
            return retv;
        DebugConsole.print(this.name + ": OMH:Handle Message: after OMHB.HandleMessage: " + msg + " for " + this.name + " with param = "+ param);

        //OMHB doesn't return the evaluated parameter just in case you need to do something funky with it.
        DebugConsole.print(this.name + ": OMH:Handle Message: after EvaluateParam: " + msg + " for " + this.name + " with param = "+ param);


        /*  COMMANDS */

 // $VitalMonitor setvitals 85 99 
        if (msg == "setvitals"){
            char[] separators = new char[] { ' ','\t'};

            string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length > 0){
                param = temp[0];
                if (param != null && param[0] == '$') 
                    param =  ep.EvaluateParam(param);
                HRate = int.Parse(param);
                HRateText.text = param;
            }
            if (temp.Length > 1){
                param = temp[1];
                if (param != null && param[0] == '$') 
                    param =  ep.EvaluateParam(param);
                SpO2 = int.Parse(param);
                SpO2Text.text = param;
            }
            if (temp.Length > 2){
                param = temp[2];
                if (param != null && param[0] == '$') 
                    param =  ep.EvaluateParam(param);
                Pressure = param;
                PressureText.text = param;
            }
            if (temp.Length > 3){
                param = temp[3];
                if (param != null && param[0] == '$') 
                    param =  ep.EvaluateParam(param);
                RespRate = float.Parse(param);
                RespRateText.text = param;
            }

            heartRateMonitor.BeatsPerMinute = HRate;
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
