using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cerebrum;

public class UITextShower : MonoBehaviour
{
    public GameObject scoreTracker;
    public Text scoreText;
    public Text missedText;
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

        if (perfect)
            PerfectEnd();
        else if (good)
            GoodEnd();
        else if (bad)
            BadEnd();

        perfect = good = bad = false;
    }

    public void DisplayText()
    {
        missedText.text += "You missed " + totalMissed + " possible check(s)!";
    }

    /// <summary>
    /// Directly writes a perfect-score result to the end screen UI without
    /// reading from OMHTaskManager. Safe to call when no task tracking is involved.
    /// </summary>
    public void ShowPerfectDirect()
    {
        if (CompleteorFail != null)
        {
            CompleteorFail.text = "SUCCESS";
            CompleteorFail.color = Color.green;
        }

        if (scoreText != null)
            scoreText.text = "ACCURACY: 100%";

        if (missedText != null)
            missedText.text = " ";
    }

    public void CommonEnd()
    {
        missedMask = "1. Apply Oxygen Mask - can be nasal prong or non-rebreather mask.";
        spotMonitor = "2. Adjust spot monitor frequency to 15 minutes.";
        informTeam = "3. Inform medical team and escalate to registar/consultant.";
        var scorer = scoreTracker.GetComponent<OMHTaskManager>();
        int numItems = scorer.tasks.Count;
        int missed = scorer.totalIncomplete;
        int outOfOrder = scorer.totalOutOfOrder;
        string oooItemString = outOfOrder == 1 ? " item was" : " items were";
        string missedItemString = missed == 1 ? " possible item!" : " possible items!";
        int percentCorrect = (int)scorer.percentCorrect;

        scoreText.text = "ACCURACY: " + percentCorrect + "%";
        if (percentCorrect == 100)
            missedText.text = " ";
        else
            missedText.text = "You should have done:\n" + missedMask + "\n" + spotMonitor + "\n" + informTeam;
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
