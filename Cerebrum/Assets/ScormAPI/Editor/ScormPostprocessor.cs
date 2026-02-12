using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Scorm {

    public class ScormPostprocessor : IPostprocessBuild {

        public int callbackOrder {
            get { return 1; }
        }

        public void OnPostprocessBuild(BuildTarget target, string path) {
            if (target != BuildTarget.WebGL) return;

            string buildFolderPath = IsDirectory(path) ? path : Path.GetDirectoryName(path);

            ScormPublishSettings settings = Resources.Load<ScormPublishSettings>(ScormPublishSettings.RelativePath);

            if (!settings.Enabled) return;

            string scormFilesPath = Path.Combine(Application.dataPath, string.Format("ScormAPI/Editor/Data/{0}", settings.GetVersionFolderName()));

            string tempFolderPath = CreateTempFolder();

            string zipFilePath = !string.IsNullOrEmpty(settings.ZipFilePath) ? settings.ZipFilePath : Path.Combine(buildFolderPath, "scorm.zip");

            CreateProjectFiles(buildFolderPath, tempFolderPath);

            CreateScormFiles(scormFilesPath, tempFolderPath);

            ParseManifest(tempFolderPath, settings);

            Debug.Log("Creating scorm, please wait...");

            SaveZip(tempFolderPath, zipFilePath);

            Debug.Log(string.Format("Scorm successfully created: {0}", zipFilePath));

            DeleteTempFolder(tempFolderPath);
        }

        private static string CreateTempFolder() {
            string folderName = Guid.NewGuid().ToString();

            string folderPath = Path.Combine(Path.GetTempPath(), folderName);

            Directory.CreateDirectory(folderPath);

            return folderPath;
        }

        private static void CreateProjectFiles(string sourcePath, string destinationPath) {
            CopyFolder(sourcePath, destinationPath);
        }

        private static void CreateScormFiles(string sourcePath, string destinationPath) {
            CopyFolder(sourcePath, destinationPath);

            IEnumerable<string> metaFiles = Directory.GetFiles(destinationPath, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".meta"));

            foreach (string metaFile in metaFiles) {
                File.Delete(metaFile);
            }
        }

        private static void ParseManifest(string path, ScormPublishSettings settings) {
            string manifestPath = Path.Combine(path, "imsmanifest.xml");

            XmlDocument doc = new XmlDocument {
                PreserveWhitespace = true
            };

            doc.Load(manifestPath);

            XmlNamespaceManager namespaces = new XmlNamespaceManager(doc.NameTable);
            namespaces.AddNamespace("x", settings.GetNamespace());

            XmlNode organizationNode = doc.SelectSingleNode("/x:manifest/x:organizations/x:organization/x:title", namespaces);
            organizationNode.InnerText = settings.CourseTitle;

            XmlNode scoNode = doc.SelectSingleNode("/x:manifest/x:organizations/x:organization/x:item/x:title", namespaces);
            scoNode.InnerText = settings.ScoTitle;

            doc.Save(manifestPath);
        }

        private static void SaveZip(string sourcePath, string destinationFilePath) {
            if (File.Exists(destinationFilePath)) {
                File.Delete(destinationFilePath);
            }

            ZipFile zip = new ZipFile(destinationFilePath);
            zip.SaveProgress += Zip_SaveProgress;
            zip.AddDirectory(sourcePath);
            zip.Save();
        }

        private static void Zip_SaveProgress(object sender, SaveProgressEventArgs e) {
            switch (e.EventType) {
                case ZipProgressEventType.Saving_BeforeWriteEntry:
                    if (e.EntriesTotal > 0) {
                        float totalPercentage = (float)e.EntriesSaved / e.EntriesTotal;

                        string title = "Scorm";
                        string info = string.Format("Adding '{0}' to zip", e.CurrentEntry.FileName);

                        EditorUtility.DisplayProgressBar(title, info, totalPercentage);
                    }
                    break;
                case ZipProgressEventType.Saving_Completed:
                    EditorUtility.ClearProgressBar();
                    break;
            }
        }

        private static void CopyFolder(string sourcePath, string destinationPath) {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)) {
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
            }
        }

        private static void DeleteTempFolder(string path) {
            Directory.Delete(path, true);
        }

        private static bool IsDirectory(string path) {
            FileAttributes attr = File.GetAttributes(path);

            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

    }

}