using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using HighlightPlus;
using System.Collections;
using GameCreator.Core;
using TMPro;

public class Stage2Handler : MonoBehaviour
{
    #region Private Variables
    
    ResultHandler resultHandler;

    //HighlightEffect highlight;

    [HideInInspector]
    public AudioSource SFX;

    [HideInInspector]
    public float timeTotal, timePrompt, timeCallHelp, timeStep1, timeStep2, timeStep3;
    float numberSpO2;
    float numberRR;
    float numberHR;
    float numberSystolic;
    float numberDiastolic;
    float numberMAP;
    float suctionGauge;

    [HideInInspector]
    public int playback;
    [HideInInspector]
    public int numberPrompts, numberWrongOrders, numberWrongItems, itemCount;
    [HideInInspector]
    public int itemNumber; // 0 for wrong item, 1 for O2 Apparatus, 2 for Suction, 3 for IV access
    [HideInInspector]
    public int reassessPatient, itemChosen, itemChosenMCQ, itemRight, preparationPatient, updateDoctor;

    [HideInInspector]
    public bool startStep1, startStep2, startStep3;
    [HideInInspector]
    public bool calledHelp, proceedItem, doneSuction, doneBringing, doneInform, doneMask, haventSuction, haventO2Mask, pairingSuction, pairing02Mask;
    [HideInInspector]
    public bool repositionPatient, changeProbeSide, bedMove, itemChosenMCQRight, startSuction, startO2Mask;
    [HideInInspector]
    public bool isDraggingSBAR, submitReady;
    [HideInInspector]
    public bool[] itemPairing;
    [HideInInspector]
    public bool[] SBARPlaced; // 0 for wrong answer, 1 for Situation, 2 for Background, 3 for Assessment, 4 for Recommendation
    bool onetime, started;
    bool[] increaseNumberDevice, dynamicNumberDevice; // 0 for nothing, 1 for SpO2, 2 for RR, 3 for HR
    [HideInInspector]
    public bool tutorial;

    #endregion

    #region Public Variables

    [Header("For API MCQ")]
    public APIHandler apiHandler;
    public ActorData __actor;
    public VerbData __verb;
    public ObjectData __objectData;
    public ContextDataSBARSubmit __context;
    public ResultData __success;
    public int API_SBARchosen = 0;
    public string[] API_SBARoption;
    public string[] API_SBARoptions;

    [Header("For API Preparation")]
    public ActorData _actor;
    public VerbData _verb;
    public ObjectData _objectData;
    public ContextDataOptions _context;

    [Header("For Sending Logs - SubmitSBAR")]
    public ActorData ___actor;
    public VerbData ___verb;
    public ObjectData ___objectData;
    public ContextDataSBARSubmit ___context;
    public ResultData ___success;
    public int ___API_SBARchosen = 0;
    public string[] ___API_SBARoption;
    public string[] ___API_SBARoptions;
    
    [Header("For Camera Panning")]
    //public CameraFollow cameraFolllow;

    [Header("For Fade In and Fade Out")]
    public GameObject fader;

    [Header("For Intro Title")]
    public RectTransform imageTitle;
    public RectTransform imageSubTitle;
    public RectTransform imageOutro;
    public GameObject buttonNextStage;
    //public GameObject buttonSummary;

    [Header("For Patient's Bed")]
    public Transform oribedRig2;
    public Transform oribedRig3;
    public Transform bedRig2;
    public Transform bedRig3;
    public float speedbed2;
    public float speedbed3;

    [Header("For Item Choose Scenario")]
    public RectTransform imagePrompt;
    public Text numberItemChosen1;
    public Text numberItemChosen2;
    
    [Header("For Item Table Scenario")]
    public SkinnedMeshRenderer blendShapePatient;
    public SkinnedMeshRenderer blendShapePatient_30;
    public GameObject Mask;
    public GameObject Tube;
    public GameObject IV;
    public GameObject Suction;
    /*
    public HighlightEffect effectMask1;
    public HighlightEffect effectMask2;
    public HighlightEffect effectSuction1;
    public HighlightEffect effectSuction2;
    public HighlightTrigger effectTriggerMask1;
    public HighlightTrigger effectTriggerMask2;
    public HighlightTrigger effectTriggerSuction1;
    public HighlightTrigger effectTriggerSuction2;*/

