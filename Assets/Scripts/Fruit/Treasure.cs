using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Treasure : MonoBehaviour {

    private Animator animator;
    public GameObject coin;
    public bool open;
    public float damageDelay;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (open == true)
        {

            animator.SetTrigger("TR_OPEN");

            if (damageDelay > 0f)
                damageDelay -= Time.deltaTime;
           // else if (damageDelay <= 0f)
           //     coin.SetActive(true);
        }
    }

    public void CoinEnable()
    {
        SoundPool.Instance.SetSound(SoundPool.Instance.TresurePool, ref SoundPool.Instance.indexTresure, this.transform);
        coin.SetActive(true);
        coin.transform.parent = this.transform.parent;
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            open = true;
        }
    }
}
