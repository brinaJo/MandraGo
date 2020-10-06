using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject P1field;
    public GameObject P2field;


    private Score score;

    private bool p1win = true;
    private bool p2win;


    public float num = 0f;
    // Use this for initialization
    void Start ()
    {

        score = GameObject.Find("ScoreManager").GetComponent<Score>();
        if(score.p1ScoreNum >=3 && score.p1ScoreNum > score.p2ScoreNum)
        {
            p1win = true;
        }
        else if(score.p2ScoreNum >= 3 && score.p1ScoreNum < score.p2ScoreNum)
        {
            p2win = true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(p1win)
        {
            num += 0.01f;
            if(num < 0.5)
            {
                p1win = false;
            }
            Vector3 pos2 = new Vector3(P1field.transform.position.x, P1field.transform.position.y + num, P1field.transform.position.z);
            P1field.transform.position = pos2;
        }
	}
}
