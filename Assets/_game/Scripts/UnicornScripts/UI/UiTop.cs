using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



namespace Unicorn.UI
{
    public class UiTop : UICanvas
    {
        [FoldoutGroup("TEXT"), SerializeField] private TextMeshProUGUI txtGold;
        [FoldoutGroup("TEXT"), SerializeField] private TextMeshProUGUI txtGem;

        [FoldoutGroup("HOLDER"), SerializeField] private Transform transCoin;
        [FoldoutGroup("HOLDER"), SerializeField] private Transform transGem;
        [FoldoutGroup("HOLDER"), SerializeField] private List<Image> listImgKeys;
        [FoldoutGroup("HOLDER"), SerializeField] private RectTransform objKey;


        [FoldoutGroup("BUTTON"), SerializeField] private ButtonFunctionController m_ButtonController;
        [FoldoutGroup("BUTTON"), SerializeField] private Button btnBackPressed;
        [FoldoutGroup("BUTTON"), SerializeField] private Button btnQuitPressed;
        [FoldoutGroup("BUTTON"), SerializeField] private Button btnAddGold;
        [FoldoutGroup("BUTTON"), SerializeField] private Button btnAddGem;


        [HideInInspector] public UnityAction<bool, bool> OnClickBtnAddGoldEvent;
        [HideInInspector] public UnityAction<bool, bool> OnClickBtnAddGemEvent;

        private void Start()
        {
            GameManager.Instance.PlayerDataManager.actionUITop += UpdateUIHaveAnim;

            UpdateUiGoldAndGem(0);
            UpdateUiGoldAndGem(2);
            UpdateLayoutKey();
            m_ButtonController.gameObject.SetActive(true);
            btnQuitPressed.gameObject.SetActive(false);
            btnBackPressed.gameObject.SetActive(!m_ButtonController.gameObject.activeSelf);
            
        }

        private void OnEnable()
        {
            btnBackPressed.onClick.AddListener(OnBackPressed);
            btnQuitPressed.onClick.AddListener(OnQuitPressed);
            btnAddGem.onClick.AddListener(() => OnClickBtnAddGem(false, true));
            btnAddGold.onClick.AddListener(() => OnClickBtnAddGold(true, false));
        }

        private void OnDisable()
        {
            btnBackPressed.onClick.RemoveListener(OnBackPressed);
            btnQuitPressed.onClick.RemoveListener(OnQuitPressed);
            btnAddGem.onClick.RemoveListener(() => OnClickBtnAddGem(false, true));
            btnAddGold.onClick.RemoveListener(() => OnClickBtnAddGold(true, false));
        }

        private void OnDestroy()
        {
            GameManager.Instance.PlayerDataManager.actionUITop -= UpdateUIHaveAnim;
        }

        private void OnClickBtnAddGold(bool IsGoToGold, bool IsGoToGem)
        {
            if(GameManager.Instance.GameStateController.CurrentGameState == GameState.LOBBY)
            {
                OnClickBtnAddGoldEvent?.Invoke(IsGoToGold, IsGoToGem);
            }    
        }

        private void OnClickBtnAddGem(bool IsGoToGold, bool IsGoToGem)
        {
            if (GameManager.Instance.GameStateController.CurrentGameState == GameState.LOBBY)
            {
                OnClickBtnAddGemEvent?.Invoke(IsGoToGold, IsGoToGem);
            }
        }

        public void ActiveButtonFunction(bool value)
        {
            btnQuitPressed.gameObject.SetActive(false);
            m_ButtonController.gameObject.SetActive(value);
            btnBackPressed.gameObject.SetActive(!m_ButtonController.gameObject.activeSelf);
        }

        public void ActiveQuitButton()
        {
            btnQuitPressed.gameObject.SetActive(true);
            m_ButtonController.gameObject.SetActive(false);
            btnBackPressed.gameObject.SetActive(false);
        }

        public void OnQuitPressed()
        {
            SoundManager.Instance.PlaySoundButton();
            GameManager.Instance.UiController.OpenUiMainLobby();
            GameManager.Instance.LoadLevel();
        }

