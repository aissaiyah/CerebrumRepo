using UnityEngine;
using UnityEngine.SceneManagement;
//using GameCreator.Variables;
//using GameCreator.Core;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Stage1Handler : MonoBehaviour
{
    #region Private Variables
    
    ResultHandler resultHandler;

    [HideInInspector]
    public AudioSource SFX;

    Vector3 originalLocation, targetLocation;
    Vector3 originalLocationSBAR, targetLocationSBAR;

    [HideInInspector]
    public int playback;
    [HideInInspector]
    public int numberPrompts, submitCount, itemChosen;

    [HideInInspector]
    public int numberDevicePlaced;

    [HideInInspector]
    public float timeTotal, timePrompt, timeStep1, timeStep2;

    [HideInInspector]
    public bool temperatureReady, monitorRight, isDraggingSBAR;
    [HideInInspector]
    public bool submitReady;
    [HideInInspector]
    public bool[] devicesPlaced; // 0 for nothing, 1 for SpO2, 2 for Blood Pressure, 3 for Temperature Tympanic
    [HideInInspector]
    public bool[] SBARPlaced; // 0 for wrong answer, 1 for Situation, 2 for Background, 3 for Assessment, 4 for Recommendation

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

    [Header("For Camera Panning")]
//    public CameraFollow cameraFolllow;

    [Header("For Fade In and Fade Out")]
    public GameObject fader;

    [Header("For Intro Title")]
    public RectTransform imageTitle;
    public RectTransform imageSubTitle;
    public RectTransform imageOutro;

    [Header("For Equipments Scenario")]
    public GameObject cursor;
    public GameObject originalcursor;
    public GameObject targetCursor;
    public float step;
    public GameObject temperature;
    public GameObject imageDoneCuff;
    public GameObject imageDoneClip;
    public GameObject imageDoneTemp;
    public GameObject imageNotDoneCuff;
    public GameObject imageNotDoneClip;
    public GameObject imageNotDoneTemp;

    [Header("For SBAR Scenario")]
    public GameObject[] SBARImagesDrag;
    public GameObject cursorSBAR;
    public GameObject originalcursorSBAR;
    public GameObject targetCursorSBAR;

    [Header("For UI Integration")]
    public GameObject buttonStart;
    public GameObject buttonSetup;
    public GameObject buttonReassessPatient;
    public RectTransform imageInstruction;
    public Text textInstruction;
    public RectTransform imagePrompt;
    public RectTransform imagePromptMonitor;
    public RectTransform imagePromptCuff;
    public RectTransform imagePromptCall;
    public Text textPrompt;
    public GameObject canvasVitalSigns;
    public GameObject buttonBackToPatient;
    public GameObject buttonSBAR;
    public GameObject buttonNextScene;
    public GameObject canvasSBAR;
    public GameObject buttonSubmit;
    public TextMeshProUGUI textHistory;
    public GameObject MCQInformation;
    public GameObject buttonVitalSigns;

    [Header("For Audio")]
    public AudioClip audioCorrect;
    public AudioClip audioWrong;
    public AudioClip audioPrompt;
    public AudioClip audioUI;
    public AudioClip audioTemperature;
    public AudioClip audioBeepReady;

    #endregion
    
    void Start()
    {
        resultHandler = GameObject.Find("Singleton - Result Handler").GetComponent<ResultHandler>();

        SFX = GetComponent<AudioSource>();

        PlaybackSetup();

        //LeanTween.alpha(fader, 0f, 2f).setDelay(0.5f);//.setOnComplete(StartGameTitle);
        Invoke("StartGameTitle", 0.5f);

        Invoke("CameraFollow", 1.5f);
    }

    void Update()
    {
        CheckAllDevicesConnected();

        if (numberDevicePlaced == 3)
        {
            buttonReassessPatient.SetActive(true);

            textPrompt.text = "Waiting Vital Signs to Stabilize";
            //LeanTween.alpha(imagePrompt, 1f, 1f).setDelay(1f).setOnComplete(FinishedPrompt);
            //LeanTween.alpha(imageInstruction, 0f, 1f).setDelay(1f);
            /*
            canvasVitalSigns.SetActive(true);
            buttonBackToPatient.SetActive(false);*/

            CheckScore(1);
            TotalScore();

            numberDevicePlaced = 0;
        }
    }

    #region Public Functions

    public void FinishedStartGameTitle()
    {
        started = true;

        //LeanTween.alpha(imageTitle, 0f, 1f).setDelay(3f);
        //LeanTween.alpha(imageSubTitle, 0f, 1f).setDelay(2f);
    }

    public void ResetTimerPrompt()
    {
        timePrompt = 0;
    }

    public void MonitorSubmit()
    {
        if (!monitorRight)
        {
            ___success.success = false;

            API_MCQ();

            //API_SBARchosen = 0;

            SFX.clip = audioWrong;
            SFX.Play();

            ShowPrompt();
        }
        else if (monitorRight)
        {
            ___success.success = true;
            API_MCQ();

            SFX.clip = audioCorrect;
            SFX.Play();
        }

        ___API_SBARchosen = 0;
    }

    public void SetupCursor()
    {
        originalLocation = new Vector3(originalcursor.transform.position.x, originalcursor.transform.position.y, originalcursor.transform.position.z);
        targetLocation = new Vector3(targetCursor.transform.position.x, targetCursor.transform.position.y, targetCursor.transform.position.z);

    }

    public void CheckAllDevicesConnected()
    {
        if (devicesPlaced[1] == true && devicesPlaced[2] == true && devicesPlaced[3] == true && !oneTimeOnly)
        {
            //send api
            APIHandler.typeAPI = 8;
            APIHandler.actor = _actor;
            APIHandler.verb = _verb;
            APIHandler.objectData = _objectData;
            APIHandler.contextOptionsAlternate = _contextOptionsAlternate;

            APIHandler.SendAPI();

            StartCoroutine(SetPromptDynamap());

            //buttonSBAR.SetActive(true);

            //originalLocationSBAR = new Vector3(cursorSBAR.transform.position.x, cursorSBAR.transform.position.y, cursorSBAR.transform.position.z);
            originalLocationSBAR = new Vector3(originalcursorSBAR.transform.position.x, originalcursorSBAR.transform.position.y, originalcursorSBAR.transform.position.z);
            targetLocationSBAR = new Vector3(targetCursorSBAR.transform.position.x, targetCursorSBAR.transform.position.y, targetCursorSBAR.transform.position.z);

            //VariablesManager.SetGlobal("Stage-1---Vital-Signs-Done", true);

            oneTimeOnly = true;
        }
    }

    public void UnLoopSound()
    {
        SFX.loop = false;
    }

    public void CheckSBAR()
    {
        API_SBAR();

        if (SBARPlaced[1] == true && SBARPlaced[2] == true && SBARPlaced[3] == true && SBARPlaced[4] == true)
        {
            //LeanTween.alpha(fader, 0f, 0.25f);

            SFX.clip = audioCorrect;
            SFX.Play();

            //buttonNextScene.SetActive(true);
            //buttonSubmit.SetActive(false);
            //canvasSBAR.SetActive(false);

            submitReady = true;
        }
        else
        {
            SFX.clip = audioWrong;
            SFX.Play();

            textPrompt.text = "Are you sure this is the right report?";
            //LeanTween.alpha(imagePrompt, 1f, 1f).setOnComplete(FinishedPrompt);

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

    public void GoToStage2()
    {
        //APIHandler.FinishScorm();

        resultHandler.Stage1TotalTime = timeTotal;
        resultHandler.Stage1TimeStep1 = timeStep1;
        resultHandler.Stage1TimeStep2 = timeStep2;
        resultHandler.Stage1PromptsCount = numberPrompts;
        resultHandler.Stage1SubmitCount = submitCount;

        resultHandler.HistoryStage1 = textHistory.text;

        //LeanTween.alpha(imageOutro, 1f, .1f);
        //LeanTween.alpha(fader, 1f, 2f);//.setOnComplete(ChangeScene);
    }

    public void PlaybackEnd()
    {
        //LeanTween.alpha(fader, 1f, 2f).setOnComplete(ChangeScene2);
    }

    #endregion

    #region Private Functions

    void CameraFollow()
    {
//        cameraFolllow.enabled = true;
    }

    void PlaybackSetup()
    {
        playback = resultHandler.Stage1Playback;

        if (playback == 0)
        {
            numberPrompts = 0; submitCount = 0; numberDevicePlaced = 0;

            timeTotal = 0; timePrompt = 0; timeStep1 = 0; timeStep2 = 0;

            devicesPlaced = new bool[4];
            for (int i = 1; i < devicesPlaced.Length; i += 1)
            {
                devicesPlaced[i] = false;
            }

            SBARPlaced = new bool[5];
            for (int i = 1; i < SBARPlaced.Length; i += 1)
            {
                SBARPlaced[i] = false;
            }

            started = false; oneTimeOnly = false; tutorial = false; temperatureReady = false; submitReady = false;
            startStep1 = false; startStep2 = false;
        }
        else if (playback == 1)
        {
            timePrompt = 0;

            devicesPlaced = new bool[4];
            for (int i = 1; i < devicesPlaced.Length; i += 1)
            {
                devicesPlaced[i] = false;
            }

            started = true; oneTimeOnly = false; tutorial = false; temperatureReady = false;
            startStep1 = true; startStep2 = false;
        }
        else if (playback == 2)
        {
            timePrompt = 0;

            SBARPlaced = new bool[5];
            for (int i = 1; i < SBARPlaced.Length; i += 1)
            {
                SBARPlaced[i] = false;
            }

            started = true;
            startStep1 = false; startStep2 = true;
        }
    }

    void ShowPrompt()
    {
        //LeanTween.alpha(imagePromptMonitor, 1f, 0.5f).setOnComplete(UnshowPrompt);
        //LeanTween.alpha(imagePromptCuff, 1f, 0.5f).setOnComplete(UnshowPrompt);
        //LeanTween.alpha(imagePromptCall, 1f, 0.5f).setOnComplete(UnshowPrompt);

        monitorRight = false;
    }

    void UnshowPrompt()
    {
        //LeanTween.alpha(imagePromptMonitor, 0f, 0.5f).setDelay(1f);
        //LeanTween.alpha(imagePromptCuff, 0f, 0.5f).setDelay(1f);
        //LeanTween.alpha(imagePromptCall, 0f, 0.5f).setDelay(1f);
    }

    void StartGameTitle()
    {
        //buttonSetup.SetActive(true);

        //LeanTween.alpha(imageTitle, 1f, 2f);
        //LeanTween.alpha(imageSubTitle, 1f, 2f).setDelay(1f).setOnComplete(StartGameTitle2);

        //Invoke("FinishedStartGameTitle", 2f);
    }

    void StartGameTitle2()
    {
        buttonStart.SetActive(true);
    }

    void FinishedPrompt()
    {
        //LeanTween.alpha(imagePrompt, 0f, 1f).setDelay(2f);
    }

    void RestartTutorial()
    {
        tutorial = true;
    }

    void RestartTutorialSBAR()
    {
        tutorialSBAR = true;
    }

    void ForceDynamapLook()
    {
        //LeanTween.alpha(canvasVitalSigns.GetComponent<RectTransform>(), 1f, 2f).setDelay(2f);
    }

    void ResetSBAR()
    {
        API_SBARchosen = 0;

        for (int i = 1; i < SBARPlaced.Length; i += 1)
        {
            SBARPlaced[i] = false;
        }

        for (int i = 0; i < SBARImagesDrag.Length; i += 1)
        {
//            SBARImagesDrag[i].GetComponent<SBARHandler_DragStage1>().ResetAll();
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Stage2");
    }

    void ChangeScene2()
    {
        SceneManager.LoadScene("ResultScreen");
    }

    IEnumerator SetPromptDynamap()
    {
        textPrompt.text = "Vital Signs Have Stabilized";
        //LeanTween.alpha(imagePrompt, 1f, 1f).setDelay(1f).setOnComplete(FinishedPrompt);

        yield return new WaitForSeconds(4);

        buttonBackToPatient.SetActive(true);
        canvasVitalSigns.SetActive(false);
        MCQInformation.SetActive(true);
        buttonVitalSigns.SetActive(true);
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