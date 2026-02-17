using System;
using UnityEngine;

namespace Scorm {

    [Serializable]
    public class ObjectiveData {

        [SerializeField]
        private string id;

        public string Id {
            get { return id; }
        }

        [SerializeField]
        private float minScore;

        public float MinScore {
            get { return minScore; }
        }

        [SerializeField]
        private float maxScore;

        public float MaxScore {
            get { return maxScore; }
        }

        [SerializeField]
        private float rawScore;

        public float RawScore {
            get { return rawScore; }
        }

        [SerializeField]
        private float scaledScore;

        public float ScaledScore {
            get { return scaledScore; }
        }

        [SerializeField]
        private string successStatus;

        public LessonStatus SuccessStatus {
            get { return ScormHelpers.StringToLessonStatus(successStatus); }
        }

        [SerializeField]
        private string completionStatus;

        public LessonStatus CompletionStatus {
            get { return ScormHelpers.StringToLessonStatus(completionStatus); }
        }

        [SerializeField]
        private float progressMeasure;

        public float ProgressMeasure {
            get { return progressMeasure; }
        }

        [SerializeField]
        private string description;

        public string Description {
            get { return description; }
        }

        public ObjectiveData(string id, float minScore, float maxScore, float rawScore, float scaledScore, LessonStatus successStatus, LessonStatus completionStatus, float progressMeasure, string description) {
            this.id = id;
            this.minScore = minScore;
            this.maxScore = maxScore;
            this.rawScore = rawScore;
            this.scaledScore = scaledScore;
            this.successStatus = ScormHelpers.LessonStatusToString(successStatus);
            this.completionStatus = ScormHelpers.LessonStatusToString(completionStatus);
            this.progressMeasure = progressMeasure;
            this.description = description;
        }

        public override string ToString() {
            return string.Format("Id: {0} MinScore: {1} MaxScore: {2} RawScore: {3} ScaledScore: {4} SuccessStatus: {5} CompletionStatus: {6} ProgressMeasure: {7} Description: {8}", id, minScore, maxScore, rawScore, scaledScore, successStatus, completionStatus, progressMeasure, description);
        }

    }

}