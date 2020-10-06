using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStone : MonoBehaviour {

    public enum Type { Red,Blue}
    public Type type;
    public GameObject Effect_Explosion;
    public GameObject Effect_BlackHoll;
    private FlyProb flyprob;
    private bool isBoom;
    private float BoomTime;
	// Use this for initialization
	void Start () {
        flyprob = GetComponent<FlyProb>();
        BoomTime =3.0f;
	}
	
	// Update is called once per frame
	void Update () {
  
        if (flyprob)
        {
            BoomTime -= 1f * Time.deltaTime;
        }

        if(BoomTime<=0f)
        {
            isBoom = true;
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == 8)
        {

            if (!isBoom)
                return;

            switch (type)
            {
                case Type.Red:
                   
                    Explosion();
                    Destroy(this.gameObject);
                    break;

                case Type.Blue:

                    SoundPool.Instance.SetSound(SoundPool.Instance.BalackPool, ref SoundPool.Instance.indexBalack, this.transform);
                    Instantiate(Effect_BlackHoll, transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                    break;
            }
           
        }
    }

    void Explosion()
    {
        SoundPool.Instance.SetSound(SoundPool.Instance.StoneExPool, ref SoundPool.Instance.indexStone, this.transform);
        Instantiate(Effect_Explosion, transform.position, Quaternion.identity);
        Collider[] colls = Physics.OverlapSphere(transform.position,3.0f);

        foreach(Collider coll in colls)
        {
            Rigidbody rigid = coll.GetComponent<Rigidbody>();
            if(rigid != null)
            {
                rigid.mass = 1.0f;
                rigid.AddExplosionForce(1000.0f, transform.position, 10.0f, 300.0f);
                

            }
        }
    }

    
}