        private void UpdateUiGoldAndGem(int _type)
        {
            switch (_type)
            {
                case 0:
                    int gold = GameManager.Instance.Profile.GetGold();
                    string formattedGold = FormatNumber(gold);
                    txtGold.text = formattedGold;
                    break;
                case 2:
                    int gem = GameManager.Instance.Profile.GetGem();
                    string formattedGem = FormatNumber(gem);
                    txtGem.text = formattedGem;
                    break;

            }
        }

        private string FormatNumber(int number)
        {
            if (number >= 1000000000) // Vượt qua 999,999,999
            {
                float millions = number / 1000000000f;
                return millions.ToString("0.#") + "B";
            }
            else if (number >= 1000000) // Vượt qua 999,999
            {
                float millions = number / 1000000f;
                return millions.ToString("0.#") + "M";
            }
            else if (number >= 1000) // Vượt qua 999
            {
                float thousands = number / 1000f;
                return thousands.ToString("0.#") + "K";
            }
            else
            {
                return number.ToString();
            }
        }

        private void UpdateUIHaveAnim(TypeItem _type)
        {
            switch (_type)
            {
                case TypeItem.Coin:
                    {
                        SetTextCoin(GameManager.Instance.Profile.GetGold());
                        PlayAnimationScale(transCoin);
                        break;
                    }
                case TypeItem.Gem:
                    {
                        SetTextGem(GameManager.Instance.Profile.GetGem());
                        PlayAnimationScale(transGem);
                        break;
                    }
                case TypeItem.Key:
                    {
                        UpdateLayoutKey(true);
                        break;
                    }
            }
        }

        private void UpdateLayoutKey(bool isAni = false)
        {
            for (int i = 0; i < listImgKeys.Count; i++)
            {
                listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(false);
            }

            int countKey = GameManager.Instance.Profile.GetKey() > 3 ? 3 : GameManager.Instance.Profile.GetKey();
            for (int i = 0; i < countKey; i++)
            {
                listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(true);
            }

            if (isAni)
                objKey.DOAnchorPos3DY(-50, 1f).OnComplete(() => { objKey.DOAnchorPos3DY(135, 1f).SetDelay(1f); });
        }

        private Tweener tweenCoin;
        private int tmpCoin;

        private Tweener tweenGem;
        private int tmpGem;

        private void SetTextCoin(int _coin)
        {
            tweenCoin = tweenCoin ?? DOTween.To(() => tmpCoin, x =>
            {
                tmpCoin = x;
                string formattedGold = FormatNumber(tmpCoin);
                txtGold.text = formattedGold.ToString();
            }, _coin, 0.3f).SetAutoKill(false).OnComplete(() =>
            {
                tmpCoin = GameManager.Instance.Profile.GetGold();
                string formattedGold = FormatNumber(tmpCoin);
                txtGold.text = formattedGold.ToString();
            });
            tweenCoin.ChangeStartValue(tmpCoin);
            tweenCoin.ChangeEndValue(_coin);
            tweenCoin.Play();
        }

        private void SetTextGem(int _gem)
        {
            tweenGem = tweenGem ?? DOTween.To(() => tmpGem, x =>
            {
                tmpGem = x;
                string formattedGem = FormatNumber(tmpGem);
                txtGem.text = formattedGem.ToString();
                
            }, _gem, 0.3f).SetAutoKill(false).OnComplete(() =>
            {
                tmpGem = GameManager.Instance.Profile.GetGem();
                string formattedGem = FormatNumber(tmpGem);
                txtGem.text = formattedGem.ToString();
            });
            tweenGem.ChangeStartValue(tmpGem);
            tweenGem.ChangeEndValue(_gem);
            tweenGem.Play();
        }

        private void PlayAnimationScale(Transform transformScale)
        {
            transformScale.DOScale(1.4f, 0.1f).OnComplete(() => { transformScale.DOScale(1, 0.05f); });
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SoundManager.Instance.PlaySoundButton();
            // TODO: Thoát ra ngoài thì update lại skin player
        }
    }


}