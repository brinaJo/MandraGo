using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken : MonoBehaviour {

    public GameObject none_broke;

    public GameObject broke;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag =="Prob"
            || coll.gameObject.tag == "Player")
        {
            broke.SetActive(true);
            Destroy(none_broke);
        }
    }
}
