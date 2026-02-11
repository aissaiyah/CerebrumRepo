using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Stage4Handler : MonoBehaviour
{
    #region Private Variables

    ResultHandler resultHandler;

    [HideInInspector]
    public AudioSource SFX;

    [HideInInspector]
    public float timeTotal, timePrompt;
    //float numberSpO2;
    float numberRR;
    float numberHR;

    [HideInInspector]
    public int numberPrompt;
    //[HideInInspector]
    public int CPRCount, wrongItem, submitCount, itemChosen, itemRight, itemRight2;
    //[HideInInspector]
    public int readyCPR;

    [HideInInspector]
    public bool rightItem, proceedItem, isDraggingSBAR;
    [HideInInspector]
    public bool pickedCPR, pickedBVM;
    [HideInInspector]
    public bool submitReady, itemChosenMCQRight;
    [HideInInspector]
    public bool[] SBARPlaced; // 0 for wrong answer, 1 for Situation, 2 for Background, 3 for Assessment, 4 for Recommendation
    bool started;
    bool[] increaseNumberDevice; // 0 for nothing, 1 for SpO2, 2 for RR, 3 for HR

    #endregion

    #region Public Variables

    [Header("For API MCQ")]
    public APIHandler apiHandler;
    public ActorData _actor;
    public VerbData _verb;
    public ObjectData _objectData;
    public ContextDataSBARSubmit _context;
    public ResultData _success;
    public int _API_SBARchosen = 0;
    public string[] _API_SBARoption;
    public string[] _API_SBARoptions;

    [Header("For Sending Logs - SubmitSBAR")]
    public ActorData __actor;
    public VerbData __verb;
    public ObjectData __objectData;
    public ContextDataSBARSubmit __context;
    public ResultData __success;
    public int __API_SBARchosen = 0;
    public string[] __API_SBARoption;
    public string[] __API_SBARoptions;

    [Header("For Camera Panning")]
    //public CameraFollow cameraFolllow;

    [Header("For Fade In and Fade Out")]
    public GameObject fader;

    [Header("For Intro Title")]
    public RectTransform imageTitle;
    public RectTransform imageSubTitle;
    public RectTransform imageOutro;

    [Header("For Syncing Patient Position")]
    public GameObject child;
    public GameObject child_30;
    public GameObject bed;
    public GameObject bed_30;
    public GameObject probeLeft;
    public GameObject probeLeft_30;
    public GameObject probeRight;
    public GameObject probeRight_30;
    public GameObject clipLeft;
    public GameObject clipRight;
    public GameObject clipLeft_30;
    public GameObject clipRight_30;

    [Header("For Item Choose Scenario")]
    public Text numberItemChosen1;
    public RectTransform imagePromptItem;    
    public Text numberItemChosen2;
    public RectTransform imagePromptItem2;

    [Header("For SBAR Scenario")]
    public GameObject[] SBARImagesDrag;

    [Header("For UI Integration")]
    public Text textnumberSpO2;
    public Text textnumberRR;
    public Text textnumberHR;
    public RectTransform imageInstruction;
    public RectTransform imageWrongPrompt;
    public Text textWrongPrompt;
    public RectTransform imagePrompt;
    public Text textPrompt;
    public GameObject buttonStart;
    public GameObject buttonBack;
    public TextMeshProUGUI textHistoryStage1;
    public TextMeshProUGUI textHistoryStage2;
    public TextMeshProUGUI textHistoryStage3;
    public TextMeshProUGUI textHistoryStage4;

    [Header("For Audio")]
    public AudioClip audioCorrect;
    public AudioClip audioWrong;
    public AudioClip audioUI;

    #endregion

    void Start()
    {
        resultHandler = GameObject.Find("Singleton - Result Handler").GetComponent<ResultHandler>();

        textHistoryStage1.text = resultHandler.HistoryStage1;
        textHistoryStage2.text = resultHandler.HistoryStage2;
        textHistoryStage3.text = resultHandler.HistoryStage3;

        SFX = GetComponent<AudioSource>();

        timeTotal = 0; timePrompt = 0;

        //numberSpO2 = 90;
        numberRR = 20;
        numberHR = 100;

        numberPrompt = 0;

        CPRCount = 0; wrongItem = 0; submitCount = 0; itemChosen = 0; itemRight = 0;

        readyCPR = 0;

        increaseNumberDevice = new bool[4];
        for (int i = 1; i < increaseNumberDevice.Length; i += 1)
        {
            increaseNumberDevice[i] = true;
        }
        SBARPlaced = new bool[5];
        for (int i = 1; i < SBARPlaced.Length; i += 1)
        {
            SBARPlaced[i] = false;
        }
        submitReady = false; started = false; rightItem = false; proceedItem = false; pickedCPR = false; pickedBVM = false;

        //LeanTween.alpha(fader, 0f, 3.5f).setDelay(0.75f);
        Invoke("StartGameTitle", 0.5f);

        Invoke("CameraFollow", 1.5f);

        //SyncPatientPosition();
    }

    void Update()
    {
        /*
        if (started)
            Timers();*/
        
        #region For Increase/Decrease Number Vital Signs
        /*
        if (increaseNumberDevice[1])
        {
            numberSpO2 -= Time.deltaTime / 2;
            textnumberSpO2.text = numberSpO2.ToString("N0") + " %";

            if (numberSpO2 <= 85)
                increaseNumberDevice[1] = false;
        }
        
        if (increaseNumberDevice[2])
        {
            numberRR -= Time.deltaTime * 2 / 5;
            textnumberRR.text = numberRR.ToString("N0") + " BpM";

            if (numberRR <= 10)
                increaseNumberDevice[2] = false;
        }

        if (increaseNumberDevice[3])
        {
            numberHR -= Time.deltaTime;
            textnumberHR.text = numberHR.ToString("N0") + " BpM";

            if (numberHR <= 80)
                increaseNumberDevice[3] = false;
        }
        */
        #endregion
        
    }

    #region Public Functions

    public void FinishedStartGameTitle()
    {
        started = true;

        //LeanTween.alpha(imageTitle, 0f, 1f);
        //LeanTween.alpha(imageSubTitle, 0f, 1f);
    }

    public void ResetTimePrompt()
    {
        timePrompt = 0;
    }

    public void ItemSubmit()
    {

        if (itemRight == 2 && itemChosen == 2)
        {
            API_MCQTRUE();

            SFX.clip = audioCorrect;
            SFX.Play();

            proceedItem = true;
        }
        else if (itemRight2 == 6 && itemChosen == 6)
        {
            API_MCQ2TRUE();

            SFX.clip = audioCorrect;
            SFX.Play();

            itemChosenMCQRight = true;
        }
        else //(itemChosen > 2 || itemRight < 2)
        {
            API_MCQ();

            SFX.clip = audioWrong;
            SFX.Play();

            ShowPrompt();
        }

        _API_SBARchosen = 0;
    }


    public void API_MCQ()
    {
        _API_SBARoptions = new string[_API_SBARchosen];
        for (int i = 0; i < _API_SBARchosen; i += 1)
        {
            _API_SBARoptions[i] = _API_SBARoption[i];
        }

        _context.extensions.options = _API_SBARoptions;

        //send api
        apiHandler.typeAPI = 6;
        apiHandler.actor = _actor;
        apiHandler.verb = _verb;
        apiHandler.objectData = _objectData;
        apiHandler.contextSBARSubmit =_context;
        apiHandler.resultData.success = false;

        apiHandler.SendAPI();
    }

    public void API_MCQTRUE()
    {
        _API_SBARoptions = new string[_API_SBARchosen];
        for (int i = 0; i < _API_SBARchosen; i += 1)
        {
            _API_SBARoptions[0] = "Board";
            _API_SBARoptions[1] = "BagMask";
        }

        _context.extensions.options = _API_SBARoptions;

        //send api
        apiHandler.typeAPI = 6;
        apiHandler.actor = _actor;
        apiHandler.verb = _verb;
        apiHandler.objectData = _objectData;
        apiHandler.contextSBARSubmit = _context;
        apiHandler.resultData = _success;

        apiHandler.SendAPI();
    }
    public void API_MCQ2TRUE()
    {
        _API_SBARoptions = new string[_API_SBARchosen];
        for (int i = 0; i < _API_SBARchosen; i += 1)
        {
            _API_SBARoptions[0] = "SterileGauze";
            _API_SBARoptions[1] = "Laryngoscope";
            _API_SBARoptions[2] = "ETTIntroducer";
            _API_SBARoptions[3] = "ETT";
            _API_SBARoptions[4] = "Colorimetric";
            _API_SBARoptions[5] = "Leukoplast";
            //_API_SBARoptions[6] = "Colorimetric";
        }

        _context.extensions.options = _API_SBARoptions;

        //send api
        apiHandler.typeAPI = 6;
        apiHandler.actor = _actor;
        apiHandler.verb = _verb;
        apiHandler.objectData = _objectData;
        apiHandler.contextSBARSubmit = _context;
        apiHandler.resultData = _success;

        apiHandler.SendAPI();
    }

    public void CheckItem()
    {
        if (rightItem)
        {
            SFX.clip = audioCorrect;
            SFX.Play();

            readyCPR++;
        }
        else
        {
            SFX.clip = audioWrong;
            SFX.Play();
        }
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

            submitCount++;

            ResetSBAR();
        }
    }

    public void SubmitSBARCount()
    {
        submitCount++;
    }

    public void API_SBAR()
    {
        __API_SBARoptions = new string[__API_SBARchosen];
        for (int i = 0; i < __API_SBARchosen; i += 1)
        {
            __API_SBARoptions[i] = __API_SBARoption[i];
        }

        __context.extensions.options = __API_SBARoptions;

        //send api
        //APIHandler.typeAPI = 6;
        //APIHandler.actor = __actor;
        //APIHandler.verb = __verb;
        //APIHandler.objectData = __objectData;
        apiHandler.contextSBARSubmit = __context;
    }


    public void GoToResult()
    {
        resultHandler.Stage4TotalTime = timeTotal;
        resultHandler.Stage4PromptsCount = numberPrompt;

        resultHandler.Stage4CPRAttempts = CPRCount;
        resultHandler.Stage4WrongItems = wrongItem;
        resultHandler.Stage4SubmitCount = submitCount;

        //LeanTween.alpha(imageOutro, 1f, 1f);
        //LeanTween.alpha(fader, 1f, 3f).setDelay(0.75f).setOnComplete(FinishGame);
    }

    #endregion

    #region Private Functions

    void CameraFollow()
    {
//        cameraFolllow.enabled = true;
    }

    void Timers()
    {
        timeTotal += Time.deltaTime;

        timePrompt += Time.deltaTime;
        if (timePrompt > 60)
        {
            Debug.Log("Too long");
            timePrompt = 0;

            numberPrompt++;
        }
    }

    void SyncPatientPosition()
    {
        if (resultHandler.Stage2ChangeProbeSide)
        {
            probeLeft.SetActive(false);
            probeLeft_30.SetActive(false);
            probeRight.SetActive(true);
            probeRight_30.SetActive(true);
            clipLeft.SetActive(false);
            clipRight.SetActive(true);
            clipLeft_30.SetActive(false);
            clipRight_30.SetActive(true);
        }
    }

    void StartGameTitle()
    {
        //LeanTween.alpha(imageTitle, 1f, 1.5f);
        //LeanTween.alpha(imageSubTitle, 1f, 1.5f).setDelay(1f).setOnComplete(StartGameTitle2);

        //Invoke("FinishedStartGameTitle", 1.5f);
    }

    void StartGameTitle2()
    {
        buttonStart.SetActive(true);
    }

    void ShowPrompt()
    {
        //LeanTween.alpha(imagePromptItem, 1f, 0.5f).setOnComplete(UnshowPrompt);
        //LeanTween.alpha(imagePromptItem2, 1f, 0.5f).setOnComplete(UnshowPrompt);

        itemChosen = 0;
        itemRight = 0;
        itemRight2 = 0;
    }

    void UnshowPrompt()
    {
        //LeanTween.alpha(imagePromptItem, 0f, 0.5f).setDelay(1f);
        //LeanTween.alpha(imagePromptItem2, 0f, 0.5f).setDelay(1f);
        //LeanTween.alpha(imageInstruction, 0f, 1f).setDelay(2f);
    }

    void ResetSBAR()
    {
        __API_SBARchosen = 0;

        for (int i = 1; i < SBARPlaced.Length; i += 1)
        {
            SBARPlaced[i] = false;
        }

        for (int i = 0; i < SBARImagesDrag.Length; i += 1)
        {
//            SBARImagesDrag[i].GetComponent<SBARHandler_DragStage4>().ResetAll();
        }
    }

    void FinishGame()
    {
        apiHandler.FinishScorm();
        //SceneManager.LoadScene("ResultScreen");
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

            case 2: //multiple mcq
                if (WrongAttempts == 0)
                    Score += 10;
                else if (WrongAttempts == 1)
                    Score += 8;
                else if (WrongAttempts == 2)
                    Score += 5;
                else if (WrongAttempts == 3)
                    Score += 3;
                else if (WrongAttempts == 4)
                    Score += 1;

                break;
        }
    }

    public void TotalScore()
    {
        resultHandler.Stage4Score += Score;
        Score = 0;
    }

    #endregion
}