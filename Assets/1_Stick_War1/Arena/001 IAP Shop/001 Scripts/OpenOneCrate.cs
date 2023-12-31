using Sirenix.OdinInspector;
using Snowyy;
using Snowyy.EquipmentSystem;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using Unicorn;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arena
{
    public class OpenOneCrate : UICanvas
    {
        [FoldoutGroup("ANIM"), SerializeField] private SkeletonGraphic skeletonGraphic;
        [FoldoutGroup("ANIM"), SerializeField] private string[] chestSkin; // 0 for white  - 1 for gold
        [FoldoutGroup("ANIM"), SerializeField] private AnimationReferenceAsset[] chestAnim;

        [FoldoutGroup("_BUTTON"), SerializeField] private Button btnClose;
        [FoldoutGroup("_BUTTON"), SerializeField] private Button btnSkip;

        [FoldoutGroup("_IMAGE"), SerializeField] private Image imgBorder;
        [FoldoutGroup("_IMAGE"), SerializeField] private Image imgIcon;

        [FoldoutGroup("_TEXT"), SerializeField] private TextMeshProUGUI txtRarity;
        [FoldoutGroup("_TEXT"), SerializeField] private TextMeshProUGUI txtName;
       
        [FoldoutGroup("_DATA"), SerializeField] private DataRarityEffect dataRarityEffect;

        [FoldoutGroup("_FX"), SerializeField] private GameObject fxBlink;
        [FoldoutGroup("_FX"), SerializeField] private GameObject fxPopStar;

        public bool canClose { get; private set; }
        public bool IsSingle { get; private set; }


        private string currentAnim;

        private void Start()
        {
            btnClose.onClick.AddListener(OnClickBtnClose);
            btnSkip.onClick.AddListener(OnClickBtnSkip);
        }

        private void OnClickBtnSkip()
        {
            OnClosePopupPressed();
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.SetMultiCounter(10);
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.ShowEachOne();
        }

        private void OnClickBtnClose()
        {
            if (!canClose) return;
            canClose = false;
            if (IsSingle)
            {
                OnClosePopupPressed();
                GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.OnClosePopupPressed();
                GameManager.Instance.UiController.UiSurvivorShop.Init();
                return;
            }

            OnBackPressed();
            GameManager.Instance.UiController.UiSurvivorShop.UiOpenCrate.ShowEachOne();
        }

        public void Init(Equipment equipmentToShow, bool isSingle = true, bool isWhite = false)
        {
            IsSingle = isSingle;
            canClose = false;
            btnSkip.gameObject.SetActive(!isSingle);
            //SETUP INFO
            imgBorder.sprite = dataRarityEffect.dictRarityBasedSprites[equipmentToShow.Rarity].borderSprite;
            imgIcon.sprite = equipmentToShow.itemTemplate.arrUpgradeIcons[(int)equipmentToShow.Rarity];

            txtRarity.color =
                txtName.color = dataRarityEffect.dictRarityBasedSprites[equipmentToShow.Rarity].rarityColor;
            txtRarity.alpha = 0;
            txtName.alpha = 0;

            txtName.text = equipmentToShow.itemTemplate.name;
            txtRarity.text = dataRarityEffect.GetRarityDisplay(equipmentToShow.Rarity);
            //SET UP CHEST DISPLAY
            if (isWhite)
            {
                skeletonGraphic.Skeleton.SetSkin(chestSkin[0]);
            }
            else
            {
                skeletonGraphic.Skeleton.SetSkin(chestSkin[1]);
            }

            skeletonGraphic.Skeleton.SetToSetupPose();

            SetAnimation(chestAnim[0], true, 1);
            //SoundManager.Instance.StopSound(soundEnum: SoundManager.GameSound.OpenCrate);
            //SHOW
            Show(true);
            //canClose = true;
            //PLAY ANIMATION
            SetAnimation(chestAnim[1], false, 1);
            //SoundManager.Instance.PlayFxSound(soundEnum: SoundManager.GameSound.OpenCrate);
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                return;
            }

            //ToggleFx(fxBlink, false);
            ToggleFx(fxPopStar, false);
        }

        public void ToggleFx(GameObject gameObject, bool isToggle)
        {
            gameObject.SetActive(isToggle);
        }

        public void ToggleCanClose(bool isToggle)
        {
            canClose = isToggle;
        }

        private void SetAnimation(AnimationReferenceAsset anim, bool loop, float timeScale)
        {
            if (anim.name.Equals(currentAnim)) return;
            skeletonGraphic.AnimationState.SetAnimation(0, anim, loop).TimeScale = timeScale;
            currentAnim = anim.name;
        }
    }
}