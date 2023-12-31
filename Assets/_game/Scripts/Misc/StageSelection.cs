using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelection : MonoBehaviour
{
    [SerializeField] private Button btnStage;
    [SerializeField] private int levelIndex;
    [SerializeField] private Image imgBg;
    [SerializeField] private List<Sprite> listSprBg;

    private void Awake()
    {
        btnStage.onClick.AddListener(StageSelect);
    }

    public void OnEnable()
    {
        int indexBg = PlayerDataManager.Instance.GetUnlockedLevel(levelIndex) ? 0 : 1;
        imgBg.sprite = listSprBg[indexBg];

        if (SceneManager.GetSceneAt(1).buildIndex - 1 == levelIndex)
        {
            imgBg.sprite = listSprBg[2];
        }
    }

    public void StageSelect()
    {
        if (PlayerDataManager.Instance.GetUnlockedLevel(levelIndex))
        {
            GameManager.Instance.ChangeLevel(levelIndex);
        }
    }
}
