using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour {
    FlyProb FP;
    public GameObject pulpe; //과일속.
    public GameObject skin;
    public GameObject banana;

   // private Rigidbody rb;

    void Awake()
    {
       // rb = GetComponent<Rigidbody>();
    }
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (pulpe != null && banana != null)
        {
            pulpe.transform.position = banana.transform.position;

            pulpe.transform.rotation = banana.transform.rotation;
        }
        if (transform.position.y >= 4)
        {
            if (skin == null)
                return;
            skin.transform.position = banana.transform.position;
            
            //banana.SetActive(false);
            pulpe.SetActive(true);
            skin.SetActive(true);

            banana.GetComponent<MeshRenderer>().enabled = false;
            pulpe.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
