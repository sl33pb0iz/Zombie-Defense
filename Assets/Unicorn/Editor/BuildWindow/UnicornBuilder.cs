using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Unicorn.Editor
{
    public partial class UnicornBuilder : OdinEditorWindow
    {
        [InfoBox("Toàn bộ checklist đều đã được tick thì mới được build final. Khi nào sẵn sàng muốn build final thì bấm nút \"I want to build final\" để hệ thống cập nhật checklist. pass : checklistPass")]
        [ReadOnly, BoxGroup("Final build checklist")] 
        public bool AOA;
        [ReadOnly, BoxGroup("Final build checklist")]
        public bool IRONSOURCE;
        [ReadOnly, BoxGroup("Final build checklist")]
        public bool firebase;
        [ReadOnly, BoxGroup("Final build checklist")]
        public bool packageName;
        [ReadOnly, BoxGroup("Final build checklist")]
        public bool splash;
        [ReadOnly, BoxGroup("Final build checklist")]
        public bool icon;

        public string bypassChecklistPassword;

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.Space(30);
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        [Button("I want to build final"), HorizontalGroup("First")]
        private void TestChecklist()
        {
            AOA = CheckAdMob();
            IRONSOURCE = CheckMaxMediation();
            firebase = CheckFirebase();
            packageName = CheckPackageName();
            splash = CheckSplash();
            icon = CheckIcon();
        }

        [Button, HorizontalGroup("Build")]
        public void BuildTest()
        {
            EditorUserBuildSettings.development = false;
            ConfigBuild.BuildGame(false);
        }
        
        [Button, HorizontalGroup("Build"), DisableIf("@(!AOA || !IRONSOURCE || !firebase || !packageName || !splash || !icon) && bypassChecklistPassword != \"checklistPass\" ")]
        public void BuildFinal()
        {
            EditorUserBuildSettings.development = false;
            ConfigBuild.BuildGame(true);
        }
        
        [Button]
        public void BuildDebug(bool deepProfile, bool scriptDebugging, bool waitForManagedDebugger)
        {
            EditorUserBuildSettings.development = true;
            EditorUserBuildSettings.buildWithDeepProfilingSupport = deepProfile;
            EditorUserBuildSettings.allowDebugging = scriptDebugging;
            EditorUserBuildSettings.waitForManagedDebugger = waitForManagedDebugger;
            ConfigBuild.BuildGame();
        }
        
        [Button]
        private void OpenBuildFolder()
        {
            ConfigBuild.OpenFileBuild();
        }
    }
}