using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour {

    public Text resultMsg1;
    public Text resultMsg2;
    public Transform stars;
    public Image background;
    public Color winColorMsg;
    public Color loseColorMsg;
    public Color winColorBG;
    public Color loseColorBG;
    public Transform btnOK;
    public Transform btnRetry;
    public Transform btnBack;

    public void Show(bool win)
    {
        this.gameObject.SetActive(true);
        stars.gameObject.SetActive(win);
        btnOK.gameObject.SetActive(win);
        btnRetry.gameObject.SetActive(!win);
        btnBack.gameObject.SetActive(!win);

        if (win)
        {
            resultMsg1.text = resultMsg2.text = "LEVEL CLEAR !";
            resultMsg2.color = winColorMsg;
            background.color = winColorBG;
        }
        else
        {
            resultMsg1.text = resultMsg2.text = "LEVEL FAILED...";
            resultMsg2.color = loseColorMsg;
            background.color = loseColorBG;
        }
    }
}
