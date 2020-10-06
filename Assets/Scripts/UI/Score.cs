using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

   
    public int Player_1_Kill;
    public int Player_2_Kill;
    public bool[] Player_1_Win;
    public bool[] Player_2_Win;
    public bool[] Player_Draw;
    public int Round;

    public int p1ScoreNum;
    public int p2ScoreNum;
    private UIctrl uictrl;
	// Use this for initialization
	void Start () {
        uictrl = FindObjectOfType<UIctrl>();
        DontDestroyOnLoad(this.gameObject);
      
	}
	
	// Update is called once per frame
	void Update ()
    {
       
	}
}
