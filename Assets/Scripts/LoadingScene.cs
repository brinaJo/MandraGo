using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingScene : MonoBehaviour
{
    private int temptime = 0;
    public Text text;
    private string dot;
    public Image LoadingBar;
    // Use this for initialization
    void Start()
    {
        temptime = System.DateTime.Now.Second;
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {


        if (System.DateTime.Now.Second - temptime > 6 || System.DateTime.Now.Second - temptime < 0)
        {

            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }
        else
        {
            LoadingBar.fillAmount = (System.DateTime.Now.Second - temptime) * 0.2f;
            text.text = "Loading" + dot;
            if (System.DateTime.Now.Second - temptime > 0)
                dot = ".";
            if (System.DateTime.Now.Second - temptime > 1)
                dot = "..";
            if (System.DateTime.Now.Second - temptime > 2)
                dot = "...";
            if (System.DateTime.Now.Second - temptime > 3)
                dot = "";
            if (System.DateTime.Now.Second - temptime > 4)
                dot = ".";
            if (System.DateTime.Now.Second - temptime > 5)
                dot = "..";
        }

    }
}
