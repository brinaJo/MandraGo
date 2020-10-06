using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollEnable : MonoBehaviour {

    public GameObject Doll;


    IEnumerator WaitDollEnable(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Doll.SetActive(true);
    }
    // Use this for initialization
	void Start ()
    {
        StartCoroutine(WaitDollEnable(5.0f));
	}
	
}
