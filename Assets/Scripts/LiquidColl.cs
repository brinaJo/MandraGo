using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidColl : MonoBehaviour
{
    private Respawn RespawnP1;
    private Respawn RespawnP2;

    public GameObject Explosion;
    public EffectPool pool;
    private int index = 0;
    void Start()
    {
        RespawnP1 = GameObject.Find("P0_RespawnPos1").GetComponent<Respawn>();
        RespawnP2 = GameObject.Find("P1_RespawnPos1").GetComponent<Respawn>();
        pool = GameObject.Find("EffectPool").GetComponent<EffectPool>();
    }
    [SerializeField]

    void OnCollisionEnter(Collision coll)
    {
       if(coll.gameObject.tag == "Player")
        {
            if(coll.gameObject.GetComponent<Mandra>().player == 0)
            {
                RespawnP1.isRespawn = true;
                RespawnP1.DeadPlayer(2.0f);
                StartCoroutine(WaitExplosion(0.2f,coll.gameObject));

            }
            if (coll.gameObject.GetComponent<Mandra>().player == 1)
            {
                RespawnP2.isRespawn = true;
                RespawnP2.DeadPlayer(2.0f);
                StartCoroutine(WaitExplosion(0.2f, coll.gameObject));

            }

        }
       if(coll.gameObject.layer == 10)
        {
            Destroy(coll.gameObject);
        }
    }
    IEnumerator WaitExplosion(float seconds,GameObject coll)
    {
        yield return new WaitForSeconds(seconds);

        pool.SetEffect(pool.LiquidEffectPool, ref index , coll.gameObject.transform);
    }
}
