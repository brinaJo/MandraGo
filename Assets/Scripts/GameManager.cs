using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public Animator Stage_4;
    public Animator Stage_5;
    public Animator Stage_6;
    public Animator Hand_anim;
    public GameObject Hand;
    public GameObject Smoke;
    

    public bool isSlow = false; 
    // Use this for initialization
    void Start () {
        StartCoroutine("StageOpen");
        StartCoroutine("Smoke_Start");
        StartCoroutine("Hand_Start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator StageOpen()
    {
        yield return new WaitForSeconds(20f);
        Stage_4.SetBool("isOpen",true);
        Stage_5.SetBool("isOpen", true);
        Stage_6.SetBool("isOpen", true);
        StopCoroutine("StageOpen");
        StartCoroutine("StageClose");
    }

    IEnumerator StageClose()
    {
        yield return new WaitForSeconds(5f);
        Stage_4.SetBool("isOpen", false);
        Stage_5.SetBool("isOpen", false);
        Stage_6.SetBool("isOpen", false);
        StopCoroutine("StageClose");
        StartCoroutine("StageOpen");
        StartCoroutine("Hand_End");
    }

    IEnumerator Smoke_Start()
    {
        yield return new WaitForSeconds(10f);
        Smoke.SetActive(true);
        SoundPool.Instance.SetSound(SoundPool.Instance.IceWindPool, ref SoundPool.Instance.indexIceWind, this.transform);
        isSlow = true;
        StopCoroutine("Smoke_Start");
        StartCoroutine("Smoke_End");
        StartCoroutine("Slow_End");
            
    }

    IEnumerator Smoke_End()
    {
        yield return new WaitForSeconds(6f);
        Smoke.SetActive(false);
        SoundPool.Instance.DisableSoundEffect(SoundPool.Instance.IceWindPool, ref SoundPool.Instance.indexIceWind);
        StopCoroutine("Smoke_End");
    }

    IEnumerator Slow_End()
    {
        yield return new WaitForSeconds(3f);
        isSlow = false;
        StopCoroutine("Slow_End");
        StartCoroutine("Smoke_Start");

    }

    IEnumerator Hand_Start()
    {
        yield return new WaitForSeconds(3f);
        Hand.SetActive(true);
        StopCoroutine("Hand_Start");
    }
    IEnumerator Hand_End()
    {
        yield return new WaitForSeconds(2f);

        Hand_anim.SetBool("isEnd", true);
        StopCoroutine("Hand_End");
        StartCoroutine("Hand_Start");
        StartCoroutine("Hand_Delete");
        
    }

    IEnumerator Hand_Delete()
    {
        yield return new WaitForSeconds(2f);
        Hand.SetActive(false);
        StopCoroutine("Hand_Delete");
    }

}
