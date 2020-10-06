using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicObj : MonoBehaviour
{

    public static MagicObj mo;

    public GameObject MagicObj1;
    public GameObject MagicObj2;

    public int indexobj1;
    public int indexobj2;
    public GameObject[] MagicObjPool1;
    public GameObject[] MagicObjPool2;
    public GameObject[] Magic_Pos;

    // Use this for initialization
    void Start()
    {
        MagicObjPool1 = ObjInit(MagicObj1, 10, this.transform);
        MagicObjPool2 = ObjInit(MagicObj2, 10, this.transform);
        StartCoroutine(Example());

    }
    private GameObject[] ObjInit(GameObject obj, int cacheSize, Transform pos)
    {
        GameObject[] go = new GameObject[cacheSize];
        for (int i = 0; i < cacheSize; ++i)
        {
            go[i] = Instantiate(obj, pos.position, pos.rotation);
            go[i].SetActive(false);
            go[i].transform.parent = this.transform;
        }
        return go;
    }
    public void SetObj(GameObject[] Obj, ref int index, Transform pos)
    {
        Obj[index].transform.position = pos.position;
        Obj[index].transform.rotation = pos.rotation;
        Obj[index].SetActive(true);
        ++index;
        index %= Obj.Length;
    }

    Transform SetPos()
    {
        int num = Random.Range(0, Magic_Pos.GetLength(0));

        return Magic_Pos[num].transform;
    }
    void Update()
    {

    }

    IEnumerator Example()
    {
        while (true)
        {
            int a = Random.Range(0, 100);
            if(a < 50)
            {
                SetObj(MagicObjPool1, ref indexobj1, SetPos());
            }
            else
            {
                SetObj(MagicObjPool2, ref indexobj2, SetPos());
            }
            yield return new WaitForSeconds(12f);//WaitForSeconds객체를 생성해서 반환. 
        }
    }
}
