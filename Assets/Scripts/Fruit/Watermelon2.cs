using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon2 : MonoBehaviour {
    /*
    private GameObject m_RWeaponObj;
    void Start()
    {
        Transform[] ChildObj;
        ChildObj = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform t in ChildObj)
        {
            if (t.name == "RightWeapon")
            {
                // 미리 아이템이 붙을 위치의 오브젝트를 찾아 놓습니다. 
                m_RWeaponObj = t.gameObject;

                break;
            }
        }
    }
    
    // 아이템 붙이는 함수(사용자 함수) 
    void SetItem(GameObject ItemObj)
    {
        // 붙이려고 하는 아이템 오브젝트가 인자로 들어오면 
        // 원본과 동일한 오브젝트 하나를 생성 시킴니다. 
        GameObject TempObj = (GameObject)Instantiate(ItemObj);

        // 생성 시킨 아이템 오브젝트의 부모를 Start() 함수에서 찾아놨던 오브젝트로 설정해 주면 
        // 그 위치에 붙습니다. 이렇게 하면 캐릭터 본에 따라 아이템도 따라 움직입니다. 
        TempObj.transform.parent = m_RWeaponObj.transform;
    }*/

    //////////////////////////////////////////////////////////////////////////
    public GameObject water1;  //큰거.
    public GameObject water2;
    public GameObject water3;
    public bool melon1 = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.y >= 4)
        {

//
            melon1 = true;
            /*
			water2.transform.position = FP.targetPos.transform.position;
			water2.transform.position = FP.targetPos.transform.position;
			water3.transform.position = FP.targetPos.transform.position;
			water3.transform.position = FP.targetPos.transform.position;
			*/



            //melon1 = true;
            //water3.GetComponent<MeshCollider>().enabled = false;
        }
    
        if (/*melon1 == true &&*/ transform.position.y >= 4)
        {

              //water2 = (GameObject)Instantiate(Resources.Load("Prefab/wdown"));
             //  water3 = (GameObject)Instantiate(Resources.Load("Prefab/wup"));
             //  water2.name = water1.name;
             //   water3.name = water1.name;
           // water2 = Resources.Load("Prefab/wdown") as GameObject;
           //  water3 = Resources.Load("Prefab/wup") as GameObject;
            Instantiate(water2).transform.position = water1.transform.position;
             Instantiate(water3).transform.position = water1.transform.position;

            water2.transform.position = water1.transform.position;
            water2.transform.rotation = water1.transform.rotation;
           water3.transform.position = water2.transform.position;
            water3.transform.rotation = water2.transform.rotation;
           water2.SetActive(true);
            water3.SetActive(true);
          // water1.SetActive(false);
            //water1.GetComponent<MeshRenderer>().enabled = false;

           // water1.GetComponent<MeshCollider>().enabled = false;


        }

    }

}
