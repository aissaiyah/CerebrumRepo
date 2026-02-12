using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Cerebrum;

public class ScormHandler : ScormHandlerBase
{
    [Header("For Sending Logs - Phone")]
	public string nameID;
	public string learnerID;

    public APIHandler APIHandler;
    public ActorData _actor;
    public VerbData _Startedverb;
    public VerbData _Closedverb;
    public VerbData _Completedverb;
    public ObjectData _GameobjectData;
    public ObjectData _StageobjectData;
    public ObjectData _ActivityobjectData;
    public ContextDataStage _contextDataStage;
	public ResultData _result;
	public ResultMessage _resultMessage;
	public ResultRaw _resultRaw;
    public ContextDataOptionsAlternate _contextOptionsAlternate;
    public VerbData _PerformedVerb;
//    public ObjectData _PerformedObjectData;

    public int stageNumber = 1;
    public string stageName= "Stage1";
    public int taskNumber = 0;
//    public int activityName = "0";
    int counter=0;
    public bool scormstart = false;

    // Reset sets default values in inspector
    void Reset()
    {
        _actor.name = "Default"; //replaced in EditJSON
        _actor.mbox = "mailto:default@mail.com";//replaced in EditJSON
        _Startedverb.id = "http://gamestrax.com/verbs/started";
        _Closedverb.id =  "http://gamestrax.com/verbs/closed";
        _Completedverb.id =  "http://gamestrax.com/verbs/completed";

        _GameobjectData.id = "http://gamestrax.com/games/BloodTransfusion";
        _GameobjectData.definition.type = "http://gamestrax.com/define/type/game";
        _StageobjectData.id = "http://gamestrax.com/games/BloodTransfusion";
        _StageobjectData.definition.type = "http://gamestrax.com/define/type/stage";
        _ActivityobjectData.definition.type = "http://gamestrax.com/define/type/activity";

        //These change for each stage/activity
        _StageobjectData.id = "http://gamestrax.com/define/type/stages/"+stageName;
        _ActivityobjectData.id = _StageobjectData.id + "/activities/" + taskNumber;

    }

    void Awake()
    {
        if (APIHandler==null)
             APIHandler = Object.FindObjectOfType<APIHandler>();
        Reset();
    }

    // Update is just testing the calls
    void Test()
    {

        counter ++;
        List<int> foo = new List<int>(3);

        if (counter==100)
            StartGame();
        if (counter==200)
            StartStage("3");
        if (counter==300)
            PerformedAction(2,true);
        if (counter==400)
            PerformedAction(5,false);
        if (counter==500)
            EndStage(true,foo);
        if (counter==600)
            EndGame();
        if (counter==900)
            FinishGame();


    }



   public override void StartGame()
    {
            print("SH:StartGame");

            APIHandler.typeAPI = 1;
            APIHandler.actor = _actor;
            APIHandler.verb = _Startedverb;
            APIHandler.objectData = _GameobjectData;
            APIHandler.contextStage = _contextDataStage;
            APIHandler.SendAPI();
    }
   public override void EndGame()
    {

            APIHandler.typeAPI = 1;
            APIHandler.actor = _actor;
            APIHandler.verb = _Closedverb;
            APIHandler.objectData = _GameobjectData;
            APIHandler.contextStage = _contextDataStage;
            APIHandler.SendAPI();
    }
 
    public override void StartStage(string name=null)
    {
        if (name!=null)
            stageName = name;    
        _StageobjectData.id = "http://gamestrax.com/define/type/stages/"+stageName;

        APIHandler.typeAPI = 1;
        APIHandler.actor = _actor;
        APIHandler.verb = _Startedverb;
        APIHandler.objectData = _StageobjectData;
        APIHandler.contextStage = _contextDataStage;
        APIHandler.SendAPI();

    }
    public override void EndStage(bool result,List<int> tasks)
    {
//        if (name!=null)
//            stageName = name;    
        _StageobjectData.id = "http://gamestrax.com/define/type/stages/"+stageName;
            _resultRaw.success = result;

        string taskSum = "QUOTEBEFORE["; //for replace to remove quote
        int numTasks = tasks.Count;
        V2 [] taskVec = new V2[numTasks]; 
        print("numTasks=" + numTasks);
        for (int i = 0;i<numTasks;i++)
        {
            if (i>0)
                taskSum += ",";
            taskSum += "[" + i + "," + tasks[i] + "]";
        }
        taskSum += "]QUOTEAFTER"; //for replace to remove quote
        //taskSum =         "QUOTEBEFORE[[0,-1],[1,1],[2,-1],[3,-1],[4,-1],[5,-1],[6,-1],[7,-1],[8,-1]]QUOTEAFTER";
        //print("taskSum = "+ taskSum);


        _resultRaw.extensions.raw_data = taskSum;

        APIHandler.typeAPI = 12;
        APIHandler.actor = _actor;
        APIHandler.verb = _Completedverb;
        APIHandler.objectData = _StageobjectData;
        APIHandler.contextStage = _contextDataStage;
        APIHandler.resultRaw = _resultRaw;
        APIHandler.SendAPI();
    }
    public override void CloseStage()
    {
            _StageobjectData.id = "http://gamestrax.com/define/type/stages/"+stageName;

        APIHandler.typeAPI = 3;
        APIHandler.actor = _actor;
        APIHandler.verb = _Closedverb;
        APIHandler.objectData = _StageobjectData;
        APIHandler.contextStage = _contextDataStage;
        APIHandler.resultData = _result;
        APIHandler.SendAPI();
    }

    public override void PerformedAction(int task, bool result)
    {
        taskNumber = task;
        _ActivityobjectData.id = _StageobjectData.id + "/activities/" + taskNumber;
        _result.success = result;

print("APIHandler = "+ APIHandler);
        APIHandler.typeAPI = 3;
        APIHandler.actor = _actor;
        APIHandler.verb = _Completedverb;
        APIHandler.objectData = _ActivityobjectData;
        APIHandler.contextStage = _contextDataStage;
        APIHandler.resultData = _result;
//            APIHandler.contextOptionsAlternate = _contextOptionsAlternate;

        APIHandler.SendAPI();

        //oneTimeOnly = true;
            //send api
    }


    public override void FinishGame()
    {
        print("FinishGame");
        APIHandler.FinishScorm();
        //SceneManager.LoadScene("ResultScreen");
    }


}

//            taskVec[i] = new V2();// {i,tasks[i]};
//            taskVec[i].v[0] = i;
//            taskVec[i].v[1] = tasks[i];
//        int [][] taskVec =  new int [2][] {new []{4,5},new []{6,7}};
//        int[,] taskVec = new int[numTasks, 2];
//        int[,] taskVec = new int[numTasks,2];
//        _resultRaw.extensions.raw_data = taskVec;
//        var A = new int[2,2] {{1,2},{11,22}};

//        print("A="+ JsonUtility.ToJson(A));
//        print("B="+ JsonUtility.ToJson(taskVec));
//        JsonUtility.ToJson
//        print("A="+ JsonConvert.SerializeObject(A));
//        print("B="+ JsonConvert.SerializeObject(taskVec));
