using UnityEngine;

namespace Scorm {

    public class ScormPublishSettings : ScriptableObject {

        public const string RelativePath = "ScormPublishSettings";

        public enum ScormVersion {
            V_1_2 = 0,
            V_2004 = 1
        }

        [SerializeField]
        private string courseTitle;

        public string CourseTitle {
            get { return courseTitle; }
            set { courseTitle = value; }
        }

        [SerializeField]
        private string scoTitle;

        public string ScoTitle {
            get { return scoTitle; }
            set { scoTitle = value; }
        }

        [SerializeField]
        private ScormVersion version = ScormVersion.V_1_2;

        public ScormVersion Version {
            get { return version; }
        }

        [SerializeField]
        private bool enabled;

        public bool Enabled {
            get { return enabled; }
            set { enabled = value; }
        }

        [SerializeField]
        private string zipFilePath;

        public string ZipFilePath {
            get { return zipFilePath; }
            set { zipFilePath = value; }
        }

        public string GetVersionFolderName() {
            switch (version) {
                case ScormVersion.V_1_2:
                    return "1_2";
                case ScormVersion.V_2004:
                    return "2004";
            }

            return string.Empty;
        }

        public string GetNamespace() {
            switch (version) {
                case ScormVersion.V_1_2:
                    return "http://www.imsproject.org/xsd/imscp_rootv1p1p2";
                case ScormVersion.V_2004:
                    return "http://www.imsglobal.org/xsd/imscp_v1p1";
            }

            return string.Empty;
        }

    }

}