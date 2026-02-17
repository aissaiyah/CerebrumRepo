using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cerebrum;

public class UITextShower : MonoBehaviour
{
    public GameObject scoreTracker;
    public Text scoreText/*, scoreText2*/;
    public Text missedText/*, missedText2*/;
    public Text CompleteorFail;
    public string missedMask, spotMonitor, informTeam;
    public int totalMissed, totalCorrect, totalIncorrect;
    public bool perfect, good, bad;
    public bool missesCounted = false;
    public bool dummypoint = false;
  
    void Update()
    {
        if (!perfect && !good && !bad)
            return;

        if(perfect)
        {
            PerfectEnd();
        }
        else if(good)
        {
            GoodEnd();
        }
        else if(bad)
        {
            BadEnd();
        }
        else
        {
            return;
        }
        perfect = good = bad = false;
    }

    public void DisplayText()
    { 
            missedText.text += "You missed " + totalMissed + " possible check(s)!";
    }  
    
    public void CommonEnd()
    {
        missedMask = "1. Apply Oxygen Mask - can be nasal prong or non-rebreather mask.";
        spotMonitor = "2. Adjust spot monitor frequency to 15 minutes.";
        informTeam = "3. Inform medical team and escalate to registar/consultant.";
        var scorer = scoreTracker.GetComponent<OMHTaskManager>();
        int numItems = 0;
        int missed = 0;
        int outOfOrder = 0;
        string oooItemString = " items were";
        string missedItemString = " possible items!";
        /*
        if (dummypoint)
        {
             numItems = scorer.tasks.Count - 1;
            //missed = scorer.totalIncorrect - 1;
            //outOfOrder = scorer.totalOutOfOrder - 1;
        }
        else
        */
        
        numItems = scorer.tasks.Count;
        missed = scorer.totalIncomplete;
        outOfOrder = scorer.totalOutOfOrder;
        
        if (outOfOrder == 1) {
            oooItemString = " item was";
        }

        if (missed == 1) {
            missedItemString = " possible item!";
        }

        int percentCorrect = (int)scorer.percentCorrect;
        //int percentCorrect = totalCorrect *100/ numItems;

        scoreText.text = "ACCURACY: " + percentCorrect + "%";
        if(percentCorrect == 100)
        {
            missedText.text = " ";
        }
        else
        {
            missedText.text = "You should have done:" + "\n" + missedMask + "\n" + spotMonitor + "\n" + informTeam;
        }


    }
    public void PerfectEnd()
    {
        CommonEnd();
        CompleteorFail.text = "SUCCESS";
        CompleteorFail.color = Color.green;
    }

    public void GoodEnd()
    {
        CommonEnd();
        CompleteorFail.text = "SUCCESS";
        CompleteorFail.color = Color.green;
    }

    public void BadEnd()
    {
        CommonEnd();
        CompleteorFail.text = "FAILURE";
        CompleteorFail.color = Color.red;
    }
}
