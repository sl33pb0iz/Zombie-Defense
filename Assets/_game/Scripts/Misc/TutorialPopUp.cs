using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPopUp : MonoBehaviour
{
    public Button buttonNext;
    public TextMeshProUGUI text;
    public string[] textTutorial;

    
    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1; 
    }


}
