using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cerebrum;

public class QuitHandler : ObjectMessageHandlerBase
{
    public ScormHandlerBase scormHandler;
    // Start is called before the first frame update
    void Awake()
    {
        if (scormHandler==null)
             scormHandler = UnityEngine.Object.FindObjectOfType<ScormHandlerBase>();
        
    }

    public override bool HandleMessage(string msg, string param, out string retString)
    {
        Debug.Log(this.name + ": QuitHandler:Handle Message " + msg + " for " + this.name + " with param = "+ param);
        DebugConsole.print("QuitHandler:Handle Message: MeshRenderer for " + this.name + " = " + mr);
        DebugConsole.print("QuitHandler:Handle Message: EP is " + ep + ", my.EP is "+ MyGameManager.instance.ep);
        ParamData res;
        retString = null;
/*
        if (ep==null)
            ep=MyGameManager.instance.ep;

        DebugConsole.print("QuitHandler:HM param="+ param);

        //if only 1 parameter, then evaluate
        if (param != null && (((param[0] == '$'  && (!param.Contains(" ") || param.Contains("+"))) || param[0] == '\'') && !param.Contains("|") && !param.Contains(","))){
            DebugConsole.print(this.name + ": OMHB: Looks like a variable " + msg + " for " + this.name + " with evaluated param = " + param);
            DebugConsole.print("OMHB: ep= "+ ep + ", msg = "+ msg);
            param =  MyGameManager.instance.ep.EvaluateParam(param);
        }
        DebugConsole.print(this.name + ": OMHB:Handle Message " + msg + " for " + this.name + " with evaluated param = " + param);
*/
        commandFound = true;  //will be set to false in default if no command found
        switch(msg)
        {

            case "startgame":
                //Debug.Log("StartGame1");
                if (scormHandler!=null){
                    //Debug.Log("StartGame2");
                    scormHandler.StartGame();
                }
                break;
            case "endgame":
                QuitGame();
                break;
            default:
                commandFound = false;
                break;
        }
        return commandFound;
    }

     public void QuitGame()
     {
        StartCoroutine(QuitGameCo());
     }

     IEnumerator QuitGameCo()
    {
        if (scormHandler!=null)
            scormHandler.EndGame();
        Debug.Log("Game is exiting");
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }
          
    }
}
