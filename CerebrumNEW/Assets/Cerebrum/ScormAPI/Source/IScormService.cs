using System.Collections.Generic;

namespace Scorm {

    public enum Version {
        Scorm_1_2 = 0,
        Scorm_2004 = 1
    }

    public enum CreditType {
        Credit = 0,
        NoCredit = 1
    }

    public enum LessonStatus {
        Passed = 0, // 1.2
        Completed = 1, // 1.2 / 2004
        Failed = 2, // 1.2
        Incomplete = 3, // 1.2 / 2004
        Browsed = 4, // 1.2
        NotAttempted = 5, // 1.2 / 2004
        Unknown = 6 // 2004
    }

    public enum EntryType {
        AbInitio = 0,
        Resume = 1,
        Empty = -1
    }

    public enum LessonMode {
        Browse = 0,
        Normal = 1,
        Review = 2
    }

    public enum ExitReason {
        TimeOut = 0,
        Suspend = 1,
        Logout = 2
    }

    public interface IScormService {

        /// <summary>
        /// Returns true if a communication session with the LMS has been correctly established.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Returns the current Scorm version.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Begins a communication session with the LMS.
        /// </summary>
        /// <param name="version">The version to use.</param>
        /// <returns>The operation result.</returns>
        bool Initialize(Version version);

        /// <summary>
        /// Indicates to the LMS that all data should be persisted.
        /// </summary>
        /// <returns>The operation result.</returns>
        bool Commit();

        /// <summary>
        /// Ends a communication session with the LMS.
        /// </summary>
        /// <returns>The operation result.</returns>
        bool Finish();

        /// <summary>
        /// Get a generic string value.
        /// </summary>
        /// <param name="element">Scorm Run-Time refence element.</param>
        /// <returns>The value.</returns>
        string GetStringValue(string element);

        /// <summary>
        /// Set a generic string value.
        /// </summary>
        /// <param name="element">Scorm Run-Time refence element.</param>
        /// <param name="value">Scorm Run-Time refence value.</param>
        /// <returns>The operation result.</returns>
        bool SetValue(string element, string value);

        /// <summary>
        /// Get a generic int value.
        /// </summary>
        /// <param name="element">Scorm Run-Time refence element.</param>
        /// <returns>The value.</returns>
        int GetIntValue(string element);

        /// <summary>
        /// Set a generic int value.
        /// </summary>
        /// <param name="element">Scorm Run-Time refence element.</param>
        /// <param name="value">Scorm Run-Time refence value.</param>
        /// <returns>The operation result.</returns>
        bool SetValue(string element, int value);

        /// <summary>
        /// Get a generic float value.
        /// </summary>
        /// <param name="element">Scorm Run-Time refence element.</param>
        /// <returns>The value.</returns>
        float GetFloatValue(string element);

        /// <summary>
        /// Set a generic float value.
        /// </summary>
        /// <param name="element">Scorm Run-Time refence element.</param>
        /// <param name="value">Scorm Run-Time refence value.</param>
        /// <returns>The operation result.</returns>
        bool SetValue(string element, float value);

        /// <summary>
        /// Identifies the learner on behalf of whom the SCO was launched.
        /// </summary>
        string GetLearnerId();

        /// <summary>
        /// Name provided for the learner by the LMS.
        /// </summary>
        string GetLearnerName();

        /// <summary>
        /// The learner’s current location in the SCO.
        /// </summary>
        string GetLessonLocation();

        /// <summary>
        /// The learner’s current location in the SCO.
        /// </summary>
        /// <param name="value">The learner's lesson location.</param>
        /// <returns>The operation result.</returns>
        bool SetLessonLocation(string value);

        /// <summary>
        /// Indicates whether the learner will be credited for performance in the SCO.
        /// </summary>
        CreditType GetCredit();

        /// <summary>
        /// Indicates whether the learner has completed and satisfied the requirements for the SCO.
        /// </summary>
        LessonStatus GetLessonStatus();

        /// <summary>
        /// Indicates whether the learner has completed and satisfied the requirements for the SCO.
        /// </summary>
        /// <param name="value">The learner's lesson status.</param>
        /// <returns>The operation result.</returns>
        bool SetLessonStatus(LessonStatus value);

        /// <summary>
        /// Asserts whether the learner has previously accessed the SCO.
        /// </summary>
        EntryType GetEntry();

        /// <summary>
        /// Number that reflects the performance of the learner relative to the range bounded by the values of min and max.
        /// </summary>
        float GetRawScore();

        /// <summary>
        /// Number that reflects the performance of the learner relative to the range bounded by the values of min and max.
        /// </summary>
        /// <param name="value">The learner's raw score.</param>
        /// <returns>The operation result.</returns>
        bool SetRawScore(float value);

        /// <summary>
        /// Maximum value in the range for the raw score.
        /// </summary>
        float GetMaxScore();

        /// <summary>
        /// Maximum value in the range for the raw score.
        /// </summary>
        /// <param name="value">The learner's max score.</param>
        /// <returns>The operation result.</returns>
        bool SetMaxScore(float value);

        /// <summary>
        /// Minimum value in the range for the raw score.
        /// </summary>
        float GetMinScore();

        /// <summary>
        /// Minimum value in the range for the raw score.
        /// </summary>
        /// <param name="value">The learner's min score.</param>
        /// <returns>The operation result.</returns>
        bool SetMinScore(float value);

        /// <summary>
        /// Sum of all of the learner’s session times (in milliseconds) accumulated in the current learner attempt.
        /// </summary>
        int GetTotalTime();

        /// <summary>
        /// Identifies one of three possible modes in which the SCO may be presented to the learner.
        /// </summary>
        LessonMode GetLessonMode();

        /// <summary>
        /// Indicates how or why the learner left the SCO.
        /// </summary>
        /// <param name="value">The reason.</param>
        /// <returns>The operation result.</returns>
        bool SetExitReason(ExitReason value);

        /// <summary>
        /// Amount of time (in milliseconds) that the learner has spent in the current learner session for this SCO.
        /// </summary>
        /// <param name="milliseconds">The session time in milliseconds.</param>
        /// <returns>The operation result.</returns>
        bool SetSessionTime(int milliseconds);

        /// <summary>
        /// Provides space to store and retrieve data between learner sessions.
        /// </summary>
        string GetSuspendData();

        /// <summary>
        /// Provides space to store and retrieve data between learner sessions.
        /// </summary>
        /// <param name="value">The value to store.</param>
        /// <returns>The operation result.</returns>
        bool SetSuspendData(string value);

        /// <summary>
        /// Textual input from the learner about the SCO.
        /// </summary>
        string GetComments();

        /// <summary>
        /// Comments or annotations associated with a SCO.
        /// </summary>
        string GetCommentsFromLms();

        /// <summary>
        /// The objectives associated with a SCO.
        /// </summary>
        List<ObjectiveData> GetObjectives();

        /// <summary>
        /// Set an objective.
        /// </summary>
        /// <param name="value">The objective data.</param>
        void SetObjective(ObjectiveData value);

        /// <summary>
        /// The learner’s preferred language for SCOs with multilingual capability.
        /// </summary>
        string GetLanguage();

    }

}