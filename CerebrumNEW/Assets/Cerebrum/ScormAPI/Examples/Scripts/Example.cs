using Scorm;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour {

    private IScormService scormService;

    public class SuspendDataTest {

        public string name;

        public string lastname;

        public SuspendDataTest(string name, string lastname) {
            this.name = name;
            this.lastname = lastname;
        }
    }

    public Transform buttonsContainer;

    public Button initButton;

    public Button genericGetValueButton;

    public Button genericSetValueButton;

    public Button getLearnerIdButton;

    public Button getLearnerNameButton;

    public Button getLessonLocationButton;

    public Button setLessonLocationButton;

    public Button getCreditButton;

    public Button getLessonStatusButton;

    public Button setLessonStatusButton;

    public Button getEntryButton;

    public Button getRawScoreButton;

    public Button setRawScoreButton;

    public Button getMaxScoreButton;

    public Button setMaxScoreButton;

    public Button getMinScoreButton;

    public Button setMinScoreButton;

    public Button getTotalTimeButton;

    public Button getLessonModeButton;

    public Button setSessionTimeButton;

    public Button getCommentsButton;

    public Button getCommentsFromLmsButton;

    public Button getLanguageButton;

    public Button getSuspendDataButton;

    public Button setSuspendDataButton;

    public Button getObjectivesButton;

    public Button setObjectiveButton;

    public Button setExitReasonButton;

    public Button finishButton;

    private List<Button> buttons;

    private float startTime;

    public delegate void LogHandler(string text);
    public event LogHandler OnMessageLogged;

    void Awake() {
#if UNITY_EDITOR
        scormService = new ScormPlayerPrefsService();
#else
        scormService = new ScormService();
#endif

        initButton.interactable = true;

        buttons = buttonsContainer.GetComponentsInChildren<Button>(true).ToList();

        ToggleInitialized(false);

        SetListeners();

        initButton.onClick.AddListener(() => {
            bool result = scormService.Initialize(Scorm.Version.Scorm_1_2);

            if (result) {
                Log("Communication initialized");

                initButton.interactable = false;

                ToggleInitialized(true);

                startTime = Time.time;
            } else {
                Log("There was an error during initialization");
            }
        });
    }

    void SetListeners() {
        genericGetValueButton.onClick.AddListener(() => {
            string value01 = null;
            int value02 = 0;
            float value03 = 0;

            switch (scormService.Version) {
                case Scorm.Version.Scorm_1_2:
                    value01 = scormService.GetStringValue("cmi.suspend_data");
                    value02 = scormService.GetIntValue("cmi.core.score.max");
                    value03 = scormService.GetFloatValue("cmi.core.score.raw");
                    break;
                case Scorm.Version.Scorm_2004:
                    value01 = scormService.GetStringValue("cmi.suspend_data");
                    value02 = scormService.GetIntValue("cmi.score.max");
                    value03 = scormService.GetFloatValue("cmi.score.raw");

                    break;
            }

            Log("Value 01: " + value01);
            Log("Value 02: " + value02);
            Log("Value 03: " + value03);
        });

        genericSetValueButton.onClick.AddListener(() => {
            switch (scormService.Version) {
                case Scorm.Version.Scorm_1_2:
                    scormService.SetValue("cmi.suspend_data", "generic test");
                    scormService.SetValue("cmi.core.score.max", 8);
                    scormService.SetValue("cmi.core.score.raw", 3.5f);
                    break;
                case Scorm.Version.Scorm_2004:
                    scormService.SetValue("cmi.suspend_data", "generic test");
                    scormService.SetValue("cmi.score.max", 8);
                    scormService.SetValue("cmi.score.raw", 3.5f);
                    break;
            }

            scormService.Commit();
        });

        getLearnerIdButton.onClick.AddListener(() => {
            string value = scormService.GetLearnerId();

            Log("LearnerId: " + value);
        });

        getLearnerNameButton.onClick.AddListener(() => {
            string value = scormService.GetLearnerName();

            Log("LearnerName: " + value);
        });

        getLessonLocationButton.onClick.AddListener(() => {
            string value = scormService.GetLessonLocation();

            Log("Lesson location: " + value);
        });

        setLessonLocationButton.onClick.AddListener(() => {
            scormService.SetLessonLocation(string.Format("custom_location_{0}", Guid.NewGuid()));
            scormService.Commit();
        });

        getCreditButton.onClick.AddListener(() => {
            CreditType value = scormService.GetCredit();

            Log("Credit: " + value);
        });

        getLessonStatusButton.onClick.AddListener(() => {
            LessonStatus value = scormService.GetLessonStatus();

            Log("LessonStatus: " + value);
        });

        setLessonStatusButton.onClick.AddListener(() => {
            scormService.SetLessonStatus(LessonStatus.Incomplete);
            scormService.Commit();
        });

        getEntryButton.onClick.AddListener(() => {
            EntryType value = scormService.GetEntry();

            Log("Entry: " + value);
        });

        getRawScoreButton.onClick.AddListener(() => {
            float value = scormService.GetRawScore();

            Log("RawScore: " + value);
        });

        setRawScoreButton.onClick.AddListener(() => {
            scormService.SetRawScore(3.5f);
            scormService.Commit();
        });

        getMaxScoreButton.onClick.AddListener(() => {
            float value = scormService.GetMaxScore();

            Log("MaxScore: " + value);
        });

        setMaxScoreButton.onClick.AddListener(() => {
            scormService.SetMaxScore(10f);
            scormService.Commit();
        });

        getMinScoreButton.onClick.AddListener(() => {
            float value = scormService.GetMinScore();

            Log("MinScore: " + value);
        });

        setMinScoreButton.onClick.AddListener(() => {
            scormService.SetMinScore(0.1f);
            scormService.Commit();
        });

        getTotalTimeButton.onClick.AddListener(() => {
            int value = scormService.GetTotalTime();

            Log("TotalTime: " + value);
        });

        getLessonModeButton.onClick.AddListener(() => {
            LessonMode value = scormService.GetLessonMode();

            Log("LessonMode: " + value);
        });

        setSessionTimeButton.onClick.AddListener(() => {
            int sessionTime = (int)((Time.time - startTime) * 1000);

            scormService.SetSessionTime(sessionTime);
            scormService.Commit();
        });

        getCommentsButton.onClick.AddListener(() => {
            string value = scormService.GetComments();

            Log("Comments: " + value);
        });

        getCommentsFromLmsButton.onClick.AddListener(() => {
            string value = scormService.GetCommentsFromLms();

            Log("CommentsFromLms: " + value);
        });

        getLanguageButton.onClick.AddListener(() => {
            string value = scormService.GetLanguage();

            Log("Language: " + value);
        });

        getSuspendDataButton.onClick.AddListener(() => {
            string value = scormService.GetSuspendData();

            Log("SuspendData: " + value);
        });

        setSuspendDataButton.onClick.AddListener(() => {
            string data = JsonUtility.ToJson(new SuspendDataTest("John", "Doe"));

            scormService.SetSuspendData(data);
            scormService.Commit();
        });

        getObjectivesButton.onClick.AddListener(() => {
            ICollection<ObjectiveData> objectives = scormService.GetObjectives();

            Log("Objective count: " + objectives.Count);

            objectives.ToList().ForEach(o => {
                Log("Objective: " + o.ToString());
            });
        });

        setObjectiveButton.onClick.AddListener(() => {
            string id = RandomString(15);
            float minScore = UnityEngine.Random.Range(0, 10);
            float maxScore = UnityEngine.Random.Range(0, 10);
            float rawScore = UnityEngine.Random.Range(0, 10);
            float scaledScore = UnityEngine.Random.Range(-1, 1);
            LessonStatus successStatus = LessonStatus.Passed;
            LessonStatus completionStatus = LessonStatus.Completed;
            float progressMeasure = UnityEngine.Random.Range(0, 1);
            string description = RandomString(15);

            ObjectiveData objectiveData = new ObjectiveData(id, minScore, maxScore, rawScore, scaledScore, successStatus, completionStatus, progressMeasure, description);

            scormService.SetObjective(objectiveData);
            scormService.Commit();
        });

        setExitReasonButton.onClick.AddListener(() => {
            scormService.SetExitReason(ExitReason.Suspend);
            scormService.Commit();
        });

        finishButton.onClick.AddListener(() => {
            if (scormService.Finish()) {
                Log("Communication terminated");

                initButton.interactable = true;

                ToggleInitialized(false);
            }
        });
    }

    void ToggleInitialized(bool status) {
        buttons.ForEach(b => {
            if (b != initButton) {
                b.interactable = status;
            }
        });
    }

    private static System.Random random = new System.Random();

    public static string RandomString(int length) {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    public void Log(string text) {
        string message = string.Format("[Scorm] - {0}", text);

        Debug.Log(message);

        if (OnMessageLogged != null) {
            OnMessageLogged(message);
        }
    }
}