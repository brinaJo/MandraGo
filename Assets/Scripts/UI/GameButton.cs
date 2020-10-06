using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameButton : MonoBehaviour
{
    private bool isButton;
    private bool isStop;
    public GameObject Button;
    public Image PlayStop;
    public Sprite Play;
    public Sprite Stop;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            if (isButton)
            {
                Button.SetActive(true);
                isButton = false;
            }
            else
            {

                Button.SetActive(false);
                isButton = true;
            }

        }

    }

    public void StopButton()
    {

        if (!isStop)
        {
            PlayStop.GetComponent<Image>().sprite = Stop;
            Time.timeScale = 0f;
            isStop = true;

        }
        else
        {
            PlayStop.GetComponent<Image>().sprite = Play;
            Time.timeScale = 1f;
            isStop = false;
        }
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Title");
    }
}
