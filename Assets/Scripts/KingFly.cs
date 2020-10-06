using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFly : MonoBehaviour
{


    public Transform king_target;
    public GameObject King_obj;

    public bool touch = false;
    public bool king_fly = false;
    float num;
    bool a;
    public LayerMask groundLayer;



    void Start()
    {



    }
    /*
     Vector3 SetPos()
     {
         int num = Random.Range(0, King_Pos.GetLength(0));

         return King_Pos[num].transform.position;
     }
     */
    /*
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
   */
    void Update()
    {
        king_target = King_obj.transform;
        /*
        if (!a)
        {
            num -= Time.deltaTime * 0.01f;
            Vector3 pos2 = new Vector3(this.transform.position.x, this.transform.position.y + num, this.transform.position.z);
            this.transform.position = pos2;
        }
        */
        if (touch == true)
        {
            Debug.Log("체스등장");
            Instantiate(King_obj, king_target.transform.position, Quaternion.identity);
            // yield return new WaitForSeconds(1f);//WaitForSeconds객체를 생성해서 반환. 
            // Destroy(this.gameObject, 0.5f);
        }


    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<FlyProb>().isFly == true && coll.tag != "panel")
        {
            Debug.Log("체스쿵");
            king_fly = true;
        }
        //coll.gameObject.GetComponent<FlyProb>().isFly == true
        if (king_fly == true && coll.tag == "panel")
        {
            touch = true;
        }

    }


}



