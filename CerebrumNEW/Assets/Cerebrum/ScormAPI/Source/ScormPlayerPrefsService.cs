using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scorm {

    public class ScormPlayerPrefsService : IScormService {

        private readonly IDictionary<string, string> stringMap = new Dictionary<string, string>();
        private readonly IDictionary<string, int> intMap = new Dictionary<string, int>();
        private readonly IDictionary<string, float> floatMap = new Dictionary<string, float>();

        public bool IsConnected { get; private set; }

        public Version Version { get; private set; }

        public bool Initialize(Version version) {
            IsConnected = true;
            Version = version;

            return true;
        }

        public bool Commit() {
            if (!IsConnected) {
                return false;
            }

            foreach (KeyValuePair<string, string> pair in stringMap) {
                PlayerPrefs.SetString(pair.Key, pair.Value);
            }

            stringMap.Clear();

            foreach (KeyValuePair<string, int> pair in intMap) {
                PlayerPrefs.SetInt(pair.Key, pair.Value);
            }

            intMap.Clear();

            foreach (KeyValuePair<string, float> pair in floatMap) {
                PlayerPrefs.SetFloat(pair.Key, pair.Value);
            }

            floatMap.Clear();

            PlayerPrefs.Save();

            return true;
        }

        public bool Finish() {
            if (!IsConnected) {
                return false;
            }

            IsConnected = false;

            return true;
        }

        public string GetStringValue(string element) {
            return PlayerPrefs.GetString(element);
        }

        public bool SetValue(string element, string value) {
            SetString(element, value);

            return true;
        }

        public int GetIntValue(string element) {
            return PlayerPrefs.GetInt(element);
        }

        public bool SetValue(string element, int value) {
            SetInt(element, value);

            return true;
        }

        public float GetFloatValue(string element) {
            return PlayerPrefs.GetFloat(element);
        }

        public bool SetValue(string element, float value) {
            SetFloat(element, value);

            return true;
        }

        public string GetLearnerId() {
            return "Dummylearnerid";
        }

        public string GetLearnerName() {
            return "Dummy learner name";
        }

        public string GetLessonLocation() {
            return PlayerPrefs.GetString("scorm_lesson_location");
        }

        public bool SetLessonLocation(string value) {
            SetString("scorm_lesson_location", value);

            return true;
        }

        public CreditType GetCredit() {
            string value = PlayerPrefs.GetString("scorm_credit", ScormHelpers.CreditTypeToString(CreditType.Credit));

            return ScormHelpers.StringToCreditType(value);
        }

        public LessonStatus GetLessonStatus() {
            string value = PlayerPrefs.GetString("scorm_lesson_status", ScormHelpers.LessonStatusToString(LessonStatus.NotAttempted));

            return ScormHelpers.StringToLessonStatus(value);
        }

        public bool SetLessonStatus(LessonStatus value) {
            string lessonStatus = ScormHelpers.LessonStatusToString(value);

            SetString("scorm_lesson_status", lessonStatus);

            return true;
        }

        public EntryType GetEntry() {
            string value = PlayerPrefs.GetString("scorm_entry", ScormHelpers.EntryTypeToString(EntryType.AbInitio));

            return ScormHelpers.StringToEntryType(value);
        }

        public float GetRawScore() {
            return PlayerPrefs.GetFloat("scorm_raw_score");
        }

        public bool SetRawScore(float value) {
            SetFloat("scorm_raw_score", value);

            return true;
        }

        public float GetMaxScore() {
            return PlayerPrefs.GetFloat("scorm_max_score");
        }

        public bool SetMaxScore(float value) {
            SetFloat("scorm_max_score", value);

            return true;
        }

        public float GetMinScore() {
            return PlayerPrefs.GetFloat("scorm_min_score");
        }

        public bool SetMinScore(float value) {
            SetFloat("scorm_min_score", value);

            return true;
        }

        public int GetTotalTime() {
            int value = PlayerPrefs.GetInt("scorm_total_time");

            return value != 0 ? value * 10 : value;
        }

        public LessonMode GetLessonMode() {
            string value = PlayerPrefs.GetString("scorm_lesson_mode", ScormHelpers.LessonModeToString(LessonMode.Normal));

            return ScormHelpers.StringToLessonMode(value);
        }

        public bool SetExitReason(ExitReason value) {
            string result = ScormHelpers.ExitReasonToString(value);

            SetString("scorm_exit_reason", result);

            return true;
        }

        public bool SetSessionTime(int milliseconds) {
            float result = milliseconds / 10f; // To centiseconds

            SetInt("scorm_session_time", (int)result);
            SetInt("scorm_total_time", (int)result);

            return true;
        }

        public string GetSuspendData() {
            return PlayerPrefs.GetString("scorm_suspend_data");
        }

        public bool SetSuspendData(string value) {
            SetString("scorm_suspend_data", value);

            return true;
        }

        public string GetComments() {
            return "Dummy comments";
        }

        public string GetCommentsFromLms() {
            return "Dummy comments from lms";
        }

        public List<ObjectiveData> GetObjectives() {
            string value = PlayerPrefs.GetString("scorm_objectives");

            try {
                ObjectivesData data = JsonUtility.FromJson<ObjectivesData>(value);

                return data != null && data.Objectives != null ? data.Objectives : new List<ObjectiveData>();
            } catch (Exception e) {
                throw new ArgumentException(string.Format("No valid objectives format: '{0}' - {1}", value, e.Message));
            }
        }

        public void SetObjective(ObjectiveData value) {
            List<ObjectiveData> objectives = GetObjectives();

            ObjectiveData resolved = objectives.FirstOrDefault(o => o.Id.Equals(value.Id));

            if (resolved != null) {
                objectives.Remove(resolved);
            }

            objectives.Add(value);

            ObjectivesData data = new ObjectivesData();
            data.Objectives.AddRange(objectives);

            string result = JsonUtility.ToJson(data);

            SetString("scorm_objectives", result);
        }

        public string GetLanguage() {
            return "en";
        }
        
        private void SetString(string key, string value) {
            stringMap[key] = value;
        }

        private void SetInt(string key, int value) {
            intMap[key] = value;
        }

        private void SetFloat(string key, float value) {
            floatMap[key] = value;
        }

    }

}
