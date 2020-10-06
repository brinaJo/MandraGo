using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdownWhale : MonoBehaviour {

    private Vector3 _startPosition;
    public float speed;
    // Use this for initialization
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _startPosition + new Vector3(0.0f, Mathf.Sin(Time.time * speed), 0.0f);
    }
}
