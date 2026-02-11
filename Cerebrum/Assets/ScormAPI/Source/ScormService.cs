using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Scorm {

    public class ScormService : IScormService {

        #region Imports
        [DllImport("__Internal")]
        private static extern bool Scorm_Initialize(string value);

        [DllImport("__Internal")]
        private static extern bool Scorm_Commit();

        [DllImport("__Internal")]
        private static extern bool Scorm_Finish();

        [DllImport("__Internal")]
        private static extern string Scorm_GetStringValue(string element);

        [DllImport("__Internal")]
        private static extern bool Scorm_SetStringValue(string element, string value);

        [DllImport("__Internal")]
        private static extern int Scorm_GetIntValue(string element);

        [DllImport("__Internal")]
        private static extern bool Scorm_SetIntValue(string element, int value);

        [DllImport("__Internal")]
        private static extern float Scorm_GetFloatValue(string element);

        [DllImport("__Internal")]
        private static extern bool Scorm_SetFloatValue(string element, float value);

        [DllImport("__Internal")]
        private static extern string Scorm_GetLearnerId();

        [DllImport("__Internal")]
        private static extern string Scorm_GetLearnerName();

        [DllImport("__Internal")]
        private static extern string Scorm_GetLessonLocation();

        [DllImport("__Internal")]
        private static extern bool Scorm_SetLessonLocation(string value);

        [DllImport("__Internal")]
        private static extern string Scorm_GetCredit();

        [DllImport("__Internal")]
        private static extern string Scorm_GetLessonStatus();

        [DllImport("__Internal")]
        private static extern bool Scorm_SetLessonStatus(string value);

        [DllImport("__Internal")]
        private static extern string Scorm_GetEntry();

        [DllImport("__Internal")]
        private static extern float Scorm_GetRawScore();

        [DllImport("__Internal")]
        private static extern bool Scorm_SetRawScore(float value);

        [DllImport("__Internal")]
        private static extern float Scorm_GetMaxScore();

        [DllImport("__Internal")]
        private static extern bool Scorm_SetMaxScore(float value);

        [DllImport("__Internal")]
        private static extern float Scorm_GetMinScore();

        [DllImport("__Internal")]
        private static extern bool Scorm_SetMinScore(float value);

        [DllImport("__Internal")]
        private static extern int Scorm_GetTotalTime();

        [DllImport("__Internal")]
        private static extern string Scorm_GetLessonMode();

        [DllImport("__Internal")]
        private static extern bool Scorm_SetExitReason(string value);

        [DllImport("__Internal")]
        private static extern bool Scorm_SetSessionTime(int value);

        [DllImport("__Internal")]
        private static extern string Scorm_GetSuspendData();

        [DllImport("__Internal")]
        private static extern bool Scorm_SetSuspendData(string value);

        [DllImport("__Internal")]
        private static extern string Scorm_GetComments();

        [DllImport("__Internal")]
        private static extern string Scorm_GetCommentsFromLMS();

        [DllImport("__Internal")]
        private static extern string Scorm_GetObjectives();

        [DllImport("__Internal")]
        private static extern void Scorm_SetObjective(string value);

        [DllImport("__Internal")]
        private static extern string Scorm_GetLanguage();
        #endregion

        public bool IsConnected { get; private set; }
        
        public Version Version { get; private set; }

        public bool Initialize(Version version) {
            string result = ScormHelpers.VersionToString(version);

            IsConnected = Scorm_Initialize(result);

            Version = version;

            return IsConnected;
        }

        public bool Commit() {
            return Scorm_Commit();
        }

        public bool Finish() {
            return Scorm_Finish();
        }

        public string GetStringValue(string element) {
            return Scorm_GetStringValue(element);
        }

        public bool SetValue(string element, string value) {
            return Scorm_SetStringValue(element, value);
        }

        public int GetIntValue(string element) {
            return Scorm_GetIntValue(element);
        }

        public bool SetValue(string element, int value) {
            return Scorm_SetIntValue(element, value);
        }

        public float GetFloatValue(string element) {
            return Scorm_GetFloatValue(element);
        }

        public bool SetValue(string element, float value) {
            return Scorm_SetFloatValue(element, value);
        }

        public string GetLearnerId() {
            return Scorm_GetLearnerId();
        }

        public string GetLearnerName() {
            return Scorm_GetLearnerName();
        }

        public string GetLessonLocation() {
            return Scorm_GetLessonLocation();
        }

        public bool SetLessonLocation(string value) {
            return Scorm_SetLessonLocation(value);
        }

        public CreditType GetCredit() {
            string value = Scorm_GetCredit();

            return ScormHelpers.StringToCreditType(value);
        }

        public LessonStatus GetLessonStatus() {
            string value = Scorm_GetLessonStatus();

            return ScormHelpers.StringToLessonStatus(value);
        }

        public bool SetLessonStatus(LessonStatus value) {
            string lessonStatus = ScormHelpers.LessonStatusToString(value);

            return Scorm_SetLessonStatus(lessonStatus);
        }

        public EntryType GetEntry() {
            string value = Scorm_GetEntry();

            return ScormHelpers.StringToEntryType(value);
        }

        public float GetRawScore() {
            return Scorm_GetRawScore();
        }

        public bool SetRawScore(float value) {
            return Scorm_SetRawScore(value);
        }

        public float GetMaxScore() {
            return Scorm_GetMaxScore();
        }

        public bool SetMaxScore(float value) {
            return Scorm_SetMaxScore(value);
        }

        public float GetMinScore() {
            return Scorm_GetMinScore();
        }

        public bool SetMinScore(float value) {
            return Scorm_SetMinScore(value);
        }

        public int GetTotalTime() {
            int value = Scorm_GetTotalTime();

            return value * 10; // in milliseconds
        }

        public LessonMode GetLessonMode() {
            string value = Scorm_GetLessonMode();

            return ScormHelpers.StringToLessonMode(value);
        }

        public bool SetExitReason(ExitReason value) {
            string result = ScormHelpers.ExitReasonToString(value);

            return Scorm_SetExitReason(result);
        }

        public bool SetSessionTime(int milliseconds) {
            float result = milliseconds / 10f; // To centiseconds

            return Scorm_SetSessionTime((int)result);
        }

        public string GetSuspendData() {
            return Scorm_GetSuspendData();
        }

        public bool SetSuspendData(string value) {
            return Scorm_SetSuspendData(value);
        }

        public string GetComments() {
            return Scorm_GetComments();
        }

        public string GetCommentsFromLms() {
            return Scorm_GetCommentsFromLMS();
        }

        public List<ObjectiveData> GetObjectives() {
            string value = Scorm_GetObjectives();

            try {
                value = string.Format("{{ \"objectives\": {0} }}", value);

                return JsonUtility.FromJson<ObjectivesData>(value).Objectives;
            } catch (Exception e) {
                throw new ArgumentException(string.Format("No valid objectives format: '{0}' - {1}", value, e.Message));
            }
        }

        public void SetObjective(ObjectiveData value) {
            string result = JsonUtility.ToJson(value);

            Scorm_SetObjective(result);
        }

        public string GetLanguage() {
            return Scorm_GetLanguage();
        }

    }

}