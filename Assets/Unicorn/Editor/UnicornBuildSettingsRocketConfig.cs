using System;
using System.IO;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Unicorn.Editor
{
    public partial class UnicornBuildSettings
    {
        private void ResetRocketConfig()
        {
            packageName = RocketConfig.package_name;
            productName = RocketConfig.ProductName;
            versionName = RocketConfig.VersionName;
            versionCode = RocketConfig.versionCode;
            version = GetVersion();
        }

        private void SaveRocketConfig()
        {
            string rocketConfigPath = Application.dataPath + "/_SDK/RocketConfig.cs";
            string text = File.ReadAllText(rocketConfigPath);
            text = Regex.Replace(text,"public const string package_name = \".+?\"", $"public const string package_name = \"{packageName}\"");
            text = Regex.Replace(text,"public const string ProductName = \".+?\"", $"public const string ProductName = \"{productName}\"");
            text = Regex.Replace(text,"public const string VersionName = \".+?\"", $"public const string VersionName = \"{versionName}\"");
            text = Regex.Replace(text,"public const int versionCode = .+?;", $"public const int versionCode = {versionCode};");
            
            File.WriteAllText(rocketConfigPath, text);
            ConfigBuild.FixSettingBuild();
            AssetDatabase.Refresh();
        }

        private void IncreaseVersion()
        {
            if (!IsValidVersionCode() || !IsValidVersionName())
            {
                throw new InvalidOperationException($"Version code or version name is invalid. \nversionName: {versionName} \nversionCode: {versionCode} ");
            }
            
            version++;
            UpdateVersionCodeAndNameBasedOnVersion();
            hasUnsavedChanges = true;
        }

        
        private void DecreaseVersion()
        {
            if (!IsValidVersionCode() || !IsValidVersionName())
            {
                throw new InvalidOperationException($"Version code or version name is invalid. \nversionName: {versionName} \nversionCode: {versionCode} ");
            }
            
            version--;
            UpdateVersionCodeAndNameBasedOnVersion();
            hasUnsavedChanges = true;
        }

        private void UpdateVersionCodeAndNameBasedOnVersion()
        {
            version = Mathf.Max(0, version);
            versionName = "1.0." + version.ToString();
            versionCode = int.Parse("2000" + version);
        }

        private int GetVersion()
        {
            if (string.IsNullOrWhiteSpace(versionName))
            {
                return 0;
            }
            
            if (!IsValidVersionCode() || !IsValidVersionName())
            {
                throw new InvalidOperationException($"Version code or version name is invalid. \nversionName: {versionName} \nversionCode: {versionCode} ");
            }

            int version = int.Parse(versionName.Split('.')[2]);
            return version;
        }

        private bool IsValidVersionName() => Regex.IsMatch(versionName, @"\d+\.\d+\.\d+");
        private bool IsValidVersionCode() => Regex.IsMatch(versionCode.ToString(), @"2000\d+");
    }
}