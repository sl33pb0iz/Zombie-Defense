using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unicorn.UI;
using Unicorn;

public class ButtonFunctionController : MonoBehaviour
{
    public Button btnHome;
    public Button btnSetting;
    public Button btnPause;

    private void Start()
    {
        btnHome.onClick.AddListener(GameManager.Instance.UiController.OpenUiQuitLevel);
        btnSetting.onClick.AddListener(GameManager.Instance.UiController.OpenUiSetting);
        btnPause.onClick.AddListener(GameManager.Instance.UiController.OpenUiPause);
    }
    

}
