using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotion : MonoBehaviour {
	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ScalePlusEmotion()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("x", 1.0f, "y", 1.0f,"time",0.3f));
    }
    public void ScaleMinusEmotion()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("x", 0.0f, "y", 0.0f,"time", 0.3f));
    }

    
}