    public BoxCollider colliderMask1;
    public BoxCollider colliderMask2;
    public BoxCollider colliderSuction1;
    public BoxCollider colliderSuction2;
    public BoxCollider colliderNurse1;
    public GameObject itemTable;

    [Header("For Suctioning Scenario")]
    public GameObject cursor;
    public GameObject targetCursor;
    public float step;
    public GameObject maskBed;
    public GameObject maskBed30;
    public GameObject maskPatient;
    public GameObject maskPatient30;
    public GameObject canvasPreparingEquipments;
    public Animator animChild;
    
    [Header("For SBAR Scenario")]
    public GameObject buttonSBAR;
    public GameObject[] SBARImagesDrag;

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
    public GameObject buttonStart;
    public GameObject buttonCheckMonitorStart;
    public GameObject canvasBeforeCallingNurse;
    public RectTransform imageBackground;
    public RectTransform imageInstruction;
    public Text textInstruction;
    public RectTransform imageWrongPrompt2;
    public Text textWrongPrompt2;
    public RectTransform imagePrompt2;
    public Text textPrompt;
    public GameObject imageGauge;
    public Text textGauge;
    public GameObject buttonCheckOtherItems;
    public GameObject buttonBackToPatient;
    public GameObject buttonUpdateDoctor;
    public GameObject buttonUpdateDoctorParent;
    public GameObject buttonIV;
    public GameObject buttonChangeProbe;
    public GameObject buttonReposition;
    public TextMeshProUGUI textHistoryStage1;
    public TextMeshProUGUI textHistoryStage2;
    public RectTransform buttonBawahQuickFix;
    public RectTransform buttonKananQuickFix;
    public RectTransform buttonAtasQuickFix;
    public RectTransform buttonKiriQuickFix;
    public GameObject buttonOxygenPairing;
    public GameObject buttonSuctionPairing;

    [Header("For Audio")]
    public AudioClip audioCorrect;
    public AudioClip audioWrong;
    public AudioClip audioUI;

    #endregion

    void Start()
    {
        resultHandler = GameObject.Find("Singleton - Result Handler").GetComponent<ResultHandler>();

        textHistoryStage1.text = resultHandler.HistoryStage1;

        SFX = GetComponent<AudioSource>();

        PlaybackSetup();

        ////LeanTween.alpha(fader, 0f, 5f).setDelay(0.75f);//.setOnComplete(StartGame);
        Invoke("StartGameTitle", 0.5f);

        Invoke("CameraFollow", 1.5f);
    }

