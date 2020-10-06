using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class ButtonCtrl : MonoBehaviour {
    public GameObject[] Button;
    public GameObject Mouse;

    private GameObject TargetButton;
    private int ButtonNum;

    public int player;
    public Player Replayer;
    private bool isDelay = false;
    void Start()
    {
        Replayer = ReInput.players.GetPlayer(this.player);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(ButtonNum<Button.Length-1)
            {
                ButtonNum++;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (ButtonNum > 0)
            {
                ButtonNum--;
            }
        }
        if (Replayer.GetAxis("Move Vertical") != 0f)
        {
            if (!isDelay)
            {
                if (Replayer.GetAxis("Move Vertical") > 0)
                {
                    if (ButtonNum > 0)
                    {
                        ButtonNum--;
                        StartCoroutine(DelayButton());
                        isDelay = true;
                    }

                }
                else if (Replayer.GetAxis("Move Vertical") < 0)
                {
                    if (ButtonNum < Button.Length - 1)
                    {
                        ButtonNum++;
                        StartCoroutine(DelayButton());
                        isDelay = true;
                    }
                }
            }
        }

        if(Replayer.GetButtonDown("OKButton"))
        {
            switch (ButtonNum)
            {
                case 0:
                    this.GetComponent<AudioSource>().Play();
                    Application.LoadLevel("Mart");
                    break;

                case 1:
                    this.GetComponent<AudioSource>().Play();
                    Application.LoadLevel("Tutorial");
                    break;

                case 2:
                    Application.Quit();
                    break;

            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            switch (ButtonNum)
            {
                case 0:
                    this.GetComponent<AudioSource>().Play();
                    Application.LoadLevel("Mart");
                    break;

                case 1:
                    this.GetComponent<AudioSource>().Play();
                    Application.LoadLevel("Tutorial");
                    break;

                case 2:
                    Application.Quit();
                    break;

            }
        }

        TargetButton = Button[ButtonNum];
        Mouse.transform.position = TargetButton.transform.position;
       
      

    }
    IEnumerator DelayButton()
    {
        yield return new WaitForSeconds(0.3f);
        isDelay = false;
    }
	public void OnButtonClickStart()
    {
        this.GetComponent<AudioSource>().Play();
        Application.LoadLevel("Mart");
    }
    public void OnButtonClickTutorial()
    {
        this.GetComponent<AudioSource>().Play();
        Application.LoadLevel("Tutorial");
    }
    public void OnButtonClickExit()
    {
        Application.Quit();
    }
}
