using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    public GameObject groupHitPrefab;
    public GameObject liquidPrefab;
    public GameObject expolsionPrefab;

    public GameObject[] HitEffectPool;
    public GameObject[] GroupHitEffectPool;
    public GameObject[] LiquidEffectPool;
    public GameObject[] ExplosionEffectPool;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        HitEffectPool = EffectInit(hitEffectPrefab, 7, this.transform);
        GroupHitEffectPool = EffectInit(groupHitPrefab, 5, this.transform);
        LiquidEffectPool = EffectInit(liquidPrefab, 5, this.transform);
        ExplosionEffectPool = EffectInit(expolsionPrefab, 5, this.transform);
    }

    private GameObject[] EffectInit(GameObject obj, int cacheSize, Transform pos)
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

    public void SetEffect(GameObject[] Obj, ref int index, Transform pos)
    {
        Obj[index].transform.position = pos.position;
        Obj[index].transform.rotation = pos.rotation;
        Obj[index].SetActive(true);
        StartCoroutine(DisableEffect(Obj[index]));
        ++index;
        index %= Obj.Length;

    }
    IEnumerator DisableEffect(GameObject Obj)
    {
        yield return new WaitForSeconds(1.0f);

        Obj.SetActive(false);
    }
}
