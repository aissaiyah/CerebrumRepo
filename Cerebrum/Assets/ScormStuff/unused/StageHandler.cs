using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using System.Collections;
using TMPro;

public class StageHandler : MonoBehaviour
{
    #region Private Variables
    
    ResultHandler resultHandler;


    [HideInInspector]
    public int numberPrompts, submitCount, itemChosen;

    [HideInInspector]
    public float timeTotal, timePrompt, timeStep1, timeStep2;


    [HideInInspector]
    public bool submitReady;
    [HideInInspector]

    public bool startStep1, startStep2;
    bool started, oneTimeOnly;
    [HideInInspector]
    public bool tutorial, tutorialSBAR;

    #endregion

    #region Public Variables

    [Header("For Sending Logs MCQ")]
    public ActorData ___actor;
    public VerbData ___verb;
    public ObjectData ___objectData;
    public ContextDataSBARSubmit ___context;
    public ResultData ___success;
    public int ___API_SBARchosen = 0;
    public string[] ___API_SBARoption;
    public string[] ___API_SBARoptions;
    [Header("For Sending Logs - Phone")]
    public APIHandler APIHandler;
    public ActorData _actor;
    public VerbData _verb;
    public ObjectData _objectData;
    public ContextDataOptionsAlternate _contextOptionsAlternate;
    [Header("For Sending Logs - SubmitSBAR")]
    public ActorData __actor;
    public VerbData __verb;
    public ObjectData __objectData;
    public ContextDataSBARSubmit __context;
    public ResultData __success;
    public int API_SBARchosen = 0;
    public string[] API_SBARoption;
    public string[] API_SBARoptions;




    #endregion
    
    void Start()
    {
//        resultHandler = GameObject.Find("Singleton - Result Handler").GetComponent<ResultHandler>();

    }

    void Update()
    {

    }

    #region Public Functions

    public void FinishedStartGameTitle()
    {
        started = true;
    }

    public void ResetTimerPrompt()
    {
        timePrompt = 0;
    }

    public void MonitorSubmit()
    {
        if (!true)
        {
            ___success.success = false;

            API_MCQ();

        }
        else
        {
            ___success.success = true;
            API_MCQ();

        }

        ___API_SBARchosen = 0;
    }



    public void CheckAllDevicesConnected()
    {
        if (!oneTimeOnly)
        {
            //send api
            APIHandler.typeAPI = 8;
            APIHandler.actor = _actor;
            APIHandler.verb = _verb;
            APIHandler.objectData = _objectData;
            APIHandler.contextOptionsAlternate = _contextOptionsAlternate;

            APIHandler.SendAPI();

            oneTimeOnly = true;
        }
    }


    public void CheckSBAR()
    {
        API_SBAR();

        if (true)//SBARPlaced[1] == true && SBARPlaced[2] == true && SBARPlaced[3] == true && SBARPlaced[4] == true)
        {

            submitReady = true;
        }
        else
        {

            submitCount++;
            numberPrompts++;

            ResetSBAR();
        }
    }

    public void SubmitSBARCount()
    {
        submitCount++;
    }

    public void API_SBAR()
    {
        API_SBARoptions = new string[API_SBARchosen];
        for (int i = 0; i < API_SBARchosen; i += 1)
        {
            API_SBARoptions[i] = API_SBARoption[i];
        }

        __context.extensions.options = API_SBARoptions;

        //send api
        APIHandler.typeAPI = 6;
        APIHandler.actor = __actor;
        APIHandler.verb = __verb;
        APIHandler.objectData = __objectData;
        APIHandler.contextSBARSubmit = __context;
    }

    public void API_MCQ()
    {
        ___API_SBARoptions = new string[___API_SBARchosen];
        for (int i = 0; i < ___API_SBARchosen; i += 1)
        {
            ___API_SBARoptions[i] = ___API_SBARoption[i];
        }

        ___context.extensions.options = ___API_SBARoptions;

        //send api
        APIHandler.typeAPI = 6;
        APIHandler.actor = ___actor;
        APIHandler.verb = ___verb;
        APIHandler.objectData = ___objectData;
        APIHandler.contextSBARSubmit = ___context;
        APIHandler.resultData = ___success;

        APIHandler.SendAPI();
    }


/*
    void FinishGame()
    {
        apiHandler.FinishScorm();
        //SceneManager.LoadScene("ResultScreen");
    }

*/

    #endregion

    #region Private Functions



    void ResetSBAR()
    {
        API_SBARchosen = 0;
/*
        for (int i = 1; i < SBARPlaced.Length; i += 1)
        {
            SBARPlaced[i] = false;
        }
*/
//        for (int i = 0; i < SBARImagesDrag.Length; i += 1)
        {
//            SBARImagesDrag[i].GetComponent<SBARHandler_DragStage1>().ResetAll();
        }
    }




    #endregion

    #region Score Handler

    [Header("For Score")]
    public int Score;
    public int WrongAttempts;

    public void CheckScore(int ScoringType)
    {
        switch (ScoringType)
        {
            case 1:
                if (WrongAttempts == 0)
                    Score += 10;
                else if (WrongAttempts == 1)
                    Score += 5;
                else if (WrongAttempts == 2)
                    Score += 1;

                break;
        }
    }

    public void TotalScore()
    {
        resultHandler.Stage1Score += Score;
        Score = 0;
    }

    #endregion
}