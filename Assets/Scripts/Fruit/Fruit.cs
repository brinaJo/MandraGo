using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {
    
    public static Fruit fruits;
  //  public Transform[] FruitGetPos; //과일 생성 위치 배열.
    public GameObject[] FruitsObj;

    void Start()
    {
        StartCoroutine(Example());


    }

    Vector3 SetPos()
    {
        float PosY = 2.0f;
        float PosX = Random.Range(-3f, 9f);
		float PosZ = Random.Range (-5f, 2f);
        //float PosZ = Random.Range(0f, 6f);
        //Vector3 Pos = Camera.main.ViewportToWorldPoint(new Vector3(PosX, PosY, 1));
		Vector3 Pos = new Vector3(PosX, PosY, PosZ);
        //Pos.z = 1;
        return Pos;
    }

    void Update()
    {
 
    }

    IEnumerator Example()
    {
        while (true)
        {
            Instantiate(FruitsObj[0], SetPos(), Quaternion.identity);
            Instantiate(FruitsObj[1], SetPos(), Quaternion.identity);
			Instantiate(FruitsObj[2], SetPos(), Quaternion.identity);
			Instantiate(FruitsObj[3], SetPos(), Quaternion.identity);
			Instantiate(FruitsObj[4], SetPos(), Quaternion.identity);
            yield return new WaitForSeconds(18f);//WaitForSeconds객체를 생성해서 반환. 
        }       
    }

   

}