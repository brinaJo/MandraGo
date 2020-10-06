using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamCtrl : MonoBehaviour
{
    public GameObject[] SeaObj;
    public GameObject[] Steam;
    public Transform Tr;
    public float DropTime;
    private float CurrentTime;
    private float Scale;
    private Collider coll;//8.16;
                          // Use this for initialization
    void Start()
    {
        CurrentTime = DropTime;
        Scale = 4f;
        coll = GetComponent<Collider>();//8.16;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime -= Time.deltaTime * 1f;

        if (CurrentTime < 0f)
        {
            int Amount = Random.Range(1, 3);
            for (int i = 0; i <= Amount; i++)
            {
                coll.enabled = true;//8.16;
                for(int j=0;j<Steam.Length;j++)
                {
                    if(j==0)
                    {
                        SoundPool.Instance.SetSound(SoundPool.Instance.WhaleWaterPool, ref SoundPool.Instance.indexWhaleWater, this.transform);
                    }
                    Steam[j].SetActive(true);
                }
                GameObject obj = Instantiate(SeaObj[Random.Range(0, SeaObj.Length)], Tr.transform.position, transform.rotation);
                obj.transform.parent = GameObject.Find("SeaObject").transform;
                Rigidbody rigid = obj.GetComponent<Rigidbody>();
                Rigidbody rigid2 = GameObject.FindWithTag("Player").GetComponent<Rigidbody>(); obj.transform.localScale = new Vector3(obj.transform.localScale.x * 8f, obj.transform.localScale.y * 8f, obj.transform.localScale.z * 8f);

                if (rigid != null)
                    rigid.AddForce(Vector3.up * 250f);
            }

            CurrentTime = DropTime;


        }

        if (Steam[0].activeSelf)
        {
            StartCoroutine("SteamScale");
            StartCoroutine("Deactive");
            EllipsoidParticleEmitter particle = Steam[0].GetComponent<EllipsoidParticleEmitter>();
            particle.localVelocity = new Vector3(0f, Scale, 0f);
        }
    }

    IEnumerator Deactive()
    {
        yield return new WaitForSeconds(5f);
        Scale = 4f;
        EllipsoidParticleEmitter particle = Steam[0].GetComponent<EllipsoidParticleEmitter>();
        particle.localVelocity = new Vector3(0f, Scale, 0f);
        coll.enabled = false;//8.16;
        for (int j = 0; j < Steam.Length; j++)
        {
            Steam[j].SetActive(false);
        }
        StopCoroutine("SteamScale");
        StopCoroutine("Deactive");

    }

    IEnumerator SteamScale()
    {
        yield return new WaitForSeconds(2f);
        Scale -= Time.deltaTime * 1.5f;


    }

    public void OnTriggerStay(Collider other)//8.16;
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rigid = other.gameObject.GetComponent<Rigidbody>();
            if (rigid != null)
                rigid.AddForce(Vector3.up * 450f);
        }
    }

}
