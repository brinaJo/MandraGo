using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIctrl : MonoBehaviour
{

    public enum GetScore { player1, player2 };//6.9
    public GetScore getscore;//6.9
    public Text timeTxt_1;
    public Text timeTxt_2;
    public GameObject[] Lose;//6.9
    public GameObject[] Win;//6.9
    public GameObject[] Draw;//6.9
    private float curentTime;
    private float Minit;
    private float Sec;
    public Text Kill1;//6.9
    public Text Kill2;//6.9
    public bool isRestart;
    private Score score;
    private bool isEnd;
    public Image[] P1ScoreUI;//6.9
    public Image[] P2ScoreUI;//6.9

    private GameObject P1;
    private GameObject P2;

    public GameObject P1Skin;
    public GameObject P2Skin;

    public Sprite blueBox;
    public Sprite redBox;

    public bool isGameEnd;
    private bool isAnimOnce;
    void Start()
    {
        curentTime = 90f;
        Sec = 30f;
        Minit = 1f;
        score = GameObject.Find("ScoreManager").GetComponent<Score>();
        P1 = GameObject.Find("Player_0");
        P2 = GameObject.Find("Player_1");
    }

    // Update is called once per frame
    void Update()
    {
        if (score == null)
            return;
        UIcolorSet();
        if (!isRestart)
        {
            curentTime -= Time.deltaTime * 1f;
            if (Sec != 0f)
            {
                if (!isRestart)
                    Sec -= Time.deltaTime * 1f;

            }
            if (Sec < 0f)
            {
                Sec = 60f;
                Minit -= 1f;
            }

            if (Sec == 10f)
            {
                timeTxt_1.text = "" + Mathf.Round(Minit) + " : 10";
                timeTxt_2.text = "" + Mathf.Round(Minit) + " : 10";
            }

            else if (Sec > 10f)
            {
                if (Sec > 59.5f)
                {
                    timeTxt_1.text = "" + Mathf.Round(Minit + 1f) + " : 00";
                    timeTxt_2.text = "" + Mathf.Round(Minit + 1f) + " : 00";
                }

                else
                {
                    timeTxt_1.text = "" + Mathf.Round(Minit) + " : " + Mathf.Round(Sec);
                    timeTxt_2.text = "" + Mathf.Round(Minit) + " : " + Mathf.Round(Sec);
                }
            }

            else if (Sec <= 9.5f)
            {
                timeTxt_1.text = "" + Mathf.Round(Minit) + " : 0" + Mathf.Round(Sec);
                timeTxt_2.text = "" + Mathf.Round(Minit) + " : 0" + Mathf.Round(Sec);
            }

            Kill1.text = "" + score.Player_1_Kill;
            Kill2.text = "" + score.Player_2_Kill;


            if (score.Player_1_Kill == 15)//6.9
            {

                if (!isRestart && getscore == GetScore.player1)
                    score.Round++;
                isRestart = true;
                score.Player_1_Win[score.Round] = true;
                Invoke("Restart", 3f);

            }

            if (score.Player_2_Kill == 15)//6.9
            {
                if (!isRestart && getscore == GetScore.player1)
                    score.Round++;
                isRestart = true;
                score.Player_2_Win[score.Round] = true;
                Invoke("Restart", 3f);

            }



            if (curentTime < 0f)//6.9
            {
                Sec = 0f;
                Minit = 0f;
                WinPlayer();
                Debug.Log("d");
            }
        }

    }

    public void Restart()//6.9
    {

        if (!isEnd)
        {
            curentTime = 0;
            score.Player_1_Kill = 0;
            score.Player_2_Kill = 0;
            isAnimOnce = false;

            //if (score.p1ScoreNum >= 3 && score.p2ScoreNum >= 3)
            //{
            //}
            //else if (score.p1ScoreNum >= 3)
            //{
            //    Application.LoadLevel("Result");
            //    isEnd = true;
            //    isGameEnd = false;
            //    return;
            //}
            //else if (score.p2ScoreNum >= 3)
            //{
            //    Application.LoadLevel("Result");
            //    isEnd = true;
            //    isGameEnd = false;
            //    return;
            //}

            if (score.Round == 1)
                Application.LoadLevel("Magic");
            if (score.Round == 2)
            {
                Application.LoadLevel("Whale0522");
            }
            if (score.Round == 3)
            {
                Application.LoadLevel("Chess");
            }
            if (score.Round == 4)
            {
                int num = Random.Range(1, 4);
                if (num == 1)
                    Application.LoadLevel("Magic");
                else if (num == 2)
                    Application.LoadLevel("Whale0522");
                else if (num == 3)
                    Application.LoadLevel("Mart");
                else if (num == 4)
                    Application.LoadLevel("Chess");
            }
            if (score.Round >= 5)
            {
                Application.LoadLevel("Title");
                //Application.LoadLevel("Result");
                Destroy(score.gameObject);
            }
            isEnd = true;
            isGameEnd = false;

        }
    }
    IEnumerator CameraMove(int whoWin)
    {

        while (true)
        {
            P1.GetComponentInChildren<Mandra>().state = MandraState.GameEnd;
            P2.GetComponentInChildren<Mandra>().state = MandraState.GameEnd;
            P1.GetComponentInChildren<CameraController3>().mode = CameraMode.Close;
            P2.GetComponentInChildren<CameraController3>().mode = CameraMode.Close;

            if (!isAnimOnce)
            {
                isAnimOnce = true;
                if (whoWin == 1)
                {
                    P1.GetComponentInChildren<Animator>().SetTrigger("goWin");
                    P2.GetComponentInChildren<Animator>().SetTrigger("goLose");
                }
                else if (whoWin == 2)
                {
                    P1.GetComponentInChildren<Animator>().SetTrigger("goLose");
                    P2.GetComponentInChildren<Animator>().SetTrigger("goWin");
                }
                else if (whoWin == 3)
                {
                    P1.GetComponentInChildren<Animator>().SetTrigger("goDraw");
                    P2.GetComponentInChildren<Animator>().SetTrigger("goDraw");
                }
            }
            yield return null;
        }
    }

    public void WinPlayer()//6.9
    {
        if (score.Player_1_Kill > score.Player_2_Kill)
        {
            if (!isRestart && getscore == GetScore.player1)
            {
                score.Round++;
                score.p1ScoreNum++;
            }
            isRestart = true;
            score.Player_1_Win[score.Round] = true;
            Invoke("Restart", 3f);
            Win[0].SetActive(true);
            Lose[1].SetActive(true);
            StartCoroutine(CameraMove(1));

            P1Skin.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_WIN") as Texture;
            P2Skin.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_LOSE") as Texture;
            P1.GetComponentInChildren<Canvas>().enabled = false;
            P2.GetComponentInChildren<Canvas>().enabled = false;

            isGameEnd = true;
        }

        else if (score.Player_1_Kill == score.Player_2_Kill)
        {
            if (!isRestart && getscore == GetScore.player1)
            {
                score.Round++;

                score.p1ScoreNum++;
                score.p2ScoreNum++;

                
            }
            isRestart = true;
            score.Player_Draw[score.Round] = true;
            Invoke("Restart", 3f);
            Draw[0].SetActive(true);
            Draw[1].SetActive(true);
            StartCoroutine(CameraMove(3));
            isGameEnd = true;

        }

        else
        {
            if (!isRestart && getscore == GetScore.player1)
            {
                score.Round++;
                score.p2ScoreNum++;
            }
            isRestart = true;
            score.Player_2_Win[score.Round] = true;
            Invoke("Restart", 3f); ;
            Win[1].SetActive(true);
            Lose[0].SetActive(true);
            StartCoroutine(CameraMove(2));
            P1.GetComponentInChildren<Canvas>().enabled = false;
            P2.GetComponentInChildren<Canvas>().enabled = false;

            P1Skin.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_LOSE") as Texture;
            P2Skin.GetComponentInChildren<Renderer>().material.mainTexture = Resources.Load("mandrake_UV_WIN") as Texture;

            isGameEnd = true;

        }

    }

    public void UIcolorSet()
    {

        if (score.Round != 0)
        {

            for (int i = 1; i <= score.p1ScoreNum; i++)
            {
                P1ScoreUI[score.p1ScoreNum - i].GetComponent<Image>().sprite = redBox; //.color = new Color(255, 0, 0, 255);
                P2ScoreUI[score.p1ScoreNum - i].GetComponent<Image>().sprite = redBox;//.color = new Color(255, 0, 0, 255);

            }
            for (int i = 4; i >= 5 - score.p2ScoreNum; i--)
            {
                P1ScoreUI[i].GetComponent<Image>().sprite = blueBox;// = new Color(0, 0, 255, 255);
                P2ScoreUI[i].GetComponent<Image>().sprite = blueBox;// = new Color(0, 0, 255, 255);
            }

        }

    }
}
