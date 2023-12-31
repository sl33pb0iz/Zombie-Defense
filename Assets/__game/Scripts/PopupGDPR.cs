
using Unicorn.UI;
using UnityEngine;

public class PopupGDPR : UICanvas
{

    public void OnClickAgree()
    {
        PlayerPrefs.SetInt("agreeGDPR", 1);

        Show(false);
    }

    public void OnClikNoAgree()
    {
        PlayerPrefs.SetInt("agreeGDPR", 2);

        Show(false);
    }

    public bool IsChecked()
    {
        if (PlayerPrefs.GetInt("showGDPR", 0) == 1)
        {
            return true;
        }
        else
        {
            PlayerPrefs.SetInt("showGDPR", 1);
            Show(true);
        }

        return false;
    }
}
