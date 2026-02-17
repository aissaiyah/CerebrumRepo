/** OMHTaskManager.cs
 * <summary>
 * Task Manager to handle the tasks (objectives) that the player must do in order to complete the
 * requirements of a scenario. Derives its base functionality from ObjectMessageHandlerBase and
 * expands from there. Designed to give points for correctly completed tasks. 
 * </summary>
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cerebrum {

public class OMHTaskManager : ObjectMessageHandlerBase
{ 
    public List<int> tasks = new List<int>();
    //public Dictionary<int, int[]> prereqs = new Dictionary<int, int[]>();
    public float pointTotal = 0;
    public float percentCorrect = 0.0f;
    public int totalIncomplete = 0;
    public int totalIncorrect = 0;
    public int totalCorrect = 0;
    public int totalOutOfOrder = 0;
    public ScormHandlerBase scormHandler;

    public int currentSceneNum=0;

    // This starts the message handler
    protected override void Start()
    {
        base.Start();
        if (scormHandler==null)
             scormHandler = UnityEngine.Object.FindObjectOfType<ScormHandlerBase>();
        //DebugConsole.print("OMHTaskManager: " + gameObject.name + " STARTED");
    }

    protected override void Awake()
    {
        base.Awake();
        //DebugConsole.print("OMHTaskManager: " + gameObject.name + " AWAKE");

    }

    void AddTasks(int numTasks)
    {
        tasks.Clear();
        for (int i = 0;i<numTasks;i++)
        {
            // Tasks are initialized to -1 to show they have not yet been attempted
            tasks.Add(-1);
        }
    }

    // This is an outdated way of setting up the number of tasks to complete and should be removed once all scenario files stop using it -Tony
    public int [] numTaskItems = { 0,13,9,8,2,12,15,16 };

    public override bool HandleMessage(string msg, string param, out string retMsg)
    {
        if (scormHandler==null)
             scormHandler = UnityEngine.Object.FindObjectOfType<ScormHandlerBase>();
        DebugConsole.print("OMHTaskManager got a MESSAGE: " + msg + " | " + param);
        retMsg = null;
        DebugConsole.print(this.name + ": OMHTaskManager: Handle Message: before OMHB.HandleMessage: " + msg + " for " + this.name + " with param = "+ param);
        bool retv = base.HandleMessage(msg,param,out retMsg);
        if (commandFound)
            return retv;
        DebugConsole.print(this.name + ": OMHTaskManager: Handle Message: after OMHB.HandleMessage: " + msg + " for " + this.name + " with param = "+ param);

        //OMHB doesn't return the evaluated parameter just in case you need to do something funky with it.
        //So... I guess we have to evaluate the parameter all over again.
        DebugConsole.print(this.name + ": OMHTaskManager: Handle Message: after EvaluateParam: " + msg + " for " + this.name + " with param = "+ param);

        /*  COMMANDS */
        ////////////////////////////////////////////////////////////////////
        // MESSAGES FOR SETTING UP THE TASKS
        ////////////////////////////////////////////////////////////////////
        if (msg == "numitems" || msg == "numtasks")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            int numTasks = int.Parse(param);
            DebugConsole.print("OMHTaskManager: SET UP TASKS: " + numTasks);
            AddTasks(numTasks);
            return true;
        }
        else if (msg == "setstage")
        {
            // This should be deprecated once all scenarios stop using hard-coded task lists - Tony
            param = GameManager.instance.ep.EvaluateParam(param);
            int sceneNum = int.Parse(param);
            currentSceneNum = sceneNum;
            if (scormHandler!=null)
                scormHandler.StartStage(currentSceneNum.ToString());
            int numTasks = numTaskItems[sceneNum];
            AddTasks(numTasks);
            DebugConsole.print("OMHTaskManager: SET TASKS: Scene # " + sceneNum + ", numTasks =" + numTasks);
            return true;
        }
        else if (msg == "endstage")// || msg == "settasks")
        {
            DebugConsole.print("OMHTaskManager: END STAGE, PercentCorrect = " + percentCorrect);
            if (scormHandler!=null)
                scormHandler.EndStage(percentCorrect>70f,tasks);
        }
        else if (msg == "closestage")// || msg == "settasks")
        {
            DebugConsole.print("OMHTaskManager: CLOSE STAGE");
            if (scormHandler!=null)
                scormHandler.CloseStage();
        }

        ////////////////////////////////////////////////////////////////////
        // MESSAGES FOR SETTING TASK STATUS
        ////////////////////////////////////////////////////////////////////
 
        //$TaskManager taskCompleted 'TaskCheckPatient"
        //takes in a string containing the name of the task, e.g. 'TaskCheckPatient', or $TaskName
        //Assumes there exists a variable with that name, and a corresponding variable with that name 
        //appended with "_Value", e.g. $TaskCheckPatient and $TaskCheckPatientValue
        else if (msg == "completed" || msg == "taskcompleted")
        {
            char[] separators = new char[] { ' ', '\t' };
            string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            DebugConsole.print("OMHTaskManager: taskcorrect temp[0]: " + temp[0]);
            if (temp.Length < 2){
                DebugConsole.print("TaskCompleted error: not enough parameters:" + param);
                return false;
            }
            string thisPatient = GameManager.instance.ep.EvaluateParam(temp[0]);                

            //DebugConsole.
            print("OMHTaskManager: patient : " + thisPatient);
            //string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //evalute to get name of task
            param = GameManager.instance.ep.EvaluateParam(temp[1]);                
            DebugConsole.print("OMHTaskManager: taskCompleted : taskName =" + param);

            //append name to make the value variable name
            string valueName = param + "_Value";

            //get the task number
            param = GameManager.instance.ep.EvaluateParam(param);
            DebugConsole.print("OMHTaskManager: got MESSAGE : " + msg + ", taskNumber = " + param);
            int thisTask = int.Parse(param);
            DebugConsole.print("OMHTaskManager: got MESSAGE : " + msg + ", TASK #: " + thisTask);// + ", taskName = " + param);

            //get the task's value
            string taskValue = null;
            taskValue = GameManager.instance.ep.EvaluateParam(valueName);
            DebugConsole.print("OMHTaskManager: got MESSAGE : " + msg + ", TASK : " + thisTask + ", valueName : " + valueName);
            DebugConsole.print("OMHTaskManager: got MESSAGE : " + msg + ", TASK : " + thisTask + ", STATUS : " + taskValue);

            //change to int
            int taskPoints = 0;
            if (taskValue != null)
            {
                DebugConsole.print("OMHTaskManager: got MESSAGE : " + msg + ", TASK : " + thisTask + ", STATUS : " + taskValue);
                taskPoints = int.Parse(taskValue);
            }

//            if (thisTask > -1 && thisTask < tasks.Count)
            {
//               tasks[thisTask] = taskPoints;
                DebugConsole.print("OMHTaskManager: PerformedAction: Task = " + thisTask + ", Points = " + taskPoints);
                scormHandler.PerformedAction(thisPatient, thisTask,taskPoints);
            }
            /*
            else 
            {
                DebugConsole.print("OMHTaskManager TASK NUMBER OUT OF BOUNDS, THIS MAY BE EXPECTED BEHAVIOR: " + thisTask);
            }*/
        }

        else {
            string errmsg = "OMHTaskManager: Command not found! " + msg + " for " + this.name + " with param = " + param;
            DebugConsole.print(errmsg);
        }
        return true;
    }

    // Update is called once per frame
    // Why would a message handler ever need to use an update function? - Tony
    protected override void Update()
    {
        //base.Update();
    }
}
} //namespace


