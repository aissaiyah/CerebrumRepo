using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cerebrum;

public class TestTubeFiller : ObjectMessageHandlerBase
{
    public float shaderMaxLev = 0.53f;
    public float shaderMinLev = 0.47f;
    public float shaderFillAmount = 0.5f;
    public float fillAmount = 0.5f;
    public GameObject LiquidHolder;
    public Material mat;
  
    public override bool HandleMessage(string msg, string param, out string retString)
    {
        retString = null;
        bool retv = base.HandleMessage(msg,param,out retString);
        if (commandFound)
            return retv;


        /*  COMMANDS */

        if (msg == "fillamount"){

            if (param != null && param[0] == '$') 
                param =  GameManager.instance.ep.EvaluateParam(param);
            fillAmount = float.Parse(param);
            SetFill();
            return true;
        }
        return false;
    }

    void SetFill()
    {
        float range = shaderMaxLev - shaderMinLev;
        shaderFillAmount =  range * (1f - fillAmount)+ shaderMinLev;
        mat.SetFloat("_FillAmount",shaderFillAmount);

    }
    // Start is called before the first frame update
    void Start()
    {
        mat = LiquidHolder.GetComponent<Renderer>().material;
        SetFill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
