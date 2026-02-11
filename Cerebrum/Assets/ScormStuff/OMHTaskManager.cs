/** OMHTaskManager.cs
 * <summary>
 * Task Manager to handle the tasks (objectives) that the player must do in order to complete the
 * requirements of a scenario. Derives its base functionality from ObjectMessageHandlerBase and
 * expands from there. Designed to give points for correctly completed tasks. Player is penalized
 * if they complete tasks out of order, so each task is given one or more prerequisites (prereqs).
 * If a task is completed without all of its prereqs, that task is marked out of order.
 * == TASK STATUS MEANING ==
 * -2 = Incomplete, and potentially out of order
 * -2 = Incomplete, and potentially out of order
 * -1 = Incomplete
 *  0 = Complete, Incorrect
 *  1 = Complete, Correct
 *  2 = Complete, Out of Order
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
    public Dictionary<int, int[]> prereqs = new Dictionary<int, int[]>();
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
        //Debug.Log("OMHTaskManager: " + gameObject.name + " STARTED");
    }

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log("OMHTaskManager: " + gameObject.name + " AWAKE");

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
        Debug.Log("OMHTaskManager got a MESSAGE: " + msg + " | " + param);
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
        ////////////////////////////////////////////////////////////////////
        // MESSAGES FOR SETTING UP THE TASKS
        ////////////////////////////////////////////////////////////////////
        else if (msg == "numitems" || msg == "numtasks")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            int numTasks = int.Parse(param);
            Debug.Log("OMHTaskManager: SET UP TASKS: " + numTasks);
            AddTasks(numTasks);
            return true;
        }
        else if (msg == "setstages" || msg == "settasks")
        {
            // This should be deprecated once all scenarios stop using hard-coded task lists - Tony
            param = GameManager.instance.ep.EvaluateParam(param);
            int sceneNum = int.Parse(param);
            currentSceneNum = sceneNum;
            if (scormHandler!=null)
                scormHandler.StartStage(currentSceneNum.ToString());
            int numTasks = numTaskItems[sceneNum];
            AddTasks(numTasks);
            Debug.Log("OMHTaskManager: SET TASKS: Scene # " + sceneNum + ", numTasks =" + numTasks);
            return true;
        }
        else if (msg == "endstage")// || msg == "settasks")
        {
            Debug.Log("OMHTaskManager: END STAGE, PercentCorrect = " + percentCorrect);
            if (scormHandler!=null)
                scormHandler.EndStage(percentCorrect>70f,tasks);
        }
        else if (msg == "closestage")// || msg == "settasks")
        {
            Debug.Log("OMHTaskManager: CLOSE STAGE");
            if (scormHandler!=null)
                scormHandler.CloseStage();
        }
        else if (msg == "setdependency" || msg == "setdependencies" || msg == "setprereq" || msg == "setprerequisite") {
            // First parameter = task # to set dependencies for
            // Second parameter = task, list of tasks, or range of tasks that act as prerequisites to the first parameter
            // For now, all tasks should be listed as space-separated list of integer values. First integer is the task #
            // to set prereqs for. All following integers are the prerequisites.
            char[] separators = new char[] { ' ', '\t', '|'};
            string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2) {
                // There have to be at least two parameter values
                return false;
            }
            for (int i=0;i< temp.Length;i++)
            {
                temp[i] = GameManager.instance.ep.EvaluateParam(temp[i]);
            }
            int taskNum = int.Parse(temp[0]);
            string[] tempStrings = new string[temp.Length - 1];
            System.Array.Copy(temp, 1, tempStrings, 0, temp.Length - 1);
            int[] tempPrereqs = new int[temp.Length - 1];
            tempPrereqs = Array.ConvertAll<string, int>(tempStrings, int.Parse);
            if (prereqs.ContainsKey(taskNum)) {
                // A new setdependency command for a task number overwrites all previous dependencies for that task number
                prereqs.Remove(taskNum);
            }
            prereqs.Add(taskNum, tempPrereqs);
            DebugConsole.print("OMHTaskManager: SET TASK DEPENDENCY FOR : " + taskNum);
            for (int i = 0; i < tempPrereqs.Length; i++) {
                DebugConsole.print("OMHTaskManager: SET TASK DEPENDENCY FOR : " + taskNum + " prereq " + i + " is: " + tempPrereqs[i]);
            }
        }
        ////////////////////////////////////////////////////////////////////
        // MESSAGES FOR SETTING TASK STATUS
        ////////////////////////////////////////////////////////////////////
        else if (msg == "incomplete" || msg == "taskincomplete")
        {
                param = GameManager.instance.ep.EvaluateParam(param);
            Debug.Log("OMHTaskManager: incomplete : " + param);
            int thisTask = int.Parse(param);
            tasks[thisTask] = -1;

            Debug.Log("OMHTaskManager Checking prereqs for : " + thisTask + " ? " + prereqs.ContainsKey(thisTask));
            if (prereqs.ContainsKey(thisTask))
            {
                foreach (int req in prereqs[thisTask])
                {
                    if (tasks[req] < -1)
                    {
                        // A prerequisite task was marked "potentially out of order"
                        // Set it back to incomplete
                        Debug.Log("OMHTaskManager TASK IS UNDONE : " + thisTask + ", SET POTENTIALLY OUT OF ORDER PREREQS BACK TO INCOMPLETE : " + req);
                        //                                tasks[thisTask] = 2;
                        tasks[req] = -1;
                        //                                return true;
                    }
                }
            }


        }
        else if (msg == "complete" || msg == "taskcorrect")
        {
            char[] separators = new char[] { ' ', '\t' };
            Debug.Log("OMHTaskManager: taskcorrect : " + param);
            string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //first param is task number
            Debug.Log("OMHTaskManager: taskcorrect temp[0]: " + temp[0]);
            if (temp.Length > 0)
                temp[0] = GameManager.instance.ep.EvaluateParam(temp[0]);
            Debug.Log("OMHTaskManager: taskcorrect temp[0]2: " + temp[0]);
            int thisTask = int.Parse(temp[0]);

            //optional 2nd param is true/false/1/0 or if not there, set to true
            string param2 = null;
            if (temp.Length > 1)
                param2 = GameManager.instance.ep.EvaluateParam(temp[1]);
            Debug.Log("OMHTaskManager: got MESSAGE : " + msg + ", TASK : " + thisTask + ", STATUS : " + param2);
            bool correct = (param2 == null || param2 == "true" || param2 == "True" || param2 == "1");
            if (thisTask > -1 && thisTask < tasks.Count)
            {

                if (correct)
                {
                    // Check that all prerequisites are completed
                    Debug.Log("OMHTaskManager Checking prereqs for : " + thisTask + " ? " + prereqs.ContainsKey(thisTask));
                    if (prereqs.ContainsKey(thisTask))
                    {
                        foreach (int req in prereqs[thisTask])
                        {
                            if (tasks[req] < 0)
                            {
                                // A prerequisite is incomplete
                                // This task is out of order
                                Debug.Log("OMHTaskManager PREREQ TASK POTENTIALLY OUT OF ORDER : " + thisTask + "HAPPENED, MISSING PREREQ : " + req);
//                                tasks[thisTask] = 2;
                                tasks[req] = -2;
//                                return true;
                            }
                        }
                    }
                    if (tasks[thisTask] > -2)
                    {
                        Debug.Log("OMHTaskManager TASK COMPLETED SUCCESSFULLY : " + thisTask);
                        tasks[thisTask] = 1;
                    }
                    else
                    {
                        Debug.Log("OMHTaskManager TASK COMPLETED SUCCESSFULLY BUT OUT OF ORDER: " + thisTask);
                        tasks[thisTask] = 2;
                    }
                }
                else
                {
                    Debug.Log("OMHTaskManager TASK INCORRECT : " + thisTask);
                    tasks[thisTask] = 0;
                }
                scormHandler.PerformedAction(thisTask,correct);
            }
            else {
                Debug.Log("OMHTaskManager TASK NUMBER OUT OF BOUNDS, THIS MAY BE EXPECTED BEHAVIOR: " + thisTask);
            }
        }

        ////////////////////////////////////////////////////////////////////
        // MESSAGES FOR TESTING TASK STATUS
        ////////////////////////////////////////////////////////////////////
        else if (msg == "iscomplete")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            Debug.Log("OMHTaskManager: iscomplete? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + (tasks[int.Parse(param)] >= 0));
            return (tasks[int.Parse(param)] >= 0);
        }
        else if(msg == "isnotcomplete")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            Debug.Log("OMHTaskManager: isnotcomplete? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + (tasks[int.Parse(param)] < 0));
            return (tasks[int.Parse(param)] < 0);
        }
        else if (msg == "taskright")
        {
            param = GameManager.instance.ep.EvaluateParam(param);
            Debug.Log("OMHTaskManager: taskright? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + (tasks[int.Parse(param)] == 1));
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
            Debug.Log("OMHTaskManager: taskwrong? raw param = " + param + ", as integer = " + int.Parse(param) + ", result = " + !(tasks[int.Parse(param)] == 1));
            if (tasks[int.Parse(param)] == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (msg == "rangecomplete")
        {
            //Check if all tasks were completed.  Returns true if they were.
            //Also computes the pointTotal inspector variable
            char[] separators = new char[] { ' ', ',', '-' };

            string[] temp = param.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            temp[0] = GameManager.instance.ep.EvaluateParam(temp[0]);
            temp[1] = GameManager.instance.ep.EvaluateParam(temp[1]);

            int start = int.Parse(temp[0]);
            int stop = int.Parse(temp[1]);
            int totalComplete = 0;

            for (int i = start; i < stop + 1; i++)
            {
                if (tasks[i] >= 0)
                {
                    totalComplete++;
                }
            }

            Debug.LogWarning("OMHTaskManager: RANGE COMPLETE CHECK REQUESTED: Total Complete from " + temp[0] + " to " + temp[1] + " = " + totalComplete);
            pointTotal = totalComplete;
            if (totalComplete == stop - start + 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (msg == "getcompletepercent")
        {
            // Start the counting from scratch in case this message is received more than once
            totalIncomplete = 0;
            totalIncorrect = 0;
            totalCorrect = 0;
            totalOutOfOrder = 0;
            int numTasks = tasks.Count;

            Debug.Log("OMHTaskManager: ========================= SCORE CARD START =================================== ");

            string taskStatus;
            for (int i = 0; i < numTasks; i++)
            {
                if (tasks[i] < 0) // incomplete or incomplete pending out of order)
                {
                    totalIncomplete++;
                    taskStatus = " - INCOMPLETE";
                }
                else if (tasks[i] == 0) // incorrect)
                {
                    totalIncorrect++;
                    taskStatus = " - INCORRECT";
                }
                else if (tasks[i] == 2) // OOO
                {
                    totalOutOfOrder++;
                    taskStatus = " - OUT OF ORDER";
                }
                else // else, this must be correct
                {
                    totalCorrect++;
                    taskStatus = " + CORRECT!!";
                }
                Debug.Log("OMHTaskManager: ======================= TASK " + i + taskStatus);
            }

            percentCorrect = totalCorrect * 100 / numTasks;
            Debug.Log("OMHTaskManager: ========================= ===== ==== ===== =================================== ");
            Debug.Log("OMHTaskManager: Correct = " + totalCorrect + " , numTasks = " + numTasks + ", % = " + percentCorrect);
            Debug.Log("OMHTaskManager: Incorrect = " + totalIncorrect + " , Incomplete = " + totalIncomplete + ", Out of Order = " + totalOutOfOrder);
            Debug.Log("OMHTaskManager: ========================= SCORE CARD END ===================================== ");
        }

        else if (msg == "allcomplete")
        {
            Debug.Log("OMHTaskManager: allcomplete? percentCorrect = " + percentCorrect);
            if (percentCorrect > 99)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (msg == "percentachieved")
        {
            Debug.Log("OMHTaskManager: percentachieved? percentCorrect = " + percentCorrect + ", param="+ param);
            if (percentCorrect > float.Parse(param))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else {
            string errmsg = "Command not found! " + msg + " for " + this.name + " with param = " + param;
            Debug.LogError(errmsg);
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