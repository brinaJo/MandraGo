using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMapCtrl : MonoBehaviour
{

    public GameObject[] ChessBlock;
    public GameObject[] ChessStair;
    public GameObject Chess;
    private float FallTime;
    private int StairNum;
    public GameObject Danger;


    public GameObject[] camera3;
    // Use this for initialization
    void Start()
    {
        ChessBlock = GameObject.FindGameObjectsWithTag("ChessBlock");

        camera3 = GameObject.FindGameObjectsWithTag("MainCamera");
        FallTime = 30f;
        StartCoroutine("Fall");
        StartCoroutine("StairFall");
        StartCoroutine("CreateChess");
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(FallTime);
        ChessBlock = GameObject.FindGameObjectsWithTag("ChessBlock");
        FallTime = 10f;
        if (ChessBlock.Length != 36)
        {
            for (int i = 0; i < 4; i++)
            {
                int rand = Random.Range(0, ChessBlock.Length);
                FallCtrl Fall = ChessBlock[rand].GetComponent<FallCtrl>();
                if(i != 0)
                {
                    Fall.playSound = true;
                }
                if (!Fall.isFall)
                    Fall.isFall = true;
                else
                    i--;

            }

            StartCoroutine("Fall");
        }
        if (ChessBlock.Length == 40)
        {
            StopCoroutine("Fall");
        }
    }

    IEnumerator StairFall()
    {
        yield return new WaitForSeconds(FallTime);
        FallTime = 10f;
        FallCtrl Fall = ChessStair[StairNum].GetComponent<FallCtrl>();
        Fall.isFall = true;
        if (StairNum != 5)
        {
            StairNum++;
            StartCoroutine("StairFall");
            for (int i = 0; i < 2; i++)
            {
                camera3[i].GetComponent<CameraController3>().shake = 1;
            }
        }
        else
        {
            StopCoroutine("StairFall");
        }
    }

    IEnumerator CreateChess()
    {
        yield return new WaitForSeconds(30f);
        GameObject chess = Instantiate(Chess, Chess.transform.position, Quaternion.identity) as GameObject;
        chess.transform.localScale = Chess.transform.localScale;
        StartCoroutine("CreateChess");
    }

}
