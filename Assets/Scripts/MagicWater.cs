using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MagicWater : MonoBehaviour {

    float num;
    bool a;
    public LayerMask groundLayer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

    void FixedUpdate()
    {
        if (!a)
        {
            RaycastHit hit = new RaycastHit();
            hit.point = this.transform.position - transform.transform.up;
            hit.normal = transform.up;
            Debug.DrawRay(this.transform.position, -Vector3.up);

            Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0f, groundLayer);

            if (hit.distance < 0.04)
            {
                a = true;
            }
        }
    }
	void Update ()
    {
        if (!a)
        {
            num -= Time.deltaTime * 0.01f;
            Vector3 pos2 = new Vector3(this.transform.position.x, this.transform.position.y + num, this.transform.position.z);
            this.transform.position = pos2;
        }

    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            int RanNum = Random.Range(0, 2);
            if (RanNum == 0)
            {
                coll.gameObject.GetComponent<Mandra>().canHackPunch = true;
                coll.gameObject.GetComponent<Mandra>().VisiblePunchIcon();
            }
            else if (RanNum == 1)
            {
                //만드라고라 암전 coll.gameObject~하면돼여
                coll.gameObject.GetComponent<Mandra>().darkness = true;
                coll.gameObject.GetComponent<Mandra>().DarkDamageCheck();
                SoundPool.Instance.SetSound(SoundPool.Instance.NotViewPool, ref SoundPool.Instance.indexNotview, coll.gameObject.transform);
            }
            Destroy(this.gameObject, 0.3f);
        }
        else
            return;
    }

}
