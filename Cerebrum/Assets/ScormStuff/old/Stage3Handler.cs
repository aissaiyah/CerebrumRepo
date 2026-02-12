using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Stage3Handler : MonoBehaviour
{
    #region Private Variables

    ResultHandler resultHandler;

    [HideInInspector]
    public AudioSource SFX;

    [HideInInspector]
    public float timeTotal, timePrompt;
    float numberSpO2;
    float numberRR;
    float numberHR;
    float numberSystolic;
    float numberDiastolic;
    float numberMAP;

    [HideInInspector]
    public int numberPrompt;
    [HideInInspector]
    public int interruptCount, wrongActions, wrongOrders, wrongItems, itemChosenMCQ, itemRight;

    [HideInInspector]
    public bool reposition, doneSuction;
    [HideInInspector]
    public bool rightItem, itemChosenMCQRight;
    [HideInInspector]
    public bool NRBmaskGiven, called5555, doneChoosingItems, onetime;
    [HideInInspector]
    public bool checkChest;
    bool started, startedChest;
    bool[] increaseNumberDevice, dynamicNumberDevice; // 0 for nothing, 1 for SpO2, 2 for RR, 3 for HR

    #endregion

    #region Public Variables

    [Header("For API")]
    public APIHandler apiHandler;
    public ActorData actor;
    public VerbData verb;
    public ObjectData objectData;
    public ContextDataSBARSubmit context;
    public ResultData success;
    public int API_SBARchosen = 0;
    public string[] API_SBARoption;
    public string[] API_SBARoptions;

    [Header("For API Preparation")]
    public ActorData _actor;
    public VerbData _verb;
    public ObjectData _objectData;
    public ContextDataOptionsAlternate _context;

    [Header("For Camera Panning")]
//    public CameraFollow cameraFolllow;

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
    public SkinnedMeshRenderer blendShapePatient;
    public SkinnedMeshRenderer blendShapePatient_30;    
    public SkinnedMeshRenderer blendShapePatientBODY;
    public SkinnedMeshRenderer blendShapePatientBODY_30;

    [Header("For Vital Signs Integration")]
    public Text textnumberSpO2;
    public Text textnumberRR;
    public Text textnumberHR;
    public Text textnumberTemp;
    public Text textnumberSystolic;
    public Text textnumberDiastolic;
    public Text textnumberMAP;
    public Text textnumberSpO2Canvas;
    public Text textnumberRRCanvas;
    public Text textnumberHRCanvas;
    public Text textnumberTempCanvas;
    public Text textnumberSystolicCanvas;
    public Text textnumberDiastolicCanvas;
    public Text textnumberMAPCanvas;

    [Header("For UI Integration")]
    public BoxCollider nurse1;
    public BoxCollider nurse2;
    public RectTransform imageWrongPrompt;
    public Text textWrongPrompt;
    public RectTransform imagePrompt;
    public Text textPrompt;
    public GameObject buttonStart;
    public GameObject canvasButtons;
    public GameObject buttonSBAR;
    public GameObject buttonNextStage;
    public RectTransform imageBackground;
    //public GameObject buttonSummary;
    public GameObject buttonVitalSigns;
    public GameObject buttonCheckChest;
    public GameObject buttonNextScene;
    public TextMeshProUGUI textHistoryStage1;
    public TextMeshProUGUI textHistoryStage2;
    public TextMeshProUGUI textHistoryStage3;

    [Header("For Choosing Items Part")]
    public RectTransform imageInstruction;
    public Text textInstruction;
    public GameObject mask;

    [Header("For Audio")]
    public AudioClip audioCorrect;
    public AudioClip audioWrong;
    public AudioClip audioUI;
    public AudioClip audioDoneSuction;

    #endregion

    void Start()
    {
        resultHandler = GameObject.Find("Singleton - Result Handler").GetComponent<ResultHandler>();

        textHistoryStage1.text = resultHandler.HistoryStage1;
        textHistoryStage2.text = resultHandler.HistoryStage2;

        SFX = GetComponent<AudioSource>();

        timeTotal = 0; timePrompt = 0;

        numberSpO2 = 90;
        numberRR = 60;
        numberHR = 160;
        numberDiastolic = 50;
        numberSystolic = 75;
        numberMAP = 58;

        interruptCount = 0; wrongActions = 0; wrongOrders = 0; wrongItems = 0;

        increaseNumberDevice = new bool[8];
        for (int i = 1; i < increaseNumberDevice.Length; i += 1)
        {
            increaseNumberDevice[i] = false;
        }
        dynamicNumberDevice = new bool[4];
        for (int i = 1; i < dynamicNumberDevice.Length; i += 1)
        {
            dynamicNumberDevice[i] = false;
        }
        reposition = true; doneSuction = true;
        rightItem = false;
        NRBmaskGiven = false; called5555 = false; doneChoosingItems = false;
        checkChest = false;
        started = false; startedChest = false;

        //LeanTween.alpha(fader, 0f, 5f).setDelay(0.75f);
        Invoke("StartGameTitle", 0.5f);

        Invoke("CameraFollow", 1.5f);

        SyncPatientPosition();
    }

    void Update()
    {
        if (startedChest)
        {
            blendShapePatientBODY.SetBlendShapeWeight(3, Mathf.PingPong(Time.time*100, 100));
            blendShapePatientBODY_30.SetBlendShapeWeight(3, Mathf.PingPong(Time.time*100, 100));
        }
        
        if (NRBmaskGiven && called5555 && doneChoosingItems)
        {
            nurse1.enabled = false;
            nurse2.enabled = false;
            buttonCheckChest.SetActive(false);
            buttonVitalSigns.SetActive(false);

            if (!onetime)
            {
                imageInstruction.gameObject.SetActive(false);

                resultHandler.HistoryStage3 = textHistoryStage3.text;

                Invoke("FinishedGame", 2f);

                //send api
                apiHandler.typeAPI = 8;
                apiHandler.actor = _actor;
                apiHandler.verb = _verb;
                apiHandler.objectData = _objectData;
                apiHandler.contextOptionsAlternate = _context;

                apiHandler.SendAPI();

                onetime = true;
            }
        }

        #region For Increase/Decrease Number Vital Signs

        if (increaseNumberDevice[1])
        {
            numberSpO2 -= Time.deltaTime / 3 * 2;
            textnumberSpO2.text = numberSpO2.ToString("N0");
            textnumberSpO2Canvas.text = numberSpO2.ToString("N0");

            if (numberSpO2 <= 85)
            {
                increaseNumberDevice[1] = false;
                //dynamicNumberDevice[1] = true;
            }
        }

        if (increaseNumberDevice[2])
        {
            numberRR -= Time.deltaTime;
            textnumberRR.text = numberRR.ToString("N0") + " BpM";
            textnumberRRCanvas.text = numberRR.ToString("N0");

            if (numberRR <= 20)
                increaseNumberDevice[2] = false;
        }

        if (increaseNumberDevice[3])
        {
            numberHR -= Time.deltaTime * 5;
            textnumberHR.text = numberHR.ToString("N0");
            textnumberHRCanvas.text = numberHR.ToString("N0");

            if (numberHR <= 100)
            {
                increaseNumberDevice[3] = false;
                //dynamicNumberDevice[2] = true;
            }
        }

        if (increaseNumberDevice[5])
        {
            numberDiastolic -= Time.deltaTime * 3 / 4;
            textnumberDiastolic.text = numberDiastolic.ToString("N0");
            textnumberDiastolicCanvas.text = numberDiastolic.ToString("N0");

            if (numberDiastolic <= 45)
                increaseNumberDevice[5] = false;
        }

        if (increaseNumberDevice[6])
        {
            numberSystolic -= Time.deltaTime * 2 / 3;
            textnumberSystolic.text = numberSystolic.ToString("N0");
            textnumberSystolicCanvas.text = numberSystolic.ToString("N0");

            if (numberSystolic <= 70)
                increaseNumberDevice[6] = false;
        }

        if (increaseNumberDevice[7])
        {
            numberMAP -= Time.deltaTime * 3 / 5;
            textnumberMAP.text = numberMAP.ToString("N0");
            textnumberMAPCanvas.text = numberMAP.ToString("N0");

            if (numberMAP <= 53)
            {
                increaseNumberDevice[7] = false;
                //dynamicNumberDevice[3] = true;
            }
        }

        #endregion

        #region Dynamic Number Vital Signs
        /*
        if (dynamicNumberDevice[1])
        {
            StartCoroutine(DynamicNumberDeviceSPO2());

            dynamicNumberDevice[1] = false;
        }

        if (dynamicNumberDevice[2])
        {
            StartCoroutine(DynamicNumberDeviceHR());

            dynamicNumberDevice[2] = false;
        }

        if (dynamicNumberDevice[3])
        {
            StartCoroutine(DynamicNumberDeviceMAP());

            dynamicNumberDevice[3] = false;
        }
        */
        #endregion
    }

    IEnumerator DynamicNumberDeviceSPO2()
    {
        while (true)
        {
            numberSpO2 = Random.Range(83, 86);
            textnumberSpO2.text = numberSpO2.ToString("N0");
            textnumberSpO2Canvas.text = numberSpO2.ToString("N0");

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DynamicNumberDeviceHR()
    {
        while (true)
        {
            numberHR = Random.Range(100, 104);
            textnumberHR.text = numberHR.ToString("N0");
            textnumberHRCanvas.text = numberHR.ToString("N0");

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DynamicNumberDeviceMAP()
    {
        while (true)
        {
            numberMAP = Random.Range(50, 52);
            textnumberMAP.text = numberMAP.ToString("N0");
            textnumberMAPCanvas.text = numberMAP.ToString("N0");
            numberSystolic = Random.Range(41, 44);
            textnumberSystolic.text = numberDiastolic.ToString("N0");
            textnumberSystolicCanvas.text = numberDiastolic.ToString("N0");
            numberDiastolic = Random.Range(69, 72);
            textnumberDiastolic.text = numberSystolic.ToString("N0");
            textnumberDiastolicCanvas.text = numberSystolic.ToString("N0");

            yield return new WaitForSeconds(1f);
        }
    }

    #region Public Functions

    public void FinishedStartGameTitle()
    {
        //LeanTween.alpha(imageTitle, 0f, 1f).setDelay(0.75f);
        //LeanTween.alpha(imageSubTitle, 0f, 1f);
    }

    public void IncreaseNumber()
    {
        started = true;

        for (int i = 1; i < increaseNumberDevice.Length; i += 1)
        {
            increaseNumberDevice[i] = true;
        }
    }

    public void StopIncreaseNumber()
    {
        for (int i = 1; i < increaseNumberDevice.Length; i += 1)
        {
            increaseNumberDevice[i] = false;
        }
    }

    public void CheckChest()
    {
        startedChest = true;
        blendShapePatient.SetBlendShapeWeight(0, 100);
        blendShapePatient_30.SetBlendShapeWeight(0, 100);
    }

    public void DoneCheckChest()
    {
        startedChest = false;
        blendShapePatientBODY.SetBlendShapeWeight(3, 0);
        blendShapePatientBODY_30.SetBlendShapeWeight(3, 0);
        blendShapePatient.SetBlendShapeWeight(0, 0);
        blendShapePatient_30.SetBlendShapeWeight(0, 0);
    }

    public void ShowBackground(bool isShow)
    {
        if (isShow)
        {
            //LeanTween.alpha(imageBackground, 1f, 1f);
        }
        else if (!isShow)
        {
            //LeanTween.alpha(imageBackground, 0f, 1f);
        }
    }

    public void DoneSuction()
    {
        SFX.clip = audioDoneSuction;
        SFX.Play();

        doneSuction = true;
    }

    public void PutMask()
    {
        //child.GetComponent<Animator>().speed = 0.6f;
        //child_30.GetComponent<Animator>().speed = 0.6f;
    }

    public void ResetTimePrompt()
    {
        timePrompt = 0;
    }

    public void MCQSubmit()
    {
        if (!itemChosenMCQRight)
        {
            success.success = false;
            API_SBAR();

            SFX.clip = audioWrong;
            SFX.Play();
        }
        else if (itemChosenMCQRight)
        {
            success.success = true;
            API_SBAR();

            SFX.clip = audioCorrect;
            SFX.Play();
        }

        API_SBARchosen = 0;
    }

    public void API_SBAR()
    {
        API_SBARoptions = new string[API_SBARchosen];
        for (int i = 0; i < API_SBARchosen; i += 1)
        {
            API_SBARoptions[i] = API_SBARoption[i];
        }

        context.extensions.options = API_SBARoptions;

        //send api
        apiHandler.typeAPI = 6;
        apiHandler.actor = actor;
        apiHandler.verb = verb;
        apiHandler.objectData = objectData;
        apiHandler.contextSBARSubmit = context;
        apiHandler.resultData = success;

        apiHandler.SendAPI();
    }

    public void CheckItem()
    {
        if (rightItem)
        {
            SFX.clip = audioCorrect;
            SFX.Play();

            //mask.SetActive(false);
        }
        else
        {
            SFX.clip = audioWrong;
            SFX.Play();

            wrongItems++;
        }
    }

    public void CheckCurrentTotalTime()
    {
        resultHandler.Stage3TotalTime = timeTotal;
        resultHandler.Stage3PromptsCount = numberPrompt;

        resultHandler.Stage3InterruptCount = interruptCount;
        resultHandler.Stage3CheckChest = checkChest;
        resultHandler.Stage3WrongActions = wrongActions;
        resultHandler.Stage3WrongOrder = wrongOrders;
        resultHandler.Stage3WrongItems = wrongItems;

        resultHandler.HistoryStage3 = textHistoryStage3.text;

        //LeanTween.alpha(imageOutro, 1f, 1f).setOnComplete(FinishedButton);
        //LeanTween.alpha(fader, 1f, 7f);

        //if (resultHandler.Stage2TotalTime + resultHandler.Stage3TotalTime > 480)
        //{
        //    Invoke("GoToScene4",7f);
        //}
        //else
        //{
            //textPrompt.text = "Congratulations!";
            ////LeanTween.alpha(imagePrompt, 1f, 1f);

            //apiHandler.FinishScorm();

            //Invoke("GoToSceneResult", 7f);
        //}
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
            textPrompt.text = "Please do something!";
            //LeanTween.alpha(imagePrompt, 0.75f, 1f).setOnComplete(FinishedPrompt);
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

        if (!resultHandler.Stage2RepositionPatient)
        {
            child.SetActive(true);
            child_30.SetActive(false);
            bed.SetActive(true);
            bed_30.SetActive(false);

            reposition = false;
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

    void FinishedButton()
    {
        //buttonSummary.SetActive(true);
        buttonNextScene.SetActive(true);
    }

    void FinishedPrompt()
    {
        //LeanTween.alpha(imagePrompt, 0f, 1f).setDelay(2f);
    }

    void FinishedGame()
    {
        nurse1.enabled = false;
        nurse2.enabled = false;

        imagePrompt.gameObject.SetActive(false);
        //LeanTween.alpha(fader, 1, 3.5f);
        //LeanTween.alpha(imageOutro, 1f, 1.5f);
        buttonNextScene.SetActive(true);
        buttonCheckChest.SetActive(false);
        buttonVitalSigns.SetActive(false);
    }

    public void GoToScene4()
    {
        SceneManager.LoadScene("Stage4");
    }

    void GoToSceneResult()
    {
        SceneManager.LoadScene("ResultScreen");
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

            case 4: //3 answer
                if (WrongAttempts == 0)
                    Score += 5;
                else if (WrongAttempts == 1)
                    Score += 1;
                else if (WrongAttempts == 2)
                    Score += 0;
                break;

            case 5: //2 answer
                if (WrongAttempts == 0)
                    Score += 3;

                break;
        }
    }

    public void TotalScore()
    {
        resultHandler.Stage3Score += Score;
        Score = 0;
    }

    #endregion
}