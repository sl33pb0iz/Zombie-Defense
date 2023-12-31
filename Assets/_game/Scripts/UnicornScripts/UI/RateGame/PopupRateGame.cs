using Google.Play.Common;
using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unicorn.UI
{
    [Singleton("Popup/PopupRateGame", true)]
    public class PopupRateGame : Singleton<PopupRateGame>
    {
        public bool isShow { get; set; }
        [SerializeField] private Button btnConfirm;
        [SerializeField] private Button btnNoThanks;
        [SerializeField] private List<Button> listBtnStar;
        [SerializeField] private List<Sprite> listSprStar;
        [SerializeField] private List<Image> listImgStar;

        private int star;

        public override void Awake()
        {
            base.Awake();

            if (!isDestroy)
            {
                Instance.Init();
            }

            if (!isShow)
            {
                Hide();
            }

            btnConfirm.onClick.AddListener(Confirm);
            btnNoThanks.onClick.AddListener(Hide);
        }

        private void Start()
        {
            for (int i = 0; i < listBtnStar.Count; i++)
            {
                int index = i + 1;
                listBtnStar[i].onClick.AddListener(() => { OnClickStar(index); });

                listImgStar[i].sprite = listSprStar[0];
            }

            star = 0;
        }

        public void Show()
        {
            isShow = true;
            gameObject.SetActive(true);
            btnConfirm.gameObject.SetActive(false);
            btnNoThanks.gameObject.SetActive(true);
        }

        public void Confirm()
        {
            if (star == 5)
            {
                StartCoroutine(RequestForReview());
                if (gameObject)
                    SoundManager.Instance.PlaySoundButton();
                return;
            }

            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            SoundManager.Instance.PlaySoundButton();

        }

        private void OnClickStar(int index)
        {
            star = index;

            for (int i = 0; i < listImgStar.Count; i++)
            {
                listImgStar[i].sprite = listSprStar[0];
            }


            for (int i = 0; i < index; i++)
            {
                listImgStar[i].sprite = listSprStar[1];
            }

            SoundManager.Instance.PlaySoundButton();
            btnConfirm.gameObject.SetActive(true);
            btnNoThanks.gameObject.SetActive(false);
        }

        #region Review in app

        private ReviewManager _reviewManager;
        private PlayReviewInfo _playReviewInfo;

        private IEnumerator RequestForReview()
        {
            WaitingCanvas.Instance.Show("");
            _reviewManager = new ReviewManager();
            //request object reqview flow
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                Debug.Log(requestFlowOperation.Error.ToString());
                WaitingCanvas.Instance.Hide();

                gameObject.SetActive(false);
                yield break;
            }

            _playReviewInfo = requestFlowOperation.GetResult();

            //lauch request review
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.Log(requestFlowOperation.Error.ToString());
                WaitingCanvas.Instance.Hide();

                gameObject.SetActive(false);
                yield break;
            }

            WaitingCanvas.Instance.Hide();
            gameObject.SetActive(false);
        }

        #endregion
    }

}