    void Update()
    {
        
        if (bedMove)
        {
            oribedRig2.position = Vector3.MoveTowards(oribedRig2.position, bedRig2.position, speedbed2);
            oribedRig3.position = Vector3.MoveTowards(oribedRig3.position, bedRig3.position, speedbed3);
        }

        if (doneSuction)
        {
            //canvasPreparingEquipments.SetActive(true);

            if (doneInform && preparationPatient == 2)
            {
                //LeanTween.alpha(imageInstruction, 0f, 1f);

                //send api
                apiHandler.typeAPI = 2;
                apiHandler.actor = _actor;
                apiHandler.verb = _verb;
                apiHandler.objectData = _objectData;
                apiHandler.contextOptions = _context;

                apiHandler.SendAPI();

                itemTable.SetActive(false);
                canvasPreparingEquipments.SetActive(true);

                //buttonIV.SetActive(true);
                //buttonUpdateDoctor.SetActive(true);
                //Invoke("InvokeButtonCallDoctor", 1.5f);
                colliderNurse1.enabled = false;
                canvasBeforeCallingNurse.SetActive(false);
                buttonChangeProbe.SetActive(false);
                buttonReposition.SetActive(false);
                //buttonBackToPatient.SetActive(false);

                doneSuction = false;
            }
        }

        #region For Increasing Number Vital Signs

        if (increaseNumberDevice[1])
        {
            numberSpO2 -= Time.deltaTime * 1 / 3;
            textnumberSpO2.text = numberSpO2.ToString("N0");
            textnumberSpO2Canvas.text = numberSpO2.ToString("N0");

            if (numberSpO2 <= 90)
            {
                increaseNumberDevice[1] = false;
                //dynamicNumberDevice[1] = true;
            }
        }

        if (increaseNumberDevice[2])
        {
            numberRR += Time.deltaTime * 2 / 3;
            textnumberRR.text = numberRR.ToString("N0");
            textnumberRRCanvas.text = numberRR.ToString("N0");

            if (numberRR >= 60)
                increaseNumberDevice[2] = false;
        }

        if (increaseNumberDevice[3])
        {
            numberHR += Time.deltaTime * 3 / 2;
            textnumberHR.text = numberHR.ToString("N0");
            textnumberHRCanvas.text = numberHR.ToString("N0");

            if (numberHR >= 160)
            {
                increaseNumberDevice[3] = false;
                //dynamicNumberDevice[2] = true;
            }
        }

        if (increaseNumberDevice[5])
        {
            numberDiastolic -= Time.deltaTime * 3 / 2;
            textnumberDiastolic.text = numberDiastolic.ToString("N0");
            textnumberDiastolicCanvas.text = numberDiastolic.ToString("N0");

            if (numberDiastolic <= 50)
                increaseNumberDevice[5] = false;
        }

        if (increaseNumberDevice[6])
        {
            numberSystolic -= Time.deltaTime * 4 / 3;
            textnumberSystolic.text = numberSystolic.ToString("N0");
            textnumberSystolicCanvas.text = numberSystolic.ToString("N0");

            if (numberSystolic <= 75)
                increaseNumberDevice[6] = false;
        }

        if (increaseNumberDevice[7])
        {
            numberMAP -= Time.deltaTime * 6 / 5;
            textnumberMAP.text = numberMAP.ToString("N0");
            textnumberMAPCanvas.text = numberMAP.ToString("N0");

            if (numberMAP <= 58)
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
            numberSpO2 = Random.Range(89, 92);
            textnumberSpO2.text = numberSpO2.ToString("N0");
            textnumberSpO2Canvas.text = numberSpO2.ToString("N0");

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DynamicNumberDeviceHR()
    {
        while (true)
        {
            numberHR = Random.Range(160, 164);
            textnumberHR.text = numberHR.ToString("N0");
            textnumberHRCanvas.text = numberHR.ToString("N0");

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DynamicNumberDeviceMAP()
    {
        while (true)
        {
            numberMAP = Random.Range(57, 59);
            textnumberMAP.text = numberMAP.ToString("N0");
            textnumberMAPCanvas.text = numberMAP.ToString("N0");
            numberSystolic = Random.Range(50, 53);
            textnumberSystolic.text = numberDiastolic.ToString("N0");
            textnumberSystolicCanvas.text = numberDiastolic.ToString("N0");
            numberDiastolic = Random.Range(73, 76);
            textnumberDiastolic.text = numberSystolic.ToString("N0");
            textnumberDiastolicCanvas.text = numberSystolic.ToString("N0");

            yield return new WaitForSeconds(1f);
        }
    }

    #region Public Functions

    public void FinishedStartGameTitle()
    {
        started = true;

        //LeanTween.alpha(imageTitle, 0f, 1f).setDelay(3f);//.setOnComplete(StartGame);
        //LeanTween.alpha(imageSubTitle, 0f, 1f).setDelay(2.5f);
    }

    public void IncreaseNumber()
    {
        for (int i = 1; i < increaseNumberDevice.Length; i += 1)
        {
            increaseNumberDevice[i] = true;
        }
    }

    public void ResetTimePrompt()
    {
        timePrompt = 0;
    }

    public void MCQSubmit()
    {
        if (!itemChosenMCQRight)
        {
            __success.success = false;
            API_MCQ();

            SFX.clip = audioWrong;
            SFX.Play();
        }
        else if (itemChosenMCQRight)
        {
            __success.success = true;
            API_MCQ();

            SFX.clip = audioCorrect;
            SFX.Play();
        }

        API_SBARchosen = 0;
    }

    public void ItemSubmit()
    {
        if (itemChosen > 4 || itemRight < 4)
        {
            __success.success = false;
            API_MCQ();

            API_SBARchosen = 0;
            itemChosen = 0;
            itemRight = 0;

            SFX.clip = audioWrong;
            SFX.Play();
        }
        else if (itemRight == 4)
        {
            __success.success = true;
            API_MCQTRUE();

            SFX.clip = audioCorrect;
            SFX.Play();

            proceedItem = true;
        }
    }

    public void API_MCQ()
    {
        API_SBARoptions = new string[API_SBARchosen];
        for (int i = 0; i < API_SBARchosen; i += 1)
        {
            API_SBARoptions[i] = API_SBARoption[i];
        }

        __context.extensions.options = API_SBARoptions;

        //send api
        apiHandler.typeAPI = 6;
        apiHandler.actor = __actor;
        apiHandler.verb = __verb;
        apiHandler.objectData = __objectData;
        apiHandler.contextSBARSubmit = __context;
        apiHandler.resultData = __success;
        //apiHandler.resultData.success = false;

        apiHandler.SendAPI();
    }

    public void API_MCQTRUE()
    {
        API_SBARoptions = new string[API_SBARchosen];
        for (int i = 0; i < API_SBARchosen; i += 1)
        {
            API_SBARoptions[0] = "SuctionApp";
            API_SBARoptions[1] = "SuctionCath";
            API_SBARoptions[2] = "O2Mask";
            API_SBARoptions[3] = "O2Flowmeter";
        }

        __context.extensions.options = API_SBARoptions;

        //send api
        apiHandler.typeAPI = 6;
        apiHandler.actor = __actor;
        apiHandler.verb = __verb;
        apiHandler.objectData = __objectData;
        apiHandler.contextSBARSubmit = __context;
        apiHandler.resultData = __success;

        apiHandler.SendAPI();
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

    public void CheckItem()
    {
        switch (itemNumber)
        {
            case 0:
                SFX.clip = audioWrong;
                SFX.Play();

                break;
            case 1:
                SFX.clip = audioCorrect;
                SFX.Play();

                reassessPatient++;

                //highlight = Mask.GetComponent<HighlightEffect>();
                //highlight.highlighted = true;

                Destroy(Mask.GetComponent<BoxCollider>());

                break;
            case 2:
                SFX.clip = audioCorrect;
                SFX.Play();

                reassessPatient++;

                //highlight = Tube.GetComponent<HighlightEffect>();
                //highlight.highlighted = true;

                Destroy(Tube.GetComponent<BoxCollider>());
                //Tube.SetActive(false);

                break;
            case 3:
                SFX.clip = audioCorrect;
                SFX.Play();

                reassessPatient++;

                //highlight = IV.GetComponent<HighlightEffect>();
                //highlight.highlighted = true;

                Destroy(IV.GetComponent<BoxCollider>());
                //IV.SetActive(false);

                break;
        }
    }

    public void CheckItemPairing()
    {/*
        effectMask1.highlighted = false;
        effectMask2.highlighted = false;
        effectSuction1.highlighted = false;
        effectSuction2.highlighted = false;

        effectTriggerMask1.enabled = true;
        effectTriggerMask2.enabled = true;
        effectTriggerSuction2.enabled = true;
        effectTriggerSuction1.enabled = true;
*/
        if (itemPairing[1] && itemPairing[2] && haventO2Mask && pairing02Mask)
        {
            SFX.clip = audioCorrect;
            SFX.Play();

            startO2Mask = true;
            pairing02Mask = false;
        }
        else if (itemPairing[3] && itemPairing[4] && haventSuction && pairingSuction)
        {
            SFX.clip = audioCorrect;
            SFX.Play();

            startSuction = true;
            pairingSuction = false;
        }
        else
        {
            SFX.clip = audioWrong;
            SFX.Play();
        }

        itemPairing[1] = false;
        itemPairing[2] = false;
        itemPairing[3] = false;
        itemPairing[4] = false;
    }

    public void PrepareSuction()
    {
        //animChild.SetBool("Suction", true);
        animChild.speed = 0;

        blendShapePatient.SetBlendShapeWeight(2, 100); //34
        //blendShapePatient_30.SetBlendShapeWeight(22, 100);

        textInstruction.text = "Hold the Suction Catherer to the Patient's Mouth";
        //LeanTween.alpha(imageInstruction, 1f, 1.5f);

        //imageGauge.SetActive(true);
    }

    public void EndSuction()
    {
        //animChild.SetBool("Suction", false);
        animChild.speed = 1;

        textPrompt.text = "Secretion is Cleared";
        ShowPrompt2();

        //blendShapePatient.SetBlendShapeWeight(22, 0);
        //blendShapePatient_30.SetBlendShapeWeight(22, 0);

        if (doneMask)
        {
            maskBed.SetActive(false);
            maskBed30.SetActive(false);
            maskPatient.SetActive(true);
            //maskPatient30.SetActive(true);
        }

        ////LeanTween.alpha(imageInstruction, 0f, 1.5f);

        colliderMask1.enabled = false;
        colliderMask2.enabled = false;
        colliderSuction1.enabled = false;
        colliderSuction2.enabled = false;

        doneSuction = true;
        haventO2Mask = true;
        haventSuction = false;

        preparationPatient++;
        //buttonUpdateDoctor.SetActive(true);
        //buttonCheckOtherItems.SetActive(true);

        Suction.SetActive(false);
        imageGauge.SetActive(false); //cubeMouth
        //Destroy(buttonCheckOtherItems);
        buttonUpdateDoctorParent.SetActive(true);
        canvasPreparingEquipments.SetActive(true);

        if (preparationPatient != 2)
        {
            //buttonBackToPatient.SetActive(true);
        }

        textHistoryStage2.text += "Oral Suctioning was done with thick secretion. ";
    }

    public void IncreaseSuction()
    {
        suctionGauge += Time.deltaTime * 9;
        //textGauge.text = suctionGauge.ToString("N0") + "%";

        if (suctionGauge > 5)
        {
            suctionGauge = -100;
            timePrompt = 0;

            SFX.clip = audioCorrect;
            SFX.Play();

            EndSuction();
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

            textPrompt.text = "Are You Sure This Is the Right Report?";
            //LeanTween.alpha(imagePrompt, 1f, 1f).setOnComplete(FinishedPrompt);

            //submitCount++;
            numberPrompts++;

            ResetSBAR();
        }
    }

    public void API_SBAR()
    {
        ___API_SBARoptions = new string[___API_SBARchosen];
        for (int i = 0; i < ___API_SBARchosen; i += 1)
        {
            ___API_SBARoptions[i] = ___API_SBARoption[i];
        }

        ___context.extensions.options = ___API_SBARoptions;

        //send api
        apiHandler.typeAPI = 6;
        apiHandler.actor = ___actor;
        apiHandler.verb = ___verb;
        apiHandler.objectData = ___objectData;
        apiHandler.contextSBARSubmit = ___context;
    }

    public void GoToStage3()
    {
        //apiHandler.FinishScorm();

        resultHandler.Stage2TotalTime = timeTotal;
        resultHandler.Stage2TimeStep1 = timeStep1;
        resultHandler.Stage2TimeStep2 = timeStep2;
        resultHandler.Stage2TimeStep3 = timeStep3;

        resultHandler.Stage2PromptsCount = numberPrompts;

        resultHandler.Stage2ChangeProbeSide = changeProbeSide;
        if (changeProbeSide)
        {
            resultHandler.Stage2Score += 5;
        }

        resultHandler.Stage2RepositionPatient = repositionPatient;
        if (repositionPatient)
        {
            resultHandler.Stage2Score += 5;
        }

        resultHandler.Stage2TimeCallForHelp = timeCallHelp;
        
        resultHandler.Stage2WrongOrders = numberWrongOrders;
        resultHandler.Stage2WrongItems = numberWrongItems;

        resultHandler.HistoryStage2 = textHistoryStage2.text;

        //LeanTween.alpha(imageOutro, 1f, 2f);
        //LeanTween.alpha(fader, 1f, 2f);
        Invoke("ChangeScene", 2f);
    }

    public void GoToStage3NEW()
    {
        SceneManager.LoadScene("Stage3");
    }

    public void PlaybackEnd1()
    {
        if (repositionPatient && changeProbeSide)
        {
            canvasBeforeCallingNurse.SetActive(false);

            //LeanTween.alpha(fader, 1f, 2f).setOnComplete(ChangeScene2);
        }
    }

    public void PlaybackEnd2()
    {
        if(doneBringing && doneInform)
        return;
            //LeanTween.alpha(fader, 1f, 2f).setOnComplete(ChangeScene2);
    }

    public void PlaybackEnd3()
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
        playback = resultHandler.Stage2Playback;

        if (playback == 0)
        {
            timePrompt = 0; timeCallHelp = 0; timeStep1 = 0; timeStep2 = 0; timeStep3 = 0;
            numberSpO2 = 94;
            numberRR = 50;
            numberHR = 140;
            numberDiastolic = 55;
            numberSystolic = 80;
            numberMAP = 63;
            suctionGauge = 0;

            numberPrompts = 0; numberWrongOrders = 0; numberWrongItems = 0;

            reassessPatient = 2; itemChosen = 0; itemRight = 0; preparationPatient = 0; updateDoctor = 0;

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
            itemPairing = new bool[5];
            for (int i = 1; i < itemPairing.Length; i += 1)
            {
                itemPairing[i] = false;
            }
            SBARPlaced = new bool[5];
            for (int i = 1; i < SBARPlaced.Length; i += 1)
            {
                SBARPlaced[i] = false;
            }
            calledHelp = false; proceedItem = false; doneSuction = false; doneBringing = false; doneInform = true; doneMask = false;
            repositionPatient = false; changeProbeSide = false; onetime = false; started = false; bedMove = false;
            startStep1 = false; startStep2 = false; startStep3 = false;
            haventSuction = true; haventO2Mask = true;
        }
        else if (playback == 1)
        {
            timePrompt = 0; timeCallHelp = 0; timeStep1 = 0;
            numberSpO2 = 94;
            numberRR = 50;
            numberHR = 140;
            numberDiastolic = 55;
            numberSystolic = 80;
            numberMAP = 63;

            reassessPatient = 0; itemChosen = 0; itemRight = 0;

            increaseNumberDevice = new bool[8];
            for (int i = 1; i < increaseNumberDevice.Length; i += 1)
            {
                increaseNumberDevice[i] = false;
            }

            calledHelp = false; proceedItem = false; doneSuction = false; doneBringing = false; doneInform = false;
            repositionPatient = false; changeProbeSide = false; onetime = false; started = true;
            startStep1 = true; startStep2 = false; startStep3 = false;
            //nurse matiin collider
        }
        else if (playback == 2)
        {
            timeCallHelp = 0; timePrompt = 0; timeCallHelp = 0; timeStep2 = 0;

            reassessPatient = 0; itemChosen = 0; itemRight = 0;

            increaseNumberDevice = new bool[8];
            for (int i = 1; i < increaseNumberDevice.Length; i += 1)
            {
                increaseNumberDevice[i] = true;
            }

            calledHelp = true; proceedItem = false; doneSuction = false; doneBringing = false; doneInform = false;
            onetime = false; started = true;
            startStep1 = false; startStep2 = true; startStep3 = false;
        }
        else if (playback == 3)
        {
            timeCallHelp = 0; timePrompt = 0; timeCallHelp = 0; timeStep3 = 0;
            suctionGauge = 0;

            reassessPatient = 0; itemChosen = 0; itemRight = 0;

            increaseNumberDevice = new bool[8];
            for (int i = 1; i < increaseNumberDevice.Length; i += 1)
            {
                increaseNumberDevice[i] = true;
            }

            calledHelp = true; proceedItem = false; doneSuction = false; doneBringing = true; doneInform = true;
            onetime = false; started = true;
            startStep1 = false; startStep2 = false; startStep3 = true;
            //item table pasang, nurse matiin collider, nursenya posisiin
        }
    }

    void StartGameTitle()
    {
        //LeanTween.alpha(imageTitle, 0.9f, 1.5f);
        //LeanTween.alpha(imageSubTitle, 0.9f, 1.5f).setDelay(1f).setOnComplete(StartGameTitle2);

        //Invoke("FinishedStartGameTitle", 1.5f);
    }

    void StartGameTitle2()
    {
        buttonStart.SetActive(true);
    }

    void FinishedPrompt()
    {
        //LeanTween.alpha(imagePrompt2, 0f, 1f).setDelay(2f);
    }

    void StartGame()
    {
        textInstruction.text = "Check the Patient and Vital Signs!";
        //LeanTween.alpha(imageInstruction, 1f, 1f);//.setOnComplete(UnshowPrompt);

        buttonCheckMonitorStart.SetActive(true);
    }

    public void InvokeButtonCallDoctor()
    {
        buttonUpdateDoctor.SetActive(true);
        buttonBackToPatient.SetActive(false);
        buttonOxygenPairing.SetActive(false);
        buttonSuctionPairing.SetActive(false);
        //LeanTween.alpha(buttonUpdateDoctor.GetComponent<RectTransform>(), 1f, 0.5f);//.setOnComplete(QuickFix);
    }

    public void UnInvokeCallDoctor()
    {
        //LeanTween.alpha(buttonUpdateDoctor.GetComponent<RectTransform>(), 0f, 0.75f).setOnComplete(QuickFix);
    }

    void QuickFix()
    {
        buttonUpdateDoctor.SetActive(false);
        /*
        //LeanTween.alpha(buttonAtasQuickFix, 0f, 0f);
        //LeanTween.alpha(buttonKiriQuickFix, 0f, 0f);
        //LeanTween.alpha(buttonKananQuickFix, 0f, 0f);
        //LeanTween.alpha(buttonBawahQuickFix, 0f, 0f);
        */
    }

    void ResetSBAR()
    {
        ___API_SBARchosen = 0;

        for (int i = 1; i < SBARPlaced.Length; i += 1)
        {
            SBARPlaced[i] = false;
        }

        for (int i = 0; i < SBARImagesDrag.Length; i += 1)
        {
//            SBARImagesDrag[i].GetComponent<SBARHandler_DragStage2>().ResetAll();
        }
    }

    void ShowPrompt()
    {
        //LeanTween.alpha(imagePrompt, 1f, 0.5f).setOnComplete(UnshowPrompt);

        itemChosen = 0;
        itemRight = 0;
    }

    void ShowPrompt2()
    {
        //LeanTween.alpha(imagePrompt2, 1f, 0.5f).setOnComplete(UnshowPrompt2);

        itemChosen = 0;
        itemRight = 0;
    }

    void UnshowPrompt()
    {
        //LeanTween.alpha(imagePrompt, 0f, 0.5f).setDelay(1f);
        //LeanTween.alpha(imagePrompt2, 0f, 0.5f).setDelay(1f);
        //LeanTween.alpha(imageInstruction, 0f, 1f).setDelay(2f);
    }
    void UnshowPrompt2()
    {
        //LeanTween.alpha(imagePrompt, 0f, 0.5f).setDelay(1f);
        //LeanTween.alpha(imagePrompt2, 0f, 0.5f).setDelay(1f);
    }

    void ChangeScene()
    {
        //SceneManager.LoadScene("Stage3");
        buttonNextStage.SetActive(true);
        //buttonSummary.SetActive(true);
    }

    void ChangeScene2()
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

            case 3: //pairing
                if (WrongAttempts <= 2)
                    Score += 10;
                else if (WrongAttempts <= 4)
                    Score += 5;
                else if (WrongAttempts <= 6)
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
        resultHandler.Stage2Score += Score;
        Score = 0;
    }

    #endregion
}