using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    #region Variables

    [Header("For SCORM")]
    public bool scormstart;
    public string nameID;
    public string learnerID;

    [Header("Stage 1 Results")]
    public float Stage1TotalTime;
    public float Stage1TimeStep1;
    public float Stage1TimeStep2;
    public int Stage1PromptsCount;
    public int Stage1SubmitCount;
    public int Stage1Score;
    //[HideInInspector]
    public int Stage1Playback;
    
    [Header("Stage 2 Results")]
    public float Stage2TotalTime;
    public float Stage2TimeStep1;
    public float Stage2TimeStep2;
    public float Stage2TimeStep3;
    public int Stage2PromptsCount;
    public bool Stage2RepositionPatient;
    public bool Stage2ChangeProbeSide;
    public float Stage2TimeCallForHelp;
    public int Stage2WrongOrders;
    public int Stage2WrongItems;
    public int Stage2Score;
    //[HideInInspector]
    public int Stage2Playback;

    [Header("Stage 3 Results")]
    public float Stage3TotalTime;
    public int Stage3PromptsCount;
    public int Stage3InterruptCount;
    public bool Stage3CheckChest;
    public int Stage3WrongActions;
    public int Stage3WrongOrder;
    public int Stage3WrongItems;
    public int Stage3Score;
    //[HideInInspector]
    public int Stage3Playback;

    [Header("Stage 4 Results")]
    public float Stage4TotalTime;
    public int Stage4PromptsCount;
    public int Stage4CPRAttempts;
    public int Stage4WrongItems;
    public int Stage4SubmitCount;
    public int Stage4Score;
    //[HideInInspector]
    public int Stage4Playback;

    #endregion

    [Header("For History")]
    public string HistoryStage1;
    public string HistoryStage2;
    public string HistoryStage3;
    public string HistoryStage4;
}