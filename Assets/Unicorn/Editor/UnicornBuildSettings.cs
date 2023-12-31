using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;

namespace Unicorn.Editor
{
    public partial class UnicornBuildSettings : OdinEditorWindow
    {
        public string packageName;
        public string productName;
        [OnValueChanged(nameof(UpdateVersionCodeAndNameBasedOnVersion))]
        public int version;
        public string versionName;
        public int versionCode;

        [PreviewField(64)]
        public Texture2D icon;
        [OnValueChanged(nameof(OnSplashChange))]
        public Sprite splash;
        [ReadOnly, PreviewField(64, ObjectFieldAlignment.Right), HideLabel]
        public Sprite previewSplash;
        [Sirenix.OdinInspector.FilePath(Extensions = "json", AbsolutePath = true)]
        public string googleServices;

        //[FoldoutGroup("Ironsource & AOA")]
        //        [InfoBox(
        //            @"Nó sẽ có dạng như này:

        //Em gửi nhé ạ anh @Tất 

        //INT: ffa5daaf8c43fef4
        //RV: 2cf13c83c80a8a6a
        //BN: 5a78b73f8d317abe

        //Admob: ca-app-pub-6336405384015455~3589735923
        //Tier1: ca-app-pub-6336405384015455/1893510872
        //Tier2: ca-app-pub-6336405384015455/6762694172
        //Tier3: ca-app-pub-6336405384015455/2823449167")]
        //[ValidateInput(nameof(ValidateMaxAOA), "Không hợp lệ, nhập tay hoặc hoặc hỏi mọi người nhé!")]
        //[OnValueChanged(nameof(OnAOAAndMaxInfoChanged))]
        //[TextArea(8, 20)]
        //[HideLabel]
        //public string maxAndAOAInfo;
        //[FoldoutGroup("MAX & AOA/Extracted values", false)]
        public string AppKeyIronsource;
        //[FoldoutGroup("MAX & AOA/Extracted values")]
        //public string RewardedAdUnitId;
        //[FoldoutGroup("MAX & AOA/Extracted values")]
        //public string BannerAdUnitId;

        [Space]
        //[FoldoutGroup("MAX & AOA/Extracted values")]
        public string Admob;
        // [FoldoutGroup("MAX & AOA/Extracted values")]
        public string AD_UNIT_ID_1;
        // [FoldoutGroup("MAX & AOA/Extracted values")]
        public string AD_UNIT_ID_2;
        // [FoldoutGroup("MAX & AOA/Extracted values")]
        public string AD_UNIT_ID_3;

        protected override void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnGUI();
            if (EditorGUI.EndChangeCheck())
            {
                hasUnsavedChanges = true;
            }
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                {
                    GUILayout.Space(30);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void OnSplashChange()
        {
            previewSplash = splash;
        }

        [HorizontalGroup("Version")]
        [Button("Giảm version")]
        private void UI_DecreaseVersion() => DecreaseVersion();
        [HorizontalGroup("Version")]
        [Button("Nâng version")]
        private void UI_IncreaseVersion() => IncreaseVersion();

        [HorizontalGroup]
        [Button("Discard", ButtonSizes.Medium)]
        protected override void OnEnable()
        {
            base.OnEnable();
            ResetRocketConfig();
            ResetAOAAndMAX();
            ResetOthers();
            hasUnsavedChanges = false;
        }

        [HorizontalGroup]
        [Button(ButtonSizes.Medium)]
        private void Save()
        {
            SaApplyMaxAndAoa();
            SaveRocketConfig();
            SaveOthers();
            hasUnsavedChanges = false;
        }
    }
}