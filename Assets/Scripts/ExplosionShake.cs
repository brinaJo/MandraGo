using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionShake : MonoBehaviour
{
    public GameObject[] camera3;
    // Use this for initialization
    void Start()
    {
        camera3 = GameObject.FindGameObjectsWithTag("MainCamera");
        for (int i = 0; i < 2; i++)
        {
            camera3[i].GetComponent<CameraController3>().shake = 1f;
        }
    }


}
