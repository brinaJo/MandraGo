using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObj : MonoBehaviour {

    public float xSpeed = 0.0f;
    public float ySpeed = 0.0f;
    public float zSpeed = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
	}
}
