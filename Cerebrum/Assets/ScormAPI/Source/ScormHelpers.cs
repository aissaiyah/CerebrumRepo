using System;

namespace Scorm {

    public static class ScormHelpers {
        
        internal static string VersionToString(Version value) {
            switch (value) {
                case Version.Scorm_1_2:
                    return "1_2";
                case Version.Scorm_2004:
                    return "2004";
                default:
                    throw new ArgumentException(string.Format("No valid version value: '{0}'", value));
            }
        }

        internal static string CreditTypeToString(CreditType value) {
            switch (value) {
                case CreditType.Credit:
                    return "credit";
                case CreditType.NoCredit:
                    return "no-credit";
                default:
                    throw new ArgumentException(string.Format("No valid credit value: '{0}'", value));
            }
        }

        internal static CreditType StringToCreditType(string value) {
            switch (value) {
                case "credit":
                    return CreditType.Credit;
                case "no-credit":
                    return CreditType.NoCredit;
                default:
                    throw new ArgumentException(string.Format("No valid credit value: '{0}'", value));
            }
        }

        internal static LessonStatus StringToLessonStatus(string value) {
            switch (value) {
                case "passed":
                    return LessonStatus.Passed;
                case "completed":
                    return LessonStatus.Completed;
                case "failed":
                    return LessonStatus.Failed;
                case "incomplete":
                    return LessonStatus.Incomplete;
                case "browsed":
                    return LessonStatus.Browsed;
                case "not attempted":
                    return LessonStatus.NotAttempted;
                case "unknown":
                    return LessonStatus.Unknown;
                default:
                    throw new ArgumentException(string.Format("No valid lessonStatus value: '{0}'", value));
            }
        }

        internal static string LessonStatusToString(LessonStatus value) {
            switch (value) {
                case LessonStatus.Passed:
                    return "passed";
                case LessonStatus.Completed:
                    return "completed";
                case LessonStatus.Failed:
                    return "failed";
                case LessonStatus.Incomplete:
                    return "incomplete";
                case LessonStatus.Browsed:
                    return "browsed";
                case LessonStatus.NotAttempted:
                    return "not attempted";
                case LessonStatus.Unknown:
                    return "unknown";
                default:
                    throw new ArgumentException(string.Format("No valid lessonStatus value: '{0}'", value));
            }
        }

        internal static string EntryTypeToString(EntryType value) {
            switch (value) {
                case EntryType.AbInitio:
                    return "ab-initio";
                case EntryType.Resume:
                    return "resume";
                case EntryType.Empty:
                    return "";
                default:
                    throw new ArgumentException(string.Format("No valid entry value: '{0}'", value));
            }
        }

        internal static EntryType StringToEntryType(string value) {
            switch (value) {
                case "ab-initio":
                    return EntryType.AbInitio;
                case "resume":
                    return EntryType.Resume;
                case "":
                    return EntryType.Empty;
                default:
                    throw new ArgumentException(string.Format("No valid entry value: '{0}'", value));
            }
        }

        internal static string LessonModeToString(LessonMode value) {
            switch (value) {
                case LessonMode.Browse:
                    return "browse";
                case LessonMode.Normal:
                    return "normal";
                case LessonMode.Review:
                    return "review";
                default:
                    throw new ArgumentException(string.Format("No valid lessonMode value: '{0}'", value));
            }
        }

        internal static LessonMode StringToLessonMode(string value) {
            switch (value) {
                case "browse":
                    return LessonMode.Browse;
                case "normal":
                    return LessonMode.Normal;
                case "review":
                    return LessonMode.Review;
                default:
                    throw new ArgumentException(string.Format("No valid lessonMode value: '{0}'", value));
            }
        }

        internal static string ExitReasonToString(ExitReason value) {
            switch (value) {
                case ExitReason.TimeOut:
                    return "time-out";
                case ExitReason.Suspend:
                    return "suspend";
                case ExitReason.Logout:
                    return "logout";
                default:
                    throw new ArgumentException(string.Format("No valid exit reason value: '{0}'", value));
            }
        }

        internal static ExitReason StringToExitReason(string value) {
            switch (value) {
                case "time-out":
                    return ExitReason.TimeOut;
                case "suspend":
                    return ExitReason.Suspend;
                case "logout":
                    return ExitReason.Logout;
                default:
                    throw new ArgumentException(string.Format("No valid exit reason value: '{0}'", value));
            }
        }

    }

}
