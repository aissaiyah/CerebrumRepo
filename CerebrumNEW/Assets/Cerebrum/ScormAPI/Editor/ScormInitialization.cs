using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scorm {

    [InitializeOnLoad]
    public static class ScormInitialization {

        static ScormInitialization() {
            string relativeFilePath = string.Format("Resources/{0}.asset", ScormPublishSettings.RelativePath);
            string filePath = Path.Combine(Application.dataPath, relativeFilePath);

            if (!File.Exists(filePath)) {
                string dirPath = Path.GetDirectoryName(filePath);

                if (!AssetDatabase.IsValidFolder(dirPath)) {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                ScormPublishSettings settings = ScriptableObject.CreateInstance<ScormPublishSettings>();

                AssetDatabase.CreateAsset(settings, string.Format("Assets/{0}", relativeFilePath));
            }
        }

    }

}
