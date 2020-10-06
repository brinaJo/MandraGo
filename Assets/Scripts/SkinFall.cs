using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinFall : MonoBehaviour
{
    private Transform tr;
    public float fallSpeed = 4f;
    private bool isGround = false;

    float num;
    bool a;
    public LayerMask groundLayer;
    // Use this for initialization
    void Start ()
    {
        tr = this.GetComponentInParent<Transform>();
	}

    void FixedUpdate()
    {
        if (!a)
        {
            RaycastHit hit = new RaycastHit();
            hit.point = this.transform.position - transform.transform.up;
            hit.normal = transform.up;
            Debug.DrawRay(this.transform.position, -Vector3.up);

            Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0f, groundLayer);

            if (hit.distance < 0.1)
            {
                a = true;
            }
        }
    }
    void Update()
    {
        if (!a)
        {
            num -= Time.deltaTime * 0.01f;
            Vector3 pos2 = new Vector3(this.transform.position.x, this.transform.position.y + num, this.transform.position.z);
            this.transform.position = pos2;
        }
    }

}
