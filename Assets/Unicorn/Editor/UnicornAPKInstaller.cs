using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Unicorn.Unicorn.Editor
{
    
    
    public class UnicornAPKInstaller: OdinEditorWindow
    {
        
        [TitleGroup("Install Apk")]
        [ValueDropdown(nameof(GetFiles))]
        public string APK;
        
        
        protected override void OnEnable()
        {
            base.OnEnable();
            var files = GetFiles().ToList();
            if (files.Count == 0)
                return;
            APK = files[0];
        }
        
        private IEnumerable<string> GetFiles()
        {
            if (!Directory.Exists(Application.dataPath + "/../Builds"))
                return Array.Empty<string>();
            
            return Directory.GetFiles(Application.dataPath + "/../Builds", "*.apk", SearchOption.AllDirectories)
                .Select(Path.GetFileNameWithoutExtension)
                .OrderByDescending(s => s);
        }
        
        [Button]
        [TitleGroup("Install Apk")]
        public void InstallAPK()
        {
            string bundletoolPath = Path.Combine(EditorApplication.applicationPath,
                "../Data/PlaybackEngines/AndroidPlayer/SDK/platform-tools");
            var apkPath = "\"" + Directory.GetFiles(Application.dataPath + "/../Builds", APK +".apk", SearchOption.AllDirectories)[0] +"\"";

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    // FileName = @"D:\Workspace\template-unicorn\Assets\Unicorn\Editor\BuildWindow\installAPK.bat",
                    FileName = "cmd.exe",
                    WorkingDirectory = Path.GetDirectoryName(bundletoolPath) ?? string.Empty,
                    Arguments = apkPath,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.EnableRaisingEvents = true;
            process.StandardInput.WriteLine($"adb install -r {apkPath}");
            process.StandardInput.WriteLine("exit");

            
            EditorUtility.DisplayProgressBar($"Installing {APK}...", "Please wait", 0);
            try
            {
                string output = process.StandardOutput.ReadToEnd();
                output = output.Substring(output.IndexOf(".apk\"", StringComparison.CurrentCultureIgnoreCase) + ".apk\"".Length);
                output = output.Trim();
                
                Debug.Log(output);
                var error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(error))
                {
                    Debug.LogError(error);
                }
                process.WaitForExit(5000);
                process.Close();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        
        [Button]
        public void OpenDebuggingPortToDevice()
        {
            string bundletoolPath = Path.Combine(EditorApplication.applicationPath,
                "../Data/PlaybackEngines/AndroidPlayer/SDK/platform-tools");

            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = Path.GetDirectoryName(bundletoolPath) ?? string.Empty,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.StandardInput.WriteLine($"adb forward tcp:34999 localabstract:Unity-{Application.identifier}");
            process.StandardInput.WriteLine("adb reverse tcp:34998 tcp:34999");
        } 
    }
}