//OLD
        ////////////////////////////////////////////////////////////////////
        // MESSAGES FOR TESTING TASK STATUS
        ////////////////////////////////////////////////////////////////////
/*
        // This should probably never be used except when testing.
        if (msg == "gain")
        {
            if (param != null)
            {
                param = GameManager.instance.ep.EvaluateParam(param);
                pointTotal += int.Parse(param);
            }
            else
            {
                pointTotal += 1;
            }
        }
        // This should probably never be used except when testing.
        else if (msg == "lose")
        {
            if (pointTotal > 0)
            {
                if (param != null)
                {
                    param = GameManager.instance.ep.EvaluateParam(param);
                    pointTotal += int.Parse(param);
                }
                else
                {
                    pointTotal += 1;
                }
            }
        }
*/
/*
        else if (msg == "iscomplete")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            DebugConsole.print("OMHTaskManager: iscomplete? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + (tasks[int.Parse(param)] >= 0));
            return (tasks[int.Parse(param)] >= 0);
        }
        else if(msg == "isnotcomplete")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            DebugConsole.print("OMHTaskManager: isnotcomplete? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + (tasks[int.Parse(param)] < 0));
            return (tasks[int.Parse(param)] < 0);
        }
        else if (msg == "taskright")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            DebugConsole.print("OMHTaskManager: taskright? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + (tasks[int.Parse(param)] == 1));
            if (tasks[int.Parse(param)] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (msg == "taskwrong")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            DebugConsole.print("OMHTaskManager: taskwrong? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + !(tasks[int.Parse(param)] == 1));
            if (tasks[int.Parse(param)] == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (msg == "complete" || msg == "taskcorrect")
        {
            char[] separators = new char[] { ' ', '\t' };
            DebugConsole.print("OMHTaskManager: taskcorrect : " + param);
            string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            DebugConsole.print("OMHTaskManager: taskcorrect temp[0]: " + temp[0]);
            if (temp.Length > 0)
                temp[0] = GameManager.instance.ep.EvaluateParam(temp[0]);
            DebugConsole.print("OMHTaskManager: taskcorrect temp[0]2: " + temp[0]);
            int thisTask = int.Parse(temp[0]);
            string param2 = null;
            if (temp.Length > 1)
                param2 = GameManager.instance.ep.EvaluateParam(temp[1]);
            DebugConsole.print("OMHTaskManager: got MESSAGE : " + msg + ", TASK : " + thisTask + ", STATUS : " + param2);
            int taskPoints = 0;
            if (param2 != null)
            {
                taskPoints = int.Parse(param);
            }

            //bool correct = (param2 == null || param2 == "true" || param2 == "True" || param2 == "1");
            if (thisTask > -1 && thisTask < tasks.Count)
            {
                tasks[thisTask] = taskPoints;
                scormHandler.PerformedAction(thisTask,taskPoints);
            }
            else 
            {
                DebugConsole.print("OMHTaskManager TASK NUMBER OUT OF BOUNDS, THIS MAY BE EXPECTED BEHAVIOR: " + thisTask);
            }
        }       
*/
