using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.UI
{
    public class PopupNoInternet : MonoBehaviour
    {
        [SerializeField] private float interval = 10;
        private float timeCheckInternet;
        private float timeToCheckInternet;

        private void Awake()
        {
            timeCheckInternet = interval;
        }

        private void Update()
        {
            CheckInternet();
        }

        private void CheckInternet()
        {

            timeCheckInternet += Time.unscaledDeltaTime;
            if (timeCheckInternet >= timeToCheckInternet)
            {
                timeCheckInternet = 0;

                if (!RocketRemoteConfig.GetBoolConfig("config_check_internet", true))
                    return;

                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    /*Time.timeScale = 1;*/
                    timeToCheckInternet = interval;
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    timeToCheckInternet = 1;
                    transform.GetChild(0).gameObject.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

}