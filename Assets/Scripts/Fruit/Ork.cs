using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ork : MonoBehaviour {
    public bool orkattack;
    public FlyProb Fp;
    // Use this for initialization
    void Start () {
        orkattack = false;

    }
	
	// Update is called once per frame
	void Update () {
       

    }

    void OnTriggerEnter(Collider call)
    {
      //  GameObject ork = GameObject.Find("ORK") as GameObject;

    }
    public void ORKKKK()
    {
        if (Fp.isFly == false)
        {
            orkattack = true;
        }
    }
}
