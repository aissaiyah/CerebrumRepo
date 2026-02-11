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
    public int totalMissed, totalCorrect, totalIncorrect;
    public bool perfect, good, bad;
    public bool missesCounted = false;
    public bool dummypoint = false;
   
    void Start()
    {
        
    }

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
//        return;
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
        missedText.text = "You correctly answered " + scorer.totalCorrect 
        + " out of a total of  " + numItems + " items. " + outOfOrder
        + oooItemString + " answered out of order and you"
        + " missed answering " + missed + missedItemString;

    }
    public void PerfectEnd()
    {
        CommonEnd();
        CompleteorFail.text = "End of stage -Pass";
        CompleteorFail.color = Color.green;
        missedText.text = "Click END to go back to the main page to review your results or continue with the next stage";
    }

    public void GoodEnd()
    {
        CommonEnd();
        CompleteorFail.text = "End of stage -Pass";
        CompleteorFail.color = Color.green;
        missedText.text = "Click END to go back to the main page to review your results or continue with the next stage";
    }

    public void BadEnd()
    {
        CommonEnd();
        CompleteorFail.text = "End of stage -Fail";
        CompleteorFail.color = Color.red;
        missedText.text = "Click END to go back to the main page to review your results or replay this stage";
    }
}
