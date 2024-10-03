using System;
using System.IO;

namespace Interface
{
    public class FirstRunManager
    {
        private readonly string _firstRunFilePath;

        public FirstRunManager(string orgName, string appName, string fileName = "FirstRunIdentifier")
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDirectory = Path.Combine(localAppDataPath, orgName, appName);
            Directory.CreateDirectory(appDirectory); // Ensure the directory exists
            _firstRunFilePath = Path.Combine(appDirectory, fileName);
        }

        public bool IsFirstRun()
        {
            return !File.Exists(_firstRunFilePath);
        }

        public void MarkFirstRun()
        {
            File.WriteAllText(_firstRunFilePath, "This file marks that the first run has occurred.");
        }
    }
}
