using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : MonoBehaviour
{
    FlyProb FP;
    Mandra MA;
    public GameObject myWater;
    public GameObject water1;  //큰거.
    public GameObject water2;
    public GameObject water3;
    public GameObject water4;

    public bool melon1 = false;
    // Use this for initialization
    void Start()
    {
        FP = this.GetComponent<FlyProb>();
    }

    // Update is called once per frame
    void Update()
    {
        if (water1 != null && water2 != null && water3 != null && water4 != null)
        {
            if (myWater.GetComponent<FlyProb>().isFly == true)
            {
                water1.transform.position = myWater.transform.position;
                water2.transform.position = myWater.transform.position;
                water3.transform.position = myWater.transform.position;
                water4.transform.position = myWater.transform.position;
            }

        }
        if (transform.position.y >= 2)
        {
            water1.SetActive(true);
            water2.SetActive(true);
            water3.SetActive(true);
            water4.SetActive(true);
            water1.transform.position = myWater.transform.position;
            water2.transform.position = myWater.transform.position;
            water3.transform.position = myWater.transform.position;
            water4.transform.position = myWater.transform.position;
            myWater.GetComponent<MeshRenderer>().enabled = false;
            myWater.GetComponent<MeshCollider>().enabled = false;
        }
    }
}