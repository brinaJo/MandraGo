using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoll : MonoBehaviour {
    public GameObject Explosion;
    float destTime;
	// Use this for initialization
	void Start () {
        destTime = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {

        destTime -= 1 * Time.deltaTime;

        if(destTime<=0f)
        {
            GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);
        }

        Collider[] colls = Physics.OverlapSphere(transform.position, 2f);



        foreach (Collider coll in colls)
        {

            if (coll.gameObject.layer != 8)
            {
                Transform Pos = coll.GetComponent<Transform>();

                Pos.transform.position = Vector3.MoveTowards(Pos.transform.position, transform.position, 5f * Time.deltaTime);
            }
        }
    }
}
